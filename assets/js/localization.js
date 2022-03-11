let languages = ['en', 'ru'];
let opposite = {
	'en': 'ru',
	'ru': 'en',
};
let lang_style;

function Localize(){
	let lang = getfromStorage("language", "en");
	let oppo = opposite[lang];
	lang_style = document.createElement ('style');
	lang_style.type = 'text/css';
	lang_style.innerHTML = `
		.content_${lang} {
			display: block;
		}
		
		.content_${oppo}, .content_inline_${oppo} {
			display: none;
		}
		
		.content_inline_${lang} {
			display: inline;
		}
	`;
	document.getElementsByTagName ('head')[0].appendChild (lang_style);
}

function changeLanguage(lang) {
	let langu = lang;
	let oppo = opposite[lang];
	lang_style.innerHTML = `
		.content_${langu} {
			display: block;
		}
		
		.content_${oppo}, .content_inline_${oppo} {
			display: none;
		}
		
		.content_inline_${langu} {
			display: inline;
		}
	`;
	puttoStorage("language", lang);
}


