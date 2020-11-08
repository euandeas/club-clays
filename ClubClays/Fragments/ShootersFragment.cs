using Android.OS;
using Android.Views;
using AndroidX.RecyclerView.Widget;
using ClubClays.DatabaseModels;
using System.Collections.Generic;
using Fragment = AndroidX.Fragment.App.Fragment;
using SQLite;
using System.IO;
using Android.Widget;
using System.Linq;
using AndroidX.Lifecycle;
using Android.Content;
using AndroidX.Activity;
using AndroidX.AppCompat.App;
using System;

namespace ClubClays.Fragments
{
    public class ShootersFragment : Fragment
    {
        // Required data types:
        //public List<Shooters> selectedShooters;
        //public List<Shooters> allShooters;

        public SelectedShooters selectedShootersModel;

        public RecyclerView allRecyclerView;
        public RecyclerView selectedRecyclerView;
        private RecyclerView.LayoutManager allLayoutManager;
        private RecyclerView.LayoutManager selectedLayoutManager;

        //SQLiteConnection db;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            View view = inflater.Inflate(Resource.Layout.fragment_shooters, container, false);

            //string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "ClubClaysData.db3");
            //db = new SQLiteConnection(dbPath);

            //allShooters = db.Table<Shooters>().ToList();
            //selectedShooters = new List<Shooters>();

            selectedShootersModel = new ViewModelProvider(Activity).Get(Java.Lang.Class.FromType(typeof(SelectedShooters))) as SelectedShooters;
            selectedShootersModel.selectedShooters = new List<Shooters>();
            selectedShootersModel.allShooters = new List<Shooters>();

            allLayoutManager = new LinearLayoutManager(Activity);
            selectedLayoutManager = new LinearLayoutManager(Activity);

            allRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.allRecyclerView);
            allRecyclerView.SetLayoutManager(allLayoutManager);
            allRecyclerView.SetAdapter(new ShootersRecyclerAdapter(this, selectedShootersModel.allShooters, "all"));

            selectedRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.selectedRecyclerView);
            selectedRecyclerView.SetLayoutManager(selectedLayoutManager);
            selectedRecyclerView.SetAdapter(new ShootersRecyclerAdapter(this, selectedShootersModel.selectedShooters, "selected"));

            return view;
        }

        public override void OnStop()
        {
            SendResult();
            base.OnStop();
        }

        public void SendResult()
        {
            Intent intent = new Intent();
            intent.PutExtra("numSelected", selectedShootersModel.selectedShooters.Count);
            TargetFragment.OnActivityResult(TargetRequestCode, 1, intent);
        }

        public void UpdateRecyclerViews(string type, int position)
        {
            if (type == "all")
            {
                selectedShootersModel.selectedShooters.Add(selectedShootersModel.allShooters[position]);
                selectedShootersModel.allShooters.RemoveAt(position);
            }
            else if (type == "selected")
            {
                selectedShootersModel.allShooters.Add(selectedShootersModel.selectedShooters[position]);
                selectedShootersModel.selectedShooters.RemoveAt(position);
            }
        }      
    }

    public class ShootersRecyclerAdapter : RecyclerView.Adapter
    {
        private List<Shooters> shooters;
        private ShootersFragment parentFragment;
        private string type;

        // Provide a reference to the views for each data item
        public class MyView : RecyclerView.ViewHolder
        {
            public View mMainView { get; set; }
            public TextView mShooterName { get; set; }
            public MyView(View view) : base(view)
            {
                mMainView = view;
            }
        }

        // Return the size of data set (invoked by the layout manager)
        public override int ItemCount
        {
            get { return shooters.Count; }
        }

        // Replace the contents of a view (invoked by layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            MyView myHolder = holder as MyView;
            myHolder.mShooterName.Text = shooters[position].Name;
        }

        // Create new views (invoked by layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View shooterCardView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.shooters_item, parent, false);
            TextView shootersName = shooterCardView.FindViewById<TextView>(Resource.Id.shootersName);

            MyView view = new MyView(shooterCardView) { mShooterName = shootersName };

            shooterCardView.Click += delegate
            {
                if (type == "all")
                {
                    parentFragment.selectedShootersModel.selectedShooters.Add(parentFragment.selectedShootersModel.allShooters[view.AdapterPosition]);
                    parentFragment.selectedShootersModel.allShooters.RemoveAt(view.AdapterPosition);
                    NotifyDataSetChanged();
                    parentFragment.selectedRecyclerView.GetAdapter().NotifyDataSetChanged();
                }
                else if (type == "selected")
                {
                    parentFragment.selectedShootersModel.allShooters.Add(parentFragment.selectedShootersModel.selectedShooters[view.AdapterPosition]);
                    parentFragment.selectedShootersModel.selectedShooters.RemoveAt(view.AdapterPosition);
                    NotifyDataSetChanged();
                    parentFragment.allRecyclerView.GetAdapter().NotifyDataSetChanged();
                }
            };

            return view;
        }

        // Provide a suitable constructor (depends on the kind of dataset)
        public ShootersRecyclerAdapter(ShootersFragment initialisingFragment, List<Shooters> shootersList, string recylerType)
        {
            shooters = shootersList;
            parentFragment = initialisingFragment;
            type = recylerType;
        }
    }
}