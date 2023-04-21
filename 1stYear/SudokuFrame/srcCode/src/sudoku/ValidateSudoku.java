/**
 * 
 */
package sudoku;

/**
 *Description: This class contains the validation of moves made 
 *	in a game of Sudoku.
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

public class ValidateSudoku {

	public boolean validInt(String num) {
		boolean valid = true;
		try {
			int val = Integer.parseInt(num);
		} //try
		catch (Exception e) {
			valid = false;
		} //catch (Exception e)
		return valid;
	} //validInt(String)

	public boolean inValidRange(int num) {
		boolean valid = true;
		if (num < 1 || num > 9)
			valid = false;

		return valid;
	} //inValidRange(int)

	public boolean inValidRow(int[] row, int val) {
		boolean valid = true;
		int start = row[0];

		if (start == val)
			valid = false;
		else {
			for (int rowFor = 0; rowFor < row.length; rowFor++) {
				if (row[rowFor] == val)
					valid = false;
			} //for (int rowFor = 0; rowFor < board.length; rowFor++)
		} //else

		return valid;
	} //inValidRow(int[], val)

	public boolean inValidColumn(int[][] board, int col, int val) {
		boolean valid = true;

		int start = board[0][col];
		if (start == val)
			valid = false;
		else {
			for (int rowFor = 0; rowFor < board[0].length; rowFor++) {
				if (board[rowFor][col] == val)
					valid = false;
			} //for (int rowFor = 0; rowFor < board[0].length; rowFor++)
		} //else

		return valid;
	} //inValidColumn(int[][], int, int)

	public boolean inValidBox(int[][] board, int row, int col, int val) {
		boolean valid = true;

		if (col % 3 == 0) {

			if (row % 3 == 0) {
				for (int rowFor = row; rowFor > (row - 3); rowFor--) {
					for (int colFor = col; colFor > (col - 3); colFor--) {
						if (rowFor != row && colFor != col)
							if (board[rowFor][colFor] == val)
								valid = false;
					} //for (int colFor = col; colFor > (col - 3); colFor--)
				} //for (int rowFor = row; rowFor > (row - 3); rowFor--)
			} //if (row % 3 == 0)

			else
				if (row % 3 == 1) {
					for (int rowFor = row; rowFor < (row + 3); rowFor++) {
						for (int colFor = col; colFor > (col - 3); colFor--) {
							if (rowFor != row && colFor != col)
								if (board[rowFor][colFor] == val)
									valid = false;
						} //for for (int rowFor = row; rowFor < (row + 3); rowFor++)
					} //for (int colFor = col; colFor > (col - 3); colFor--)
				} //if (row % 3 == 1)

				else
					if (row % 3 == 2) {
						for (int rowFor = row - 1; rowFor < ((row - 1) + 3); rowFor++) {
							for (int colFor = col; colFor > (col - 3); colFor--) {
								if (rowFor != row && colFor != col)
									if (board[rowFor][colFor] == val)
										valid = false;
							} //for (int rowFor = row - 1; rowFor < ((row - 1) + 3); rowFor++)
						} //for (int colFor = col; colFor > (col - 3); colFor--)
					} //if (row % 3 == 2)

		} //if (col % 3 == 0)

		else
			if (col % 3 == 1) {

				if (row % 3 == 0) {
					for (int rowFor = row; rowFor > (row - 3); rowFor--) {
						for (int colFor = (col + 2); colFor > ((col + 2) - 3); colFor--) {
							if (rowFor != row && colFor != col)
								if (board[rowFor][colFor] == val)
									valid = false;
						} //for (int colFor = (col + 2); colFor > ((col + 2) - 3); colFor--)
					} //for (int rowFor = row; rowFor > (row - 3); rowFor--)
				} //if (row % 3 == 0)

				else
					if (row % 3 == 1) {
						for (int rowFor = row; rowFor < (row + 3); rowFor++) {
							for (int colFor = col; colFor < (col + 3); colFor++) {
								if (rowFor != row && colFor != col)
									if (board[rowFor][colFor] == val)
										valid = false;
							} //for for (int rowFor = row; rowFor > (row + 3); rowFor++)
						} //for (int colFor = col; colFor > (col + 3); colFor++)
					} //if (row % 3 == 1)

					else
						if (row % 3 == 2) {
							for (int rowFor = (row - 1); rowFor < ((row - 1) + 3); rowFor++) {
								for (int colFor = col; colFor < (col + 3); colFor++) {
									if (rowFor != row && colFor != col)
										if (board[rowFor][colFor] == val)
											valid = false;
								} //for for (int rowFor = (row - 1); rowFor > ((row - 1) + 3); rowFor++)
							} //for (int colFor = col; colFor > (col + 3); colFor++)
						} //if (row % 3 == 2)

			} //if (col % 3 == 1)

			else
				if (col % 3 == 2) {

					if (row % 3 == 0) {
						for (int rowFor = row; rowFor > (row - 3); rowFor--) {
							for (int colFor = (col + 1); colFor > ((col + 1) - 3); colFor--) {
								if (rowFor != row && colFor != col)
									if (board[rowFor][colFor] == val)
										valid = false;
							} //for (int colFor = (col + 1); colFor > ((col + 1) - 3); colFor--)
						} //for (int rowFor = row; rowFor > (row - 3); rowFor--)
					} //if (row % 3 == 0)

					else
						if (row % 3 == 1) {
							for (int rowFor = row; rowFor < (row + 3); rowFor++) {
								for (int colFor = (col - 1); colFor < ((col - 1)
										+ 3); colFor++) {
									if (rowFor != row && colFor != col)
										if (board[rowFor][colFor] == val)
											valid = false;
								} //for (int colFor = (col - 1); colFor > ((col - 1) + 3); colFor++)
							} //for for (int rowFor = row; rowFor > (row + 3); rowFor++)
						} //if (row % 3 == 1)

						else
							if (row % 3 == 2) {
								for (int rowFor = (row - 1); rowFor < ((row - 1)
										+ 3); rowFor++) {
									for (int colFor = (col - 1); colFor < ((col - 1)
											+ 3); colFor++) {
										if (rowFor != row && colFor != col)
											if (board[rowFor][colFor] == val)
												valid = false;
									} //for (int colFor = (col - 1); colFor > ((col - 1) + 3); colFor++)
								} //for for (int rowFor = (row - 1); rowFor > ((row - 1) + 3); rowFor++)
							} //if (row % 3 == 2)

				} //if (col % 3 == 2)

		return valid;
	} //inValidBox(int[][], int, int, int)

}// ValidateSudoku class