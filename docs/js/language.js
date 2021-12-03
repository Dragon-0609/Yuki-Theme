
function getfromStorage(key, def) {
	let tsf = localStorage.getItem (key);
	if (tsf === null)
		return def;
	else
		return tsf;
}

function getorsetStorage(key, def) {
	let tsf = localStorage.getItem (key);
	if (tsf === null) {
		localStorage.setItem (key, def);
		return def;
	} else
		return tsf;
}

function puttoStorage(key, val) {
	localStorage.setItem (key, val);
}



function getLanguage(){
	let lg = getorsetStorage("lang", "en");
	$('.content').addClass("d-none");
	$('.content_'+lg).removeClass('d-none');
}

function chooseLang(lang) {
	puttoStorage("lang", lang);
	getLanguage();
}

getLanguage();