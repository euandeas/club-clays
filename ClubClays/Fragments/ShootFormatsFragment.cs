using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.Fragment.App;
using AndroidX.Lifecycle;
using AndroidX.RecyclerView.Widget;
using Google.Android.Material.FloatingActionButton;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using Fragment = AndroidX.Fragment.App.Fragment;
using FragmentTransaction = AndroidX.Fragment.App.FragmentTransaction;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

namespace ClubClays.Fragments
{
    public class ShootFormatsFragment : Fragment
    {

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            View view = inflater.Inflate(Resource.Layout.fragment_shoot_formats, container, false);

            Toolbar toolbar = view.FindViewById<Toolbar>(Resource.Id.toolbar);
            ((AppCompatActivity)Activity).SetSupportActionBar(toolbar);
            ActionBar supportBar = ((AppCompatActivity)Activity).SupportActionBar;
            supportBar.SetDisplayHomeAsUpEnabled(true);
            supportBar.SetDisplayShowHomeEnabled(true);

            List<DatabaseModels.ShootFormats> shootFormats;

            string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "ClubClaysData.db3");
            using (var db = new SQLiteConnection(dbPath))
            {
                shootFormats = db.Table<DatabaseModels.ShootFormats>().OrderByDescending(s => s.FormatName).ToList();
            }

