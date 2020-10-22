using Android.OS;
using Android.Views;
using AndroidX.RecyclerView.Widget;
using ClubClays.DatabaseModels;
using System.Collections.Generic;
using Fragment = AndroidX.Fragment.App.Fragment;
using SQLite;
using System.IO;
using System;

namespace ClubClays.Fragments
{
    public class ShootersFragment : Fragment
    {
        // Required data types:
        private List<Shooters> selectedShooters;
        private List<Shooters> allShooters;

        SQLiteConnection db;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            View view = inflater.Inflate(Resource.Layout.fragment_shooters, container, false);

            string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "ClubClaysData.db3");
            db = new SQLiteConnection(dbPath);

            var dbQuery = db.Table<Shooters>();
            allShooters = dbQuery.ToList();
            Console.WriteLine(allShooters);

            return view;
        }
    }

    public class ShootersRecyclerAdapter : RecyclerView.Adapter
    {
        private List<Shooters> shooters;

        // Provide a reference to the views for each data item
        public class MyView : RecyclerView.ViewHolder
        {
            public View mMainView { get; set; }
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
            throw new NotImplementedException();
        }

        // Create new views (invoked by layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            throw new NotImplementedException();
        }

        // Provide a suitable constructor (depends on the kind of dataset)
        public ShootersRecyclerAdapter(List<Shooters> shootersList)
        {
            shooters = shootersList;
        }
    }
}