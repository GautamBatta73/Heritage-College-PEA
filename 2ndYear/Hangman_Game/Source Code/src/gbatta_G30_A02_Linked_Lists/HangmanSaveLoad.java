/**
 * 
 */
package gbatta_G30_A02_Linked_Lists;

/**
 *Description: This class contains the serialization/deserialization
 *	 for the hangman game.
 *  
 * @author Gautam Batta
 */

import java.io.File;
import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.ObjectInputStream;
import java.io.ObjectOutputStream;

public class HangmanSaveLoad {

	public static void saveToFile(Hangman game, String name) throws Exception {
		if (game == null)
			throw new NullPointerException(
					"Could Not Save Game.\n Please try again.");
		else
			if (name.isBlank())
				throw new Exception("!Name must not be blank");
			else {
				File saveFolder = new File("./saves");
				if (!saveFolder.exists()) {
					boolean created = saveFolder.mkdir();
					if (!created)
						throw new IOException("Could not create the saves folder");
				} //if (!saveFolder.exists()) 

				File file = new File("./saves/" + name.toLowerCase() + ".sav");
				if (!file.exists())
					file.createNewFile();

				FileOutputStream fileOut = new FileOutputStream(file, false);
				ObjectOutputStream out = new ObjectOutputStream(fileOut);
				out.writeObject(game);
				out.flush();
				out.close();
			} //else
	} //saveToFile(Hangman, String)

	public static Hangman loadFromFile(String playerName) throws Exception {
		Hangman game = null;
		File saveFolder = new File("./saves");
		if (playerName.isBlank())
			throw new Exception("!Name must not be blank");
		else {
			if (!saveFolder.exists())
				throw new FileNotFoundException("Could not find the saves folder");

			FileInputStream fileIn = new FileInputStream(
					"./saves/" + playerName.toLowerCase() + ".sav");
			ObjectInputStream in = new ObjectInputStream(fileIn);
			game = (Hangman) in.readObject();
			in.close();
		} //else

		if (game == null)
			throw new FileNotFoundException(
					"Could Not Get Save.\n Please try again.");

		return game;
	} //LoadFromFile(String)

}// HangmanSaveLoad class
