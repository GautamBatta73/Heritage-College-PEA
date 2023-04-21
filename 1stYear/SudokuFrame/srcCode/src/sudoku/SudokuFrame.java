/**
 * 
 */
package sudoku;

/**
 *Description: This class contains the actual game of Sudoku in a JFrame.
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

import javax.swing.JFrame;
import javax.swing.JLabel;
import javax.swing.JMenu;
import javax.swing.JMenuBar;
import javax.swing.JMenuItem;
import javax.swing.JOptionPane;
import javax.swing.JPanel;
import javax.swing.JTextArea;
import javax.swing.JTextField;
import javax.swing.SwingConstants;
import javax.swing.border.LineBorder;
import java.awt.Color;
import java.awt.EventQueue;
import java.awt.FileDialog;
import java.awt.Font;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.io.FileNotFoundException;
import java.io.IOException;
import javax.swing.JButton;

public class SudokuFrame extends JFrame {
	private SudokuGame sudoku;

	private JPanel contentPane;
	private JMenuBar menuBar;
	private JPanel sudokuPanel;
	private JPanel inputPanel;
	private boolean fileSelected;
	private String fileName;

	private JMenu fileMenu;
	private JMenuItem loadItem;
	private JMenuItem saveItem;
	private JMenuItem undoItem;
	private JMenuItem exitItem;

	private JMenu gameMenu;
	private JMenuItem aboutItem;
	private JMenuItem helpItem;

	private JTextField fldColumn;
	private JTextField fldRow;
	private JTextField fldValue;
	private JLabel lblColumn;
	private JLabel lblRow;
	private JLabel lblValue;

	private JTextArea sudokuDisplay;
	private JButton moveBtn;

	public SudokuFrame() {
		fileSelected = false;
		fileName = "";

		setTitle("SudokuFrame");
		setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
		setBounds(100, 100, 586, 645);

		menuBar = new JMenuBar();
		setJMenuBar(menuBar);

		fileMenu = new JMenu("File");
		menuBar.add(fileMenu);

		loadItem = new JMenuItem("Load File");
		loadItem.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent e) {
				if (!fileSelected) {
					boolean valid = true;
					sudokuSelectFile();
					try {
						sudoku = new SudokuGame(fileName);
					} //try
					catch (FileNotFoundException x) {
						valid = false;
						errorWindow(
								"File Could Not Be Found.\n Please Make Sure It is In The Corect Directory.",
								"File Error");
					} //catch (FileNotFoundException)
					catch (Exception z) {
						valid = false;
						errorWindow(z.getMessage() + "\n", "File Error");
					} //catch (Exception)

					if (valid) {
						displayBoard();
						enableFlds(true);

						if (gameWon())
							userWinsGameFrame();
					} //if (valid)
					else
						fileSelected = false;
				} //if (!fileSelected)
				else
					errorWindow(
							"You must save and quit this game in order to load another file.",
							"Error");
			} //actionPerformed(ActionEvent)
		});
		fileMenu.add(loadItem);

		saveItem = new JMenuItem("Save");
		saveItem.setEnabled(false);
		saveItem.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent e) {
				saveGame();
			} //actionPerformed(ActionEvent)
		});
		fileMenu.add(saveItem);

		undoItem = new JMenuItem("Undo");
		undoItem.setEnabled(false);
		undoItem.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent e) {
				undoMove();
			} //actionPerformed(ActionEvent)
		});
		fileMenu.add(undoItem);

		exitItem = new JMenuItem("Exit");
		exitItem.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent e) {
				exitGame();
			} //actionPerformed(ActionEvent)
		});
		fileMenu.add(exitItem);

		gameMenu = new JMenu("Game");
		menuBar.add(gameMenu);

		aboutItem = new JMenuItem("About");
		aboutItem.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent e) {
				aboutGame();
			} //actionPerformed(ActionEvent)
		});
		gameMenu.add(aboutItem);

		helpItem = new JMenuItem("Help");
		helpItem.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent e) {
				gameHelp();
			} //actionPerformed(ActionEvent)
		});
		gameMenu.add(helpItem);
		contentPane = new JPanel();
		contentPane.setBorder(null);

		setContentPane(contentPane);
		contentPane.setLayout(null);

		sudokuPanel = new JPanel();
		sudokuPanel.setBounds(0, 0, 570, 408);
		contentPane.add(sudokuPanel);

		sudokuDisplay = new JTextArea();
		sudokuDisplay.setBorder(new LineBorder(new Color(0, 0, 0), 3, true));
		sudokuDisplay.setEditable(false);
		sudokuDisplay.setFont(new Font("Monospaced", Font.PLAIN, 20));
		sudokuDisplay.setRows(13);
		sudokuDisplay.setColumns(25);
		sudokuPanel.add(sudokuDisplay);

		inputPanel = new JPanel();
		inputPanel.setBounds(0, 408, 570, 186);
		contentPane.add(inputPanel);
		inputPanel.setLayout(null);

		lblRow = new JLabel("Row (Y-Value):");
		lblRow.setBounds(140, 47, 174, 22);
		lblRow.setHorizontalAlignment(SwingConstants.RIGHT);
		lblRow.setFont(new Font("Tahoma", Font.BOLD, 12));
		inputPanel.add(lblRow);

		lblColumn = new JLabel("Column (X-Value):");
		lblColumn.setBounds(140, 11, 174, 22);
		lblColumn.setHorizontalAlignment(SwingConstants.RIGHT);
		lblColumn.setFont(new Font("Tahoma", Font.BOLD, 12));
		inputPanel.add(lblColumn);

		lblValue = new JLabel("Value:");
		lblValue.setBounds(140, 83, 174, 22);
		lblValue.setHorizontalAlignment(SwingConstants.RIGHT);
		lblValue.setFont(new Font("Tahoma", Font.BOLD, 12));
		inputPanel.add(lblValue);

		fldColumn = new JTextField();
		fldColumn.setEnabled(false);
		fldColumn.setFont(new Font("Tahoma", Font.PLAIN, 12));
		fldColumn.setBounds(318, 13, 44, 20);
		inputPanel.add(fldColumn);
		fldColumn.setColumns(10);

		fldRow = new JTextField();
		fldRow.setEnabled(false);
		fldRow.setFont(new Font("Tahoma", Font.PLAIN, 12));
		fldRow.setColumns(10);
		fldRow.setBounds(318, 49, 44, 20);
		inputPanel.add(fldRow);

		fldValue = new JTextField();
		fldValue.setEnabled(false);
		fldValue.setFont(new Font("Tahoma", Font.PLAIN, 12));
		fldValue.setColumns(10);
		fldValue.setBounds(318, 85, 44, 20);
		inputPanel.add(fldValue);

		moveBtn = new JButton("Make Move");
		moveBtn.setEnabled(false);
		moveBtn.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent e) {
				makeMove();
			} //actionPerformed(ActionEvent)
		});
		moveBtn.setFont(new Font("Tahoma", Font.BOLD, 13));
		moveBtn.setBounds(227, 124, 115, 40);
		inputPanel.add(moveBtn);

		enableFlds(false);
	} //SudokuFrame()

	public static void main(String[] args) {
		EventQueue.invokeLater(new Runnable() {
			public void run() {
				try {
					SudokuFrame frame = new SudokuFrame();
					frame.setVisible(true);
				} //try
				catch (Exception e) {
					e.printStackTrace();
				} //catch (Exception e)
			} //run()
		});
	} //main(String[])

	private void displayBoard() {
		sudokuDisplay.setText("");
		for (int row = 1; row < sudoku.getSudokuBoard().length; row++) {
			boolean rowReset = true;
			for (int col = 1; col < sudoku.getSudokuBoard()[row].length; col++) {

				if (row % 3 == 1 && rowReset) {
					for (int i = 1; i < sudoku.getSudokuBoard()[row].length + 16; i++) {
						sudokuDisplay.append("-");
						if (i == sudoku.getSudokuBoard()[row].length + 15)
							sudokuDisplay.append("\n");
					} //for (int i = 1; i < sudoku.getSudokuBoard()[row].length + 13; i++) 
					rowReset = false;
				} //if (row % 3 == 1 && rowReset)

				if (col % 3 == 1)
					sudokuDisplay.append("| ");

				if (sudoku.getSudokuBoard()[row][col] == -1)
					sudokuDisplay.append("* ");
				else
					sudokuDisplay.append(sudoku.getSudokuBoard()[row][col] + " ");

			} //for (int col = 1; col < sudoku.getSudokuBoard()[row].length; col++)
			sudokuDisplay.append("|\n");

			if (row == sudoku.getSudokuBoard().length - 1) {
				for (int i = 1; i < sudoku.getSudokuBoard()[row].length + 16; i++) {
					sudokuDisplay.append("-");
				} //for (int i = 1; i < sudoku.getSudokuBoard()[row].length + 13; i++) 
				rowReset = false;
			} //if (row % 3 == 1 && rowReset)

		} //for (int row = 1; row < sudoku.getSudokuBoard().length; row++)
	} //displayBoard()

	private boolean gameWon() {
		boolean won = true;

		for (int row = 1; row < sudoku.getSudokuBoard().length; row++) {
			for (int col = 1; col < sudoku.getSudokuBoard()[row].length; col++) {
				if (sudoku.getSudokuBoard()[row][col] == -1)
					won = false;
			} //for (int col = 1; col < sudoku.getSudokuBoard()[row].length; col++)
		} //for (int row = 1; row < sudoku.getSudokuBoard().length; row++)

		return won;
	} //gameWon()

	private void userWinsGameFrame() {
		JLabel congrats = new JLabel(
				"<html><center><font size='4'>" + "<u>You Won!</u>"
						+ "<br>Congratulations<br>" + "<br>You May Save and/or"
						+ "<br>Load another Game!" + "</font></center><html>");

		infoWindow(congrats, "Congratulations");
		fileSelected = false;
	} //userWinsGameFrame()

	private void gameHelp() {
		String help = "Sudoku is a logic based, number placement puzzle.\n"
				+ "The board is a 9 x 9 grid with 81 squares. Some of the squares are already filled in at the start of the puzzle.\n\n"
				+ "The player must fill in the rest of the squares by observing these 3 rules: \n"
				+ "\t- You must place the numbers 1 – 9 in each row without repeating a number.\n"
				+ "\t- You must place the numbers 1 – 9 in each column without repeating a number.\n"
				+ "\t- You must place the numbers 1 – 9 in each of the marked 3 x 3 boxes without repeating a number.\n";
		infoWindow(help, "Help");
	} //gameHelp()

	private void aboutGame() {
		JLabel about = new JLabel("<html><center><font size='5'>"
				+ "<u>SudokuFrame</u>" + "<br>Gautam Batta" + "<br>2022"
				+ "<br>Heritage College" + "</font></center><html>");

		infoWindow(about, "About The Game");
	} //aboutGame()

	private void makeMove() {
		int row = 0;
		int col = 0;
		int val = 0;

		boolean valid = true;
		try {
			col = Integer.parseInt(fldColumn.getText());
		} //try
		catch (NumberFormatException a) {
			errorWindow("Column Is Not an Integer.\n Please Try Agaín.",
					"Column Error");
			valid = false;
		} //catch (NumberFormatException)
		catch (Exception b) {
			errorWindow("An Unexpected Error Occured.\n Please Try Agaín.",
					"Column Error");
			valid = false;
		} //catch (Exception)

		try {
			row = Integer.parseInt(fldRow.getText());
		} //try
		catch (NumberFormatException a) {
			errorWindow("Row Is Not an Integer.\n Please Try Agaín.", "Row Error");
			valid = false;
		} //catch (NumberFormatException)
		catch (Exception b) {
			errorWindow("An Unexpected Error Occured.\n Please Try Agaín.",
					"Row Error");
			valid = false;
		} //catch (Exception)

		try {
			val = Integer.parseInt(fldValue.getText());
		} //try
		catch (NumberFormatException a) {
			errorWindow("Value Is Not an Integer.\n Please Try Agaín.",
					"Value Error");
			valid = false;
		} //catch (NumberFormatException)
		catch (Exception b) {
			errorWindow("An Unexpected Error Occured.\n Please Try Agaín.",
					"Value Error");
			valid = false;
		} //catch (Exception)

		try {
			sudoku.addToBoard(row, col, val);
		} //try
		catch (Exception e) {
			errorWindow(e.getMessage() + "\n", "Move Error");
			valid = false;
		} //catch (Exception)

		if (valid) {
			sudoku.setLastRow(row);
			sudoku.setLastColumn(col);
		} //if (valid)

		displayBoard();

		if (gameWon())
			userWinsGameFrame();
	} //makeMove()

	public void undoMove() {
		if (sudoku.getLastRow() > 0 && sudoku.getLastColumn() > 0) {
			sudoku.undoMove();
			infoWindow("Succesfully Undone!", "Undo");
		} //if (sudoku.getLastRow() > 0 && sudoku.getLastColumn() > 0)
		else
			errorWindow("You Must Make a Move, Before You Can Undo It.",
					"Undo Error");

		displayBoard();
	} //undoMove()

	public void saveGame() {
		boolean saved = true;
		try {
			sudoku.saveGame();
		} //try
		catch (FileNotFoundException x) {
			errorWindow("The original file could not be found.", "File Error");
			saved = false;
		} //catch (FileNotFoundException)
		catch (IOException y) {
			errorWindow("An Error Occured while saving.\n Please Try Agaín.",
					"File Error");
			saved = false;
		} //catch (IOException)
		catch (Exception z) {
			errorWindow("An Unexpected Error Occured.\n Please Try Agaín.",
					"File Error");
			saved = false;
		} //catch (Exception)

		if (saved)
			infoWindow("Succesfully Saved!", "Save Game");

		displayBoard();
	} //saveGame()

	public void exitGame() {
		infoWindow("Goodbye!", "Exit Game");
		this.dispose();
	} //exitGame()

	private void sudokuSelectFile() {
		FileDialog sudokuFileDialog = new FileDialog(this, "Select Sudoku File",
				FileDialog.LOAD);
		sudokuFileDialog.setVisible(true);
		String directoryName = sudokuFileDialog.getDirectory();
		String file = sudokuFileDialog.getFile();

		if (directoryName != null && file != null)
			fileName = directoryName + file;
		else {
			fileName = "";
			infoWindow("As no filename was entered, the default will be\n sudoku.txt",
					"File");
		} //else

		fileSelected = true;
	} //sudokuSelectFile()

	private void enableFlds(boolean enable) {
		fldColumn.setEnabled(enable);
		fldRow.setEnabled(enable);
		fldValue.setEnabled(enable);
		moveBtn.setEnabled(enable);
		saveItem.setEnabled(enable);
		undoItem.setEnabled(enable);
	} //private void enableFlds(boolean)

	//These are here because, apparently, I can not add a JOptionPane inside most of my methods, so I created my own methods
	private void errorWindow(String errMessage, String errTitle) {
		JOptionPane.showMessageDialog(this, errMessage, errTitle,
				JOptionPane.ERROR_MESSAGE);
	} //errorWindow(String, String)

	private void infoWindow(String errMessage, String errTitle) {
		JOptionPane.showMessageDialog(this, errMessage, errTitle,
				JOptionPane.INFORMATION_MESSAGE);
	} //infoWindow(String, String)

//overloaded for the aboutGame() and userWinsGameFrame() methods, so that it may be centered and styled.
	private void infoWindow(JLabel errMessage, String errTitle) { 
		JOptionPane.showMessageDialog(this, errMessage, errTitle,
				JOptionPane.INFORMATION_MESSAGE);
	} //infoWindow(JLabel, String)

}// SudokuFrame class