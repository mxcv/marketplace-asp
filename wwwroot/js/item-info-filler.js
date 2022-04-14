getCountries()
	.then(function (countries) {
		$('.city-id').each(function () {
			let cityId = +$(this).text();
			for (country of countries)
				for (region of country.regions)
					for (city of region.cities)
						if (city.id == cityId) {
							$(this).next().text($(this).hasClass('long') ? `${country.name}, ${region.name}, ${city.name}` : city.name);
							$(this).remove();
							return;
						}
		});
	})
	.catch(console.log);

getCategories()
	.then(function (categories) {
		$('.category-id').each(function () {
			$(this).next().text(categories.find(c => c.id == $(this).text()).title);
			$(this).remove();
		});
	})
	.catch(console.log);

getCurrencies()
	.then(function (currencies) {
		$('.currency-id').each(function () {
			let currency = currencies.find(c => c.id == $(this).text());
			let formatter = new Intl.NumberFormat(currency.languageTag, {
				style: 'currency',
				currency: currency.code,
			});

			$(this).next().text(formatter.format(parseFloat($(this).next().text())));
			$(this).remove();
		});
	})
	.catch(console.log);

var dateOptionsAccurate = { month: 'long', day: '2-digit', hour: '2-digit', minute: '2-digit' };
var dateOptionsApproximate= { year: 'numeric', month: 'long' };
$('.time-iso').each(function () {
	$(this).next().text(new Date($(this).text()).toLocaleString(
		$('html').attr('lang'),
		$(this).hasClass('accurate') ? dateOptionsAccurate : dateOptionsApproximate)
	);
	$(this).remove();
});
