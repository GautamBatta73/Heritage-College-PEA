/**
 * 
 */
package gbatta_G30_A02_Linked_Lists;

/**
 *Description: This class contains the JFrame for the hangman game.
 *  
 * @author Gautam Batta
 */

import java.awt.EventQueue;

import javax.swing.JFrame;
import javax.swing.JPanel;
import javax.swing.border.EmptyBorder;
import javax.swing.text.AttributeSet;
import javax.swing.text.BadLocationException;
import javax.swing.text.PlainDocument;
import javax.swing.JTable;
import javax.swing.JScrollPane;

import java.awt.CardLayout;
import javax.swing.JMenuBar;
import javax.swing.JMenu;
import javax.swing.JMenuItem;
import javax.swing.JOptionPane;

import java.awt.Font;
import java.awt.Color;
import java.awt.Dimension;

import javax.swing.JLabel;
import javax.swing.SwingConstants;
import javax.swing.JTextField;
import javax.swing.ImageIcon;
import javax.swing.JButton;

import java.awt.event.ActionListener;
import java.io.FileNotFoundException;
import java.awt.event.ActionEvent;
import javax.swing.JTextArea;

public class HangmanFrame extends JFrame {
	private Hangman hangman;
	private Player player;
	private Scoreboard score;

	private JTextField fldName;
	private JPanel contentPane;
	private JPanel gamePanel;
	private JPanel loadPanel;
	private JMenuBar menuBar;
	private JMenu gameMenu;
	private JMenuItem newItem;
	private JMenuItem saveExitItem;
	private JMenuItem exitItem;
	private JMenu helpMenu;
	private JMenuItem scoreItem;
	private JMenuItem hintItem;
	private JLabel gameTitle;
	private JLabel lblName;
	private JButton btnGuess;
	private JLabel loadTitle;
	private JButton btnLoad;
	private JLabel lblGuess;
	private JTextField fldGuess;
	private JTextArea answerArea;
	private JLabel lblHangIMG;
	private ImageIcon hangIMG;
	private JTextArea badGuesses;

