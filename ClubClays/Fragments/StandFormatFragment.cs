using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.AppCompat.View.Menu;
using AndroidX.CardView.Widget;
using AndroidX.Lifecycle;
using AndroidX.RecyclerView.Widget;
using Google.Android.Material.FloatingActionButton;
using System;
using System.Collections.Generic;
using Fragment = AndroidX.Fragment.App.Fragment;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

namespace ClubClays.Fragments
{
    public class StandFormatFragment : Fragment
    {
        private StandFormatRecyclerAdapter recyclerAdapter;
        private TextView standNum;
        private TextView numShots;
        private ShootFormat shootFormat;

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
            HasOptionsMenu = true;

            standNum = view.FindViewById<TextView>(Resource.Id.standNum);
            numShots = view.FindViewById<TextView>(Resource.Id.numShots);

            shootFormat = new ViewModelProvider(Activity).Get(Java.Lang.Class.FromType(typeof(ShootFormat))) as ShootFormat;
            List<string> shotFormats = new List<string>();

            if (Arguments.GetBoolean("NewStand", false))
            {
                standNum.Text = $"Stand {Arguments.GetInt("StandNum")}";
                numShots.Text = "0";
            }
            else
            {
                Stand stand = shootFormat.stands[Arguments.GetInt("StandNum")-1];

                standNum.Text = $"Stand {Arguments.GetInt("StandNum")}";
                numShots.Text = $"{stand.numClays}";
                shotFormats = stand.shotFormat;
            }

            LinearLayoutManager layoutManager = new LinearLayoutManager(Activity);
            RecyclerView shootsRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerView);
            shootsRecyclerView.SetLayoutManager(layoutManager);
            recyclerAdapter = new StandFormatRecyclerAdapter(shotFormats, ref numShots);
            ItemTouchHelper.Callback callback = new ItemMoveSwipeCallback(recyclerAdapter);
            ItemTouchHelper touchHelper = new ItemTouchHelper(callback);
            touchHelper.AttachToRecyclerView(shootsRecyclerView);
            shootsRecyclerView.SetAdapter(recyclerAdapter);     

            FloatingActionButton fab = view.FindViewById<FloatingActionButton>(Resource.Id.addButton);
            fab.Click += Fab_Click;

            return view;
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            inflater.Inflate(Resource.Menu.stand_formats_toolbar_menu, menu);

            if (menu is MenuBuilder)
            {
                MenuBuilder m = (MenuBuilder)menu;
                m.SetOptionalIconsVisible(true);
            }

            base.OnCreateOptionsMenu(menu, inflater);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Resource.Id.save_format)
            {
                if (Arguments.GetBoolean("NewStand", false) && recyclerAdapter.ItemCount != 0)
                {
                    shootFormat.stands.Add(new Stand(recyclerAdapter.ShotsFormat));
                }
                else if (recyclerAdapter.ItemCount != 0)
                {
                    shootFormat.stands[Arguments.GetInt("StandNum") - 1].shotFormat = recyclerAdapter.ShotsFormat;
                }
                else if (recyclerAdapter.ItemCount == 0)
                {
                    shootFormat.stands.RemoveAt(Arguments.GetInt("StandNum") - 1);
                }

                Activity.SupportFragmentManager.PopBackStack();
            }

            return base.OnOptionsItemSelected(item);
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
                    if (numShots.Text == "10")
                    {
                        Toast.MakeText(Activity, "No more then 10 shots supported per stand", ToastLength.Short).Show();
                    }
                    else
                    {
                        recyclerAdapter.AddItem("Single");
                    }
                    break;
                case Resource.Id.pair:
                    if (numShots.Text == "10" || numShots.Text == "9" )
                    {
                        Toast.MakeText(Activity, "No more then 10 shots supported per stand", ToastLength.Short).Show();
                    }
                    else
                    {
                        recyclerAdapter.AddItem("Pair");
                    }
                    break;
            };
        }
    }

    public class StandFormatRecyclerAdapter : RecyclerView.Adapter, ItemMoveSwipeCallback.ItemTouchHelperContract
    {
        private List<string> shotsFormats;
        private TextView shotsTextView;
        private int numShots;
        private Color defColor;

        public List<string> ShotsFormat => shotsFormats;

        public void AddItem(string type)
        {
            shotsFormats.Add(type);
            NumberOfShots();
            NotifyDataSetChanged();
        }

        public void NumberOfShots()
        {
            numShots = 0;
            foreach (string format in shotsFormats)
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
            public View MainView { get; set; }
            public TextView ShotType { get; set; }
            public CardView ShootCard { get; set; }
            public MyView(View view) : base(view)
            {
                MainView = view;
            }
        }

        // Return the size of data set (invoked by the layout manager)
        public override int ItemCount => shotsFormats.Count;

        // Replace the contents of a view (invoked by layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            MyView myHolder = holder as MyView;
            myHolder.ShotType.Text = shotsFormats[position];
        }

        // Create new views (invoked by layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View shootCardView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.shot_format_item, parent, false);
            CardView shootCard = shootCardView.FindViewById<CardView>(Resource.Id.shootersCard);
            TextView shotType = shootCardView.FindViewById<TextView>(Resource.Id.shotType);

            defColor = new Color(((CardView)shootCardView).CardBackgroundColor.DefaultColor);
            
            MyView view = new MyView(shootCardView) { ShotType = shotType, ShootCard = shootCard };

            return view;
        }

        public void OnRowMoved(int fromPosition, int toPosition)
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

        public void OnRowSelected(RecyclerView.ViewHolder myViewHolder)
        {
            ((MyView)myViewHolder).ShootCard.SetCardBackgroundColor(Color.Gray);
        }

        public void OnRowClear(RecyclerView.ViewHolder myViewHolder)
        {
            ((MyView)myViewHolder).ShootCard.SetCardBackgroundColor(defColor);
        }

        public static void Swap(List<string> list, int indexA, int indexB)
        {
            string tmp = list[indexA];
            list[indexA] = list[indexB];
            list[indexB] = tmp;
        }

        public void OnSwiped(RecyclerView.ViewHolder myViewHolder, int direction)
        {
            if (direction == ItemTouchHelper.Left)
            {
                int pos = myViewHolder.AbsoluteAdapterPosition;
                shotsFormats.RemoveAt(pos);
                NumberOfShots();
                NotifyItemRemoved(pos);
            }

        }

        // Provide a suitable constructor (depends on the kind of dataset)
        public StandFormatRecyclerAdapter(List<string> shotsFormats, ref TextView shotsTextView)
        {
            this.shotsFormats = shotsFormats;
            this.shotsTextView = shotsTextView;
        }
    }
}