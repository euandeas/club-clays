using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.RecyclerView.Widget;
using ClubClays.DatabaseModels;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using Fragment = AndroidX.Fragment.App.Fragment;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

namespace ClubClays.Fragments
{
    public class StandFormatFragment : Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            View view = inflater.Inflate(Resource.Layout.fragment_stand_format, container, false);

            Toolbar toolbar = view.FindViewById<Toolbar>(Resource.Id.toolbar);
            ((AppCompatActivity)Activity).SetSupportActionBar(toolbar);
            ActionBar supportBar = ((AppCompatActivity)Activity).SupportActionBar;
            supportBar.SetDisplayHomeAsUpEnabled(true);
            supportBar.SetDisplayShowHomeEnabled(true);

            List<StandShotsFormats> shotFormats;

            string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "ClubClaysData.db3");
            using (var db = new SQLiteConnection(dbPath))
            {
                shotFormats = db.Table<StandShotsFormats>().Where(s => s.StandFormatId == Arguments.GetInt("StandFormatID")).OrderByDescending(s => s.ShotNum).ToList();
            }

            LinearLayoutManager LayoutManager = new LinearLayoutManager(Activity);
            RecyclerView shootsRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerView);
            shootsRecyclerView.SetLayoutManager(LayoutManager);
            shootsRecyclerView.SetAdapter(new StandFormatRecyclerAdapter(shotFormats));

            return view;
        }
    }

    public class StandFormatRecyclerAdapter : RecyclerView.Adapter
    {
        private List<StandShotsFormats> shotsFormats;


        // Provide a reference to the views for each data item
        public class MyView : RecyclerView.ViewHolder
        {
            public View mMainView { get; set; }
            public TextView mShotType { get; set; }
            public MyView(View view) : base(view)
            {
                mMainView = view;
            }
        }

        // Return the size of data set (invoked by the layout manager)
        public override int ItemCount
        {
            get { return shotsFormats.Count; }
        }

        // Replace the contents of a view (invoked by layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            MyView myHolder = holder as MyView;
            myHolder.mShotType.Text = shotsFormats[position].Type;
        }

        // Create new views (invoked by layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View shootCardView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.shot_format_item, parent, false);
            TextView shotType = shootCardView.FindViewById<TextView>(Resource.Id.shotType);

            MyView view = new MyView(shootCardView) { mShotType = shotType};

            return view;
        }

        // Provide a suitable constructor (depends on the kind of dataset)
        public StandFormatRecyclerAdapter(List<StandShotsFormats> shotsFormats)
        {
            this.shotsFormats = shotsFormats;
        }
    }
}