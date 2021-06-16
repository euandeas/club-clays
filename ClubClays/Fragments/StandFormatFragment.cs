using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.Activity;
using AndroidX.AppCompat.App;
using AndroidX.RecyclerView.Widget;
using ClubClays.DatabaseModels;
using Google.Android.Material.FloatingActionButton;
using SQLite;
using System;
using System.Collections.Generic;
using Fragment = AndroidX.Fragment.App.Fragment;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

namespace ClubClays.Fragments
{
    public class StandFormatFragment : Fragment
    {
        private StandFormatRecyclerAdapter recyclerAdapter;

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

            toolbar.FindViewById<TextView>(Resource.Id.saveButton).Click += Save_Click;

            List<StandShotsFormats> shotFormats;

            string dbPath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "ClubClaysData.db3");
            using (var db = new SQLiteConnection(dbPath))
            {
                shotFormats = db.Table<StandShotsFormats>().Where(s => s.StandFormatId == Arguments.GetInt("StandFormatID")).OrderByDescending(s => s.ShotNum).ToList();
            }

            LinearLayoutManager LayoutManager = new LinearLayoutManager(Activity);
            RecyclerView shootsRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerView);
            shootsRecyclerView.SetLayoutManager(LayoutManager);
            recyclerAdapter = new StandFormatRecyclerAdapter(shotFormats, Arguments.GetInt("StandFormatID"));
            ItemTouchHelper.Callback callback = new ItemMoveSwipeCallback(recyclerAdapter);
            ItemTouchHelper touchHelper = new ItemTouchHelper(callback);
            touchHelper.AttachToRecyclerView(shootsRecyclerView);
            shootsRecyclerView.SetAdapter(recyclerAdapter);     

            FloatingActionButton fab = view.FindViewById<FloatingActionButton>(Resource.Id.addButton);
            fab.Click += Fab_Click;

            return view;
        }

        private void Save_Click(object sender, EventArgs e)
        {
            string dbPath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "ClubClaysData.db3");
            using (var db = new SQLiteConnection(dbPath))
            {
                db.Table<StandShotsFormats>().Where(s => s.StandFormatId == Arguments.GetInt("StandFormatID")).Delete();
                int numClays = 0;
                foreach (StandShotsFormats shot in recyclerAdapter.ShotsFormat)
                {
                    db.Insert(shot);

                    if (shot.Type == "Pair")
                    {
                        numClays += 2;
                    }

                    if (shot.Type == "Single")
                    {
                        numClays += 1;
                    }
                }

                db.CreateCommand($"UPDATE StandFormats SET NumClays = {numClays} WHERE ID = {Arguments.GetInt("StandFormatID")};").ExecuteNonQuery();
            }

            Activity.SupportFragmentManager.PopBackStack();
        }
    
        private void Fab_Click(object sender, EventArgs e)
        {
            PopupMenu popup = new PopupMenu(Context, (sender as View));
            popup.MenuItemClick += Popup_MenuItemClick;
            MenuInflater inflater = popup.MenuInflater;
            inflater.Inflate(Resource.Menu.shot_menu, popup.Menu);
            popup.Show();
        }

        private void Popup_MenuItemClick(object sender, PopupMenu.MenuItemClickEventArgs e)
        {
            switch (e.Item.ItemId)
            {
                case Resource.Id.single:
                    recyclerAdapter.AddItem("Single");
                    break;
                case Resource.Id.pair:
                    recyclerAdapter.AddItem("Pair");
                    break;
            };
        }
    }

    public class StandFormatRecyclerAdapter : RecyclerView.Adapter, ItemMoveSwipeCallback.ItemTouchHelperContract
    {
        private List<StandShotsFormats> shotsFormats;
        private int standFormatId;

        public List<StandShotsFormats> ShotsFormat { get { return shotsFormats; } }

        public void AddItem(string type)
        {
            shotsFormats.Add(new StandShotsFormats { ShotNum = shotsFormats.Count + 1, Type = type, StandFormatId = standFormatId});
        }

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

        public void onRowMoved(int fromPosition, int toPosition)
        {
            if (fromPosition < toPosition)
            {
                for (int i = fromPosition; i < toPosition; i++)
                {
                    Swap(shotsFormats, i, i + 1);
                }
            }
            else
            {
                for (int i = fromPosition; i > toPosition; i--)
                {
                    Swap(shotsFormats, i, i - 1);
                }
            }
            NotifyItemMoved(fromPosition, toPosition);
        }

        public void onRowSelected(RecyclerView.ViewHolder myViewHolder)
        {
            myViewHolder.ItemView.SetBackgroundColor(Color.Gray);
        }

        public void onRowClear(RecyclerView.ViewHolder myViewHolder)
        {
            myViewHolder.ItemView.SetBackgroundColor(Color.White);
        }

        public static void Swap(List<StandShotsFormats> list, int indexA, int indexB)
        {
            StandShotsFormats tmp = list[indexA];
            list[indexA] = list[indexB];
            list[indexB] = tmp;

            list[indexA].ShotNum = indexA + 1;
            list[indexB].ShotNum = indexB + 1;
        }

        public void onSwiped(RecyclerView.ViewHolder myViewHolder, int pos)
        {
            shotsFormats.RemoveAt(pos);
            NotifyItemRemoved(pos);
        }

        // Provide a suitable constructor (depends on the kind of dataset)
        public StandFormatRecyclerAdapter(List<StandShotsFormats> shotsFormats, int standFormatId)
        {
            this.shotsFormats = shotsFormats;
            this.standFormatId = standFormatId;
        }
    }
}