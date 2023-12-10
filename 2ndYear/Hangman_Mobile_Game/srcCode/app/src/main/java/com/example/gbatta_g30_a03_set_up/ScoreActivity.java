package com.example.gbatta_g30_a03_set_up;

import android.content.Intent;
import android.graphics.Typeface;
import android.os.Bundle;
import android.util.TypedValue;
import android.view.Gravity;
import android.view.View;
import android.widget.Button;
import android.widget.TableLayout;
import android.widget.TableRow;
import android.widget.TextView;

import androidx.appcompat.app.AlertDialog;
import androidx.appcompat.app.AppCompatActivity;
import androidx.appcompat.view.ContextThemeWrapper;

import hangman_game.Scoreboard;

public class ScoreActivity extends AppCompatActivity implements View.OnClickListener {

    TableLayout scoresTable;
    String[] column;
    Scoreboard score;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_score);
        findViewById(R.id.btnBack).setOnClickListener(this);

        try {
            Intent intent = getIntent();
            if (intent != null) {
                if (intent.hasExtra("Scoreboard"))
                    score = intent.getSerializableExtra("Scoreboard", Scoreboard.class);
                else score = new Scoreboard(this);
            }
        } catch (Exception e) {
            String message = e.getMessage();

            if (!isFinishing()) {
                AlertDialog.Builder builder = new AlertDialog.Builder(new ContextThemeWrapper(this, R.style.AlertDialogStyle));
                if (message.charAt(0) == '!') {
                    builder.setTitle("Invalid").setMessage(message.substring(1)).setIcon(android.R.drawable.ic_dialog_info).setPositiveButton("OK", (dialog, which) -> {
                        dialog.dismiss();
                        onBackPressed();
                    });
                } else {
                    builder.setTitle("Error");
                    builder.setMessage("An Unexpected Error Occurred: " + message);
                    builder.setIcon(android.R.drawable.ic_dialog_alert);
                    builder.setPositiveButton("OK", (dialog, which) -> {
                        dialog.dismiss();
                        onBackPressed();
                    });
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

        scoresTable = findViewById(R.id.scoresTable);
        column = new String[]{"Name", "Games Played", "Games Won"};

        createTable();
    }

    private void createTable() {
        String[][] row = new String[score.getPlayersNum()][3];

        for (int i = 0; i < score.getPlayersNum(); i++) {
            String formatName = "";
            String[] words = score.getNextPlayer(i).getName().split("\\s");
            for (String word : words) {
                char capital = Character.toUpperCase(word.charAt(0));
                formatName += (capital + word.substring(1) + " ");
            }

            row[i][0] = formatName.trim();
            row[i][1] = (score.getNextPlayer(i).getGamesPlayed() + "");
            row[i][2] = (score.getNextPlayer(i).getGamesWon() + "");
        }

        TableRow headerRow = new TableRow(this);
        for (String header : column) {
            headerRow.addView(addTextView(header, true));
        }
        scoresTable.addView(headerRow);

        for (String[] rowData : row) {
            TableRow tabRow = new TableRow(this);
            for (String value : rowData) {
                tabRow.addView(addTextView(value, false));
            }
            scoresTable.addView(tabRow);
        }
    }

    private TextView addTextView(String value, boolean isHeader) {
        TextView textView = new TextView(this);
        textView.setText(value);
        textView.setTextSize(TypedValue.COMPLEX_UNIT_SP, 20);
        TableRow.LayoutParams params = new TableRow.LayoutParams(TableRow.LayoutParams.WRAP_CONTENT, TableRow.LayoutParams.MATCH_PARENT);
        textView.setPadding(8, 8, 8, 8);
        textView.setBackgroundResource(R.drawable.cell_shape);
        if (isHeader) {
            textView.setTextSize(TypedValue.COMPLEX_UNIT_SP, 25);
            textView.setTextColor(0xFFFFFFFF);
            textView.setGravity(Gravity.CENTER);
            textView.setTypeface(Typeface.DEFAULT, Typeface.BOLD);
            params = new TableRow.LayoutParams(TableRow.LayoutParams.WRAP_CONTENT, TableRow.LayoutParams.MATCH_PARENT, 1f);
        }
        textView.setLayoutParams(params);
        return textView;
    }

    public void onClick(View v) {
        int btnClick = v.getId();
        if (btnClick == R.id.btnBack) onBackPressed();
    }
}