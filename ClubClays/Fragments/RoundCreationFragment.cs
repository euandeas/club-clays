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
using Google.Android.Material.TextView;

namespace ClubClays.Fragments
{
    public class RoundCreationFragment : Fragment, IFragmentResultListener
    {
        private Disciplines.IBaseDiscipline discipline;

        private TextView shootersSelection;
        private TextInputLayout customStandFormattingLayout;
        private TextView customStandFormatting;
        AutoCompleteTextView subdisciplineSpinner;

        private ShooterStandData standShooterModel;
        private bool setdefaultspinner;
        private TextInputLayout subDisciplineLayoutsLayout;
        private AutoCompleteTextView subDisciplineLayouts;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
            string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "ClubClaysData.db3");
            SQLiteConnection db = new SQLiteConnection(dbPath);
            standShooterModel = new ViewModelProvider(Activity).Get(Java.Lang.Class.FromType(typeof(ShooterStandData))) as ShooterStandData;
            standShooterModel.allShooters = db.Table<Shooters>().ToList();

            setdefaultspinner = true;
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

            subDisciplineLayoutsLayout = view.FindViewById<TextInputLayout>(Resource.Id.layoutOptions);
            subDisciplineLayouts = view.FindViewById<AutoCompleteTextView>(Resource.Id.layoutOptionsDropdown);
            subDisciplineLayouts.Click += SubDisciplineLayouts_Click;

            customStandFormattingLayout = view.FindViewById<TextInputLayout>(Resource.Id.stands);
            customStandFormatting = view.FindViewById<TextInputEditText>(Resource.Id.standsEditText);
            customStandFormatting.Text = "Blank";
            customStandFormatting.Click += StandFormatting_Click;

            RadioGroup disciplinesRadioGroup = view.FindViewById<RadioGroup>(Resource.Id.disciplines);
            disciplinesRadioGroup.CheckedChange += DisciplinesRadioGroup_Click;

            subdisciplineSpinner = view.FindViewById<AutoCompleteTextView>(Resource.Id.subdisciplineDropdown);

            if (setdefaultspinner)
            {
                ChangeSubDisciplineSpinner(disciplinesRadioGroup);
                setdefaultspinner = false;
            }

            shootersSelection = view.FindViewById<TextInputEditText>(Resource.Id.shootersEditText);
            shootersSelection.Text = "0 Shooter(s) Selected";
            shootersSelection.Click += ShootersSelection_Click;

            FloatingActionButton fab = view.FindViewById<FloatingActionButton>(Resource.Id.nextButton);
            fab.Click += NextButton_Click;

