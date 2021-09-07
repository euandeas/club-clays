using Android.OS;
using Android.Views;
using Android.Widget;
using System;
using Fragment = AndroidX.Fragment.App.Fragment;
using FragmentTransaction = AndroidX.Fragment.App.FragmentTransaction;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;
using AndroidX.AppCompat.App;
using Google.Android.Material.TextField;
using Google.Android.Material.FloatingActionButton;
using Google.Android.Material.DatePicker;

namespace ClubClays.Fragments
{
    public class ShootCreationFragment : Fragment, IMaterialPickerOnPositiveButtonClickListener
    {
        private TextView datePickerView;
        private TextInputEditText locationInput;
        private MaterialDatePicker picker;
        private DateTime date;
        private TextInputEditText titleInput;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            View view = inflater.Inflate(Resource.Layout.fragment_shoot_creation, container, false);

            Toolbar toolbar = view.FindViewById<Toolbar>(Resource.Id.toolbar);
            ((AppCompatActivity)Activity).SetSupportActionBar(toolbar);
            ActionBar supportBar = ((AppCompatActivity)Activity).SupportActionBar;
            supportBar.SetDisplayHomeAsUpEnabled(true);
            supportBar.SetDisplayShowHomeEnabled(true);

            titleInput = view.FindViewById<TextInputEditText>(Resource.Id.titleEditText);

            date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            datePickerView = view.FindViewById<TextInputEditText>(Resource.Id.dateEditText);
            datePickerView.Text = $"{date:MMMM} {date:dd}, {date:yyyy}";
            datePickerView.Click += DatePickerView_Click;

            locationInput = view.FindViewById<TextInputEditText>(Resource.Id.locationEditText);

            FloatingActionButton fab = view.FindViewById<FloatingActionButton>(Resource.Id.nextButton);
            fab.Click += NextButton_Click;

            return view;
        }

        private void NextButton_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(titleInput.Text))
            {
                titleInput.Text = "";
            }
            
            if (string.IsNullOrWhiteSpace(locationInput.Text))
            {
                locationInput.Text = "";
            }

            RoundCreationFragment fragment = new RoundCreationFragment();
            Bundle args = new Bundle();
            args.PutString("shootTitle", titleInput.Text);
            args.PutString("shootLocation", locationInput.Text);
            args.PutLong("shootDate", date.Ticks);
            args.PutInt("shootID", 0);

            fragment.Arguments = args;

            FragmentTransaction fragmentTx = Activity.SupportFragmentManager.BeginTransaction();             
            fragmentTx.Replace(Resource.Id.container, fragment);
            fragmentTx.Commit();       
        }

        private void DatePickerView_Click(object sender, EventArgs e)
        {
            TimeSpan diff = DateTime.Now - new DateTime(1970, 1, 1);
            var constraintsBuilder = new CalendarConstraints.Builder();
            constraintsBuilder.SetValidator(DateValidatorPointBackward.Before((long)diff.TotalMilliseconds));
            constraintsBuilder.SetEnd((long)diff.TotalMilliseconds);

            MaterialDatePicker.Builder mDatePicker = MaterialDatePicker.Builder.DatePicker();
            mDatePicker.SetSelection((long)(date - new DateTime(1970, 1, 1)).TotalMilliseconds);
            mDatePicker.SetCalendarConstraints(constraintsBuilder.Build());
            picker = mDatePicker.Build();
            picker.AddOnPositiveButtonClickListener(this);

            picker.Show(ChildFragmentManager, "");
        }

        public void OnPositiveButtonClick(Java.Lang.Object p0)
        {
            date = new DateTime(1970, 1, 1).Add(TimeSpan.FromMilliseconds((double)p0));
            datePickerView.Text = $"{date:MMMM} {date:dd}, {date:yyyy}";
        }
    }
}