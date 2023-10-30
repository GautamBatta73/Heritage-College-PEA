package gbatta_G30_A02_Linked_Lists;

import java.util.Scanner;

public class Test {
	public static void main(String[] args) {
		try {
			Scoreboard score = new Scoreboard();
			score.addPlayer(new Player("Gautam"));
			score.getNextPlayer(0).setGamesWon(1);

			score.addPlayer(new Player("John"));
			score.getNextPlayer(0).setGamesPlayed(1);

			score.addPlayer(new Player("Me"));
			score.getNextPlayer(0).setGamesPlayed(1);
			score.getNextPlayer(0).setGamesWon(1);

			for (int i = 0; i < score.getPlayersNum(); i++)
				System.out.println(score.getNextPlayer(i).toString() + "\n");

		}
		catch (Exception e) {
			e.printStackTrace();
		}
	}
}