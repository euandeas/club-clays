﻿using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using System;
using Fragment = AndroidX.Fragment.App.Fragment;
using FragmentTransaction = AndroidX.Fragment.App.FragmentTransaction;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;
using DatePickerDialog = Android.App.DatePickerDialog;
using AndroidX.AppCompat.App;
using AndroidX.Lifecycle;
using SQLite;
using System.IO;
using ClubClays.DatabaseModels;
using System.Collections.Generic;
using AndroidX.Fragment.App;
using Google.Android.Material.TextField;

namespace ClubClays.Fragments
{
    public class GeneralDataFragment : Fragment, DatePickerDialog.IOnDateSetListener
    {
        private TextView datePickerView;
        private AlertDialog trackingTypeDialog;
        private TextInputLayout trackingTypeLayout;
        private TextView trackingTypePickerView;
        private string userOverallAction;
        private string discipline;
        private string location;
        private TextInputLayout shootersLayout;
        private TextInputLayout dateLayout;
        private DateTime date;

        private TextView shootersSelection;
        private TextInputLayout standFormattingLayout;
        private TextView standFormatting;
        private Switch formatSwitch;
        private TextView startStandLabel;
        private Switch startSwitch;
        private LinearLayout formatSwitchLayout;
        private TextInputLayout startStandLayout;
        private EditText startStandInput;
        private TextView optionsLabel;
        private CheckBox rotateShootersCheckBox;

        private ShooterStandData standShooterModel;

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
            trackingTypeLayout = view.FindViewById<TextInputLayout>(Resource.Id.trackingtype);
            trackingTypePickerView = view.FindViewById<TextInputEditText>(Resource.Id.trackingtypeEditText);
            trackingTypePickerView.Click += TrackingTypePickerView_Click;

            AutoCompleteTextView spinner = view.FindViewById<AutoCompleteTextView>(Resource.Id.disciplineDropdown);
            spinner.ItemClick += new EventHandler<AdapterView.ItemClickEventArgs>(spinner_ItemSelected);
            //spinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(spinner_ItemSelected);
            var adapter = ArrayAdapter.CreateFromResource(view.Context, Resource.Array.disciplines, Android.Resource.Layout.SimpleSpinnerItem);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            spinner.Adapter = adapter;

            date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            datePickerView = view.FindViewById<TextInputEditText>(Resource.Id.dateEditText);
            datePickerView.Text = $"{date:MMMM} {date:dd}, {date:yyyy}";
            datePickerView.Click += DatePickerView_Click;

            EditText locationInput = view.FindViewById<TextInputEditText>(Resource.Id.locationEditText);
            location = locationInput.Text;

            shootersLayout = view.FindViewById<TextInputLayout>(Resource.Id.shooters);
            shootersSelection = view.FindViewById<TextInputEditText>(Resource.Id.shootersEditText);
            shootersSelection.Text = "0 Shooter(s) Selected";
            shootersSelection.Click += ShootersSelection_Click;

            string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "ClubClaysData.db3");
            SQLiteConnection db = new SQLiteConnection(dbPath);
            standShooterModel = new ViewModelProvider(Activity).Get(Java.Lang.Class.FromType(typeof(ShooterStandData))) as ShooterStandData;
            standShooterModel.allShooters = db.Table<Shooters>().ToList();
            db.Close();
            standShooterModel.selectedShooters = new List<Shooters>();

            formatSwitch = view.FindViewById<Switch>(Resource.Id.formatSwitch);
            formatSwitch.CheckedChange += FormatSwitch_CheckedChange;

            standFormattingLayout = view.FindViewById<TextInputLayout>(Resource.Id.stands);
            standFormatting = view.FindViewById<TextInputEditText>(Resource.Id.standsEditText);
            standFormatting.Text = "0 Stand(s) Setup";
            standFormatting.Click += StandFormatting_Click; ;

            standShooterModel.standFormats = new List<StandFormats>();

            startSwitch = view.FindViewById<Switch>(Resource.Id.startingStandSwitch);
            startSwitch.CheckedChange += StartSwitch_CheckedChange;
            startStandLayout = view.FindViewById<TextInputLayout>(Resource.Id.customStartStand);
            startStandInput = view.FindViewById<TextInputEditText>(Resource.Id.startStandEditText);
            startSwitch.Checked = false;
            startStandLayout.Visibility = ViewStates.Gone;

