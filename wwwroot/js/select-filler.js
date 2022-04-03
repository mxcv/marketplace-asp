if ($('#country-select').length)
	getCountries()
		.then(function (countries) {
			for (country of countries)
				$('#country-select').append($('<option>', { text: country.name, value: country.id }));

			let selectedCountry, selectedRegion;
			$('#country-select').change(function () {
				selectedCountry = countries.find(c => c.id == $(this).val());
				updateSubLocations('#region-select', selectedCountry, 'regions');
			});
			$('#region-select').change(function () {
				selectedRegion = selectedCountry?.regions.find(r => r.id == $(this).val());
				updateSubLocations('#city-select', selectedRegion, 'cities');
			});
		})
		.catch(console.log);

if ($('#category-select').length)
	getCategories()
		.then(function (categories) {
			for (category of categories)
				$('#category-select').append($('<option>', { text: category.title, value: category.id }));
		})
		.catch(console.log);

if ($('#currency-select').length)
	getCurrencies()
		.then(function (currencies) {
			for (currency of currencies)
				$('#currency-select').append($('<option>', { text: currency.symbol, value: currency.id }));
		})
		.catch(console.log);

function updateSubLocations(select, selectedLocation, subLocationsName) {
	$(select + ' option:not(:first-child)').remove();
	if (selectedLocation)
		for (region of selectedLocation[subLocationsName])
			$(select).append($('<option>', { text: region.name, value: region.id }));
	$(select).change();
}
