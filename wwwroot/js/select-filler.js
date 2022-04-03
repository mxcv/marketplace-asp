if ($('#country-select').length)
	getCountries()
		.then(function (countries) {
			let selectedCountry, selectedRegion;

			for (country of countries)
				$('#country-select').append($('<option>', { text: country.name, value: country.id }));

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

function updateSubLocations(select, selectedLocation, subLocationsName) {
	$(select + ' option:not(:first-child)').remove();
	if (selectedLocation)
		for (region of selectedLocation[subLocationsName])
			$(select).append($('<option>', { text: region.name, value: region.id }));
	$(select).change();
}