            optionsLabel = view.FindViewById<TextView>(Resource.Id.optionsLabel);
            rotateShootersCheckBox = view.FindViewById<CheckBox>(Resource.Id.rotateShootersCheckBox);

            TextView nextButton = view.FindViewById<TextView>(Resource.Id.nextButton);
            nextButton.Click += NextButton_Click;

            return view;
        }

        private void StartSwitch_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            if (e.IsChecked == true)
            {
                startStandLayout.Visibility = ViewStates.Visible;
            }
            else if (e.IsChecked == false)
            {
                startStandLayout.Visibility = ViewStates.Gone;
                startStandInput.Text = "1";
            }
        }

        private void FormatSwitch_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            if (e.IsChecked == true)
            {
                standFormattingLayout.Visibility = ViewStates.Visible;
                startSwitch.Visibility = ViewStates.Visible;
            }
            else if (e.IsChecked == false)
            {
                startSwitch.Visibility = ViewStates.Gone;
                standFormattingLayout.Visibility = ViewStates.Gone;
                startStandLayout.Visibility = ViewStates.Gone;
                startStandInput.Text = "1";
            }
        }

        private void StandFormatting_Click(object sender, EventArgs e)
        {
            FragmentTransaction fragmentTx = Activity.SupportFragmentManager.BeginTransaction();
            StandSetupFragment standSetupFragment = new StandSetupFragment();
            fragmentTx.Add(Resource.Id.container, standSetupFragment);
            fragmentTx.AddToBackStack(null);
            fragmentTx.Commit();
            Activity.SupportFragmentManager.SetFragmentResultListener("2", this, new FragResult(this));
        }

        private void NextButton_Click(object sender, EventArgs e)
        {
            if (standShooterModel.selectedShooters.Count == 0)
            {
                Toast.MakeText(Activity, $"No shooters selected!", ToastLength.Short).Show();
                return;
            }
            if (((userOverallAction == "Add Shoot") || ((formatSwitch.Checked == true) && (userOverallAction == "New Shoot"))) && (standShooterModel.standFormats.Count == 0))
            {
                Toast.MakeText(Activity, $"No stands created!", ToastLength.Short).Show();
                return;
            }

            int startStand = 1;
            if (startSwitch.Checked)
            {
                int standNum = int.Parse(startStandInput.Text);
                if (standNum > standShooterModel.standFormats.Count)
                {
                    Toast.MakeText(Activity, $"Starting stand does not exist!", ToastLength.Short).Show();
                    startStandInput.Text = "1";
                    return;
                }
                else
                {
                    startStand = standNum;
                }
            }

            FragmentTransaction fragmentTx = Activity.SupportFragmentManager.BeginTransaction();

            if (userOverallAction == "New Shoot")
            {
                ShootScoreManagement activeShootModel = new ViewModelProvider(Activity).Get(Java.Lang.Class.FromType(typeof(ShootScoreManagement))) as ShootScoreManagement;
                activeShootModel.InitialiseBasics(standShooterModel.selectedShooters, date, location, rotateShootersCheckBox.Checked, discipline, startStand);

                if (formatSwitch.Checked == true)
                {
                    activeShootModel.InitialiseStands(standShooterModel.standFormats);

                    fragmentTx.Replace(Resource.Id.container, new ScoreTakingFragment());
                    fragmentTx.Commit();
                    standShooterModel.Dispose();
                }
                else if (formatSwitch.Checked == false)
                {
                    AlertDialog.Builder builder = new AlertDialog.Builder(Activity);
                    builder.SetTitle("Add First Stand");

                    View view = LayoutInflater.From(Activity).Inflate(Resource.Layout.dialogfragment_addstand, null);

                    EditText standType = view.FindViewById<EditText>(Resource.Id.newStandType);
                    EditText standFormat = view.FindViewById<EditText>(Resource.Id.newStandFormat);
                    EditText numOfPairs = view.FindViewById<EditText>(Resource.Id.newNumOfPairs);

                    builder.SetView(view);
                    builder.SetPositiveButton("Add", (c, ev) =>
                    {
                        activeShootModel.AddStand(new StandFormats { StandType = standType.Text, StandFormat = standFormat.Text, NumPairs = int.Parse(numOfPairs.Text) });
                        fragmentTx.Replace(Resource.Id.container, new ScoreTakingFragment());
                        fragmentTx.Commit();
                        standShooterModel.Dispose();
                    });
                    builder.SetNegativeButton("Cancel", (c, ev) => { return; });

                    builder.Show();
                }
            }
            else if (userOverallAction == "Add Shoot")
            {
                AddShoot activeShootModel = new ViewModelProvider(Activity).Get(Java.Lang.Class.FromType(typeof(AddShoot))) as AddShoot;

                //fragmentTx.Replace(Resource.Id.container, );
                //fragmentTx.Commit();
                standShooterModel.Dispose();
            }
        }

        private void ShootersSelection_Click(object sender, EventArgs e)
        { 
            FragmentTransaction fragmentTx = Activity.SupportFragmentManager.BeginTransaction();
            ShootersFragment shootersFragment = new ShootersFragment();
            fragmentTx.Add(Resource.Id.container, shootersFragment);
            fragmentTx.AddToBackStack(null);
            fragmentTx.Commit();
            Activity.SupportFragmentManager.SetFragmentResultListener("1", this, new FragResult(this));
        }

        public class FragResult : Java.Lang.Object, IFragmentResultListener
        {
            GeneralDataFragment context;
            public FragResult(GeneralDataFragment context)
            {
                this.context = context;
            }
            public void OnFragmentResult(string p0, Bundle p1)
            {
                if (p0 == "1")
                {
                    context.shootersSelection.Text = $"{p1.GetInt("numSelected", 0)} Shooter(s) Selected";
                }
                else if (p0 == "2")
                {
                    context.standFormatting.Text = $"{p1.GetInt("standsCreated", 0)} Stand(s) Setup";
                }
            }
        }

        private void spinner_ItemSelected(object sender, AdapterView.ItemClickEventArgs e)
        {
            discipline = (sender as AutoCompleteTextView).Text;
        }

        private void TrackingTypePickerView_Click(object sender, EventArgs e)
        {
            trackingTypeDialog.Show();
        }

        private AlertDialog TrackingTypeDialogBuilder()
        {
            AlertDialog.Builder dialogBuilder = new AlertDialog.Builder(Activity);
            dialogBuilder.SetTitle("What would you like do to?");
            dialogBuilder.SetSingleChoiceItems(Resource.Array.tracking_types, -1, new EventHandler<DialogClickEventArgs>(DialogOnClickListerner));
            dialogBuilder.SetCancelable(false);
            return dialogBuilder.Create();
        }

        private void DialogOnClickListerner(object sender, DialogClickEventArgs e)
        {
            switch (e.Which)
            {
                case 0:
                    userOverallAction = "New Shoot";
                    formatSwitch.Visibility = ViewStates.Visible;
                    formatSwitch.Checked = true;
                    datePickerView.Enabled = false;
                    date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                    datePickerView.Text = $"{date:MMMM} {date:dd}, {date:yyyy}";
                    optionsLabel.Visibility = ViewStates.Visible;
                    rotateShootersCheckBox.Visibility = ViewStates.Visible;
                    startSwitch.Checked = false;
                    startSwitch.Visibility = ViewStates.Visible;
                    startStandInput.Visibility = ViewStates.Visible;
                    break;
                case 1:
                    userOverallAction = "Add Shoot";
                    formatSwitch.Visibility = ViewStates.Gone;
                    standFormatting.Visibility = ViewStates.Visible;
                    datePickerView.Enabled = true;
                    optionsLabel.Visibility = ViewStates.Gone;
                    rotateShootersCheckBox.Visibility = ViewStates.Gone;
                    startSwitch.Checked = false;
                    startSwitch.Visibility = ViewStates.Gone;
                    startStandInput.Visibility = ViewStates.Gone;
                    break;
            }
            trackingTypePickerView.Text = userOverallAction;
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
            date = new DateTime(year, month + 1, dayOfMonth);
            datePickerView.Text = $"{date:MMMM} {date:dd}, {year}";
        }
    }
}