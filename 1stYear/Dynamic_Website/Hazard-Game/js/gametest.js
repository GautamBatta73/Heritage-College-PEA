let dice = new Dice();
let user = new User();
let game = new Game();
const testCases = [
	{
		statement: "Player 1 bets $20 on a main of 7 (Valid).",
		bet: 20,
		main: 7
	},
	{
		statement: "Player 2 bets $30 on a main of 10 (Invalid).",
		bet: 30,
		main: 10
	},
	{
		statement: "Re-Enters: Player 2 bets $30 on a main of 7 (Valid).",
		bet: 30,
		main: 6
	},
	{
		statement: "Player 3 bets $25 on a main of 8 (Valid).",
		bet: 25,
		main: 8
	},
	{
		statement: "Player 4 bets $50 on a main of 2 (Invalid).",
		bet: 50,
		main: 2
	},
	{
		statement: "Re-Enters: Player 4 bets $50 on a main of 5 (Valid).",
		bet: 50,
		main: 5
	},
	{
		statement: "Player 5 bets $100 on a main of 9 (Valid).",
		bet: 100,
		main: 9
	},
	{
		statement: "Player 6 bets $250 on a main of 8 (Invalid).",
		bet: 250,
		main: 8
	},
	{
		statement: "Re-Enters: Player 6 bets $200 on a main of 8 (Valid).",
		bet: 200,
		main: 8
	},
	{
		statement: "Player 7 bets $1 on a main of 9 (Valid).",
		bet: 1,
		main: 9
	},
	{
		statement: "Player 8 bets \"alot\" on a main of 5 (Invalid).",
		bet: "alot",
		main: 5
	},
	{
		statement: "Re-Enters: Player 8 bets $13 on a main of \"6\" (Valid).",
		bet: 13,
		main: "6"
	},
	{
		statement: "Player 8 bets \"I am rich\" on a main of \"21\" (Invalid).",
		bet: "I am rich",
		main: "21"
	},
];

for (let i = 0; i < testCases.length; i++) {
	console.log(`\n${testCases[i].statement}`);

	console.log(`Bet: $${testCases[i].bet}`);
	if (testCases[i].bet < 1) {
		console.error(`${testCases[i].bet} is too low`);
	} else if (isNaN(testCases[i].bet)) {
		console.error(`That is not a number`);
	} else if (testCases[i].bet > user.bankBalance + 100) {
		console.error(`${testCases[i].bet} is too much over your bank's balance`);
	} else {
		game.moneyBet = testCases[i].bet;

		console.log(`Balance: $100`);

		console.log(`Main: ${testCases[i].main}`);
		if (testCases[i].main < 5) {
			console.error(`${testCases[i].main} must be higher than or equal to 5`);
		} else if (testCases[i].main > 9) {
			console.error(`${testCases[i].main} must be lower than or equal to 9`);
		} else if (isNaN(testCases[i].main)) {
			console.error(`That is not a number`);
		} else {
			game.diceBet = testCases[i].main;


			dice.diceRoll();
			console.log(`Rolled: (${dice.rolledDice1}, ${dice.rolledDice2})`);
			game.diceRollCheck();

			console.log(user.winOrLoseBet());
			console.log(`New Bank Balance: $${user.bankBalance}`);
		} //diceBet else
	} //moneyBet else

	dice = new Dice();
	user = new User();
	game = new Game();
} //for(let i = 0; i < testCases.length; i++)