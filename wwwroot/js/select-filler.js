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
		.then(categories => fillSelect(categories, '#category-select', 'title'))
		.catch(console.log);

if ($('#currency-select').length)
	getCurrencies()
		.then(currencies => fillSelect(currencies, '#currency-select', 'symbol'))
		.catch(console.log);

function updateSubLocations(select, selectedLocation, subLocationsName, defaultSubLocation) {
	$(select + ' option:not(:first-child)').remove();
	if (selectedLocation)
		for (subLocation of selectedLocation[subLocationsName])
			$(select).append($('<option>', { text: subLocation.name, value: subLocation.id }));
	setDefaultValue(select, defaultSubLocation);
	$(select).prop('disabled', !selectedLocation).change();
}

function fillSelect(items, select, textName) {
	for (item of items)
		$(select).append($('<option>', { text: item[textName], value: item.id }));

	if ($(select + ' option').length == items.length)
		$(select).val($(select + ' option').first().val());
	else
		setDefaultValue(select, $(select).val());
}

function setDefaultValue(select, value) {
	if (value) {
		$(select + ' option').first().val('');
		$(select).val(value);
	}
}
