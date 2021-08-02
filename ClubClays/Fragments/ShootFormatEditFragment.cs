using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.Activity;
using AndroidX.AppCompat.App;
using AndroidX.AppCompat.View.Menu;
using AndroidX.Fragment.App;
using AndroidX.Lifecycle;
using AndroidX.RecyclerView.Widget;
using ClubClays.DatabaseModels;
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
        string titleText = "";
        private TextInputEditText title;
        private int shootFormatID;
        ShootFormat shootFormat;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            shootFormat = new ViewModelProvider(Activity).Get(Java.Lang.Class.FromType(typeof(ShootFormat))) as ShootFormat;
            shootFormat.stands = new List<Stand>();

            if (!Arguments.GetBoolean("NewShoot", false))
            {
                shootFormatID = Arguments.GetInt("ShootFormatID");

                string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "ClubClaysData.db3");
                using (var db = new SQLiteConnection(dbPath))
                {
                    titleText = db.Table<ShootFormats>().Where(s => s.Id == shootFormatID).ToList()[0].FormatName;
                    var standFormats = db.Table<StandFormats>().Where(s => s.ShootFormatId == shootFormatID).OrderBy(s => s.StandNum).ToList();

                    foreach (StandFormats stand in standFormats)
                    {
                        List<string> shotFormat = new List<string>();
                        var standShots = db.Table<StandShotsFormats>().Where(s => s.StandFormatId == stand.Id).OrderBy(s => s.ShotNum).ToList();
                        foreach (StandShotsFormats shot in standShots)
                        {
                            shotFormat.Add(shot.Type);
                        }

                        var standObj = new Stand(shotFormat)
                        {
                            id = stand.Id
                        };
                        shootFormat.stands.Add(standObj);
                    }
                }

                shootFormat.originalStands = shootFormat.stands;
            }
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
            HasOptionsMenu = true;

            title = view.FindViewById<TextInputEditText>(Resource.Id.shootFormatTitleEditText);

            title.Text = titleText;       

            LinearLayoutManager LayoutManager = new LinearLayoutManager(Activity);
            RecyclerView shootsRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerView);
            shootsRecyclerView.SetLayoutManager(LayoutManager);
            recyclerAdapter = new ShootFormatRecyclerAdapter(ref shootFormat.stands, Activity);
            shootsRecyclerView.SetAdapter(recyclerAdapter);

            FloatingActionButton fab = view.FindViewById<FloatingActionButton>(Resource.Id.addButton);
            fab.Click += Fab_Click;

            return view;
        }
        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            inflater.Inflate(Resource.Menu.shoots_formats_toolbar_menu, menu);

            if (menu is MenuBuilder)
            {
                MenuBuilder m = (MenuBuilder)menu;
                m.SetOptionalIconsVisible(true);
            }

            base.OnCreateOptionsMenu(menu, inflater);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "ClubClaysData.db3");

            if (item.ItemId == Resource.Id.delete_format)
            {
                if (!Arguments.GetBoolean("NewShoot", false))
                {
                    using (var db = new SQLiteConnection(dbPath))
                    {
                        db.Delete<ShootFormats>(shootFormatID);
                        foreach (Stand stand in shootFormat.originalStands)
                        {
                            db.Delete<StandFormats>(stand.id);
                            db.CreateCommand($"DELETE FROM StandShotsFormats WHERE StandFormatId = {stand.id};").ExecuteNonQuery();
                        }
                    }
                }

                Activity.SupportFragmentManager.PopBackStack();
            }
            else if (item.ItemId == Resource.Id.save_format)
            {
                if (title.Text != "" && recyclerAdapter.ItemCount != 0)
                {
                    int numClays = 0;
                    foreach (Stand stand in recyclerAdapter.standFormats)
                    {
                        numClays += stand.numClays;
                    }


                    if (Arguments.GetBoolean("NewShoot", false))
                    {
                        using (var db = new SQLiteConnection(dbPath))
                        {
                            ShootFormats shootFormat = new ShootFormats() { FormatName = title.Text, NumStands = recyclerAdapter.ItemCount, ClayAmount = numClays };
                            db.Insert(shootFormat);
                            shootFormatID = shootFormat.Id;
                        }
                    }
                    else
                    {
                        using (var db = new SQLiteConnection(dbPath))
                        {
                            db.CreateCommand($"UPDATE ShootFormats SET FormatName = '{title.Text}', NumStands = {recyclerAdapter.ItemCount}, ClayAmount = {numClays} WHERE ID = {shootFormatID};").ExecuteNonQuery();
                            foreach (Stand stand in shootFormat.originalStands)
                            {
                                db.Delete<StandFormats>(stand.id);
                                db.CreateCommand($"DELETE FROM StandShotsFormats WHERE StandFormatId = {stand.id};").ExecuteNonQuery();
                            }   
                        }
                    }

                    using (var db = new SQLiteConnection(dbPath))
                    {
                        int standNum = 1;
                        foreach (Stand stand in recyclerAdapter.standFormats)
                        {
                            StandFormats standFormat = new StandFormats() { ShootFormatId = shootFormatID, NumClays = stand.numClays, StandNum = standNum++ };
                            db.Insert(standFormat);
                            int shotNum = 1;
                            foreach (string shot in stand.shotFormat)
                            {
                                StandShotsFormats standShotFormat = new StandShotsFormats() { StandFormatId = standFormat.Id, ShotNum = shotNum++, Type = shot };
                                db.Insert(standShotFormat);
                            }
                        }
                    }

                    if (Arguments.GetBoolean("Selectable", false))
                    {
                        var standShooterModel = new ViewModelProvider(Activity).Get(Java.Lang.Class.FromType(typeof(ShooterStandData))) as ShooterStandData;
                        using (var db = new SQLiteConnection(dbPath))
                        {
                            var selectedShootFormat = db.Get<ShootFormats>(shootFormatID);
                            standShooterModel.selectedFormat = selectedShootFormat;
                        }
                    }

                    Activity.SupportFragmentManager.PopBackStack();
                }
            }

            return base.OnOptionsItemSelected(item);
        }

        private void Fab_Click(object sender, EventArgs e)
        {
            StandFormatFragment fragment = new StandFormatFragment();
            Bundle args = new Bundle();
            args.PutInt("StandNum", recyclerAdapter.ItemCount + 1);
            args.PutBoolean("NewStand", true);
            fragment.Arguments = args;

            FragmentTransaction fragmentTx = Activity.SupportFragmentManager.BeginTransaction();
            fragmentTx.Replace(Resource.Id.container, fragment);
            fragmentTx.AddToBackStack(null);
            fragmentTx.Commit();
        }

    }

    public class ShootFormatRecyclerAdapter : RecyclerView.Adapter
    {
        public List<Stand> standFormats;
        private FragmentActivity activity;

        // Provide a reference to the views for each data item
        public class MyView : RecyclerView.ViewHolder
        {
            public View MainView { get; set; }
            public TextView StandNum { get; set; }
            public TextView NumOfShots { get; set; }
            public ImageButton EditButton { get; set; }
            public MyView(View view) : base(view)
            {
                MainView = view;
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
            myHolder.StandNum.Text = $"Stand {position+1}";
            myHolder.NumOfShots.Text = $"{standFormats[position].numClays} Shot(s)";
        }

        // Create new views (invoked by layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View shootCardView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.stand_format_item, parent, false);
            TextView shootFormatTitle = shootCardView.FindViewById<TextView>(Resource.Id.standNum);
            TextView numStands = shootCardView.FindViewById<TextView>(Resource.Id.numOfShots);
            ImageButton editButton = shootCardView.FindViewById<ImageButton>(Resource.Id.standFormatEditButton);

            MyView view = new MyView(shootCardView) { StandNum = shootFormatTitle, NumOfShots = numStands, EditButton = editButton };

            editButton.Click += delegate
            {
                StandFormatFragment fragment = new StandFormatFragment();
                Bundle args = new Bundle();
                args.PutInt("StandNum", view.AbsoluteAdapterPosition+1);
                fragment.Arguments = args;

                FragmentTransaction fragmentTx = activity.SupportFragmentManager.BeginTransaction();
                fragmentTx.Replace(Resource.Id.container, fragment);
                fragmentTx.AddToBackStack(null);
                fragmentTx.Commit();
            };

            return view;
        }

        // Provide a suitable constructor (depends on the kind of dataset)
        public ShootFormatRecyclerAdapter(ref List<Stand> standFormats, FragmentActivity activity)
        {
            this.standFormats = standFormats;
            this.activity = activity;
        }
    }
}