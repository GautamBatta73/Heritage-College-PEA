package com.example.gbatta_g30_a03_set_up;

import android.content.Context;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.TextView;

import java.util.List;

public class CustomSpinnerAdapter extends ArrayAdapter<CharSequence> {
    private final Context context;

    public CustomSpinnerAdapter(Context context, int resource, List<CharSequence> items) {
        super(context, resource, items);
        this.context = context;
    }

    @Override
    public View getDropDownView(int position, View convertView, ViewGroup parent) {
        if (convertView == null) {
            convertView = View.inflate(getContext(), R.layout.custom_dropdown_item, null);
        }

        TextView view = (TextView) convertView;

        if (position == 0) {
            view.setBackgroundColor(0xFF54A5D5);
            view.setPadding(32, 20, 0, 20);
        } else {
            view = (TextView) View.inflate(getContext(), R.layout.custom_dropdown_item, null);
            view.setText(getItem(position));
        }

        return view;
    }

    @Override
    public View getView(int position, View convertView, ViewGroup parent) {
        if (convertView == null) {
            convertView = View.inflate(getContext(), R.layout.custom_dropdown_item, null);
        }

        TextView view = (TextView) convertView;

        if (position == 0) {
            view.setBackgroundColor(0xFF54A5D5);
            view.setPadding(16, 20, 0, 20);
        } else {
            view = (TextView) View.inflate(getContext(), R.layout.custom_dropdown_item, null);
            view.setText(getItem(position));
        }

        return view;
    }
}
