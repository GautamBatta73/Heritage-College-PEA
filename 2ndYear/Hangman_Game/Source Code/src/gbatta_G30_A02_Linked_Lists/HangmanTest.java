package gbatta_G30_A02_Linked_Lists;

import static org.junit.jupiter.api.Assertions.*;

import java.io.FileNotFoundException;

import org.junit.jupiter.api.Test;

class HangmanTest {

	@Test
	void fileNotFoundTest() {
		//Check if FileNotFoundException is triggered if the file does not exist.
		Throwable thrown = assertThrows(FileNotFoundException.class, () -> {
			Hangman t1 = new Hangman("notExistingfile.txt");
		}, "Test case 1 - FileNotFoundException was not thrown.");

		assertEquals(FileNotFoundException.class, thrown.getClass(),
				"Test Case 1: FileNotFoundException was thrown");
	} //fileNotFoundTest()

	@Test
	void fileFoundTest() {
		//Check if the file is properly read if the file exists.
		try {
			Hangman t2 = new Hangman("TEST_word_db.txt");
			assertTrue(true, "Test Case 2: An Exception was not thrown");
		}
		catch (Exception e) {
			fail("Test Case 2: An Exception was thrown");
		}
	} //fileFoundTest()

	@Test
	void incorrectGuessBoolTest() {
		//Check if an incorrect guess results in a false.
		try {
			Hangman t3 = new Hangman("TEST_word_db.txt");
			assertEquals(false, t3.guess('z'),
					"Test Case 3: \"z\" is not in the word");
		}
		catch (Exception e) {
			fail("Test Case 3: An Exception was thrown");
		}
	} //incorrectGuessBoolTest()

	@Test
	void incorrectGuessNumTest() {
		//Check if an incorrect guess reflects on the incorrectSize int.
		try {
			Hangman t4 = new Hangman("TEST_word_db.txt");
			t4.guess('z');
			assertEquals(1, t4.getIncorrectSize(),
					"Test Case 4: One incorrect guess exists");
		}
		catch (Exception e) {
			fail("Test Case 4: An Exception was thrown");
		}
	} //incorrectGuessNumTest()

	@Test
	void incorrectGuessListTest() {
		//Check if an incorrect guess reflects on the incorrectGuesses.
		try {
			Hangman t5 = new Hangman("TEST_word_db.txt");
			t5.guess('z');
			assertEquals(true, t5.getIncorrectGuesses().contains("z"),
					"Test Case 5: The incorrect guessed char appears in the list.");
		}
		catch (Exception e) {
			fail("Test Case 5: An Exception was thrown");
		}
	} //incorrectGuessListTest()

	@Test
	void correctGuessBoolTest() {
		//Check if a correct guess results in a true.
		try {
			Hangman t6 = new Hangman("TEST_word_db.txt");
			assertEquals(true, t6.guess('T'), "Test Case 6: \"T\" is in the word"); //Word is "Awesome!"
		}
		catch (Exception e) {
			fail("Test Case 6: An Exception was thrown");
		}
	} //correctGuessBoolTest()

	@Test
	void correctGuessListTest() {
		//Check if a correct guess reflects on the currentGuessedAnswer.
		try {
			Hangman t7 = new Hangman("TEST_word_db.txt");
			t7.guess('t');
			assertEquals(true,
					t7.getCurrentGuessedAnswer().contains("T")
							|| t7.getCurrentGuessedAnswer().contains("t"),
					"Test Case 7: The guessed char appears in the currentGuessedAnwser.");
		}
		catch (Exception e) {
			fail("Test Case 7: An Exception was thrown");
		}
	} //correctGuessListTest()

	@Test
	void hintTest1() {
		//Check if asking for a hint results in losing a life (incrementing the incorrectSize int).
		try {
			Hangman t8 = new Hangman("TEST_word_db.txt");
			t8.getHint();
			assertEquals(1, t8.getIncorrectSize(),
					"Test Case 8: User loses a life, after asking for hint.");
		}
		catch (Exception e) {
			fail("Test Case 8: An Exception was thrown");
		}
	} //hintTest1()

	@Test
	void hintTest2() {
		//Check if asking for a hint results in receiving a valid hint.
		try {
			Hangman t9 = new Hangman("TEST_word_db.txt");
			char hint = t9.getHint();
			assertEquals(true, t9.guess(hint),
					"Test Case 9: User gets a valid hint.");
		}
		catch (Exception e) {
			fail("Test Case 9: An Exception was thrown");
		}
	} //hintTest2()

	@Test
	void isGuessAndAnswerEqual() {
		//Check if the currentAnswer and the currentGuessedAnswer(blanks) are the same size.
		try {
			Hangman t10 = new Hangman("TEST_word_db.txt");
			t10.nextGame(); //Skips word.
			String guessLengthWithoutSpace = t10.getCurrentGuessedAnswer()
					.replaceAll(" ", "");
			String ansLengthWithoutSpace = t10.getCurrentAnswer().replaceAll(" ", "");

			assertEquals(true,
					guessLengthWithoutSpace.length() == ansLengthWithoutSpace.length(),
					"Test Case 10: The currentAnswer and the currentGuessedAnswer(blanks) are the same size.");
		}
		catch (Exception e) {
			fail("Test Case 10: An Exception was thrown");
		}
	} //hintTest2()

	@Test
	void noFileGame() {
		//Check if NullPointerException is triggered if the default constructor is used(no file given)
		//	and the the user tries to go to start a new game.
		Throwable thrown = assertThrows(NullPointerException.class, () -> {
			Hangman t11 = new Hangman();
			t11.nextGame();
		}, "Test case 11: - NullPointerException was not thrown.");

		assertEquals(NullPointerException.class, thrown.getClass(),
				"Test Case 11: NullPointerException was thrown");
	} //noFileGame()

	@Test
	void noFileGuess() {
		//Check if NullPointerException is triggered if the default constructor is used(no file given)
		//	and the the user tries guess a letter.
		Throwable thrown = assertThrows(NullPointerException.class, () -> {
			Hangman t11 = new Hangman();
			t11.guess('s');
		}, "Test case 12: - NullPointerException was not thrown.");

		assertEquals(NullPointerException.class, thrown.getClass(),
				"Test Case 12: NullPointerException was thrown");
	} //noFileGuess()

	@Test
	void noFileHint() {
		//Check if NullPointerException is triggered if the default constructor is used(no file given)
		//	and the the user tries guess a letter.
		Throwable thrown = assertThrows(NullPointerException.class, () -> {
			Hangman t11 = new Hangman();
			t11.getHint();
		}, "Test case 13: - NullPointerException was not thrown.");

		assertEquals(NullPointerException.class, thrown.getClass(),
				"Test Case 13: NullPointerException was thrown");
	} //noFileHint()

}// HangmanTest class
