let isDiceRolling = false;
const cad = new Intl.NumberFormat('en-us');
const userInfo = document.querySelector(`#userInfo`);
const error = document.querySelector(`#errorDiv`);
const result = document.querySelector(`#result`);
const paraBal = document.querySelector(`#bal`);
const imgDice1 = document.querySelector(`#rolledDiceImg1`);
const imgDice2 = document.querySelector(`#rolledDiceImg2`);

const quitBtn = document.querySelector(`#quit`);
const notMeBtn = document.createElement(`button`);
const logOutDialog = document.querySelector(`dialog.logOutScreen`);
const quitDialog = document.querySelector(`dialog.quitWindow`);

const gameForm = document.querySelector(`#gameForm`);
const betInput = document.querySelector(`#fldBet`);
const mainInput = document.querySelector(`#fldMain`);
betInput.addEventListener(`blur`, validBet);
mainInput.addEventListener(`blur`, validMain);
quitBtn.addEventListener(`click`, quit);
addEventListener(`error`, () => {
	paraAdd(error, `unexpected`, `An Unexpected Error Occured.`);
	paraAdd(error, `reload`, `Site will restart in 5 seconds.`);
	document.querySelector(`.unexpected`).scrollIntoView();
	setTimeout(function() {
		location.href = `intro.html`;
	}, 5000);
});

let dice = new Dice();
let game = new Game();
let user = new User();

user.firstName = localStorage.getItem(`firstName`);
user.lastName = localStorage.getItem(`lastName`);
user.userName = localStorage.getItem(`userName`);
user.phoneNumber = localStorage.getItem(`phoneNum`);
user.city = localStorage.getItem(`cityName`);
user.emailAddress = localStorage.getItem(`emailAddress`);
user.bankBalance = parseInt(localStorage.getItem(`balance`));

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

function paraRemove(div, paraClass) {
	let className = `.` + paraClass;
	document.querySelectorAll(className).forEach(e => e.remove());
	if (div.children.length < 1)
		div.style.display = `none`;
} //paraRemove(div, paraClass)

function paraAdd(div, paraClass, paraStr) {
	div.style.display = `block`;
	let paragraph = document.createElement(`p`);
	let paraNode = document.createTextNode(paraStr);
	paragraph.appendChild(paraNode);
	paragraph.className = paraClass;
	div.append(paragraph);
} //paraAdd(div, paraClass, paraStr)

function updateBal() {
	localStorage.setItem(`balance`, user.bankBalance);
	paraBal.textContent = `$${cad.format(user.bankBalance)}`;
} //updateBal()

paraAdd(userInfo, `name`, user.fullName());
paraAdd(userInfo, `userName`, user.userName);
paraAdd(userInfo, `phoneNum`, user.phoneNumber);
paraAdd(userInfo, `city`, user.city);
paraAdd(userInfo, `email`, user.emailAddress);
paraAdd(userInfo, `lastLogin`, localStorage.getItem(`lastLogIn`));

notMeBtn.addEventListener(`click`, logOut);
notMeBtn.appendChild(document.createTextNode(`Log Out`));
paraAdd(userInfo, `logOut`, ` `);
document.querySelector(`p.logOut`).appendChild(notMeBtn);

updateBal();

if (!isVal(localStorage.getItem(`lastName`))
	|| !isVal(localStorage.getItem(`firstName`))
	|| !isVal(localStorage.getItem(`userName`))
	|| !isVal(localStorage.getItem(`phoneNum`))
	|| !isVal(localStorage.getItem(`cityName`))
	|| !isVal(localStorage.getItem(`emailAddress`))
	|| isNaN(localStorage.getItem(`balance`))
	|| isNaN(localStorage.getItem(`lastLogin`))) {

	if (!isVal(localStorage.getItem(`lastName`))
		|| !isVal(localStorage.getItem(`firstName`))
		|| !isVal(localStorage.getItem(`userName`))
		|| !isVal(localStorage.getItem(`phoneNum`))
		|| !isVal(localStorage.getItem(`cityName`))
		|| !isVal(localStorage.getItem(`emailAddress`))
		|| isNaN(localStorage.getItem(`balance`))
		|| isNaN(localStorage.getItem(`lastLogin`)))
		paraAdd(error, `error`, `All user info is undefined or null. Please re-create your account.`);
	else {
		if (!isVal(localStorage.getItem(`lastName`)))
			paraAdd(error, `error`, `Last name is undefined or null. Please re-add your last name.`);
		if (!isVal(localStorage.getItem(`firstName`)))
			paraAdd(error, `error`, `First name is undefined or null. Please re-add your first name.`);
		if (!isVal(localStorage.getItem(`userName`)))
			paraAdd(error, `error`, `Username is undefined or null. Please re-add your username.`);
		if (!isVal(localStorage.getItem(`phoneNum`)))
			paraAdd(error, `error`, `Phone number is undefined or null. Please re-add your phone number.`);
		if (!isVal(localStorage.getItem(`cityName`)))
			paraAdd(error, `error`, `City is undefined or null. Please re-add your city.`);
		if (!isVal(localStorage.getItem(`emailAddress`)))
			paraAdd(error, `error`, `Email address is undefined or null. Please re-add your email address.`);
		if (isNaN(localStorage.getItem(`balance`)))
			paraAdd(error, `error`, `Balance is undefined or null. Please re-add your balance.`);
		if (isVal(localStorage.getItem(`lastLogin`)))
			paraAdd(error, `error`, `Last Login is undefined or null. Please login again.`);
	} //else

	paraAdd(error, `reload`, `Site will restart in 5 seconds.`);
	document.querySelector(`.reload`).scrollIntoView();
	setTimeout(function() {
		location.href = `intro.html`;
	}, 5000);
} //large if

