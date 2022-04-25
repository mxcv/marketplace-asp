var dateOptionsMonth = { year: 'numeric', month: 'long' };
var dateOptionsDay = { year: 'numeric', month: 'numeric', day: 'numeric' };
var dateOptionsTime = { month: 'long', day: '2-digit', hour: '2-digit', minute: '2-digit' };

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

$('.time-iso').each(function () {
	let dateOptions;
	if ($(this).hasClass('month'))
		dateOptions = dateOptionsMonth;
	else if ($(this).hasClass('day'))
		dateOptions = dateOptionsDay;
	else
		dateOptions = dateOptionsTime;

	$(this).next().text(new Date($(this).text()).toLocaleString($('html').attr('lang'), dateOptions));
	$(this).remove();
});
