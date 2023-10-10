package Hangman;

import java.awt.EventQueue;

import javax.swing.JFrame;
import javax.swing.JPanel;
import javax.swing.border.EmptyBorder;
import javax.swing.text.MaskFormatter;
import javax.swing.SwingConstants;
import java.awt.Font;

import javax.swing.JTextArea;
import javax.swing.JLabel;
import java.awt.Color;

import javax.swing.UIManager;
import javax.swing.JButton;
import javax.swing.JMenuBar;
import javax.swing.JFormattedTextField;
import javax.swing.JMenu;
import javax.swing.JMenuItem;
import javax.swing.ImageIcon;
import java.awt.event.ActionListener;
import java.awt.event.ActionEvent;

public class HangmanFrame extends JFrame {

	private JPanel contentPane;
	private int rand;

	/**
	 * Launch the application.
	 */
	public static void main(String[] args) {
		EventQueue.invokeLater(new Runnable() {
			public void run() {
				try {
					HangmanFrame frame = new HangmanFrame();
					frame.setVisible(true);
				}
				catch (Exception e) {
					e.printStackTrace();
				}
			}
		});
	}

	/**
	 * Create the frame.
	 * @throws Exception 
	 */
	public HangmanFrame() throws Exception {
		setTitle("Hangman");
		setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
		setBounds(100, 100, 722, 403);
		contentPane = new JPanel();
		contentPane.setBorder(new EmptyBorder(5, 5, 5, 5));

		setContentPane(contentPane);
		contentPane.setLayout(null);

		JLabel lblNewLabel = new JLabel("Hangman");
		lblNewLabel.setVerticalAlignment(SwingConstants.TOP);
		lblNewLabel.setHorizontalAlignment(SwingConstants.CENTER);
		lblNewLabel.setFont(new Font("Tahoma", Font.BOLD, 30));
		lblNewLabel.setBounds(243, 25, 219, 37);
		contentPane.add(lblNewLabel);

		JLabel lblNewLabel_1 = new JLabel("Guess:");
		lblNewLabel_1.setFont(new Font("Tahoma", Font.PLAIN, 24));
		lblNewLabel_1.setBounds(32, 70, 82, 34);
		contentPane.add(lblNewLabel_1);

		JFormattedTextField txtH = new JFormattedTextField(new MaskFormatter("?"));
		txtH.setColumns(1);
		txtH.setHorizontalAlignment(SwingConstants.CENTER);
		txtH.setText("");
		txtH.setFont(new Font("Tahoma", Font.PLAIN, 20));
		txtH.setBounds(108, 72, 30, 34);
		contentPane.add(txtH);

		JTextArea txtrIncorrectGuesses = new JTextArea();
		txtrIncorrectGuesses.setText("Incorrect Guesses: ");
		txtrIncorrectGuesses.setBackground(new Color(240, 240, 240));
		txtrIncorrectGuesses.setFont(new Font("Tahoma", Font.PLAIN, 23));
		txtrIncorrectGuesses.setEditable(false);
		txtrIncorrectGuesses.setBounds(32, 119, 279, 42);
		contentPane.add(txtrIncorrectGuesses);

		JTextArea txtrIncorrectGuesses_1 = new JTextArea();
		txtrIncorrectGuesses_1.setLineWrap(true);
		txtrIncorrectGuesses_1.setWrapStyleWord(true);
		txtrIncorrectGuesses_1
				.setText("_ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _");
		txtrIncorrectGuesses_1.setFont(new Font("Tahoma", Font.BOLD, 30));
		txtrIncorrectGuesses_1.setEditable(false);
		txtrIncorrectGuesses_1
				.setBackground(UIManager.getColor("Button.background"));
		txtrIncorrectGuesses_1.setBounds(32, 195, 353, 129);
		contentPane.add(txtrIncorrectGuesses_1);

		JPanel panel = new JPanel();
		panel.setBackground(new Color(240, 240, 240));
		panel.setBounds(395, 70, 301, 311);
		contentPane.add(panel);

		ImageIcon imageIcon = new ImageIcon(new ImageIcon("./hang0.png").getImage()
				.getScaledInstance(190, 240, java.awt.Image.SCALE_SMOOTH));
		JLabel label = new JLabel(imageIcon);
		panel.add(label);

		rand = 1;
		JButton btnNewButton = new JButton("Guess");
		btnNewButton.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent e) {
				label.setIcon(
						new ImageIcon(new ImageIcon("./hang" + rand + ".png").getImage()
								.getScaledInstance(190, 240, java.awt.Image.SCALE_SMOOTH)));
				rand++;
				if (rand > 6)
					rand = 0;

			}
		});
		btnNewButton.setFont(new Font("Tahoma", Font.PLAIN, 20));
		btnNewButton.setBounds(230, 316, 96, 37);
		contentPane.add(btnNewButton);

		JMenuBar menuBar = new JMenuBar();
		menuBar.setFont(new Font("Segoe UI", Font.PLAIN, 14));
		menuBar.setBounds(0, 0, 722, 22);
		contentPane.add(menuBar);

		JMenu mnNewMenu = new JMenu("Game");
		mnNewMenu.setFont(new Font("Segoe UI", Font.PLAIN, 14));
		mnNewMenu.setBounds(0, 221, 262, -221);
		menuBar.add(mnNewMenu);

		JMenuItem mntmNewMenuItem = new JMenuItem("New");
		mntmNewMenuItem.setFont(new Font("Segoe UI", Font.PLAIN, 14));
		mnNewMenu.add(mntmNewMenuItem);

		JMenuItem mntmNewMenuItem_1 = new JMenuItem("Save & Quit");
		mntmNewMenuItem_1.setFont(new Font("Segoe UI", Font.PLAIN, 14));
		mnNewMenu.add(mntmNewMenuItem_1);

		JMenuItem mntmNewMenuItem_2 = new JMenuItem("Exit");
		mntmNewMenuItem_2.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent e) {
				setVisible(false);
				dispose();
			}
		});
		mntmNewMenuItem_2.setFont(new Font("Segoe UI", Font.PLAIN, 14));
		mnNewMenu.add(mntmNewMenuItem_2);

		JMenu mnNewMenu2 = new JMenu("Help");
		mnNewMenu2.setFont(new Font("Segoe UI", Font.PLAIN, 14));
		mnNewMenu.setFont(new Font("Segoe UI", Font.PLAIN, 14));
		menuBar.add(mnNewMenu2);

		JMenuItem mntmNewMenuItem_12 = new JMenuItem("Hint");
		mntmNewMenuItem_12.setFont(new Font("Segoe UI", Font.PLAIN, 14));
		mnNewMenu2.add(mntmNewMenuItem_12);

		JMenuItem mntmNewMenuItem2 = new JMenuItem("Score");
		mntmNewMenuItem2.setFont(new Font("Segoe UI", Font.PLAIN, 14));
		mnNewMenu2.add(mntmNewMenuItem2);
		mntmNewMenuItem.setFont(new Font("Segoe UI", Font.PLAIN, 14));
		mntmNewMenuItem_1.setFont(new Font("Segoe UI", Font.PLAIN, 14));

	}
}
