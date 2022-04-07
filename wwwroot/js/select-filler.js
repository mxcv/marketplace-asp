if ($('#country-select').length)
	getCountries()
		.then(function (countries) {
			for (country of countries)
				$('#country-select').append($('<option>', { text: country.name, value: country.id }));

			let selectedCountry, selectedRegion;
			let defaultRegion = $('#region-select').val(),
				defaultCity = $('#city-select').val();

			$('#region-select').prop('disabled', true);
			$('#city-select').prop('disabled', true);

			$('#country-select').change(function () {
				selectedCountry = countries.find(c => c.id == $(this).val());
				updateSubLocations('#region-select', selectedCountry, 'regions', defaultRegion);
				defaultRegion = null;
			});
			$('#region-select').change(function () {
				selectedRegion = selectedCountry?.regions.find(r => r.id == $(this).val());
				updateSubLocations('#city-select', selectedRegion, 'cities', defaultCity);
				defaultCity = null;
			});

			setDefaultValue('#country-select', $('#country-select').val());
			$('#country-select').change();
		})
		.catch(console.log);

if ($('#category-select').length)
	getCategories()
		.then(function (categories) {
			for (category of categories)
				$('#category-select').append($('<option>', { text: category.title, value: category.id }));
			setDefaultValue('#category-select', $('#category-select').val());
		})
		.catch(console.log);

if ($('#currency-select').length)
	getCurrencies()
		.then(function (currencies) {
			for (currency of currencies)
				$('#currency-select').append($('<option>', { text: currency.symbol, value: currency.id }));
			setDefaultValue('#currency-select', $('#currency-select').val());
		})
		.catch(console.log);

function updateSubLocations(select, selectedLocation, subLocationsName, defaultSubLocation) {
	$(select + ' option:not(:first-child)').remove();
	if (selectedLocation)
		for (region of selectedLocation[subLocationsName])
			$(select).append($('<option>', { text: region.name, value: region.id }));
	setDefaultValue(select, defaultSubLocation);
	$(select).prop('disabled', !selectedLocation).change();
}
function setDefaultValue(select, value) {
	if (value) {
		$(select + ' option').first().val('');
		$(select).val(value);
	}
}
