package com.example.gbatta_g30_a03_set_up;

import static android.net.NetworkCapabilities.NET_CAPABILITY_INTERNET;

import android.content.Context;
import android.content.DialogInterface;
import android.content.Intent;
import android.graphics.Color;
import android.graphics.drawable.Drawable;
import android.net.ConnectivityManager;
import android.net.Uri;
import android.os.Bundle;
import android.view.KeyEvent;
import android.view.View;
import android.view.inputmethod.EditorInfo;
import android.view.inputmethod.InputMethodManager;
import android.widget.AdapterView;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ImageButton;
import android.widget.ImageView;
import android.widget.Spinner;
import android.widget.TextView;
import android.widget.Toast;
import android.widget.VideoView;

import androidx.appcompat.app.AlertDialog;
import androidx.appcompat.app.AppCompatActivity;
import androidx.appcompat.view.ContextThemeWrapper;

import java.io.InputStream;
import java.util.Arrays;
import java.util.List;
import java.util.Scanner;

import hangman_game.Hangman;
import hangman_game.HangmanSaveLoad;
import hangman_game.Player;
import hangman_game.Scoreboard;

public class GameActivity extends AppCompatActivity implements View.OnClickListener, AdapterView.OnItemSelectedListener, TextView.OnEditorActionListener {
    private Hangman hangman;
    private Player player;
    private Scoreboard score;
    private Spinner dropdownMenu;
    private EditText fldGuess;
    private ImageView hangImg;
    private ImageButton btnGuess;
    private TextView badGuesses;
    private TextView answerArea;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_game);

        try {
            Intent intent = getIntent();
            if (intent != null) {
                if (intent.hasExtra("Scoreboard"))
                    score = intent.getSerializableExtra("Scoreboard", Scoreboard.class);
                else score = new Scoreboard(this);

                if (intent.hasExtra("Hangman"))
                    hangman = intent.getSerializableExtra("Hangman", Hangman.class);
                else {
                    InputStream inputStream = getAssets().open("word_db.txt");
                    Scanner s = new Scanner(inputStream).useDelimiter("\\A");
                    String path = s.hasNext() ? s.next() : "";
                    hangman = new Hangman(path);
                }

                if (intent.hasExtra("Player"))
                    player = intent.getSerializableExtra("Player", Player.class);
                else player = new Player();
            }
        } catch (Exception e) {
            handleException(e);
        }
        fldGuess = findViewById(R.id.fldGuess);
        dropdownMenu = findViewById(R.id.dropdownMenu);
        btnGuess = findViewById(R.id.btnGuess);
        hangImg = findViewById(R.id.hangmanImg);
        badGuesses = findViewById(R.id.badGuesses);
        answerArea = findViewById(R.id.answerArea);
        refresh();

        List<CharSequence> items = Arrays.asList(getResources().getTextArray(R.array.options));
        CustomSpinnerAdapter adapter = new CustomSpinnerAdapter(this, android.R.layout.simple_spinner_item, items);
        adapter.setDropDownViewResource(R.layout.custom_dropdown_item);
        dropdownMenu.setAdapter(adapter);
        dropdownMenu.setOnItemSelectedListener(this);

        fldGuess.setOnEditorActionListener(this);
        btnGuess.setOnClickListener(this);
    }

    public void onClick(View v) {
        int btnClick = v.getId();
        if (btnClick == btnGuess.getId()) {
            btnGuess.setEnabled(false);
            userGuess();

            btnGuess.postDelayed(() -> btnGuess.setEnabled(true), 1000);
        }
    }


    public void onItemSelected(AdapterView<?> parent, View view, int position, long id) {
        //1New Game
        //2Save
        //3Scoreboard
        //4Rules
        //5Hint
        switch (position) {
            case 1:
                startNewGame();
                break;
            case 2:
                saveGame();
                break;
            case 3:
                viewScoreboard();
                break;
            case 4:
                viewRules();
                break;
            case 5:
                showHintDialog();
                break;
            default:
                break;
        }
        dropdownMenu.setSelection(0);
    }

    private void startNewGame() {
        try {
            score = null;
            hangman = null;
            player = null;

            Intent i1 = new Intent(this, MainActivity.class);
            startActivity(i1);
        } catch (Exception e) {
            handleException(e);
        }
    }

    private void saveGame() {
        try {
            HangmanSaveLoad.saveToFile(this, hangman, player.getName());

            int index = score.findPlayer(player.getName());
            score.getNextPlayer(index).setGamesPlayed(player.getGamesPlayed());
            score.getNextPlayer(index).setGamesWon(player.getGamesWon());

            score.savePlayers(this);
        } catch (Exception e) {
            handleException(e);
        }
    }

    private void viewScoreboard() {
        try {
            Intent i3 = new Intent(this, ScoreActivity.class);
            i3.putExtra("Scoreboard", score);
            startActivity(i3);
        } catch (Exception e) {
            handleException(e);
        }
    }

    private void viewRules() {
        Intent i4 = new Intent(this, RulesActivity.class);
        startActivity(i4);
    }

    private void showHintDialog() {
        try {
            showHintDialog(hangman.getHint() + "");
        } catch (Exception e) {
            handleException(e);
        }
        refresh();
    }

    @Override
    public boolean onEditorAction(TextView v, int actionId, KeyEvent event) {
        boolean done = false;
        if (actionId == EditorInfo.IME_ACTION_DONE || event.getAction() == KeyEvent.ACTION_DOWN)
            done = userGuess();

        return done;
    }

    private boolean userGuess() {
        boolean done = false;

        try {
            if (!fldGuess.getText().toString().isBlank()) {
                if (!hangman.guess(fldGuess.getText().charAt(0)))
                    Toast.makeText(this, "'" + fldGuess.getText().charAt(0) + "' is Incorrect!", Toast.LENGTH_SHORT).show();
                else done = true;
            } else throw new Exception("!You Cannot Guess Nothing!");
        } catch (Exception e1) {
            String message = e1.getMessage();

            if (!isFinishing()) {
                AlertDialog.Builder builder = new AlertDialog.Builder(new ContextThemeWrapper(this, R.style.AlertDialogStyle));
                if (message.charAt(0) == '!') {
                    builder.setTitle("Invalid").setMessage(message.substring(1)).setIcon(android.R.drawable.ic_dialog_info).setPositiveButton("OK", (dialog, which) -> dialog.dismiss());
                } else {
                    builder.setTitle("Error");
                    builder.setMessage("An Unexpected Error Occurred: " + message);
                    builder.setIcon(android.R.drawable.ic_dialog_alert);
                    builder.setPositiveButton("OK", (dialog, which) -> dialog.dismiss());
                }

                AlertDialog dialog = builder.create();

                dialog.setOnShowListener(dialogInterface -> {
                    TextView msgDialog = dialog.findViewById(android.R.id.message);
                    Button b1 = dialog.getButton(AlertDialog.BUTTON_POSITIVE);
                    if (b1 != null) b1.setTextColor(0xFF000000);
                    if (msgDialog != null) msgDialog.setTextSize(20);
                });

                if (!isFinishing()) {
                    dialog.show();
                }
            }
        }

        InputMethodManager imm = (InputMethodManager) getSystemService(INPUT_METHOD_SERVICE);
        imm.hideSoftInputFromWindow(fldGuess.getWindowToken(), 0);

        refresh();
        return done;
    }

    private void refresh() {
        fldGuess.setText("");
        badGuesses.setText("Incorrect Guesses: " + hangman.getIncorrectGuesses());
        answerArea.setText(hangman.getCurrentGuessedAnswer());
        try {
            String imgLink = "hang" + hangman.getIncorrectSize() + ".png";

            InputStream stream = getAssets().open(imgLink);
            Drawable d = Drawable.createFromStream(stream, null);
            hangImg.setImageDrawable(d);

        } catch (Exception e) {
            String message = e.getMessage();

            if (!isFinishing()) {
                AlertDialog.Builder builder = new AlertDialog.Builder(new ContextThemeWrapper(this, R.style.AlertDialogStyle));
                builder.setTitle("Error");
                builder.setMessage("An Unexpected Error Occurred: " + message);
                builder.setIcon(android.R.drawable.ic_dialog_alert);
                builder.setPositiveButton("OK", (dialog, which) -> dialog.dismiss());

                AlertDialog dialog = builder.create();

                dialog.setOnShowListener(dialogInterface -> {
                    TextView msgDialog = dialog.findViewById(android.R.id.message);
                    Button b1 = dialog.getButton(AlertDialog.BUTTON_POSITIVE);
                    if (b1 != null) b1.setTextColor(0xFF000000);
                    if (msgDialog != null) msgDialog.setTextSize(20);
                });

                if (!isFinishing()) {
                    dialog.show();
                }
            }
        }

        if (hangman.getIncorrectSize() > 5) {
            showGameResultDialog("You Lost!", "The Man Was Hanged!\n The Word Was: " + hangman.getCurrentAnswer());
        } else if (hangman.hasWon()) {
            player.setGamesWon(player.getGamesWon() + 1);
            showGameResultDialog("Congratulations!", "You Got The Word!");
        }

        if (hangman.getIncorrectSize() > 5 || hangman.hasWon()) {
            try {
                hangman.nextGame();
                refresh();
            } catch (Exception e) {
                handleException(e);
            }

            try {
                player.setGamesPlayed(player.getGamesPlayed() + 1);

                int index = score.findPlayer(player.getName());
                score.getNextPlayer(index).setGamesPlayed(player.getGamesPlayed());
                score.getNextPlayer(index).setGamesWon(player.getGamesWon());
                score.savePlayers(this);
            } catch (Exception e) {
                handleException(e);
            }
        }
    }

    @Override
    public void onNothingSelected(AdapterView<?> parent) {
    }

    @Override
    public void onBackPressed() {
    }

    private void showAlert(String title, String message, int icon, DialogInterface.OnClickListener onClickListener) {
        AlertDialog.Builder builder = new AlertDialog.Builder(new ContextThemeWrapper(this, R.style.AlertDialogStyle));
        builder.setTitle(title);
        builder.setIcon(icon);
        builder.setMessage(message);
        builder.setPositiveButton("OK", onClickListener);

        AlertDialog dialog = builder.create();
        dialog.setOnShowListener(dialogInterface -> {
            TextView msgDialog = dialog.findViewById(android.R.id.message);
            Button b1 = dialog.getButton(AlertDialog.BUTTON_POSITIVE);
            if (b1 != null) b1.setTextColor(0xFF000000);
            if (msgDialog != null) msgDialog.setTextSize(20);
        });

        if (!isFinishing()) {
            dialog.show();
        }
    }

    private void handleException(Exception e) {
        String message = e.getMessage();

        if (!isFinishing()) {
            if (message.charAt(0) == '!') {
                showAlert("Invalid", message.substring(1), android.R.drawable.ic_dialog_info, (dialog, which) -> dialog.dismiss());
            } else {
                showAlert("Error", "An Unexpected Error Occurred: " + message, android.R.drawable.ic_dialog_alert, (dialog, which) -> finishAffinity());
            }
        }
    }

    private void showGameResultDialog(String title, String message) {
        showAlert(title, message, android.R.drawable.ic_dialog_info, (dialog, which) -> dialog.dismiss());
    }

    private void showHintDialog(String hint) {
        try {
            showAlert("Hint", "Your Hint is '" + hint + "'.", android.R.drawable.ic_dialog_info, (dialog, which) -> dialog.dismiss());
        } catch (Exception e) {
            handleException(e);
        }
    }    
}
