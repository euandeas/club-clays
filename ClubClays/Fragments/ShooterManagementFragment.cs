using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.RecyclerView.Widget;
using ClubClays.DatabaseModels;
using Google.Android.Material.FloatingActionButton;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Fragment = AndroidX.Fragment.App.Fragment;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

namespace ClubClays.Fragments
{
    public class ShooterManagementFragment : Fragment
    {
        private List<Shooters> shooters;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            View view = inflater.Inflate(Resource.Layout.fragment_shooters, container, false);

            Toolbar toolbar = view.FindViewById<Toolbar>(Resource.Id.toolbar);
            ((AppCompatActivity)Activity).SetSupportActionBar(toolbar);
            ActionBar supportBar = ((AppCompatActivity)Activity).SupportActionBar;
            supportBar.SetDisplayHomeAsUpEnabled(true);
            supportBar.SetDisplayShowHomeEnabled(true);

            view.FindViewById<TextView>(Resource.Id.allLabel).Visibility = ViewStates.Gone;
            view.FindViewById<TextView>(Resource.Id.selectedLabel).Visibility = ViewStates.Gone;

            LinearLayoutManager allLayoutManager = new LinearLayoutManager(Activity);

            RecyclerView  shootersRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.allRecyclerView);
            shootersRecyclerView.SetLayoutManager(allLayoutManager);

