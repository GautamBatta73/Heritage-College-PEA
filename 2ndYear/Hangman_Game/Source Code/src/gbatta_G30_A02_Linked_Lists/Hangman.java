/**
 * 
 */
package gbatta_G30_A02_Linked_Lists;

/**
 *Description: This class contains the logic for the hangman game.
 *  
 * @author Gautam Batta
 */

import java.io.File;
import java.io.FileNotFoundException;
import java.io.Serializable;
import java.util.Scanner;

import linked_data_structures.DoublyLinkedList;
import linked_data_structures.SinglyLinkedList;

public class Hangman implements Serializable {
	private File fileName;
	private DoublyLinkedList<Character> incorrectGuesses;
	private SinglyLinkedList<String> allAnswers;
	private SinglyLinkedList<Character> currentAnswer;
	private SinglyLinkedList<Character> currentGuessedAnswer;
	private int incorrectSize;
	private boolean won;

	public Hangman() {
		fileName = null;
		incorrectGuesses = new DoublyLinkedList<Character>();
		allAnswers = new SinglyLinkedList<String>();
		currentAnswer = new SinglyLinkedList<Character>();
		currentGuessedAnswer = new SinglyLinkedList<Character>();
		incorrectSize = 0;
		won = false;
	} //Hangman()

	public Hangman(String file) throws Exception {
		incorrectGuesses = new DoublyLinkedList<Character>();
		allAnswers = new SinglyLinkedList<String>();
		currentAnswer = new SinglyLinkedList<Character>();
		currentGuessedAnswer = new SinglyLinkedList<Character>();
		incorrectSize = 0;
		won = false;
		setFileName(file);
	} //Hangman(String)

	public void setFileName(String file) throws Exception {
		File temp = new File(file);
		if (temp.isFile())
			fileName = temp;
		else
			throw new FileNotFoundException("Could not find " + file
					+ "\n Make sure it is in the root directory and try again.");

		setAllAnswers();
	} //setFileName(String)

	private void setAllAnswers() throws Exception {
		Scanner scanFile = new Scanner(fileName);

		while (scanFile.hasNext()) {
			String temp = scanFile.nextLine();
			allAnswers.add(temp);
		} //while (scanner.hasNext())

		scanFile.close();
		nextGame();
	} //setAllAnswers()

	public void nextGame() throws Exception {
		won = false;
		if (fileName != null) {
			String currAns = allAnswers.getElementAt(0);
			incorrectGuesses = new DoublyLinkedList<Character>();
			currentAnswer = new SinglyLinkedList<Character>();
			currentGuessedAnswer = new SinglyLinkedList<Character>();
			incorrectSize = 0;

			for (int i = currAns.length() - 1; i >= 0; i--) {
				if (!Character.isLetter(currAns.charAt(i)))
					currentGuessedAnswer.add(currAns.charAt(i));
				else
					currentGuessedAnswer.add('_');

				currentAnswer.add(currAns.charAt(i));
			} //for (int i = 0; i < currAns.length(); i++)
		} //if (fileName != null) 
		else
			throw new NullPointerException(
					"Hangman word file has not been declared.");

		allAnswers.remove(0);
	} //nextGame()

	public boolean guess(char guess) throws Exception {
		boolean found = false;
		if (!won) {
			if (fileName != null) {
				if (Character.isLetter(guess)
						|| getCurrentAnswer().contains(guess + "")) {
					if (incorrectSize > -1 && incorrectSize < 6) {
						char lowerGuess = Character.toLowerCase(guess);
						for (int i = 0; i < currentAnswer.getLength(); i++) {
							char curr = Character.toLowerCase(currentAnswer.getElementAt(i));
							if (curr == lowerGuess) {
								found = true;
								currentGuessedAnswer.remove(i);
								currentGuessedAnswer.add(currentAnswer.getElementAt(i), i);
							} //if (curr == lowerGuess)
						} //for (int i = 0; i < currentAnswer.getLength(); i++)

						for (int i = 0; i < incorrectGuesses.getLength(); i++) {
							char curr = Character
									.toLowerCase(incorrectGuesses.getElementAt(i));
							if (curr == lowerGuess)
								found = true;
						} //for (int i = 0; i < currentAnswer.getLength(); i++)
					} //if (incorrectSize > -1 && incorrectSize < 7)
					else
						throw new IndexOutOfBoundsException("!Out of hints and lives.");

					if (!found) {
						incorrectSize++;
						incorrectGuesses.add(guess);
					} //if (!found)
				} //if (Character.isLetter(guess))
				else
					if (!Character.isLetter(guess))
						throw new Exception("!You Can Not Guess Non-Letters.");
			} //if (fileName != null) 
			else
				throw new NullPointerException(
						"Hangman word file has not been declared.");
		} //if(!won)

		checkAnswer();

		return found;
	} //guess(char)

