/**
 * 
 */
package sudoku;

/**
 *Description: This class contains the for a game of Sudoku.
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

import java.io.IOException;
import java.util.Scanner;
import java.io.FileNotFoundException;
import java.io.File;
import java.io.FileWriter;

public class SudokuGame {
	private ValidateSudoku valid;
	private int[][] sudokuBoard;
	private String fileName;
	private int lastRow;
	private int lastColumn;

	public SudokuGame(String file)
			throws IOException, FileNotFoundException, Exception {
		valid = new ValidateSudoku();
		sudokuBoard = new int[10][10];
		lastRow = 0;
		lastColumn = 0;
		setFileName(file);
	} //SudokuGame()

	public int[][] getSudokuBoard() {
		return sudokuBoard;
	} //getSudokuBoard()

	public void addToBoard(int row, int col, int valParam) throws Exception {

		if (valid.inValidRange(row) && valid.inValidRange(col)) {
			if (sudokuBoard[row][col] == -1) {
				if (valid.inValidRange(valParam)
						&& valid.inValidRow(sudokuBoard[row], valParam)
						&& valid.inValidColumn(sudokuBoard, col, valParam)
						&& valid.inValidBox(sudokuBoard, row, col, valParam))
					sudokuBoard[row][col] = valParam;
				else if (!valid.inValidRange(valParam))
					throw new Exception(
							"Value Not In Valid Range (1-9).\n Please Try Again.");
				else if (!valid.inValidRow(sudokuBoard[row], valParam))
					throw new Exception("Value Already In Row.\n Please Try Again.");
				else if (!valid.inValidColumn(sudokuBoard, col, valParam))
					throw new Exception(
							"Value Already In Column.\n Please Try Again.");
				else if (!valid.inValidBox(sudokuBoard, row, col, valParam))
					throw new Exception(
							"Value Already In Box.\n Please Try Again.");
			} //if (sudokuBoard[row][col] == -1)
			else
				throw new Exception(
						"There Is Already a Value In That Position.\n Please Try Again.");

		} //if (valid.inValidRange(row) && valid.inValidRange(col))
		else if (!valid.inValidRange(col))
			throw new Exception(
					"Column Not In Valid Range (1-9).\n Please Try Again.");
		else if (!valid.inValidRange(row))
			throw new Exception(
					"Row Not In Valid Range (1-9).\n Please Try Again.");
	} //addToBoard(int, int, int)

	private void addToEmptyBoard(int row, int col, int valParam) throws Exception {

		if (valid.inValidRange(valParam)
				&& valid.inValidRow(sudokuBoard[row], valParam)
				&& valid.inValidColumn(sudokuBoard, col, valParam)
				&& valid.inValidBox(sudokuBoard, row, col, valParam))
			sudokuBoard[row][col] = valParam;
		else if (!valid.inValidRange(valParam))
			throw new Exception(
					valParam + " Not In Valid Range (1-9) at " + col + ", " + row + ".\n Please Try Again.");
		else if (!valid.inValidRow(sudokuBoard[row], valParam))
			throw new Exception(valParam + " Already In Row: " + row + ".\n Please Try Again.");
		else if (!valid.inValidColumn(sudokuBoard, col, valParam))
			throw new Exception(
					valParam + " Already In Column: " + col + ".\n Please Try Again.");
		else if (!valid.inValidBox(sudokuBoard, row, col, valParam))
			throw new Exception(
					valParam + " Already in Box at " + col + ", " + row + ".\n Please Try Again.");
	} //addToEmptyBoard(int, int, int)

	public String getFileName() {
		return fileName;
	} //getFileName()

	public int getLastRow() {
		return lastRow;
	} //getlastRow()

	public int getLastColumn() {
		return lastColumn;
	} //getlastColumn()

	public void setFileName(String file)
			throws IOException, FileNotFoundException, Exception {
		String tempFile = "";

		if (file.isEmpty())
			tempFile = "sudoku.txt";
		else
			tempFile = file;

		fileName = tempFile;
		setBoardFromFile();
	} //setFileName(String)

	public void setLastRow(int lastRowParam) {
		lastRow = lastRowParam;
	} //setlastRow(int)

	public void setLastColumn(int lastColParam) {
		lastColumn = lastColParam;
	} //setlastColumn(int)

	public void undoMove() {
		sudokuBoard[lastRow][lastColumn] = -1;

		lastRow = 0;
		lastColumn = 0;
	} //undoMove()

	public void saveGame() throws IOException, FileNotFoundException, Exception {
		File file = new File(fileName);
		FileWriter writer = new FileWriter(file);

		writer.write(""); //clears the file
		writer.close(); //closes the overwriting instance

		writer = new FileWriter(file, true);
		for (int row = 1; row < sudokuBoard.length; row++) {
			for (int col = 1; col < sudokuBoard[row].length; col++) {
				if (sudokuBoard[row][col] == -1)
					writer.write("*~");
				else
					writer.write(sudokuBoard[row][col] + "~");
			} //for (int col = 1; col < sudokuBoard[row].length; col++)
			if (row != (sudokuBoard.length - 1))
				writer.write("\n");
		} //for (int row = 1; row < sudokuBoard.length; row++)

		writer.close();
	} //saveGame() throws IOException, FileNotFoundException, Exception

	public void setBoardFromFile()
			throws IOException, FileNotFoundException, Exception {
		File file = new File(fileName);
		Scanner scanFile = new Scanner(file);
		scanFile.useDelimiter("~|~\r\n");

		for (int row = 1; scanFile.hasNext(); row++) {
			if (row > 9)
				throw new Exception(
						"The file has too many rows or columns.\n There must be exactly " + (sudokuBoard.length - 1));
			for (int col = 1; col < sudokuBoard[row].length; col++) {
				String val = scanFile.next().replaceAll("\\r|\\n", "");

				if (val.equalsIgnoreCase("*"))
					sudokuBoard[row][col] = -1;
				else if (valid.validInt(val)) {
					int valInt = Integer.parseInt(val);
					addToEmptyBoard(row, col, valInt);
				} //if (valid.validInt(val))
				else
					throw new IOException(
							"The file does not have valid Integers, or they were not found.");
			} //for (int col = 1; col < sudokuBoard[row].length; col++)
			if (!scanFile.hasNext() && row < 9)
				throw new Exception(
						"The file has too little rows or columns.\n There must be exactly " + (sudokuBoard.length - 1));
		} //for (int row = 1; row < sudokuBoard.length; row++)

		scanFile.close();
	} //setBoardFromFile() throws IOException, FileNotFoundException, Exception

}// SudokuGame class