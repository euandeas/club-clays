using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.Fragment.App;
using AndroidX.Lifecycle;
using AndroidX.RecyclerView.Widget;
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
            shootsRecyclerView.SetAdapter(new ShootFormatsRecyclerAdapter(shootFormats, Activity));

            return view;
        }
    }

    public class ShootFormatsRecyclerAdapter : RecyclerView.Adapter
    {
        private List<DatabaseModels.ShootFormats> shootFormats;
        private FragmentActivity activity;
        private ShooterStandData standShooterModel;

        // Provide a reference to the views for each data item
        public class MyView : RecyclerView.ViewHolder
        {
            public View mMainView { get; set; }
            public TextView mShootFormatTitle { get; set; }
            public TextView mNumStands { get; set; }
            public ImageView mSelecetedIcon { get; set; }
            public ImageButton mEditButton { get; set; }
            public MyView(View view) : base(view)
            {
                mMainView = view;
            }
        }

        // Return the size of data set (invoked by the layout manager)
        public override int ItemCount
        {
            get { return shootFormats.Count + 1; }
        }

        // Replace the contents of a view (invoked by layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            MyView myHolder = holder as MyView;
            if (position == 0)
            {
                myHolder.mShootFormatTitle.Text = "Blank";
                myHolder.mNumStands.Text = $"Add stands on the go";
                myHolder.mEditButton.Visibility = ViewStates.Gone;

                if (standShooterModel.selectedFormat == null)
                {
                    myHolder.mSelecetedIcon.Visibility = ViewStates.Visible;
                }
                else
                {
                    myHolder.mSelecetedIcon.Visibility = ViewStates.Gone;
                }
            }
            else
            {
                myHolder.mShootFormatTitle.Text = $"{shootFormats[position].FormatName}";
                myHolder.mNumStands.Text = $"{shootFormats[position].NumStands} Stand(s)";

                if (standShooterModel.selectedFormat == shootFormats[position])
                {
                    myHolder.mSelecetedIcon.Visibility = ViewStates.Visible;
                }
                else
                {
                    myHolder.mSelecetedIcon.Visibility = ViewStates.Gone;
                }
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

            MyView view = new MyView(shootCardView) { mShootFormatTitle = shootFormatTitle, mNumStands = numStands, mSelecetedIcon = selectedIcon, mEditButton = editButton };

            shootCardView.Click += delegate
            {
                if (view.AdapterPosition == 0)
                {
                    standShooterModel.selectedFormat = null;
                }
                else
                {
                    standShooterModel.selectedFormat = shootFormats[view.AdapterPosition - 1];
                }
                
                activity.SupportFragmentManager.SetFragmentResult("2", null);
                activity.SupportFragmentManager.PopBackStack();
            };

            editButton.Click += delegate
            {
                ShootFormatEditFragment fragment = new ShootFormatEditFragment();
                Bundle args = new Bundle();
                args.PutInt("ShootFormatID", shootFormats[view.AdapterPosition - 1].Id);
                fragment.Arguments = args;

                FragmentTransaction fragmentTx = activity.SupportFragmentManager.BeginTransaction();
                fragmentTx.Replace(Resource.Id.container, fragment);
                fragmentTx.AddToBackStack(null);
                fragmentTx.Commit();
            };

            return view;
        }


        // Provide a suitable constructor (depends on the kind of dataset)
        public ShootFormatsRecyclerAdapter(List<DatabaseModels.ShootFormats> shootFormats, FragmentActivity activity)
        {
            this.shootFormats = shootFormats;
            this.activity = activity;
            standShooterModel = new ViewModelProvider(activity).Get(Java.Lang.Class.FromType(typeof(ShooterStandData))) as ShooterStandData;
        }
    }
}