            return view;
        }

        private void SubDisciplineLayouts_Click(object sender, EventArgs e)
        {

        }

        private void DisciplinesRadioGroup_Click(object sender, EventArgs e)
        {
            ChangeSubDisciplineSpinner(((RadioGroup)sender));
        }

        public void ChangeSubDisciplineSpinner(RadioGroup sender)
        {
            switch (sender.CheckedRadioButtonId)
            {
                case Resource.Id.trap:
                    subdisciplineSpinner.ItemClick += TrapSubDisciplineSelected;
                    var adapter1 = ArrayAdapter.CreateFromResource(sender.Context, Resource.Array.trap, Android.Resource.Layout.SimpleSpinnerItem);
                    adapter1.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
                    subdisciplineSpinner.Adapter = adapter1;

                    var defaultItem1 = subdisciplineSpinner.Adapter.GetItem(0).ToString();
                    subdisciplineSpinner.SetText(defaultItem1, false);
                    TrapSubDisciplineSelection(defaultItem1);
                    break;
                case Resource.Id.skeet:
                    subdisciplineSpinner.ItemClick += SkeetSubDisciplineSelected;
                    var adapter2 = ArrayAdapter.CreateFromResource(sender.Context, Resource.Array.skeet, Android.Resource.Layout.SimpleSpinnerItem);
                    adapter2.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
                    subdisciplineSpinner.Adapter = adapter2;

                    var defaultItem2 = subdisciplineSpinner.Adapter.GetItem(0).ToString();
                    subdisciplineSpinner.SetText(defaultItem2, false);
                    SkeetSubDisciplineSelection(defaultItem2);
                    break;
                case Resource.Id.sporting:
                    subdisciplineSpinner.ItemClick += SportingSubDisciplineSelected;
                    var adapter3 = ArrayAdapter.CreateFromResource(sender.Context, Resource.Array.sporting, Android.Resource.Layout.SimpleSpinnerItem);
                    adapter3.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
                    subdisciplineSpinner.Adapter = adapter3;

                    var defaultItem3 = subdisciplineSpinner.Adapter.GetItem(0).ToString();
                    subdisciplineSpinner.SetText(defaultItem3, false);
                    SportingSubDisciplineSelection(defaultItem3);
                    break;
            }  
        }

        private void SportingSubDisciplineSelected(object sender, AdapterView.ItemClickEventArgs e)
        {
            SportingSubDisciplineSelection((sender as AutoCompleteTextView).Text);
        }

        private void SkeetSubDisciplineSelected(object sender, AdapterView.ItemClickEventArgs e)
        {
            SkeetSubDisciplineSelection((sender as AutoCompleteTextView).Text);
        }

        private void TrapSubDisciplineSelected(object sender, AdapterView.ItemClickEventArgs e)
        {
            TrapSubDisciplineSelection((sender as AutoCompleteTextView).Text);
        }

        public void TrapSubDisciplineSelection(string text)
        {
            switch (text)
            {
                case "American Trap":
                    subDisciplineLayoutsLayout.Visibility = ViewStates.Gone;
                    customStandFormattingLayout.Visibility = ViewStates.Gone;
                    break;

                case "American Trap Doubles":
                    subDisciplineLayoutsLayout.Visibility = ViewStates.Gone;
                    customStandFormattingLayout.Visibility = ViewStates.Gone;
                    break;

                case "Double Rise":
                    subDisciplineLayoutsLayout.Visibility = ViewStates.Gone;
                    customStandFormattingLayout.Visibility = ViewStates.Gone;
                    break;

                case "Down - The - Line":
                    subDisciplineLayoutsLayout.Visibility = ViewStates.Gone;
                    customStandFormattingLayout.Visibility = ViewStates.Gone;
                    break;

                case "Olympic Double Trap":
                    subDisciplineLayoutsLayout.Visibility = ViewStates.Gone;
                    customStandFormattingLayout.Visibility = ViewStates.Gone;
                    break;

                case "Olympic Trap":
                    subDisciplineLayoutsLayout.Visibility = ViewStates.Gone;
                    customStandFormattingLayout.Visibility = ViewStates.Gone;
                    break;

                case "Single Barrel":
                    subDisciplineLayoutsLayout.Visibility = ViewStates.Gone;
                    customStandFormattingLayout.Visibility = ViewStates.Gone;
                    break;

                case "Automatic Ball Trap":
                    subDisciplineLayoutsLayout.Visibility = ViewStates.Gone;
                    customStandFormattingLayout.Visibility = ViewStates.Gone;
                    break;

                case "Helice(ZZ)":
                    subDisciplineLayoutsLayout.Visibility = ViewStates.Gone;
                    customStandFormattingLayout.Visibility = ViewStates.Gone;
                    break;

                case "Universal Trench":
                    subDisciplineLayoutsLayout.Visibility = ViewStates.Gone;
                    customStandFormattingLayout.Visibility = ViewStates.Gone;
                    break;

                case "Wobble Trap":
                    subDisciplineLayoutsLayout.Visibility = ViewStates.Gone;
                    customStandFormattingLayout.Visibility = ViewStates.Gone;
                    break;

                case "Nordic Trap":
                    subDisciplineLayoutsLayout.Visibility = ViewStates.Gone;
                    customStandFormattingLayout.Visibility = ViewStates.Gone;
                    break;
            }
        }

        public void SkeetSubDisciplineSelection(string text)
        {
            switch (text)
            {
                case "American Skeet":
                    subDisciplineLayoutsLayout.Visibility = ViewStates.Gone;
                    customStandFormattingLayout.Visibility = ViewStates.Gone;
                    break;

                case "English Skeet":
                    subDisciplineLayoutsLayout.Visibility = ViewStates.Gone;
                    customStandFormattingLayout.Visibility = ViewStates.Gone;
                    break;

                case "Olympic Skeet":
                    subDisciplineLayoutsLayout.Visibility = ViewStates.Visible;
                    customStandFormattingLayout.Visibility = ViewStates.Gone;
                    break;

                case "Olympic Skeet Final":
                    subDisciplineLayoutsLayout.Visibility = ViewStates.Visible;
                    customStandFormattingLayout.Visibility = ViewStates.Gone;
                    break;

                case "Olympic Skeet(Pre - 2012)":
                    subDisciplineLayoutsLayout.Visibility = ViewStates.Gone;
                    customStandFormattingLayout.Visibility = ViewStates.Gone;
                    break;

                case "Skeet Doubles":
                    subDisciplineLayoutsLayout.Visibility = ViewStates.Visible;
                    customStandFormattingLayout.Visibility = ViewStates.Gone;
                    break;

                case "Skeet Shoot - Off":
                    subDisciplineLayoutsLayout.Visibility = ViewStates.Gone;
                    customStandFormattingLayout.Visibility = ViewStates.Gone;
                    break;

                case "Wobble Skeet":
                    subDisciplineLayoutsLayout.Visibility = ViewStates.Gone;
                    customStandFormattingLayout.Visibility = ViewStates.Gone;
                    break;
            }
        }

        public void SportingSubDisciplineSelection(string text)
        {
            switch (text)
            {
                case "Compak Sporting":
                    subDisciplineLayoutsLayout.Visibility = ViewStates.Gone;
                    customStandFormattingLayout.Visibility = ViewStates.Visible;
                    break;

                case "English Sporting":
                    subDisciplineLayoutsLayout.Visibility = ViewStates.Gone;
                    customStandFormattingLayout.Visibility = ViewStates.Visible;
                    break;

                case "FITASC Sporting":
                    subDisciplineLayoutsLayout.Visibility = ViewStates.Gone;
                    customStandFormattingLayout.Visibility = ViewStates.Visible;
                    break;

                case "Five Stand":
                    subDisciplineLayoutsLayout.Visibility = ViewStates.Gone;
                    customStandFormattingLayout.Visibility = ViewStates.Gone;
                    break;

                case "Sportrap":
                    subDisciplineLayoutsLayout.Visibility = ViewStates.Gone;
                    customStandFormattingLayout.Visibility = ViewStates.Gone;
                    break;

                case "Super Sporting":
                    subDisciplineLayoutsLayout.Visibility = ViewStates.Gone;
                    customStandFormattingLayout.Visibility = ViewStates.Visible;
                    break;
            }
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
            activeShootModel.Initialise(standShooterModel.selectedShooters, standShooterModel.selectedFormat, new DateTime(Arguments.GetLong("shootDate")), Arguments.GetString("shootLocation"), null, Arguments.GetString("shootTitle"));
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

        public void OnFragmentResult(string p0, Bundle p1)
        {
            if (p0 == "1")
            {
                shootersSelection.Text = $"{p1.GetInt("numSelected", 0)} Shooter(s) Selected";
            }
            else if (p0 == "2")
            {
                customStandFormatting.Text = $"{p1.GetString("titleText")}";
            }
        }
    }
}

