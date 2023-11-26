/**
 *
 */
package hangman_game;

import android.content.Context;

import java.io.File;
import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.ObjectInputStream;
import java.io.ObjectOutputStream;
import java.io.Serializable;

import linked_data_structures.DoublyLinkedList;

/**
 * Description: This class contains the scoreboard for the hangman game.
 *
 * @author Gautam Batta
 */

public class Scoreboard implements Serializable {
    private int playersNum;
    private DoublyLinkedList<Player> playerList;

    public Scoreboard(Context context) throws Exception {
        loadPlayers(context);
        playersNum = playerList.getLength();
        sortList();
    } //Scoreboard()

    public void addPlayer(Player player) throws Exception {
        if (player != null) {
            playerList.add(player);
            playersNum++;
        } //if(name != null)
        else throw new NullPointerException("Player must not be Null.\n Please try again.");

        sortList();
    } //addPlayer(String)

    public int playerGamesPlayed(String name, boolean gamesWon) throws Exception {
        int gamesPlayed = 0;

        if (findPlayer(name) > -1) {
            int index = findPlayer(name);
            gamesPlayed = getNextPlayer(index).getGamesPlayed();
            if (gamesWon) gamesPlayed += getNextPlayer(index).getGamesWon();
        } //if (found)
        else throw new Exception("Could Not Find The Player.\n Please try again.");

        return gamesPlayed;
    } //playerGamesPlayed(String, boolean)

    public Player getNextPlayer(int index) {
        Player next = null;

        if (index > -1 && index < playersNum) next = playerList.getElementAt(index);

        return next;
    } //getNextPlayer(int)

    public int getPlayersNum() {
        return playersNum;
    } //getPlayersNum()

    public int findPlayer(String name) {
        int foundIndex = -1;
        boolean found = false;

        for (int i = 0; i < playerList.getLength() && !found; i++) {
            Player curr = playerList.getElementAt(i);
            if (curr.getName().equalsIgnoreCase(name)) {
                foundIndex = i;
                found = true;
            } //if ((curr.getName()).equalsIgnoreCase(name))
        } //for (int i = 0; i < playerList.getLength() && !found; i++)

        return foundIndex;
    } //findPlayer(String)

    public void savePlayers(Context context) throws Exception {
        File file = new File(context.getFilesDir(), "score.ser");
        if (!file.exists()) file.createNewFile();

        FileOutputStream fileOut = new FileOutputStream(file, false);
        ObjectOutputStream out = new ObjectOutputStream(fileOut);
        out.writeObject(playerList);
        out.flush();
        out.close();
    } //savePlayers()

    private void loadPlayers(Context context) throws Exception {
        try {
            File file = new File(context.getFilesDir(), "score.ser");

            FileInputStream fileIn = new FileInputStream(file);
            ObjectInputStream in = new ObjectInputStream(fileIn);
            playerList = (DoublyLinkedList<Player>) in.readObject();
            in.close();
        } //try
        catch (FileNotFoundException e) {
            playerList = new DoublyLinkedList<Player>();
            savePlayers(context);
        }
    } //loadPlayers()

    private void sortList() {
        for (int i = 0; i < playersNum - 1; i++) {
            for (int j = 0; j < playersNum - i - 1; j++) {
                Player curr = playerList.getElementAt(j);
                Player next = playerList.getElementAt(j + 1);
                if (curr.getName().compareToIgnoreCase(next.getName()) > 0) {
                    Player temp = curr;
                    playerList.remove(j);
                    playerList.add(next, j);
                    playerList.remove(j + 1);
                    playerList.add(temp, j + 1);
                } //if (curr.getName().compareToIgnoreCase(next.getName()) > 0)
            } //for (int j = 0; j < playersNum - i - 1; j++)
        } //for (int i = 0; i < playersNum - 1; i++)
    } //sortList()

}// Scoreboard class
