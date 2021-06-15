using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.Fragment.App;
using AndroidX.RecyclerView.Widget;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using Fragment = AndroidX.Fragment.App.Fragment;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

namespace ClubClays.Fragments
{
    public class ShootFormatEditFragment : Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            View view = inflater.Inflate(Resource.Layout.fragment_shoot_format_edit, container, false);

            Toolbar toolbar = view.FindViewById<Toolbar>(Resource.Id.toolbar);
            ((AppCompatActivity)Activity).SetSupportActionBar(toolbar);
            ActionBar supportBar = ((AppCompatActivity)Activity).SupportActionBar;
            supportBar.SetDisplayHomeAsUpEnabled(true);
            supportBar.SetDisplayShowHomeEnabled(true);

            List<DatabaseModels.StandFormats> standFormats;

            string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "ClubClaysData.db3");
            using (var db = new SQLiteConnection(dbPath))
            {
                standFormats = db.Table<DatabaseModels.StandFormats>().Where(s => s.ShootFormatId == Arguments.GetInt("ShootFormatID")).OrderByDescending(s => s.StandNum).ToList();
            }

            LinearLayoutManager LayoutManager = new LinearLayoutManager(Activity);
            RecyclerView shootsRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerView);
            shootsRecyclerView.SetLayoutManager(LayoutManager);
            shootsRecyclerView.SetAdapter(new ShootFormatRecyclerAdapter(standFormats, Activity));

            return view;
        }
    }

    public class ShootFormatRecyclerAdapter : RecyclerView.Adapter
    {
        List<DatabaseModels.StandFormats> standFormats;
        private FragmentActivity activity;
        private int shootFormatId;

        // Provide a reference to the views for each data item
        public class MyView : RecyclerView.ViewHolder
        {
            public View mMainView { get; set; }
            public TextView mStandNum { get; set; }
            public TextView mNumOfShots { get; set; }
            public ImageButton mEditButton { get; set; }
            public MyView(View view) : base(view)
            {
                mMainView = view;
            }
        }

        // Return the size of data set (invoked by the layout manager)
        public override int ItemCount
        {
            get { return standFormats.Count; }
        }

        // Replace the contents of a view (invoked by layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            MyView myHolder = holder as MyView;
            myHolder.mStandNum.Text = $"Stand {standFormats[position].StandNum}";
            myHolder.mNumOfShots.Text = $"{standFormats[position].NumClays} Shot(s)";
        }

        // Create new views (invoked by layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View shootCardView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.stand_format_item, parent, false);
            TextView shootFormatTitle = shootCardView.FindViewById<TextView>(Resource.Id.standNum);
            TextView numStands = shootCardView.FindViewById<TextView>(Resource.Id.numOfShots);
            ImageButton editButton = shootCardView.FindViewById<ImageButton>(Resource.Id.standFormatEditButton);

            MyView view = new MyView(shootCardView) { mStandNum = shootFormatTitle, mNumOfShots = numStands, mEditButton = editButton };

            editButton.Click += delegate
            {
                StandFormatFragment fragment = new StandFormatFragment();
                Bundle args = new Bundle();
                args.PutInt("StandFormatID", standFormats[view.AdapterPosition - 1].Id);
                fragment.Arguments = args;

                FragmentTransaction fragmentTx = activity.SupportFragmentManager.BeginTransaction();
                fragmentTx.Replace(Resource.Id.container, fragment);
                fragmentTx.AddToBackStack(null);
                fragmentTx.Commit();
            };

            return view;
        }

        // Provide a suitable constructor (depends on the kind of dataset)
        public ShootFormatRecyclerAdapter(List<DatabaseModels.StandFormats> standFormats, FragmentActivity activity)
        {
            this.standFormats = standFormats;
            this.activity = activity;
        }
    }
}