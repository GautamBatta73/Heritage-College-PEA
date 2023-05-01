const alphabet = [`a`, `b`, `c`, `d`, `e`, `f`, `g`, `h`, `i`, `j`, `k`, `l`, `m`, `n`, `o`, `p`, `q`, `r`, `s`, `t`, `u`, `v`, `w`, `x`, `y`, `z`];
const specialChar = [`.`, ` `, `!`, `?`, `(`, `)`, `,`, `\'`];
const fldShift = document.querySelector(`#fldShift`);
const fldText = document.querySelector(`#fldText`);
const ans = document.querySelector(`#result`);
const reverseBtn = document.querySelector(`#reverseBtn`);

fldText.addEventListener(`input`, encrypt);
fldShift.addEventListener(`input`, encrypt);
reverseBtn.addEventListener(`click`, reverse);

function encrypt() {
	let valid = true;
	ans.textContent = ``;
	if (ans.className === `error`)
		ans.classList.remove(`error`);

	if (fldText.value.length < 1) {
		ans.textContent = `err: Enter some text`;
		ans.className = `error`;
	} else if (fldShift.value.length < 1) {
		ans.textContent = `err: Enter a shift`;
		ans.className = `error`;
	} else {
		let arr = (fldText.value.toLowerCase()).split(``);
		let y = parseInt(fldShift.value);
		let x;
		let result;

		for (let i = 0; i < arr.length; i++) {
			if (alphabet.includes(arr[i]) && valid) {
				x = (alphabet.indexOf(arr[i]));
				result = ((x + y) % 26);

				if (result < 0)
					result = 26 - (result * -1);

				ans.append(`${alphabet[result]}`);
				valid = true;
			} else if ((specialChar.includes(arr[i]) && valid)) {
				ans.append(`${arr[i]}`);
				valid = true;
			} else if ((!isNaN(arr[i])) && valid) {
				x = parseInt(arr[i]);
				result = (x + y);

				ans.append(`${result}`);
				valid = true;
			} else {
				ans.textContent = `err: Not a valid character`;
				ans.className = `error`;
				valid = false;
			} //else
		} //for (let i = 0; i < arr.length; i++)
	} //else
} //encrypt()

function reverse() {	
	if (fldText.value.length < 1) {
		ans.textContent = `err: Enter some text`;
		ans.className = `error`;
	} else if (fldShift.value.length < 1) {
		ans.textContent = `err: Enter a shift`;
		ans.className = `error`;
	} else {
		let tempText = fldText.value;
		let tempAns = ans.value;
		
		fldText.value = ``;
		ans.textContent = ``;
		
		fldText.value = `${tempAns}`;
		ans.textContent = `${tempText}`;
		
		fldShift.value = `${parseInt(fldShift.value) * -1}`;
		
		encrypt();
	} //else
} //reverse