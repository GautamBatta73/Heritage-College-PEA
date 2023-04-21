/**
 * 
 */
package sudoku;

/**
 *Description: This class contains the actual game of Sudoku.
 *	Sudoku is a logic based, number placement puzzle.
 * 	The board is a 9 x 9 grid with 81 squares. Some of the 
 * 	squares are already filled in at the start of the puzzle.
 * 	The player must fill in the rest of the squares by observing 
 *  these 3 rules:
 *  
 *  You must place the numbers 1 – 9 in each row without repeating a number.
 *  
 *  You must place the numbers 1 – 9 in each column without repeating a number.
 *  
 *  You must place the numbers 1 – 9 in each of the marked 3 x 3 boxes without repeating
 *  a number.
 *  
 * @author Gautam Batta
 */

import java.util.Scanner;
import java.io.IOException;
import java.io.FileNotFoundException;

public class SudokuInterface {
	private SudokuGame sudoku;
	private Scanner input;

	public SudokuInterface(String file)
			throws IOException, FileNotFoundException, Exception {
		sudoku = new SudokuGame(file);
		input = new Scanner(System.in);
	} //SudokuInterface() throws IOException, FileNotFoundException, Exception

	public static void main(String[] args) {
		Scanner temp = new Scanner(System.in);
		SudokuInterface sudokuCli = null;

		boolean play = true;
		do {
			System.out.println("\nWelcome to Gautam's Java Sudoku!");
			System.out
					.println("Type Q to exit the game or H to get an introduction.\n");

			boolean fileValid;
			do {
				fileValid = true;
				System.out.print("Enter The File Name for The Sudoku Game: ");
				String file = temp.nextLine();

				if (file.isEmpty())
					System.out.println(
							"As no filename was entered, the default will be\n sudoku.txt");
				if (file.equalsIgnoreCase("q"))
					exitGame();
				else
					if (file.equalsIgnoreCase("h")) {
						System.out.println();
						gameHelp();
						fileValid = false;
					}
					else {
						try {
							sudokuCli = new SudokuInterface(file);
						} //try
						catch (FileNotFoundException x) {
							System.err.println(
									"File Could Not Be Found.\n Please Make Sure It is In The Corect Directory.\n");
							fileValid = false;
						} //catch (FileNotFoundException)
						catch (IOException y) {
							System.err.println(y.getMessage() + "\n");
							fileValid = false;
						} //catch (IOException)
						catch (Exception z) {
							System.err.println(z.getMessage() + "\n");
							fileValid = false;
						} //catch (Exception)
					} //else
			} while (!fileValid);

			boolean won = false;
			do {
				if (sudokuCli.gameWon())
					won = true;
				else
					sudokuCli.makeMove();
			} while (!won);

			System.out.println();
			sudokuCli.displayBoard();
			System.out.println("Congratulations, You Won!");

			System.out.print("\nPlay Again? (yes/no): ");
			String playAgain = sudokuCli.input.next();

			if (playAgain.equalsIgnoreCase("y") || playAgain.equalsIgnoreCase("yes"))
				play = true;
			else
				play = false;
		} while (play);

		exitGame();
		temp.close();
	} //main(String[])

	public void undoMove() {
		if (sudoku.getLastRow() > 0 && sudoku.getLastColumn() > 0) {
			sudoku.undoMove();
			System.out.println("Un-done!\n");
		} //if (sudoku.getLastRow() > 0 && sudoku.getLastColumn() > 0)
		else
			System.err.println("You Must Make a Move, Before You Can Undo It.\n");
	} //undoMove()

	public void saveGame() {
		boolean saved = true;
		try {
			sudoku.saveGame();
		} //try
		catch (FileNotFoundException x) {
			System.err.println("The original file could not be found.");
			saved = false;
		} //catch (FileNotFoundException)
		catch (IOException y) {
			System.err.println("An Error Occured while saving.\n Please Try Agaín.");
			saved = false;
		} //catch (IOException)
		catch (Exception z) {
			System.err.println("An Unexpected Error Occured.\n Please Try Agaín.\n");
			saved = false;
		} //catch (Exception)

		if (saved)
			System.out.println("Saved!\n");
	} //saveGame()

	public static void exitGame() {
		System.out.println("GoodBye!");
		System.exit(0);
	} //exitGame()

