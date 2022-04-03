var countries;

async function getCountries() {
	return await getValues('countries', 'locations');
}

async function getValues(variable, path) {
	if (!window[variable]) {
		let response = await fetch('/api/' + path);
		if (!response.ok)
			throw new Error(response.status);
		variable = await response.json();
    }
	return variable;
}
