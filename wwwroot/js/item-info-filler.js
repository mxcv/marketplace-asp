getCountries()
	.then(function (countries) {
		$('.city').each(function () {
			let cityId = +$(this).text();
			for (country of countries)
				for (region of country.regions)
					for (city of region.cities)
						if (city.id == cityId) {
							$(this).text(city.name);
							return;
						}
		});
	})
	.catch(console.log);
