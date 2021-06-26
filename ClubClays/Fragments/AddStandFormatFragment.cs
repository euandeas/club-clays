using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.Activity;
using AndroidX.AppCompat.App;
using AndroidX.Lifecycle;
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
    public class AddStandFormatFragment : Fragment
    {
        private AddStandFormatRecyclerAdapter recyclerAdapter;
        private TextView standNum;
        private TextView numShots;

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

            var scoreManagementModel = new ViewModelProvider(Activity).Get(Java.Lang.Class.FromType(typeof(Shoot))) as Shoot;

            standNum = view.FindViewById<TextView>(Resource.Id.standNum);
            numShots = view.FindViewById<TextView>(Resource.Id.numShots);

            standNum.Text = $"Stand {scoreManagementModel.NumStands + 1}";
            numShots.Text = "0";

            LinearLayoutManager LayoutManager = new LinearLayoutManager(Activity);
            RecyclerView shootsRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerView);
            shootsRecyclerView.SetLayoutManager(LayoutManager);
            recyclerAdapter = new AddStandFormatRecyclerAdapter(new List<string>(), ref numShots);
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
            var scoreManagementModel = new ViewModelProvider(Activity).Get(Java.Lang.Class.FromType(typeof(Shoot))) as Shoot;    
            scoreManagementModel.AddStand(new Stand(recyclerAdapter.ShotsLayout));
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

    public class AddStandFormatRecyclerAdapter : RecyclerView.Adapter, ItemMoveSwipeCallback.ItemTouchHelperContract
    {
        private TextView shotsTextView;
        private List<string> shotsLayout;
        int numShots;

        public List<string> ShotsLayout
        {
            get
            {
                return shotsLayout;
            }
        }

        public void AddItem(string type)
        {
            shotsLayout.Add(type);
            NumberOfShots();
            NotifyDataSetChanged();
        }

        public void NumberOfShots() 
        {
            numShots = 0;
            foreach (string format in shotsLayout)
            {
                if (format == "Pair")
                {
                    numShots += 2;
                }

                if (format == "Single")
                {
                    numShots += 1;
                }
            }

            shotsTextView.Text = $"{numShots}";
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
            get { return shotsLayout.Count; }
        }

        // Replace the contents of a view (invoked by layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            MyView myHolder = holder as MyView;
            myHolder.mShotType.Text = shotsLayout[position];
        }

        // Create new views (invoked by layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View shootCardView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.shot_format_item, parent, false);
            TextView shotType = shootCardView.FindViewById<TextView>(Resource.Id.shotType);

            MyView view = new MyView(shootCardView) { mShotType = shotType };

            return view;
        }

        public void onRowMoved(int fromPosition, int toPosition)
        {
            if (fromPosition < toPosition)
            {
                for (int i = fromPosition; i < toPosition; i++)
                {
                    Swap(shotsLayout, i, i + 1);
                }
            }
            else
            {
                for (int i = fromPosition; i > toPosition; i--)
                {
                    Swap(shotsLayout, i, i - 1);
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

        public static void Swap<T>(IList<T> list, int indexA, int indexB)
        {
            T tmp = list[indexA];
            list[indexA] = list[indexB];
            list[indexB] = tmp;
        }

        public void onSwiped(RecyclerView.ViewHolder myViewHolder, int direction)
        {
            if (direction == ItemTouchHelper.Left)
            {
                int pos = myViewHolder.AdapterPosition;
                shotsLayout.RemoveAt(pos);
                NumberOfShots();
                NotifyItemRemoved(pos);
            }

        }

        // Provide a suitable constructor (depends on the kind of dataset)
        public AddStandFormatRecyclerAdapter(List<string> shotsLayout, ref TextView shotsTextView)
        {
            this.shotsTextView = shotsTextView;
            this.shotsLayout = shotsLayout;
        }
    }
}