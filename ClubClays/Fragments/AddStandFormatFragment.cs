using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.AppCompat.View.Menu;
using AndroidX.Lifecycle;
using AndroidX.RecyclerView.Widget;
using Google.Android.Material.FloatingActionButton;
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
            HasOptionsMenu = true;

            var scoreManagementModel = new ViewModelProvider(Activity).Get(Java.Lang.Class.FromType(typeof(Shoot))) as Shoot;

            standNum = view.FindViewById<TextView>(Resource.Id.standNum);
            numShots = view.FindViewById<TextView>(Resource.Id.numShots);

            standNum.Text = $"Stand {scoreManagementModel.NumStands + 1}";
            numShots.Text = "0";

            LinearLayoutManager layoutManager = new LinearLayoutManager(Activity);
            RecyclerView shootsRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerView);
            shootsRecyclerView.SetLayoutManager(layoutManager);
            recyclerAdapter = new AddStandFormatRecyclerAdapter(new List<string>(), ref numShots);
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
                if (recyclerAdapter.ItemCount != 0)
                {
                    var scoreManagementModel = new ViewModelProvider(Activity).Get(Java.Lang.Class.FromType(typeof(Shoot))) as Shoot;
                    scoreManagementModel.AddStand(new Stand(recyclerAdapter.ShotsLayout));

                    Bundle result = new Bundle();
                    result.PutBoolean("StandAdded", true);
                    Activity.SupportFragmentManager.SetFragmentResult("1", result);

                    Activity.SupportFragmentManager.PopBackStack();
                }
                else
                {
                    Toast.MakeText(Activity, "No shots added!", ToastLength.Short).Show();
                }
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

        public List<string> ShotsLayout => shotsLayout;

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
            public View MainView { get; set; }
            public TextView ShotType { get; set; }
            public MyView(View view) : base(view)
            {
                MainView = view;
            }
        }

        // Return the size of data set (invoked by the layout manager)
        public override int ItemCount => shotsLayout.Count;

        // Replace the contents of a view (invoked by layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            MyView myHolder = holder as MyView;
            myHolder.ShotType.Text = shotsLayout[position];
        }

        // Create new views (invoked by layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View shootCardView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.shot_format_item, parent, false);
            TextView shotType = shootCardView.FindViewById<TextView>(Resource.Id.shotType);

            MyView view = new MyView(shootCardView) { ShotType = shotType };

            return view;
        }

        public void OnRowMoved(int fromPosition, int toPosition)
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

        public void OnRowSelected(RecyclerView.ViewHolder myViewHolder)
        {
            myViewHolder.ItemView.SetBackgroundColor(Color.Gray);
        }

        public void OnRowClear(RecyclerView.ViewHolder myViewHolder)
        {
            myViewHolder.ItemView.SetBackgroundColor(Color.White);
        }

        public static void Swap<T>(IList<T> list, int indexA, int indexB)
        {
            T tmp = list[indexA];
            list[indexA] = list[indexB];
            list[indexB] = tmp;
        }

        public void OnSwiped(RecyclerView.ViewHolder myViewHolder, int direction)
        {
            if (direction == ItemTouchHelper.Left)
            {
                int pos = myViewHolder.AbsoluteAdapterPosition;
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