	private void makeMove() {
		System.out.println(
				"\nType Q at any time to exit the game, S to save the game, H to get an introduction, or U to undo your last move.\n");
		displayBoard();

		int row = 0;
		int col = 0;
		int val = 0;

		boolean broke = false;
		boolean valid = true;
		do {
			valid = true;
			System.out.print("\nEnter a column(x-value) for the board: ");
			String temp = input.next();

			if (temp.equalsIgnoreCase("q")) {
				exitGame();
			} //if (temp.equalsIgnoreCase("q"))
			else
				if (temp.equalsIgnoreCase("s")) {
					saveGame();
					broke = true;
				} //if (temp.equalsIgnoreCase("s"))
				else
					if (temp.equalsIgnoreCase("u")) {
						undoMove();
						broke = true;
					} //if (temp.equalsIgnoreCase("u"))
					else
						if (temp.equalsIgnoreCase("h")) {
							System.out.println();
							gameHelp();
							broke = true;
						} //if (temp.equalsIgnoreCase("h"))
						else {
							try {
								col = Integer.parseInt(temp);
							} //try
							catch (NumberFormatException a) {
								System.err
										.println("Column Is Not an Integer.\n Please Try Agaín.\n");
								valid = false;
							} //catch (Exception)
							catch (Exception b) {
								System.err.println(
										"An Unexpected Error Occured.\n Please Try Agaín.\n");
								valid = false;
							} //catch (Exception)
						} //else
		} while (!valid);

		if (!broke) {
			do {
				valid = true;
				System.out.print("Enter a row(y-value) for the board: ");
				String temp = input.next();

				if (temp.equalsIgnoreCase("q")) {
					exitGame();
				} //if (temp.equalsIgnoreCase("q"))
				else
					if (temp.equalsIgnoreCase("s")) {
						saveGame();
						broke = true;
					} //if (temp.equalsIgnoreCase("s"))
					else
						if (temp.equalsIgnoreCase("u")) {
							undoMove();
							broke = true;
						} //if (temp.equalsIgnoreCase("u"))
						else
							if (temp.equalsIgnoreCase("h")) {
								System.out.println();
								gameHelp();
								broke = true;
							} //if (temp.equalsIgnoreCase("h"))
							else {
								try {
									row = Integer.parseInt(temp);
								} //try
								catch (NumberFormatException a) {
									System.err
											.println("Row Is Not an Integer.\n Please Try Agaín.\n");
									valid = false;
								} //catch (Exception)
								catch (Exception b) {
									System.err.println(
											"An Unexpected Error Occured.\n Please Try Agaín.\n");
									valid = false;
								} //catch (Exception)
							} //else
			} while (!valid);
		} //if (!broke)

		if (!broke) {
			do {
				valid = true;
				System.out.print(
						"Enter the number you want to put in " + col + ", " + row + ": ");
				String temp = input.next();

				if (temp.equalsIgnoreCase("q")) {
					exitGame();
				} //if (temp.equalsIgnoreCase("q"))
				else
					if (temp.equalsIgnoreCase("s")) {
						saveGame();
						broke = true;
					} //if (temp.equalsIgnoreCase("s"))
					else
						if (temp.equalsIgnoreCase("u")) {
							undoMove();
							broke = true;
						} //if (temp.equalsIgnoreCase("u"))
						else
							if (temp.equalsIgnoreCase("h")) {
								System.out.println();
								gameHelp();
								broke = true;
							} //if (temp.equalsIgnoreCase("h"))
							else {
								try {
									val = Integer.parseInt(temp);
								} //try
								catch (NumberFormatException a) {
									System.err.println(
											"Value Is Not an Integer.\n Please Try Agaín.\n");
									valid = false;
								} //catch (Exception)
								catch (Exception b) {
									System.err.println(
											"An Unexpected Error Occured.\n Please Try Agaín.\n");
									valid = false;
								} //catch (Exception)
							} //else
			} while (!valid);
		} //if (!broke)

		if (!broke) {
			valid = true;
			try {
				sudoku.addToBoard(row, col, val);
			} //try
			catch (Exception e) {
				System.err.println(e.getMessage() + "\n");
				valid = false;
			} //catch (Exception)

			if (valid) {
				sudoku.setLastRow(row);
				sudoku.setLastColumn(col);
			} //if (valid)
		} //if (!broke)

	} //makeMove()

	private static void gameHelp() {
		System.out.println("Sudoku is a logic based, number placement puzzle.\n"
				+ "The board is a 9 x 9 grid with 81 squares. Some of the squares are already filled in at the start of the puzzle.\n"
				+ "The player must fill in the rest of the squares by observing these 3 rules: \n"
				+ "\t- You must place the numbers 1 – 9 in each row without repeating a number.\n"
				+ "\t- You must place the numbers 1 – 9 in each column without repeating a number.\n"
				+ "\t- You must place the numbers 1 – 9 in each of the marked 3 x 3 boxes without repeating a number.\n");
	} //gameHelp()

	private boolean gameWon() {
		boolean won = true;

		for (int row = 1; row < sudoku.getSudokuBoard().length; row++) {
			for (int col = 1; col < sudoku.getSudokuBoard()[row].length; col++) {
				if (sudoku.getSudokuBoard()[row][col] == -1)
					won = false;
			} //for (int col = 1; col < sudoku.getSudokuBoard()[row].length; col++)
		} //for (int row = 1; row < sudoku.getSudokuBoard().length; row++)

		return won;
	} //gameHelp()

	private void displayBoard() {
		for (int row = 1; row < sudoku.getSudokuBoard().length; row++) {
			boolean rowReset = true;
			for (int col = 1; col < sudoku.getSudokuBoard()[row].length; col++) {

				if (row % 3 == 1 && rowReset) {
					for (int i = 1; i < sudoku.getSudokuBoard()[row].length + 16; i++) {
						System.out.print("-");
						if (i == sudoku.getSudokuBoard()[row].length + 15)
							System.out.println();
					} //for (int i = 1; i < sudoku.getSudokuBoard()[row].length + 13; i++) 
					rowReset = false;
				} //if (row % 3 == 1 && rowReset)

				if (col % 3 == 1)
					System.out.print("| ");

				if (sudoku.getSudokuBoard()[row][col] == -1)
					System.out.print("* ");
				else
					System.out.print(sudoku.getSudokuBoard()[row][col] + " ");

			} //for (int col = 1; col < sudoku.getSudokuBoard()[row].length; col++)
			System.out.println("|");

			if (row == sudoku.getSudokuBoard().length - 1) {
				for (int i = 1; i < sudoku.getSudokuBoard()[row].length + 16; i++) {
					System.out.print("-");
					if (i == sudoku.getSudokuBoard()[row].length + 15)
						System.out.println();
				} //for (int i = 1; i < sudoku.getSudokuBoard()[row].length + 13; i++) 
				rowReset = false;
			} //if (row % 3 == 1 && rowReset)

		} //for (int row = 1; row < sudoku.getSudokuBoard().length; row++)
	} //displayBoard()

}// SudokuInterface class