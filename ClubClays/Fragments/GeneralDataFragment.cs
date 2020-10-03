using Android.App;
using Android.Icu.Util;
using Android.OS;
using Android.Views;
using Android.Widget;
using Java.Util;
using System;
using Fragment = AndroidX.Fragment.App.Fragment;

namespace ClubClays.Fragments
{
    public class GeneralDataFragment : Fragment, DatePickerDialog.IOnDateSetListener
    {
        private TextView datePickerView;
        DateTime date;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            View view = inflater.Inflate(Resource.Layout.fragment_general_data, container, false);

            date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            datePickerView = view.FindViewById<TextView>(Resource.Id.datePicker);
            datePickerView.Text = $"{date.ToString("MMMM")} {date.ToString("dd")}, {date.ToString("yyyy")}";
            datePickerView.Click += DatePickerView_Click;

            return view;
        }

        private void DatePickerView_Click(object sender, EventArgs e)
        {
            DatePickerDialog datePicker = new DatePickerDialog(Activity, this, date.Year, date.Month - 1, date.Day);
            TimeSpan diff = DateTime.Now - new DateTime(1970, 1, 1);
            datePicker.DatePicker.MaxDate = (long)diff.TotalMilliseconds;
            datePicker.Show();
        }

        public void OnDateSet(DatePicker view, int year, int month, int dayOfMonth)
        {
            date =  new DateTime(year, month + 1, dayOfMonth);
            datePickerView.Text = $"{date.ToString("MMMM")} {date.ToString("dd")}, {year}";
        }

    }
}