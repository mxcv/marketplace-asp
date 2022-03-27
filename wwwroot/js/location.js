var counties, selectedCountry;

async function getCountries() {
	let response = await fetch('/api/locations', {
		method: 'GET',
		headers: { 'Accept': 'application/json' }
	});
	if (!response.ok)
		throw new Error(response.status);
	return await response.json();
}

getCountries()
	.then(countries => {
		this.countries = countries;
		for (country of countries)
			$('#country-select').append($('<option>', { text: country.name, value: country.id }));
	})
	.catch(console.log);

$('#country-select').change(function () {
	selectedCountry = countries.find(c => c.id == $(this).val());
	$('#region-select option:not(:first-child)').remove();
	if (selectedCountry)
		for (region of selectedCountry.regions)
			$('#region-select').append($('<option>', { text: region.name, value: region.id }));
	$('#region-select').change();
});
$('#region-select').change(function () {
	let selectedRegion = selectedCountry ? selectedCountry.regions.find(r => r.id == $(this).val()) : undefined;
	$('#city-select option:not(:first-child)').remove();
	if (selectedRegion)
		for (city of selectedRegion.cities)
			$('#city-select').append($('<option>', { text: city.name, value: city.id }));
});