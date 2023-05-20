$(function() {
	const introForm = $(`#introForm`);
	const fName = $(`#fldFName`);
	const lName = $(`#fldLName`);
	const uName = $(`#fldUName`);
	const pNum = $(`#fldPNum`);
	const cityInput = $(`#fldCity`);
	const emailInput = $(`#fldEmail`);
	const balInput = $(`#fldBalance`);
	const dayNames = [`Sunday`, `Monday`, `Tuesday`, `Wednesday`, `Thursday`, `Friday`, `Saturday`];
	const monthNames = [`January`, `February`, `March`, `April`, `May`, `June`, `July`, `August`, `September`, `October`, `November`, `December`];

	function isVal(val) {
		let valid = true;

		if (val === null || val === `null`)
			valid = false;
		if (val === ``)
			valid = false;
		if (val === undefined || val === `undefined`)
			valid = false;

		return valid;
	} //isVal(val)

	jQuery.validator.addMethod("divisibleBy5", function(value, element) {
		if (value % 5 === 0)
			return true;
		else
			return false;
	}, 'Balance must be divisible by 5.');

	introForm.validate({
		onfocusout: function(element) {
			this.element(element);
		},

		rules: {
			fldFName: {
				required: true,
				maxlength: 30,
				pattern: /^[a-z]([0-9]|[a-z]|\s|`|-)*([0-9]|[a-z])$/i
			},
			fldLName: {
				required: true,
				maxlength: 40,
				pattern: /^[a-z]([0-9]|[a-z]|\s|`|-)*([0-9]|[a-z])$/i
			},
			fldUName: {
				required: true,
				maxlength: 6,
				minlength: 6,
				pattern: /^[a-z][(@|#|$|%|&)]{3}[A-Z]\d$/
			},
			fldPNum: {
				required: true,
				pattern: /^1-\d\d\d-\d\d\d-\d\d\d\d$/
			},
			fldCity: {
				required: true,
				maxlength: 55,
				pattern: /^[a-z]*$/i
			},
			fldEmail: {
				required: true,
				pattern: /^([a-z]|[0-9]|[\-]|[\.])+@([a-z]|[0-9]|[\.])+[\.](com|org)$/i
			},
			fldBalance: {
				required: true,
				digits: true,
				min: 5,
				max: 5000,
				divisibleBy5: true
			}
		},
		messages: {
			fldFName: {
				required: `First Name must be between 1 and 30 characters.`,
				maxlength: `First Name must be between 1 and 30 characters.`,
				pattern: `First Name can only start with letters, end with letters/numbers, and contain numbers/letters/single-quotation marks/hyphens.`
			},
			fldLName: {
				required: `Last Name must be between 1 and 40 characters.`,
				maxlength: `Last Name must be between 1 and 40 characters.`,
				pattern: `Last Name can only start with letters, end with letters/numbers, and contain numbers/letters/single-quotation marks/hyphens.`
			},
			fldUName: {
				required: `Username must be 6 characters long.`,
				maxlength: `Username must be 6 characters long.`,
				minlength: `Username must be 6 characters long.`,
				pattern: `Username can only start with  a lowercase letter, then it must have any 3 characters of the following (@, #, $, %, &), then an uppercase letter, and finally a digit (0-9).`
			},
			fldPNum: {
				required: `Phone Number must be in the following Format (1-###-###-###).`,
				pattern: `Phone Number must be in the following Format (1-###-###-###).`
			},
			fldCity: {
				required: `City must be between 1 and 55 characters.`,
				maxlength: `City must be between 1 and 55 characters.`,
				pattern: `City can only contain alphabetical letters.`
			},
			fldEmail: {
				required: `Email must be have any amount of letters, numbers, dashes, or periods, then followed by an @ and any amount of letters, numbers, or periods. Then you must put \`.com\` or \`.org\` (namevalue@domain.[com/org]).`,
				pattern: `Email must be have any amount of letters, numbers, dashes, or periods, then followed by an @ and any amount of letters, numbers, or periods. Then you must put \`.com\` or \`.org\` (namevalue@domain.[com/org]).`
			},
			fldBalance: {
				digits: `Balance must be a number.`,
				required: `Balance must be between 5 and 5000.`,
				min: `Balance must be between 5 and 5000.`,
				max: `Balance must be between 5 and 5000.`
			}
		},
		errorLabelContainer: `#errorDiv`
	});

	$(window).on(`load`, function() {
		let valid = true;

		if (!isVal(localStorage.getItem(`lastName`)))
			valid = false;
		if (!isVal(localStorage.getItem(`firstName`)))
			valid = false;
		if (!isVal(localStorage.getItem(`userName`)))
			valid = false;
		if (!isVal(localStorage.getItem(`phoneNum`)))
			valid = false;
		if (!isVal(localStorage.getItem(`cityName`)))
			valid = false;
		if (!isVal(localStorage.getItem(`emailAddress`)))
			valid = false;
		if (isNaN(localStorage.getItem(`balance`)) || localStorage.getItem(`balance`) < 1)
			valid = false;

		if (valid) {
			localStorage.setItem(`lastLogIn`, formattedDate());
			location.href = `game.html`;
		} //if (valid)
		else
			invalidForm();
	}); //$(window).on(`load)

	function validForm() {
		localStorage.setItem(`firstName`, fName.val());
		console.log(fName.val());
		localStorage.setItem(`lastName`, lName.val());
		localStorage.setItem(`userName`, uName.val());
		localStorage.setItem(`phoneNum`, pNum.val());
		localStorage.setItem(`cityName`, cityInput.val());
		localStorage.setItem(`emailAddress`, emailInput.val());
		localStorage.setItem(`balance`, balInput.val());
		localStorage.setItem(`lastLogIn`, formattedDate());
	} //validForm()

	function invalidForm() {
		localStorage.removeItem(`firstName`);
		localStorage.removeItem(`lastName`);
		localStorage.removeItem(`userName`);
		localStorage.removeItem(`phoneNum`);
		localStorage.removeItem(`cityName`);
		localStorage.removeItem(`emailAddress`);
		localStorage.removeItem(`balance`);
		localStorage.removeItem(`lastLogIn`);
	} //invalidForm()

	introForm.on(`submit`, function() {
		if (introForm.valid())
			validForm();
		else
			invlidForm();

		return introForm.valid();
	});

	function formattedDate() {
		let today = new Date();
		let formatDate = ``;
		let formatTime = ``;

		formatDate = `${dayNames[today.getDay()]}, ${monthNames[today.getMonth()]} ${today.getDate()}, ${today.getFullYear()}`;

		let hours = today.getHours();
		let amPM = `am`;
		if (hours === 0) {
			hours = 12;
			amPM = `am`;
		}
		else if (hours > 12) {
			hours -= 12;
			amPM = `pm`;
		}

		let minutes = today.getMinutes() + "";
		if (minutes < 10)
			minutes = "0" + minutes;

		formatTime = `${hours}:${minutes} ${amPM}`;

		return `${formatDate} at ${formatTime}`;
	} //formattedDate()
});