class Dice {
	#dice1;
	#dice2;
	#rolledDice1;
	#rolledDice2;
	#rolledDiceTotal;
	#mainRoll;
	#chanceDice1;
	#chanceDice2;

	constructor() {
		this.#dice1 = 0;
		this.#dice2 = 0;
		this.#rolledDice1 = 0;
		this.#rolledDice2 = 0;
		this.#chanceDice1 = 0;
		this.#chanceDice2 = 0;
		this.#rolledDiceTotal = 0;
	} //constructor()

	get rolledDiceTotal() {
		return (this.#rolledDice1 + this.rolledDice2);
	} //get rolledDiceTotal()

	get rolledDice1() {
		return this.#rolledDice1;
	} //get rolledDice1()

	get rolledDice2() {
		return this.#rolledDice2;
	} //get rolledDice2()

	get chanceDice1() {
		return this.#chanceDice1;
	} //get chanceDice1()

	get chanceDice2() {
		return this.#chanceDice2;
	} //get chanceDice2()

	set chanceDice1(diceNum) {
		this.#chanceDice1 = diceNum;
	} //set chanceDice1(diceNum)

	set chanceDice2(diceNum) {
		this.#chanceDice2 = diceNum;
	} //set chanceDice2(diceNum)

	diceRoll() {
		let roll1 = (Math.random() * 5) + 1;
		let roll2 = (Math.random() * 5) + 1;
		this.#rolledDice1 = Number(roll1.toFixed(0));
		this.#rolledDice2 = Number(roll2.toFixed(0));
	} //diceRoll()

}// Dice class

class User {
	#firstName;
	#lastName;
	#userName;
	#phoneNumber;
	#city;
	#emailAddress;
	#bankBalance;

	constructor() {
		this.#firstName = `Unknown`;
		this.#lastName = `Unknown`;
		this.#userName = `Unknown`;
		this.#phoneNumber = 'Unknown';
		this.#city = `Unknown`;
		this.#emailAddress = `Unknown`;
		this.#bankBalance = 100;
	} //constructor()

	fullName() {
		return `${this.#firstName} ${this.#lastName}`;
	} //fullName()

	get userName() {
		return this.#userName;
	} //get username() 

	get phoneNumber() {
		return this.#phoneNumber;
	} //get phoneNumber()

	get city() {
		return this.#city;
	} //get city()

	get emailAddress() {
		return this.#emailAddress;
	} //emailAddress()

	set firstName(firstName) {
		this.#firstName = firstName;
	} //set firsttName() 

	set lastName(lastName) {
		this.#lastName = lastName;
	} //set lastName() 

	set userName(userName) {
		this.#userName = userName;
	} //set userName() 

	set phoneNumber(phoneNumber) {
		this.#phoneNumber = phoneNumber;
	} //set phoneNumber()

	set city(city) {
		this.#city = city;
	} //set city()

	set emailAddress(emailAddress) {
		this.#emailAddress = emailAddress;
	} //emailAddress()

	get bankBalance() {
		return this.#bankBalance;
	} //get bankBalance()

	set bankBalance(bankBalance) {
		this.#bankBalance = bankBalance;
	} //get bankBalance(bankBalance)

	winOrLoseBet() {
		let oldBalance = this.#bankBalance;
		let newBal = 0;
		if (game.afterRoll === `win`) {
			this.#bankBalance += game.moneyBet;
			newBal = this.#bankBalance - oldBalance;
		} else {
			this.#bankBalance -= game.moneyBet;
			newBal = -1 * (this.#bankBalance - oldBalance);
		} //else
		return (`You ${game.afterRoll} $${cad.format(newBal)}`);
	} //winOrLoseBet()
}// User class

class Game {
	#moneyBet;
	#diceBet;
	#afterRoll;
	#isChance;

	constructor() {
		this.#moneyBet = 0;
		this.#diceBet = 0;
		this.#afterRoll = ``;
		this.#isChance = false;
	} //constructor()

	get moneyBet() {
		return this.#moneyBet;
	} //get monetBet()

	get diceBet() {
		return this.#diceBet;
	} //get diceBet()

	get afterRoll() {
		return this.#afterRoll;
	} //get afterRoll()

	get isChance() {
		return this.#isChance;
	} //get isChance()

	set moneyBet(bet) {
		this.#moneyBet = bet;
	} //set moneyBet(bet)

	set diceBet(bet) {
		this.#diceBet = bet;
	} //set diceBet(bet)

	diceRollCheck() {
		if (this.diceBet === dice.rolledDiceTotal) {
			this.#afterRoll = `win`;
		} else if (dice.rolledDiceTotal === 2 || dice.rolledDiceTotal === 3) {
			this.#afterRoll = `lost`;
		} else if (dice.rolledDiceTotal === 11 || dice.rolledDiceTotal === 12) {
			if (this.diceBet === 5 || this.diceBet === 9) {
				this.#afterRoll = `lost`;
			} else if (dice.rolledDiceTotal == 11) {
				if (this.diceBet === 7) {
					this.#afterRoll = `win`;
				} else {
					this.#afterRoll = `lost`;
				} //else
			} else {
				if (this.diceBet === 6 || this.diceBet === 8) {
					this.#afterRoll = `win`;
				} else {
					this.#afterRoll = `lost`;
				} //else
			} //else
		} else
			this.#isChance = true;
	} //diceRoll()

	diceRollChance() {
		this.#afterRoll = `chance`;

		dice.chanceDice1 = dice.rolledDice1;
		dice.chanceDice2 = dice.rolledDice2;
		dice.diceRoll();

		if (this.diceBet === dice.rolledDiceTotal) {
			this.#afterRoll = `lost`;
			this.#isChance = false;
		} else {
			if (!(dice.rolledDiceTotal === 2 || dice.rolledDiceTotal === 3)) {
				if (!(dice.rolledDiceTotal === 11 || dice.rolledDiceTotal === 12)) {
					this.#afterRoll = `win`;
					this.#isChance = false;
				} //if (!(dice.rolledDiceTotal === 11 || dice.rolledDiceTotal === 12)
			} //if (!(dice.rolledDiceTotal === 2 || dice.rolledDiceTotal === 3))
		} //else
	} //diceRollChance()

}// Game class