function validBet() {
	paraRemove(error, `betError`)
	let valid = true;

	if (isNaN(betInput.value)) {
		paraAdd(error, `betError`, `Bet must be a number.`);
		valid = false;
	} else if (betInput.value.indexOf('.') < 0 && parseInt(betInput.value) >= 1 && parseInt(betInput.value) <= parseInt(localStorage.getItem(`balance`))) {
		valid = true;
		game.moneyBet = parseInt(betInput.value);
	} else if (betInput.value.indexOf('.') > -1) {
		paraAdd(error, `betError`, `Bet must be an integer (No Decimals).`);
		valid = false;
	} else if (parseInt(betInput.value) < 1) {
		paraAdd(error, `betError`, `Bet must be at least 1.`);
		valid = false;
	} else if (parseInt(betInput.value) >= parseInt(localStorage.getItem(`balance`))) {
		paraAdd(error, `betError`, `Bet must be less than or equal to your current balance.`);
		valid = false;
	} else if (betInput.value.length < 1) {
		paraAdd(error, `betError`, `Bet cannot be empty.`);
		valid = false;
	} else {
		paraAdd(error, `betError`, `Bet has an unexpected error.`);
		valid = false;
	} //else

	return valid;
} //validBet()

//stuff
function validMain() {
	paraRemove(error, `mainError`);
	let valid = true;

	if (isNaN(mainInput.value)) {
		paraAdd(error, `mainError`, `Main must be a number.`);
		valid = false;
	} else if (mainInput.value.indexOf('.') < 0 && parseInt(mainInput.value) >= 5 && parseInt(mainInput.value) <= 9) {
		valid = true;
		game.diceBet = parseInt(mainInput.value);
	} else if (mainInput.value.indexOf('.') > -1) {
		paraAdd(error, `mainError`, `Main must be an integer (No Decimals).`);
		valid = false;
	} else if (parseInt(mainInput.value) < 5) {
		paraAdd(error, `mainError`, `Main must be at least 5.`);
		valid = false;
	} else if (parseInt(mainInput.value) > 9) {
		paraAdd(error, `mainError`, `Main must be less than 9.`);
		valid = false;
	} else if (mainInput.value.length < 1) {
		paraAdd(error, `mainError`, `Main cannot be empty.`);
		valid = false;
	} else {
		paraAdd(error, `mainError`, `Main has an unexpected error.`);
		valid = false;
	} //else

	return valid;
} //validMain()

gameForm.onsubmit = function(e) {
	let valid = true;

	if (!validBet())
		valid = false;
	if (!validMain())
		valid = false;

	if (valid)
		roll();


	e.preventDefault();
} //gameForm.onsubmit

let roll = function() {
	if (!isDiceRolling) {
		let paragraph = document.querySelector(`#result>p`);
		paragraph.textContent = ``;

		if (game.isChance)
			game.diceRollChance();
		else {
			dice.diceRoll();
			game.diceRollCheck();
		} //else

		if (game.isChance) {
			paragraph.appendChild(document.createTextNode(`Rolled Chance: (${dice.rolledDice1}, ${dice.rolledDice2})`));
			paragraph.appendChild(document.createElement(`br`));
			paragraph.appendChild(document.createTextNode(`Please Re-Roll`));
		} else {
			paragraph.appendChild(document.createTextNode(`Rolled (${dice.rolledDice1}, ${dice.rolledDice2})`));
			paragraph.append(document.createElement(`br`), document.createTextNode(user.winOrLoseBet()));
		} //else

		imgDice1.setAttribute(`src`, `images/dice${dice.rolledDice1}.png`);
		rollImg(imgDice1);
		imgDice1.setAttribute(`alt`, `Rolled a ${dice.rolledDice1}`);

		imgDice2.setAttribute(`src`, `images/dice${dice.rolledDice2}.png`);
		rollImg(imgDice2);
		imgDice2.setAttribute(`alt`, `Rolled a ${dice.rolledDice2}`);

		result.append(paragraph);
		updateBal();

		if (user.bankBalance < 1)
			quit();
	} //if (isDiceRolling)
} //roll()

function quit() {
	quitDialog.showModal();
	let finalBal = document.querySelector(`span.quitWindow`);
	let returnBtn = document.querySelector(`button.quitWindow`);

	if (parseInt(localStorage.getItem(`balance`)) < 1) {
		finalBal.textContent = `You ran out of Money.`;
		finalBal.append(document.createElement(`br`), document.createTextNode(`You can Try Again Later.`));
	} else
		finalBal.textContent = `Your Final Balance was: $${cad.format(user.bankBalance)}`;

	returnBtn.addEventListener(`click`, () => location.href = `index.html`);
} //quit()

function logOut() {
	logOutDialog.showModal();
	let confirmBtn = document.querySelector(`#confirmBtn`);
	let closeBtn = document.querySelector(`#closeBtn`);

	closeBtn.addEventListener(`click`, () => logOutDialog.close());

	confirmBtn.addEventListener(`click`, () => {
		localStorage.removeItem(`balance`);
		localStorage.removeItem(`emailAddress`);
		localStorage.removeItem(`cityName`);
		localStorage.removeItem(`phoneNum`);
		localStorage.removeItem(`userName`);
		localStorage.removeItem(`lastName`);
		localStorage.removeItem(`firstName`);
		localStorage.removeItem(`lastLogin`);
		location.href = `index.html`;
	});
} //logOut()

function rollImg(img) {
	isDiceRolling = true;
	let num = 130;
	let animation = setInterval(() => {
		img.style.rotate = `${num}deg`;
		num--;
		if (num === 0) {
			img.style.rotate = `0deg`;
			clearInterval(animation);
			isDiceRolling = false;
		}
	}, 1);
} //rollImg(img)








