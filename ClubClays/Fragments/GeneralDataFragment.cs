using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using System;
using Fragment = AndroidX.Fragment.App.Fragment;
using FragmentTransaction = AndroidX.Fragment.App.FragmentTransaction;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;
using DatePickerDialog = Android.App.DatePickerDialog;
using AndroidX.AppCompat.App;

namespace ClubClays.Fragments
{
    public class GeneralDataFragment : Fragment, DatePickerDialog.IOnDateSetListener
    {
        private TextView datePickerView;
        private AlertDialog trackingTypeDialog;
        private TextView trackingTypePickerView;
        private string trackingType;
        private string discipline;
        private DateTime date;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            View view = inflater.Inflate(Resource.Layout.fragment_general_data, container, false);
            
            Toolbar toolbar = view.FindViewById<Toolbar>(Resource.Id.toolbar);
            ((AppCompatActivity)Activity).SetSupportActionBar(toolbar);
            ActionBar supportBar = ((AppCompatActivity)Activity).SupportActionBar;
            supportBar.SetDisplayHomeAsUpEnabled(true);
            supportBar.SetDisplayShowHomeEnabled(true);

            trackingTypeDialog = TrackingTypeDialogBuilder();
            trackingTypeDialog.Show();
            trackingTypePickerView = view.FindViewById<TextView>(Resource.Id.trackingtypePicker);
            trackingTypePickerView.Click += TrackingTypePickerView_Click;

            Spinner spinner = view.FindViewById<Spinner>(Resource.Id.disciplinesPicker);
            spinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);
            var adapter = ArrayAdapter.CreateFromResource(view.Context, Resource.Array.disciplines, Android.Resource.Layout.SimpleSpinnerItem);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinner.Adapter = adapter;

            date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            datePickerView = view.FindViewById<TextView>(Resource.Id.datePicker);
            datePickerView.Text = $"{date:MMMM} {date:dd}, {date:yyyy}";
            datePickerView.Click += DatePickerView_Click;

            var shootersSelection = view.FindViewById<TextView>(Resource.Id.shootersPicker);
            shootersSelection.Text = "0 Shooter(s) Selected";
            shootersSelection.Click += ShootersSelection_Click;

            return view;
        }

        private void ShootersSelection_Click(object sender, EventArgs e)
        {
            FragmentTransaction fragmentTx = Activity.SupportFragmentManager.BeginTransaction();
            ShootersFragment shootersFragment = new ShootersFragment();
            shootersFragment.SetTargetFragment(this, 1);
            fragmentTx.Add(Resource.Id.container, shootersFragment);
            fragmentTx.AddToBackStack(null);
            fragmentTx.Commit();
        }

        public override void OnActivityResult(int requestCode, int resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
        }

        private void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {          
            discipline = (sender as Spinner).GetItemAtPosition(e.Position).ToString();
        }

        private void TrackingTypePickerView_Click(object sender, EventArgs e)
        {
            trackingTypeDialog.Show();
        }

        private AlertDialog TrackingTypeDialogBuilder()
        {
            AlertDialog.Builder dialogBuilder = new AlertDialog.Builder(Activity);
            dialogBuilder.SetTitle("Tracking Type");
            dialogBuilder.SetSingleChoiceItems(Resource.Array.tracking_types, -1, new EventHandler<DialogClickEventArgs>(DialogOnClickListerner));
            return dialogBuilder.Create();
        }

        private void DialogOnClickListerner(object sender, DialogClickEventArgs e)
        {
            switch (e.Which)
            {
                case 0:
                    trackingType = "New Format";
                    break;
                case 1:
                    trackingType = "Track Format";
                    break;
                case 2:
                    trackingType = "Load Format";
                    break;
                case 3:
                    trackingType = "Add Previous";
                    break;
            }
            trackingTypePickerView.Text = trackingType;
            (sender as AlertDialog).Dismiss();
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
            datePickerView.Text = $"{date:MMMM} {date:dd}, {year}";
        }

    }
}