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

namespace ClubClays.Fragments
{
    public class RoundCreationFragment : Fragment, IFragmentResultListener
    {
        private string discipline;

        private TextView shootersSelection;
        private TextView standFormatting;
        AutoCompleteTextView subdisciplineSpinner;

        private ShooterStandData standShooterModel;

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
            View view = inflater.Inflate(Resource.Layout.fragment_round_creation, container, false);

            Toolbar toolbar = view.FindViewById<Toolbar>(Resource.Id.toolbar);
            ((AppCompatActivity)Activity).SetSupportActionBar(toolbar);
            ActionBar supportBar = ((AppCompatActivity)Activity).SupportActionBar;
            supportBar.SetDisplayHomeAsUpEnabled(true);
            supportBar.SetDisplayShowHomeEnabled(true);

            RadioGroup disciplinesRadioGroup = view.FindViewById<RadioGroup>(Resource.Id.disciplines);
            disciplinesRadioGroup.CheckedChange += DisciplinesRadioGroup_Click;

            subdisciplineSpinner = view.FindViewById<AutoCompleteTextView>(Resource.Id.subdisciplineDropdown);
            subdisciplineSpinner.ItemClick += Spinner_ItemSelected;
            var adapter = ArrayAdapter.CreateFromResource(view.Context, Resource.Array.trap, Android.Resource.Layout.SimpleSpinnerItem);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            subdisciplineSpinner.Adapter = adapter;

            string defaultItem = subdisciplineSpinner.Adapter.GetItem(0).ToString();
            subdisciplineSpinner.SetText(defaultItem, false);
            discipline = defaultItem;

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

        private void DisciplinesRadioGroup_Click(object sender, EventArgs e)
        {    
            switch (((RadioGroup)sender).CheckedRadioButtonId)
            {
                case Resource.Id.trap:
                    subdisciplineSpinner.ItemClick += Spinner_ItemSelected;
                    var adapter1 = ArrayAdapter.CreateFromResource(((RadioGroup)sender).Context, Resource.Array.trap, Android.Resource.Layout.SimpleSpinnerItem);
                    adapter1.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
                    subdisciplineSpinner.Adapter = adapter1;
                    break;
                case Resource.Id.skeet:
                    subdisciplineSpinner.ItemClick += Spinner_ItemSelected;
                    var adapter2 = ArrayAdapter.CreateFromResource(((RadioGroup)sender).Context, Resource.Array.skeet, Android.Resource.Layout.SimpleSpinnerItem);
                    adapter2.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
                    subdisciplineSpinner.Adapter = adapter2;
                    break;
                case Resource.Id.sporting:
                    subdisciplineSpinner.ItemClick += Spinner_ItemSelected;
                    var adapter3 = ArrayAdapter.CreateFromResource(((RadioGroup)sender).Context, Resource.Array.sporting, Android.Resource.Layout.SimpleSpinnerItem);
                    adapter3.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
                    subdisciplineSpinner.Adapter = adapter3;
                    break;
            }

            string defaultItem = subdisciplineSpinner.Adapter.GetItem(0).ToString();
            subdisciplineSpinner.SetText(defaultItem, false);
            discipline = defaultItem;
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

            FragmentTransaction fragmentTx = Activity.SupportFragmentManager.BeginTransaction();

            Shoot activeShootModel = new ViewModelProvider(Activity).Get(Java.Lang.Class.FromType(typeof(Shoot))) as Shoot;
            activeShootModel.Initialise(standShooterModel.selectedShooters, standShooterModel.selectedFormat, new DateTime(Arguments.GetLong("shootDate")), Arguments.GetString("shootLocation"), discipline, Arguments.GetString("shootTitle"));    
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

