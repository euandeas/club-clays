using Android.OS;
using Android.Views;
using Android.Widget;
using System;
using Fragment = AndroidX.Fragment.App.Fragment;
using FragmentTransaction = AndroidX.Fragment.App.FragmentTransaction;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;
using AndroidX.AppCompat.App;
using AndroidX.Lifecycle;
using SQLite;
using System.IO;
using ClubClays.DatabaseModels;
using AndroidX.Fragment.App;
using Google.Android.Material.TextField;
using Google.Android.Material.FloatingActionButton;
using Google.Android.Material.DatePicker;

namespace ClubClays.Fragments
{
    public class GeneralDataFragment : Fragment, IMaterialPickerOnPositiveButtonClickListener, IFragmentResultListener
    {
        private TextView datePickerView;
        private string discipline;
        private TextInputEditText locationInput;
        private MaterialDatePicker picker;
        private DateTime date;

        private TextView shootersSelection;
        private TextView standFormatting;

        private ShooterStandData standShooterModel;
        private TextInputEditText titleInput;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
            string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "ClubClaysData.db3");
            SQLiteConnection db = new SQLiteConnection(dbPath);
            standShooterModel = new ViewModelProvider(Activity).Get(Java.Lang.Class.FromType(typeof(ShooterStandData))) as ShooterStandData;
            standShooterModel.allShooters = db.Table<Shooters>().ToList();
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

            titleInput = view.FindViewById<TextInputEditText>(Resource.Id.titleEditText);

            AutoCompleteTextView spinner = view.FindViewById<AutoCompleteTextView>(Resource.Id.disciplineDropdown);
            spinner.ItemClick += Spinner_ItemSelected;
            var adapter = ArrayAdapter.CreateFromResource(view.Context, Resource.Array.disciplines, Android.Resource.Layout.SimpleSpinnerItem);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinner.Adapter = adapter;

            string defaultItem = spinner.Adapter.GetItem(0).ToString();
            spinner.SetText(defaultItem, false);
            discipline = defaultItem;

            date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            datePickerView = view.FindViewById<TextInputEditText>(Resource.Id.dateEditText);
            datePickerView.Text = $"{date:MMMM} {date:dd}, {date:yyyy}";
            datePickerView.Click += DatePickerView_Click;

            locationInput = view.FindViewById<TextInputEditText>(Resource.Id.locationEditText);

            shootersSelection = view.FindViewById<TextInputEditText>(Resource.Id.shootersEditText);
            shootersSelection.Text = "0 Shooter(s) Selected";
            shootersSelection.Click += ShootersSelection_Click;

            standFormatting = view.FindViewById<TextInputEditText>(Resource.Id.standsEditText);
            standFormatting.Text = "Blank";
            standFormatting.Click += StandFormatting_Click;

            FloatingActionButton fab = view.FindViewById<FloatingActionButton>(Resource.Id.nextButton);
            fab.Click += NextButton_Click;

            return view;
        }

        private void StandFormatting_Click(object sender, EventArgs e)
        {
            FragmentTransaction fragmentTx = Activity.SupportFragmentManager.BeginTransaction();
            ShootFormatsFragment standSetupFragment = new ShootFormatsFragment();
            Bundle args = new Bundle();
            args.PutBoolean("selectable", true);
            standSetupFragment.Arguments = args;
            fragmentTx.Replace(Resource.Id.container, standSetupFragment);
            fragmentTx.AddToBackStack(null);
            fragmentTx.Commit();
            Activity.SupportFragmentManager.SetFragmentResultListener("2", this, this);
        }

        private void NextButton_Click(object sender, EventArgs e)
        {
            if (standShooterModel.selectedShooters.Count == 0)
            {
                Toast.MakeText(Activity, "No shooters selected!", ToastLength.Short).Show();
                return;
            }

            if (string.IsNullOrWhiteSpace(titleInput.Text))
            {
                titleInput.Text = "";
            }
            
            if (string.IsNullOrWhiteSpace(locationInput.Text))
            {
                locationInput.Text = "";
            }

            FragmentTransaction fragmentTx = Activity.SupportFragmentManager.BeginTransaction();

            Shoot activeShootModel = new ViewModelProvider(Activity).Get(Java.Lang.Class.FromType(typeof(Shoot))) as Shoot;
            activeShootModel.Initialise(standShooterModel.selectedShooters, standShooterModel.selectedFormat, date, locationInput.Text, discipline, titleInput.Text);    
            fragmentTx.Replace(Resource.Id.container, new ScoreTakingFragment());
            fragmentTx.Commit();       
        }

        private void ShootersSelection_Click(object sender, EventArgs e)
        { 
            FragmentTransaction fragmentTx = Activity.SupportFragmentManager.BeginTransaction();
            ShootersFragment shootersFragment = new ShootersFragment();
            fragmentTx.Replace(Resource.Id.container, shootersFragment);
            fragmentTx.AddToBackStack(null);
            fragmentTx.Commit();
            Activity.SupportFragmentManager.SetFragmentResultListener("1", this, this);
        }

        private void Spinner_ItemSelected(object sender, AdapterView.ItemClickEventArgs e)
        {
            discipline = (sender as AutoCompleteTextView).Text;
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

        public void OnFragmentResult(string p0, Bundle p1)
        {
            if (p0 == "1")
            {
                shootersSelection.Text = $"{p1.GetInt("numSelected", 0)} Shooter(s) Selected";
            }
            else if (p0 == "2")
            {
                standFormatting.Text = $"{p1.GetString("titleText")}";
            }
        }
    }
}
