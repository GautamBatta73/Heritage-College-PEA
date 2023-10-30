package gbatta_G30_A02_Linked_Lists;

import static org.junit.jupiter.api.Assertions.*;

import java.io.FileNotFoundException;

import org.junit.jupiter.api.Test;

class ScoreboardTest {

	@Test
	void addPlayerTest() {
		//Check a successful and valid add to the playerList.
		try {
			Scoreboard t1 = new Scoreboard();
			int num1 = t1.getPlayersNum();
			t1.addPlayer(new Player("Joe"));
			int num2 = t1.getPlayersNum();

			assertEquals(true, num2 > num1, "Test Case 1: A player was added.");
		}
		catch (Exception e) {
			fail("Test Case 1: An Exception was thrown");
		}
	} //addPlayerTest()

	@Test
	void addNullPlayerTest() {
		//Check a unsuccessful and null add to the playerList.
		Throwable thrown = assertThrows(NullPointerException.class, () -> {
			Scoreboard t2 = new Scoreboard();
			t2.addPlayer(null);
		}, "Test case 2 - NullPointerException was not thrown.");

		assertEquals(NullPointerException.class, thrown.getClass(),
				"Test Case 2: NullPointerException was thrown");
	} //addNullPlayerTest() 

	@Test
	void playerFound() {
		//Check if a valid player is found.
		try {
			Scoreboard t3 = new Scoreboard();
			t3.addPlayer(new Player("Joe"));
			t3.addPlayer(new Player("Moe"));
			t3.addPlayer(new Player("Woe"));
			t3.addPlayer(new Player("Po"));
			int index = t3.findPlayer("woe");

			assertEquals(true, (index != -1), "Test Case 3: \'Woe\' Was Found.");
		}
		catch (Exception e) {
			fail("Test Case 3: An Exception was thrown");
		}
	} //playerFound()

	@Test
	void playerNotFound() {
		//Check if a player is not found.
		try {
			Scoreboard t4 = new Scoreboard();
			t4.addPlayer(new Player("Joe"));
			t4.addPlayer(new Player("Moe"));
			t4.addPlayer(new Player("Woe"));
			t4.addPlayer(new Player("Po"));
			int index = t4.findPlayer("no");

			assertEquals(true, (index == -1), "Test Case 4: \'No\' Was Not Found.");
		}
		catch (Exception e) {
			fail("Test Case 4: An Exception was thrown");
		}
	} //playerNotFound()

	@Test
	void validNextPlayer() {
		//Check if a valid index is searched for Player.
		try {
			Scoreboard t5 = new Scoreboard();
			t5.addPlayer(new Player("Joe"));
			t5.addPlayer(new Player("Moe"));
			t5.addPlayer(new Player("Woe"));
			t5.addPlayer(new Player("Po"));
			Player t5a = t5.getNextPlayer(t5.getPlayersNum() - 1);

			assertEquals(Player.class, t5a.getClass(),
					"Test Case 5: The Player was found.");
		}
		catch (Exception e) {
			fail("Test Case 5: An Exception was thrown");
		}
	} //validNextPlayer()

	@Test
	void invalidNextPlayer() {
		//Check if an invalid index is searched for Player.
		try {
			Scoreboard t6 = new Scoreboard();
			t6.addPlayer(new Player("Joe"));
			t6.addPlayer(new Player("Moe"));
			t6.addPlayer(new Player("Woe"));
			t6.addPlayer(new Player("Po"));
			Player t6a = t6.getNextPlayer(t6.getPlayersNum() + 1);

			assertEquals(null, t6a, "Test Case 6: The Player was not found.");
		}
		catch (Exception e) {
			fail("Test Case 6: An Exception was thrown");
		}
	} //invalidNextPlayer()

}
