package gbatta_G30_A02_Linked_Lists;

import static org.junit.jupiter.api.Assertions.*;

import java.io.FileNotFoundException;

import org.junit.jupiter.api.MethodOrderer.OrderAnnotation;
import org.junit.jupiter.api.Order;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.TestMethodOrder;

@TestMethodOrder(OrderAnnotation.class)

class HangmanSaveLoadTest {

	@Test
	@Order(1)
	void goodSaveTest() {
		//Check a successful and valid save.
		try {
			Hangman t1 = new Hangman("TEST_word_db.txt");
			t1.guess('a'); //Word is "Awesome!"
			t1.guess(t1.getHint());
			//No incorrect letters

			HangmanSaveLoad.saveToFile(t1, "Gautam");
			assertTrue(true, "Test Case 1: An Exception was not thrown");
		}
		catch (Exception e) {
			fail("Test Case 1: An Exception was thrown");
		}
	} //goodSaveTest()

	@Test
	@Order(2)
	void goodLoadTest() {
		//Check a successful and valid load from the last save.
		try {
			Hangman t2 = HangmanSaveLoad.loadFromFile("Gautam");

			assertEquals(0, t2.getIncorrectGuesses().length(),
					"Test Case 2: No incorrect guesses exist");
		}
		catch (Exception e) {
			fail("Test Case 2: An Exception was thrown");
		}
	} //goodSaveTest()

	@Test
	@Order(3)
	void nullSaveTest() {
		//Try to save a null game (Null Hangman Object).
		Throwable thrown = assertThrows(NullPointerException.class, () -> {
			Hangman t3 = null;
			HangmanSaveLoad.saveToFile(t3, "nullGAME");
		}, "Test case 3 - NullPointerException was not thrown.");

		assertEquals(NullPointerException.class, thrown.getClass(),
				"Test Case 3: NullPointerException was thrown");
	} //nullSaveTest()

	@Test
	@Order(4)
	void noExistLoadTest() {
		//Try to load a non-existent save.
		Throwable thrown = assertThrows(FileNotFoundException.class, () -> {
			Hangman t2 = HangmanSaveLoad.loadFromFile("Batman");
		}, "Test case 4 - An Exception was not thrown.");

		assertEquals(FileNotFoundException.class, thrown.getClass(),
				"Test case 4 - An Exception was thrown.");
	} //noExistLoadTest()

}// HangmanSaveLoadTest class
