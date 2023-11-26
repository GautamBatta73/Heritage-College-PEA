package com.example.gbatta_g30_a03_set_up;

import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.TextView;

import androidx.appcompat.app.AlertDialog;
import androidx.appcompat.app.AppCompatActivity;
import androidx.appcompat.view.ContextThemeWrapper;

import java.io.FileNotFoundException;
import java.io.InputStream;
import java.util.Scanner;
import java.util.concurrent.atomic.AtomicBoolean;

import hangman_game.Hangman;
import hangman_game.HangmanSaveLoad;
import hangman_game.Player;
import hangman_game.Scoreboard;

public class MainActivity extends AppCompatActivity implements View.OnClickListener {

    private EditText fldName;
    private Scoreboard score;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        fldName = findViewById(R.id.fldName);
        score = null;

        Button goGameBtn = findViewById(R.id.GoGameBtn);
        goGameBtn.setOnClickListener(this);
    }

    @Override
    public void onClick(View v) {
        if (v.getId() == R.id.GoGameBtn) {
            handleGoGameButtonClick();
        }
    }

    private void handleGoGameButtonClick() {
        AtomicBoolean valid = new AtomicBoolean(false);
        Hangman hangman = null;
        Player player = new Player();
        score = null;
        String name = fldName.getText().toString().trim().toLowerCase();

        try {
            InputStream inputStream = getAssets().open("word_db.txt");
            Scanner s = new Scanner(inputStream).useDelimiter("\\A");
            String path = s.hasNext() ? s.next() : "";
            hangman = new Hangman(path);
            score = new Scoreboard(this);
        } catch (Exception e) {
            handleException(e);
        }

        try {
            hangman = HangmanSaveLoad.loadFromFile(this, name);
            valid.set(true);
        } catch (FileNotFoundException e1) {
            handleNewGameFileNotFound(name, hangman, player);
        } catch (Exception e1) {
            handleCorruptSave(name, hangman, player);
        }

        if (valid.get()) {
            goGame(hangman, player, name);
        }
    }

    private void handleNewGameFileNotFound(String name, Hangman hangman, Player player) {
        AlertDialog.Builder builder = new AlertDialog.Builder(new ContextThemeWrapper(this, R.style.AlertDialogStyle));
        builder.setTitle("New Game/Save Not Found");
        builder.setMessage("There is No Saved Game For That Name.\n Do You Want to Create/Start a New Game?");
        builder.setIcon(android.R.drawable.ic_dialog_alert);
        builder.setPositiveButton("Yes", (dialog, which) -> {
            try {
                HangmanSaveLoad.saveToFile(this, hangman, name);
            } catch (Exception e) {
                handleException(e);
            }
            goGame(hangman, player, name);
        });
        builder.setNegativeButton("No", (dialog, which) -> dialog.dismiss());

        showAlertDialog(builder);
    }

    private void handleCorruptSave(String name, Hangman hangman, Player player) {
        AlertDialog.Builder builder = new AlertDialog.Builder(new ContextThemeWrapper(this, R.style.AlertDialogStyle));
        builder.setTitle("Corrupted Save");
        builder.setMessage("The Save File For That Name Cannot Be Loaded.\n Do You Want to Create/Start a New Game?");
        builder.setIcon(android.R.drawable.ic_dialog_alert);
        builder.setPositiveButton("Yes", (dialog, which) -> {
            try {
                HangmanSaveLoad.saveToFile(this, hangman, name);
            } catch (Exception e) {
                handleException(e);
            }
            goGame(hangman, player, name);
        });
        builder.setNegativeButton("No", (dialog, which) -> dialog.dismiss());

        showAlertDialog(builder);
    }

    private void goGame(Hangman hangman, Player player, String name) {
        player.setName(name);
        player.setGamesPlayed(0);
        player.setGamesWon(0);
        fldName.setText("");

        try {
            int index = score.findPlayer(player.getName());
            if (index < 0) {
                score.addPlayer(player);
            } else {
                player.setGamesPlayed(score.getNextPlayer(index).getGamesPlayed());
                player.setGamesWon(score.getNextPlayer(index).getGamesWon());
            }
        } catch (Exception e) {
            handleException(e);
        }

        startGameActivity(hangman, player);
    }

    private void startGameActivity(Hangman hangman, Player player) {
        Intent intent = new Intent(this, GameActivity.class);
        intent.putExtra("Hangman", hangman);
        intent.putExtra("Player", player);
        intent.putExtra("Scoreboard", score);
        startActivity(intent);
    }

    private void handleException(Exception e) {
        String message = e.getMessage();
        if (!isFinishing()) {
            AlertDialog.Builder builder = new AlertDialog.Builder(new ContextThemeWrapper(this, R.style.AlertDialogStyle));
            if (message.charAt(0) == '!') {
                builder.setTitle("Invalid").setMessage(message.substring(1)).setIcon(android.R.drawable.ic_dialog_info).setPositiveButton("OK", (dialog, which) -> dialog.dismiss());
            } else {
                builder.setTitle("Error");
                builder.setMessage("An Unexpected Error Occurred: " + message);
                builder.setIcon(android.R.drawable.ic_dialog_alert);
                builder.setPositiveButton("OK", (dialog, which) -> finishAffinity());
            }

            showAlertDialog(builder);
        }
    }

    private void showAlertDialog(AlertDialog.Builder builder) {
        AlertDialog dialog = builder.create();
        dialog.setOnShowListener(dialogInterface -> {
            TextView msgDialog = dialog.findViewById(android.R.id.message);
            Button b1 = dialog.getButton(AlertDialog.BUTTON_POSITIVE);
            Button b2 = dialog.getButton(AlertDialog.BUTTON_NEGATIVE);
            if (b1 != null) {
                b1.setTextColor(0xFF000000);
                b1.setTextSize(20);
            }
            if (b2 != null) {
                b2.setTextColor(0xFF000000);
                b2.setTextSize(20);
            }
            if (msgDialog != null) msgDialog.setTextSize(20);
        });

        if (!isFinishing()) {
            dialog.show();
        }
    }

    @Override
    public void onBackPressed() {
    }
}