package com.example.gbatta_g30_a03_set_up;

import android.os.Bundle;
import android.view.View;

import androidx.appcompat.app.AppCompatActivity;

public class AboutActivity extends AppCompatActivity implements View.OnClickListener {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_about);
        findViewById(R.id.btnBack).setOnClickListener(this);
    }

    public void onClick(View v) {
        int btnClick = v.getId();
        if (btnClick == R.id.btnBack)
            onBackPressed();
    }
}