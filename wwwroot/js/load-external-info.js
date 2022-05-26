var countries, categories, currencies;

if (localStorage.getItem('lang') != $('html').attr('lang')) {
	localStorage.clear();
	localStorage.setItem('lang', $('html').attr('lang'));
}

async function getCountries() {
	return await getValues('countries', 'locations');
}
async function getCategories() {
	return await getValues('categories', 'categories');
}
async function getCurrencies() {
	return await getValues('currencies', 'currencies');
}

async function getValues(variable, path) {
	if (!window[variable]) {
		let valuesJSON = localStorage.getItem(variable);
		if (!valuesJSON) {
			let response = await fetch(`${basePath}/api/${path}`);
			if (!response.ok)
				throw new Error(response.status);
			valuesJSON = await response.text();
			localStorage.setItem(variable, valuesJSON);
		}
		variable = JSON.parse(valuesJSON);
	}
	return variable;
}