            string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "ClubClaysData.db3");
            SQLiteConnection db = new SQLiteConnection(dbPath);
            shooters = db.Table<Shooters>().ToList();
            db.Close();
            shootersRecyclerView.SetAdapter(new ShooterManagementRecyclerAdapter(ref shooters, this));

            FloatingActionButton fab = view.FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.Click += Fab_Click;

            return view;
        }

        private void Fab_Click(object sender, EventArgs e)
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(Activity);
            builder.SetTitle("Add New Shooter");

            View view = LayoutInflater.From(Activity).Inflate(Resource.Layout.dialogfragment_addshooter, null);

            EditText shooterName = view.FindViewById<EditText>(Resource.Id.newShootersName);
            EditText shooterClass = view.FindViewById<EditText>(Resource.Id.newShooterClass);

            builder.SetView(view);
            builder.SetPositiveButton("Add", (c, ev) =>
            {
                //Shooters newShooter = new Shooters { Name = shooterName.Text, Class = shooterClass.Text };

                string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "ClubClaysData.db3");
                using SQLiteConnection db = new SQLiteConnection(dbPath);
                db.CreateCommand($"INSERT OR IGNORE INTO Shooters(Name, Class) VALUES ('{shooterName.Text}', '{shooterClass.Text}');").ExecuteNonQuery();
                var shooter = db.Table<Shooters>().Where(s => s.Name == shooterName.Text).ToList();
                db.Close();

                shooters.Add(shooter[0]);
            });
            builder.SetNegativeButton("Cancel", (c, ev) => { });

            builder.Show();
        }
    }

    public class ShooterManagementRecyclerAdapter : RecyclerView.Adapter
    {
        private List<Shooters> shooters;
        private Context cont;

        // Provide a reference to the views for each data item
        public class MyView : RecyclerView.ViewHolder
        {
            public View mMainView { get; set; }
            public TextView mShooterName { get; set; }
            public TextView mShooterClass { get; set; }
            public MyView(View view) : base(view)
            {
                mMainView = view;
            }
        }

        // Return the size of data set (invoked by the layout manager)
        public override int ItemCount => shooters.Count;

        // Replace the contents of a view (invoked by layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            MyView myHolder = holder as MyView;
            myHolder.mShooterName.Text = shooters[position].Name;
            myHolder.mShooterClass.Text = shooters[position].Class;
        }

        // Create new views (invoked by layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View shooterCardView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.shootermanagement_item, parent, false);
            TextView shootersName = shooterCardView.FindViewById<TextView>(Resource.Id.shootersName);
            TextView shootersClass = shooterCardView.FindViewById<TextView>(Resource.Id.shootersClass);
            ImageButton editButton = shooterCardView.FindViewById<ImageButton>(Resource.Id.shooterEditButton);
            ImageButton deleteButton = shooterCardView.FindViewById<ImageButton>(Resource.Id.shooterDeleteButton);

            MyView view = new MyView(shooterCardView) { mShooterName = shootersName, mShooterClass = shootersClass };

            editButton.Click += delegate
            {
                AlertDialog.Builder builder = new AlertDialog.Builder(cont);
                builder.SetTitle("Edit Shooter");

                View layoutview = LayoutInflater.From(cont).Inflate(Resource.Layout.dialogfragment_addshooter, null);

                EditText shooterName = layoutview.FindViewById<EditText>(Resource.Id.newShootersName);
                EditText shooterClass = layoutview.FindViewById<EditText>(Resource.Id.newShooterClass);

                shooterName.Text = shooters[view.AdapterPosition].Name;
                shooterClass.Text = shooters[view.AdapterPosition].Class;

                builder.SetView(layoutview);
                builder.SetPositiveButton("Save", (c, ev) =>
                {
                    string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "ClubClaysData.db3");
                    using (var db = new SQLiteConnection(dbPath))
                    {
                        db.CreateCommand($"UPDATE Shooters SET Name = '{shooterName.Text}', Class = '{shooterClass.Text}' WHERE Id = {shooters[view.AdapterPosition].Id};").ExecuteNonQuery(); 
                    }

                    shooters[view.AdapterPosition].Name = shooterName.Text;
                    shooters[view.AdapterPosition].Class = shooterClass.Text;
                    NotifyDataSetChanged();
                });
                builder.SetNegativeButton("Cancel", (c, ev) => { });

                builder.Show();
            };
            
            deleteButton.Click += delegate
            {
                AlertDialog.Builder builder = new AlertDialog.Builder(cont);
                builder.SetMessage("Are you sure you want to delete this shooter? This will remove all their accompanying data from shoots.");
                builder.SetPositiveButton("Yes", (c, ev) =>
                {
                    string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "ClubClaysData.db3");
                    using (var db = new SQLiteConnection(dbPath))
                    {
                        int shooterId = shooters[view.AdapterPosition].Id;
                        db.Delete<Shooters>(shooterId);
                        var potentialShootsToDelete = db.Table<OverallScores>().Where(s => s.ShooterId == shooterId).ToList();
                        db.CreateCommand($"DELETE FROM OverallScores WHERE ShooterId = {shooterId};").ExecuteNonQuery();
                        var standScores = db.Table<StandScores>().Where(s => s.ShooterId == shooterId).ToList();
                        db.CreateCommand($"DELETE FROM StandScores WHERE ShooterId = {shooterId};").ExecuteNonQuery();
                        foreach (var standScore in standScores) 
                        {
                            db.CreateCommand($"DELETE FROM StandShotsLink WHERE StandScoresId = {standScore.Id};").ExecuteNonQuery();
                        }

                        foreach (var potentialShoot in potentialShootsToDelete)
                        {
                            var listOfScoresForShoot = db.Table<OverallScores>().Where(s => s.ShootId == potentialShoot.ShootId).ToList();
                            if (listOfScoresForShoot.Count == 0)
                            {
                                db.Delete<Shoots>(potentialShoot.ShootId);
                            }
                        }
                    }
                    shooters.Remove(shooters[view.AdapterPosition]);
                    NotifyDataSetChanged();
                });

                builder.SetNegativeButton("No", (c, ev) => { });
                builder.Show();
            };

            return view;
        }

        // Provide a suitable constructor (depends on the kind of dataset)
        public ShooterManagementRecyclerAdapter(ref List<Shooters> shootersList, ShooterManagementFragment context)
        {
            shooters = shootersList;
            cont = context.Context;
        }
    } 
}