	public HangmanFrame() throws Exception {
		hangman = new Hangman("word_db.txt");
		player = new Player();
		score = new Scoreboard();
		setResizable(false);
		setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
		setBounds(100, 100, 558, 253);
		setLocationRelativeTo(null);
		setTitle("HangmanFrame");
		contentPane = new JPanel();
		contentPane.setBorder(new EmptyBorder(5, 5, 5, 5));

		CardLayout cl = new CardLayout(0, 0);
		setContentPane(contentPane);
		contentPane.setLayout(cl);

		gamePanel = new JPanel();
		contentPane.add(gamePanel, "gamePanel");
		gamePanel.setLayout(null);

		loadPanel = new JPanel();
		contentPane.add(loadPanel, "loadPanel");
		loadPanel.setLayout(null);

		menuBar = new JMenuBar();
		menuBar.setForeground(new Color(0, 0, 0));
		menuBar.setFont(new Font("Arial", Font.PLAIN, 16));
		menuBar.setBounds(0, 0, 652, 22);
		loadPanel.add(menuBar);

		gameMenu = new JMenu("Game");
		gameMenu.setForeground(new Color(0, 0, 0));
		gameMenu.setFont(new Font("Arial", Font.PLAIN, 16));
		menuBar.add(gameMenu);
		gameMenu.setEnabled(false);

		newItem = new JMenuItem("New");
		newItem.setForeground(new Color(0, 0, 0));
		newItem.setFont(new Font("Arial", Font.PLAIN, 16));
		newItem.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent e) {
				setBounds(100, 100, 558, 253);
				setLocationRelativeTo(null);
				menuBar.setBounds(0, 0, 652, 22);
				gamePanel.remove(menuBar);
				cl.show(contentPane, "loadPanel");
				loadPanel.add(menuBar);
				gameMenu.setEnabled(false);
				helpMenu.setEnabled(false);
			} //actionPerformed(ActionEvent)
		});
		gameMenu.add(newItem);

		saveExitItem = new JMenuItem("Save & Exit");
		saveExitItem.setForeground(new Color(0, 0, 0));
		saveExitItem.setFont(new Font("Arial", Font.PLAIN, 16));
		gameMenu.add(saveExitItem);
		saveExitItem.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent e) {
				try {
					HangmanSaveLoad.saveToFile(hangman, player.getName());

					int index = score.findPlayer(player.getName());
					score.getNextPlayer(index).setGamesPlayed(player.getGamesPlayed());
					score.getNextPlayer(index).setGamesWon(player.getGamesWon());

					score.savePlayers();
					setVisible(false);
					dispose();

				}
				catch (Exception e1) {
					String message = e1.getMessage();
					if (message.charAt(0) == '!')
						JOptionPane.showMessageDialog(contentPane, message.substring(1),
								"Invalid", JOptionPane.INFORMATION_MESSAGE);
					else
						JOptionPane.showMessageDialog(contentPane,
								"An Unexpected Error Occured: " + message, "Error",
								JOptionPane.ERROR_MESSAGE);
				} //catch (Exception)
			} //actionPerformed(ActionEvent)
		});

		exitItem = new JMenuItem("Exit");
		exitItem.setForeground(new Color(0, 0, 0));
		exitItem.setFont(new Font("Arial", Font.PLAIN, 16));
		gameMenu.add(exitItem);
		exitItem.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent e) {
				try {
					int index = score.findPlayer(player.getName());
					score.getNextPlayer(index).setGamesPlayed(player.getGamesPlayed());
					score.getNextPlayer(index).setGamesWon(player.getGamesWon());
					score.savePlayers();
				} //try
				catch (Exception e1) {
					JOptionPane.showMessageDialog(contentPane,
							"An Unexpected Error Occured: " + e1.getMessage(), "Error",
							JOptionPane.ERROR_MESSAGE);
				} //catch (Exception)

				setVisible(false);
				dispose();
			} //actionPerformed(ActionEvent)
		});

		helpMenu = new JMenu("Help");
		helpMenu.setForeground(new Color(0, 0, 0));
		helpMenu.setFont(new Font("Arial", Font.PLAIN, 16));
		menuBar.add(helpMenu);
		helpMenu.setEnabled(false);

		scoreItem = new JMenuItem("Score");
		scoreItem.setForeground(new Color(0, 0, 0));
		scoreItem.setFont(new Font("Arial", Font.PLAIN, 16));
		helpMenu.add(scoreItem);
		scoreItem.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent e) {
				String column[] = { "Name", "Games Played", "Games Won" };
				String row[][] = new String[score.getPlayersNum()][3];

				for (int i = 0; i < score.getPlayersNum(); i++) {
					row[i][0] = score.getNextPlayer(i).getName();
					row[i][1] = score.getNextPlayer(i).getGamesPlayed() + "";
					row[i][2] = score.getNextPlayer(i).getGamesWon() + "";
				} //for (int i = 0; i < score.getPlayersNum(); i++)

				JTable scoresTable = new JTable(row, column) {
					public boolean isCellEditable(int row, int column) {
						return false;
					};
				};
				scoresTable.setFont(new Font("Arial", Font.PLAIN, 14));
				scoresTable.getTableHeader().setFont(new Font("Arial", Font.BOLD, 16));
				scoresTable.setRowHeight(scoresTable.getRowHeight() + 1);
				JScrollPane scores = new JScrollPane(scoresTable);
				scores.setPreferredSize(
						new Dimension(scoresTable.getPreferredSize().width + 150,
								scoresTable.getPreferredSize().height + 25));

				JOptionPane.showMessageDialog(contentPane, scores, "Scores",
						JOptionPane.INFORMATION_MESSAGE);
			} //actionPerformed(ActionEvent)
		});

		hintItem = new JMenuItem("Hint");
		hintItem.setForeground(new Color(0, 0, 0));
		hintItem.setFont(new Font("Arial", Font.PLAIN, 16));
		helpMenu.add(hintItem);
		hintItem.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent e) {
				try {
					JOptionPane.showMessageDialog(contentPane,
							"Your Hint is '" + hangman.getHint() + "'", "Hint",
							JOptionPane.INFORMATION_MESSAGE);
					refresh();
				} //try
				catch (Exception e1) {
					String message = e1.getMessage();
					if (message.charAt(0) == '!')
						JOptionPane.showMessageDialog(contentPane, message.substring(1),
								"Invalid", JOptionPane.INFORMATION_MESSAGE);
					else
						JOptionPane.showMessageDialog(contentPane,
								"An Unexpected Error Occured: " + message, "Error",
								JOptionPane.ERROR_MESSAGE);
				} //catch (Exception)
			} //actionPerformed(ActionEvent)
		});

		gameTitle = new JLabel("Hangman");
		gameTitle.setHorizontalAlignment(SwingConstants.CENTER);
		gameTitle.setFont(new Font("Arial", Font.BOLD, 50));
		gameTitle.setBounds(274, 21, 260, 57);
		gamePanel.add(gameTitle);

		lblGuess = new JLabel("Guess: ");
		lblGuess.setEnabled(true);
		lblGuess.setHorizontalAlignment(SwingConstants.RIGHT);
		lblGuess.setFont(new Font("Arial", Font.PLAIN, 30));
		lblGuess.setBounds(10, 109, 111, 45);
		gamePanel.add(lblGuess);

		fldGuess = new JTextField();
		fldGuess.setHorizontalAlignment(SwingConstants.CENTER);
		fldGuess.setEnabled(true);
		fldGuess.setEditable(true);
		fldGuess.setFont(new Font("Arial", Font.PLAIN, 28));
		fldGuess.setColumns(1);
		fldGuess.setBounds(126, 111, 34, 41);
		gamePanel.add(fldGuess);
		fldGuess.setDocument(new PlainDocument() {
			public void insertString(int offs, String str, AttributeSet a)
					throws BadLocationException {
				if (str == null || fldGuess.getText().length() >= 1) {
					return;
				} //if (str == null || fldGuess.getText().length() >= 1)

				super.insertString(offs, str, a);
			} //insertString(int, String, AttributeSet)
		});

		badGuesses = new JTextArea();
		badGuesses.setEditable(false);
		badGuesses.setBackground(new Color(240, 240, 240));
		badGuesses.setWrapStyleWord(false);
		badGuesses.setLineWrap(true);
		badGuesses.setRows(2);
		badGuesses.setColumns(10);
		badGuesses.setFont(new Font("Arial", Font.PLAIN, 30));
		badGuesses.setBounds(10, 197, 524, 45);
		gamePanel.add(badGuesses);

		answerArea = new JTextArea();
		answerArea.setEditable(false);
		answerArea.setWrapStyleWord(false);
		answerArea.setText("stuff");
		answerArea.setRows(2);
		answerArea.setLineWrap(true);
		answerArea.setFont(new Font("Monospaced", Font.PLAIN, 45));
		answerArea.setColumns(10);
		answerArea.setBackground(new Color(240, 240, 240));
		answerArea.setBounds(10, 273, 430, 206);
		gamePanel.add(answerArea);

		lblHangIMG = new JLabel("");
		lblHangIMG.setBounds(435, 27, 402, 520);
		gamePanel.add(lblHangIMG);

		btnGuess = new JButton("Guess");
		btnGuess.setFont(new Font("Arial", Font.BOLD, 39));
		btnGuess.setBounds(299, 490, 161, 57);
		btnGuess.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent e) {
				try {
					if (!fldGuess.getText().isBlank()) {
						if (!hangman.guess(fldGuess.getText().charAt(0)))
							JOptionPane.showMessageDialog(contentPane,
									"'" + fldGuess.getText().charAt(0) + "' is Incorrect!",
									"Invalid", JOptionPane.INFORMATION_MESSAGE);
					}
					else
						throw new Exception("!You Cannot Guess Nothing!");
				}
				catch (Exception e1) {
					String message = e1.getMessage();
					if (message.charAt(0) == '!')
						JOptionPane.showMessageDialog(contentPane, message.substring(1),
								"Invalid", JOptionPane.INFORMATION_MESSAGE);
					else
						JOptionPane.showMessageDialog(contentPane,
								"An Unexpected Error Occured: " + message, "Error",
								JOptionPane.ERROR_MESSAGE);
				} //catch (Exception)
				refresh();
			} //actionPerformed(ActionEvent)
		});
		gamePanel.add(btnGuess);

		loadTitle = new JLabel("Load/Create Save");
		loadTitle.setHorizontalAlignment(SwingConstants.CENTER);
		loadTitle.setFont(new Font("Arial", Font.BOLD, 29));
		loadTitle.setBounds(136, 22, 260, 48);
		loadPanel.add(loadTitle);

		lblName = new JLabel("Name: ");
		lblName.setHorizontalAlignment(SwingConstants.RIGHT);
		lblName.setFont(new Font("Arial", Font.PLAIN, 19));
		lblName.setBounds(120, 81, 61, 32);
		loadPanel.add(lblName);

		fldName = new JTextField();
		fldName.setFont(new Font("Arial", Font.PLAIN, 19));
		fldName.setBounds(179, 82, 226, 30);
		loadPanel.add(fldName);
		fldName.setColumns(10);

		btnLoad = new JButton("Play");
		btnLoad.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent e) {
				boolean valid = false;
				try {
					hangman = HangmanSaveLoad.loadFromFile(fldName.getText().trim());
					valid = true;
				} //try
				catch (FileNotFoundException e1) {
					try {
						int choice = JOptionPane.showOptionDialog(contentPane,
								"There is No Save File With That Name.\n Do You Want to Create Start a New Game?",
								"New Game/Save Not Found", JOptionPane.YES_NO_OPTION,
								JOptionPane.QUESTION_MESSAGE, null,
								new String[] { "Yes", "No" }, "Option 1" // Default selection
						);

						if (choice == JOptionPane.YES_OPTION) {
							HangmanSaveLoad.saveToFile(hangman, fldName.getText().trim());
							valid = true;
						} //if (choice == JOptionPane.YES_OPTION)
					} //try
					catch (Exception e2) {
						String message = e1.getMessage();
						if (message.charAt(0) == '!')
							JOptionPane.showMessageDialog(contentPane, message.substring(1),
									"Invalid", JOptionPane.INFORMATION_MESSAGE);
						else
							JOptionPane.showMessageDialog(contentPane,
									"An Unexpected Error Occured: " + message, "Error",
									JOptionPane.ERROR_MESSAGE);
					} //catch (Exception)
				} //catch (FileNotFoundException)
				catch (Exception e1) {
					String message = e1.getMessage();
					if (message.charAt(0) == '!')
						JOptionPane.showMessageDialog(contentPane, message.substring(1),
								"Invalid", JOptionPane.INFORMATION_MESSAGE);
					else
						JOptionPane.showMessageDialog(contentPane,
								"An Unexpected Error Occured: " + message, "Error",
								JOptionPane.ERROR_MESSAGE);
				} //catch (Exception)

				if (valid) {
					player.setName(fldName.getText().trim());
					player.setGamesPlayed(0);
					player.setGamesWon(0);
					fldName.setText("");

					try {
						int index = score.findPlayer(player.getName());
						if (index < 0)
							score.addPlayer(player);
						else {
							player
									.setGamesPlayed(score.getNextPlayer(index).getGamesPlayed());
							player.setGamesWon(score.getNextPlayer(index).getGamesWon());
						}
					} //try
					catch (Exception e1) {
						JOptionPane.showMessageDialog(contentPane,
								"An Unexpected Error Occured: " + e1.getMessage(), "Error",
								JOptionPane.ERROR_MESSAGE);
					} //catch (Exception)

					refresh();
					setBounds(100, 100, 833, 596);
					menuBar.setBounds(0, 0, 852, 22);
					setLocationRelativeTo(null);
					loadPanel.remove(menuBar);
					cl.show(contentPane, "gamePanel");
					gamePanel.add(menuBar);
					gameMenu.setEnabled(true);
					helpMenu.setEnabled(true);
				} //if (valid)
			} //actionPerformed(ActionEvent)
		});
		btnLoad.setFont(new Font("Arial", Font.BOLD, 39));
		btnLoad.setBounds(206, 136, 120, 57);
		loadPanel.add(btnLoad);

		cl.show(contentPane, "loadPanel");
	} //HangmanFrame()

	public static void main(String[] args) {
		EventQueue.invokeLater(new Runnable() {
			public void run() {
				try {
					HangmanFrame frame = new HangmanFrame();
					frame.setVisible(true);
				}
				catch (Exception e) {
					String message = e.getMessage();
					if (message.charAt(0) == '!')
						JOptionPane.showMessageDialog(null, message.substring(1), "Invalid",
								JOptionPane.INFORMATION_MESSAGE);
					else
						JOptionPane.showMessageDialog(null,
								"An Unexpected Error Occured: " + message, "Error",
								JOptionPane.ERROR_MESSAGE);

					System.exit(0);
				} //catch(Exception)
			} //run()
		});
	} //main(String[])

	private void refresh() {
		fldGuess.setText("");
		badGuesses.setText("Incorrect Guesses: " + hangman.getIncorrectGuesses());
		answerArea.setText(hangman.getCurrentGuessedAnswer());

		hangIMG = new ImageIcon(
				new ImageIcon("./hang" + hangman.getIncorrectSize() + ".png").getImage()
						.getScaledInstance(lblHangIMG.getWidth(), lblHangIMG.getHeight(),
								java.awt.Image.SCALE_SMOOTH));

		lblHangIMG.setIcon(hangIMG);

		if (hangman.getIncorrectSize() > 5) {
			JOptionPane.showMessageDialog(this,
					"The Man Was Hanged!\n The Word Was: " + hangman.getCurrentAnswer(),
					"You Lost!", JOptionPane.INFORMATION_MESSAGE);
		} //if (hangman.getIncorrectSize() > 5)
		else
			if (hangman.hasWon()) {
				player.setGamesWon(player.getGamesWon() + 1);
				JOptionPane.showMessageDialog(this, "You Got The Word!",
						"Congratulations!", JOptionPane.INFORMATION_MESSAGE);
			}

		if (hangman.getIncorrectSize() > 5 || hangman.hasWon()) {
			try {
				hangman.nextGame();
				refresh();
			} //try
			catch (Exception e) {
				JOptionPane.showMessageDialog(this,
						"An Unexpected Error Occured: " + e.getMessage(), "Error",
						JOptionPane.ERROR_MESSAGE);
			} //catch (Exception)
			player.setGamesPlayed(player.getGamesPlayed() + 1);
		} //if (hangman.getIncorrectSize() > 5 || hangman.hasWon())
	} //refresh()
}
// HangmanFrame class