	public String getIncorrectGuesses() {
		String strIncorrectGuesses = "";
		for (int i = incorrectGuesses.getLength() - 1; i >= 0; i--) {
			if (i > 0)
				strIncorrectGuesses += (incorrectGuesses.getElementAt(i) + ", ");
			else
				strIncorrectGuesses += incorrectGuesses.getElementAt(i);
		} //for (int i = 0; i < incorrectGuesses.getLength(); i++)

		return strIncorrectGuesses;
	} //getIncorrectGuesses()

	public String getCurrentAnswer() {
		String strAnswer = "";
		for (int i = 0; i < currentAnswer.getLength(); i++)
			strAnswer += currentAnswer.getElementAt(i);

		return strAnswer;
	} //getCurrentAnswer()

	public String getCurrentGuessedAnswer() {
		String strCurrentGuessedAnswer = "";

		for (int i = 0; i < currentGuessedAnswer.getLength(); i++) {
			if (i < currentGuessedAnswer.getLength() - 1)
				strCurrentGuessedAnswer += (currentGuessedAnswer.getElementAt(i) + " ");
			else
				strCurrentGuessedAnswer += currentGuessedAnswer.getElementAt(i);
		} //for (int i = 0; i < currentGuessedAnswer.getLength(); i++)

		return strCurrentGuessedAnswer;
	} //getCurrentGuessedAnswer()

	public int getIncorrectSize() {
		return incorrectSize;
	} //getIncorrectSize()

	public char getHint() throws Exception {
		char hint = '0';

		if (!won) {
			if (fileName != null) {
				String currAns = getCurrentGuessedAnswer().replaceAll(" ", "");
				if ((currAns.indexOf("_") == currAns.lastIndexOf("_"))
						|| (getCurrentAnswer()
								.charAt(currAns.indexOf("_")) == getCurrentAnswer()
										.charAt(currAns.lastIndexOf("_"))))
					throw new Exception("!You cannot win with a hint.");
				else
					if (incorrectSize > -1 && incorrectSize < 5)
						hint = getRandChar();
					else
						throw new IndexOutOfBoundsException("!Out of hints and lives.");
			} //if (fileName != null) 
			else
				throw new NullPointerException(
						"Hangman word file has not been declared.");
		} //if(!won)
		else
			throw new Exception(
					"!You have won, so you can stop hanging the poor man.");

		if (hint == '0')
			throw new Exception("Could Not Get Hint.\n Please try again.");

		incorrectSize++;
		return hint;
	} //getHint()

	public boolean hasWon() {
		return won;
	} //hasWon()

	private void checkAnswer() {
		boolean isEqual = true;

		for (int i = 0; i < currentAnswer.getLength() && isEqual; i++) {
			if (currentAnswer.getElementAt(i) != currentGuessedAnswer.getElementAt(i))
				isEqual = false;
		} //for (int i = 0; i < currentAnswer.getLength() && isEqual; i++)

		if (isEqual)
			won = true;
	} //checkAnswer()

	private char getRandChar() throws Exception {
		char randChar = '0';
		String spacedAnswer = getCurrentAnswer().replace("", " ").trim();
		char rand = ' ';

		while (rand == ' ') {
			int randomIndex = (int) (Math.random() * spacedAnswer.length());
			rand = spacedAnswer.charAt(randomIndex);
			if (!getCurrentGuessedAnswer().contains(rand + "") && rand != ' ')
				randChar = rand;
			else
				rand = ' ';
		} //while (rand == ' ')

		return randChar;
	} //getRandChar()

}// Hangman class