            LinearLayoutManager LayoutManager = new LinearLayoutManager(Activity);
            RecyclerView shootsRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerView);
            shootsRecyclerView.SetLayoutManager(LayoutManager);
            shootsRecyclerView.SetAdapter(new ShootFormatsRecyclerAdapter(shootFormats, Activity, Arguments.GetBoolean("selectable")));

            FloatingActionButton fab = view.FindViewById<FloatingActionButton>(Resource.Id.addButton);
            fab.Click += Fab_Click;

            return view;
        }

        private void Fab_Click(object sender, EventArgs e)
        {
            ShootFormatEditFragment fragment = new ShootFormatEditFragment();
            Bundle args = new Bundle();
            args.PutBoolean("NewShoot", true);
            args.PutBoolean("Selectable", Arguments.GetBoolean("selectable"));
            fragment.Arguments = args;

            FragmentTransaction fragmentTx = Activity.SupportFragmentManager.BeginTransaction();
            fragmentTx.Replace(Resource.Id.container, fragment);
            fragmentTx.AddToBackStack(null);
            fragmentTx.Commit();
        }
    }

    public class ShootFormatsRecyclerAdapter : RecyclerView.Adapter
    {
        private List<DatabaseModels.ShootFormats> shootFormats;
        private FragmentActivity activity;
        private bool selectable;
        private ShooterStandData standShooterModel;

        // Provide a reference to the views for each data item
        public class MyView : RecyclerView.ViewHolder
        {
            public View MainView { get; set; }
            public TextView ShootFormatTitle { get; set; }
            public TextView NumStands { get; set; }
            public ImageView SelecetedIcon { get; set; }
            public ImageButton EditButton { get; set; }
            public MyView(View view) : base(view)
            {
                MainView = view;
            }
        }

        // Return the size of data set (invoked by the layout manager)
        public override int ItemCount
        {
            get { 
                if (selectable)
                {
                    return shootFormats.Count + 1;
                }
                else
                {
                    return shootFormats.Count;
                }
            }
        }

        // Replace the contents of a view (invoked by layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            MyView myHolder = holder as MyView;
            if (selectable)
            {
                if (position == 0)
                {
                    myHolder.ShootFormatTitle.Text = "Blank";
                    myHolder.NumStands.Text = "Add stands on the go";
                    myHolder.EditButton.Visibility = ViewStates.Gone;
                    myHolder.SelecetedIcon.Visibility = (standShooterModel.selectedFormat == null) ? ViewStates.Visible : ViewStates.Gone;
                }
                else
                {
                    myHolder.ShootFormatTitle.Text = $"{shootFormats[position-1].FormatName}";
                    myHolder.NumStands.Text = $"{shootFormats[position-1].NumStands} Stand(s)";

                    if (standShooterModel.selectedFormat != null)
                    {
                        myHolder.SelecetedIcon.Visibility = (standShooterModel.selectedFormat.Id == shootFormats[position - 1].Id) ? ViewStates.Visible : ViewStates.Gone;
                    }
                    else
                    {
                        myHolder.SelecetedIcon.Visibility = ViewStates.Gone;
                    }
                } 
            }
            else
            {
                myHolder.ShootFormatTitle.Text = $"{shootFormats[position].FormatName}";
                myHolder.NumStands.Text = $"{shootFormats[position].NumStands} Stand(s)";
                myHolder.SelecetedIcon.Visibility = ViewStates.Gone;
            }           
        }

        // Create new views (invoked by layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View shootCardView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.shoot_format_item, parent, false);
            TextView shootFormatTitle = shootCardView.FindViewById<TextView>(Resource.Id.formatTitle);
            TextView numStands = shootCardView.FindViewById<TextView>(Resource.Id.numOfStands);
            ImageView selectedIcon = shootCardView.FindViewById<ImageView>(Resource.Id.shootFormatSelected);
            ImageButton editButton = shootCardView.FindViewById<ImageButton>(Resource.Id.shootFormatEditButton);

            MyView view = new MyView(shootCardView) { ShootFormatTitle = shootFormatTitle, NumStands = numStands, SelecetedIcon = selectedIcon, EditButton = editButton };

            shootCardView.Click += delegate
            {
                if (selectable)
                {
                    Bundle result = new Bundle();
                    if (view.AbsoluteAdapterPosition == 0)
                    {
                        standShooterModel.selectedFormat = null;
                        result.PutString("titleText", "Blank");
                        result.PutString("bottomText", "Add stands on the go");
                    }
                    else
                    {
                        standShooterModel.selectedFormat = shootFormats[view.AbsoluteAdapterPosition - 1];
                        result.PutString("titleText", shootFormats[view.AbsoluteAdapterPosition - 1].FormatName);
                        result.PutString("bottomText", $"{shootFormats[view.AbsoluteAdapterPosition - 1].NumStands} Stand(s)");
                    }

                    activity.SupportFragmentManager.SetFragmentResult("2", result);
                    activity.SupportFragmentManager.PopBackStack();
                }
            };

            editButton.Click += delegate
            {
                ShootFormatEditFragment fragment = new ShootFormatEditFragment();
                Bundle args = new Bundle();
                args.PutInt("ShootFormatID", selectable ? shootFormats[view.AbsoluteAdapterPosition - 1].Id : shootFormats[view.AbsoluteAdapterPosition].Id);
                args.PutBoolean("Selectable", selectable);

                fragment.Arguments = args;
                FragmentTransaction fragmentTx = activity.SupportFragmentManager.BeginTransaction();
                fragmentTx.Replace(Resource.Id.container, fragment);
                fragmentTx.AddToBackStack(null);
                fragmentTx.Commit();
            };

            return view;
        }


        // Provide a suitable constructor (depends on the kind of dataset)
        public ShootFormatsRecyclerAdapter(List<DatabaseModels.ShootFormats> shootFormats, FragmentActivity activity, bool selectable)
        {
            this.shootFormats = shootFormats;
            this.activity = activity;
            this.selectable = selectable;

            if (selectable)
            {
                standShooterModel = new ViewModelProvider(activity).Get(Java.Lang.Class.FromType(typeof(ShooterStandData))) as ShooterStandData;

                Bundle result = new Bundle();
                if (standShooterModel.selectedFormat == null)
                {
                    result.PutString("titleText", "Blank");
                    result.PutString("bottomText", "Add stands on the go");
                }
                else
                {
                    result.PutString("titleText", standShooterModel.selectedFormat.FormatName);
                    result.PutString("bottomText", $"{standShooterModel.selectedFormat.NumStands} Stand(s)");
                }

                activity.SupportFragmentManager.SetFragmentResult("2", result);
            }
        }
    }
}