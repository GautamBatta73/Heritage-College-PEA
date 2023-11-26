/**
 *
 */
package hangman_game;

import java.io.Serializable;

/**
 * Description: This class represents a player for the hangman game.
 *
 * @author Gautam Batta
 */

public class Player implements Serializable {
    private String name;
    private int gamesPlayed;
    private int gamesWon;

    public Player() {
        name = "Unknown";
        gamesPlayed = 0;
        gamesWon = 0;
    } //Player()

    public Player(String playerName) {
        name = playerName;
        gamesPlayed = 0;
        gamesWon = 0;
    } //Player(String)

    public String getName() {
        return name;
    } //getName()

    public void setName(String playerName) {
        name = playerName;
    } //setName(String)

    public int getGamesWon() {
        return gamesWon;
    } //getGamesWon()

    public void setGamesWon(int num) {
        gamesWon = num;
    } //setGamesWon(int)

    public int getGamesPlayed() {
        return gamesPlayed;
    } //getGamesPlayed()

    public void setGamesPlayed(int num) {
        gamesPlayed = num;
    } //setGamesPlayed(int)

    public String toString() {
        return "Name: " + name + "\nGames Played: " + gamesPlayed + "\nGames Won: " + gamesWon;
    } //toString()

    public boolean equals(Object obj) {
        boolean isEqual = true;

        if (obj instanceof Player) {
            Player objPlayer = (Player) obj;
            if (!objPlayer.getName().equalsIgnoreCase(name)) isEqual = false;
        } //if(obj instanceof Player)
        else isEqual = false;

        return isEqual;
    } //equals(Object)

}// Player class
