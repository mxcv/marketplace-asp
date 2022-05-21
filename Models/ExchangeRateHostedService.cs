using System.Globalization;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Marketplace.Models
{
	public class ExchangeRateHostedService : BackgroundService
	{
		private readonly IServiceScopeFactory scopeFactory;
		private readonly ExchangeRateConfiguration exchangeConfig;
		private readonly TimeSpan refreshInterval = TimeSpan.FromDays(1);

		public ExchangeRateHostedService(IServiceScopeFactory scopeFactory, IOptions<ExchangeRateConfiguration> exchangeOptions)
		{
			this.scopeFactory = scopeFactory;
			exchangeConfig = exchangeOptions.Value;
		}

		protected override async Task ExecuteAsync(CancellationToken cancellationToken)
		{
			TimeSpan delay = TimeSpan.Zero;
			using (var scope = scopeFactory.CreateScope())
			{
				var db = scope.ServiceProvider.GetRequiredService<MarketplaceDbContext>();
				var lastRequest = await db.ExchangeInfoRequests.SingleOrDefaultAsync();
				if (lastRequest != null && refreshInterval > DateTimeOffset.UtcNow - lastRequest.Created)
					delay = refreshInterval - (DateTimeOffset.UtcNow - lastRequest.Created);
				else await RefreshExchangeRates(db);
			}
			if (delay != TimeSpan.Zero)
			{
				await Task.Delay(delay);
				if (!cancellationToken.IsCancellationRequested)
					await RefreshExchangeRates();
			}

			var timer = new PeriodicTimer(refreshInterval);
			while (await timer.WaitForNextTickAsync(cancellationToken))
				await RefreshExchangeRates();
		}

		private async Task RefreshExchangeRates()
		{
			using (var scope = scopeFactory.CreateScope())
			{
				var db = scope.ServiceProvider.GetRequiredService<MarketplaceDbContext>();
				await RefreshExchangeRates();
			}
		}

		private async Task RefreshExchangeRates(MarketplaceDbContext db)
		{
			var lastRequest = await db.ExchangeInfoRequests.Include(x => x.BaseCurrency).SingleOrDefaultAsync();
			if (lastRequest == null)
			{
				lastRequest = new ExchangeInfoRequest() {
					BaseCurrency = await db.Currencies.Where(x => x.Id == 1).FirstAsync()
				};
				db.ExchangeInfoRequests.Add(lastRequest);
			}

			var exchanges = await db.Exchanges.Include(x => x.Currency).ToListAsync();
			if (exchanges.Count == 0)
			{
				foreach (var currency in await db.Currencies.ToListAsync())
					exchanges.Add(new Exchange() { Currency = currency });
				db.Exchanges.AddRange(exchanges);
			}

			await SetExchangeRatesAsync(exchanges, lastRequest);
			lastRequest.Created = DateTimeOffset.UtcNow;
			await db.SaveChangesAsync();
		}

		private async Task SetExchangeRatesAsync(IEnumerable<Exchange> exchanges, ExchangeInfoRequest exchangeInfo)
		{
			var currencyCodes = exchanges
				.Select(x => new KeyValuePair<string, Exchange>(new RegionInfo(x.Currency.LanguageTag).ISOCurrencySymbol, x))
				.ToDictionary(x => x.Key, x => x.Value);
			string url = string.Format("https://api.apilayer.com/exchangerates_data/latest?base={0}&symbols={1}",
				new RegionInfo(exchangeInfo.BaseCurrency.LanguageTag).ISOCurrencySymbol,
				string.Join(",", currencyCodes.Select(x => x.Key))
			);

			HttpClient client = new HttpClient();
			using (var request = new HttpRequestMessage(HttpMethod.Get, url))
			{
				request.Headers.Add("apikey", exchangeConfig.ApiKey);
				var response = await client.SendAsync(request);
				response.EnsureSuccessStatusCode();
				JsonObject? jsonObject = JsonNode.Parse(await response.Content.ReadAsStringAsync())?.AsObject();
				if (jsonObject == null || jsonObject["success"]?.GetValue<bool>() == false)
					throw new JsonException();
				foreach (var r in jsonObject["rates"]?.Deserialize<Dictionary<string, decimal>>() ?? throw new JsonException())
					currencyCodes[r.Key].Rate = r.Value;
			}
		}
	}
}
