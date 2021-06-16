using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.Fragment.App;
using AndroidX.RecyclerView.Widget;
using Google.Android.Material.FloatingActionButton;
using Google.Android.Material.TextField;
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
        ShootFormatRecyclerAdapter recyclerAdapter;
        private TextInputEditText title;

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

            toolbar.FindViewById<TextView>(Resource.Id.saveButton).Click += Save_Click;

            List<DatabaseModels.StandFormats> standFormats;

            title = view.FindViewById<TextInputEditText>(Resource.Id.shootFormatTitleEditText);

            string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "ClubClaysData.db3");
            using (var db = new SQLiteConnection(dbPath))
            {
                int id = Arguments.GetInt("ShootFormatID");
                title.Text = db.Table<DatabaseModels.ShootFormats>().Where(s => s.Id == id).ToList()[0].FormatName;
                standFormats = db.Table<DatabaseModels.StandFormats>().Where(s => s.ShootFormatId == id).OrderBy(s => s.StandNum).ToList();
            }

            LinearLayoutManager LayoutManager = new LinearLayoutManager(Activity);
            RecyclerView shootsRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerView);
            shootsRecyclerView.SetLayoutManager(LayoutManager);
            recyclerAdapter = new ShootFormatRecyclerAdapter(standFormats, Activity);
            shootsRecyclerView.SetAdapter(recyclerAdapter);

            FloatingActionButton fab = view.FindViewById<FloatingActionButton>(Resource.Id.addButton);
            fab.Click += Fab_Click;

            return view;
        }

        private void Save_Click(object sender, EventArgs e)
        {
            int numClays = 0;
            foreach (DatabaseModels.StandFormats stand in recyclerAdapter.standFormats)
            {
                numClays += stand.NumClays;
            }
            
            string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "ClubClaysData.db3");
            using (var db = new SQLiteConnection(dbPath))
            {
                db.CreateCommand($"UPDATE ShootFormats SET FormatName = '{title.Text}', NumStands = {recyclerAdapter.ItemCount}, ClayAmount = {numClays} WHERE ID = {Arguments.GetInt("ShootFormatID")};").ExecuteNonQuery();
            }

            Activity.SupportFragmentManager.PopBackStack();
        }

        private void Fab_Click(object sender, EventArgs e)
        {
            StandFormatFragment fragment = new StandFormatFragment();
            Bundle args = new Bundle();

            DatabaseModels.StandFormats standFormat;


            string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "ClubClaysData.db3");
            using (var db = new SQLiteConnection(dbPath))
            {
                standFormat = new DatabaseModels.StandFormats { ShootFormatId = Arguments.GetInt("ShootFormatID"), NumClays = 0, StandNum = recyclerAdapter.ItemCount + 1};
                db.Insert(standFormat);
            }

            args.PutInt("StandFormatID", standFormat.Id);
            fragment.Arguments = args;

            FragmentTransaction fragmentTx = Activity.SupportFragmentManager.BeginTransaction();
            fragmentTx.Replace(Resource.Id.container, fragment);
            fragmentTx.AddToBackStack(null);
            fragmentTx.Commit();
        }
    }

    public class ShootFormatRecyclerAdapter : RecyclerView.Adapter
    {
        public List<DatabaseModels.StandFormats> standFormats;
        private FragmentActivity activity;

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
                args.PutInt("StandFormatID", standFormats[view.AdapterPosition].Id);
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