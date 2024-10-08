﻿using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.RecyclerView.Widget;
using ClubClays.DatabaseModels;
using Google.Android.Material.Dialog;
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
            MaterialAlertDialogBuilder builder = new MaterialAlertDialogBuilder(Activity);
            builder.SetTitle("Add New Shooter");

            View view = LayoutInflater.From(Activity).Inflate(Resource.Layout.dialogfragment_addshooter, null);

            EditText shooterName = view.FindViewById<EditText>(Resource.Id.newShootersName);
            EditText shooterClass = view.FindViewById<EditText>(Resource.Id.newShooterClass);

            int affected;
            builder.SetView(view);
            builder.SetPositiveButton("Add", (EventHandler<DialogClickEventArgs>)null);
            builder.SetNegativeButton("Cancel", (c, ev) => { });

            var dialog = builder.Create();
            dialog.Show();

            dialog.GetButton((int)DialogButtonType.Positive).Click += (sender, args) =>
            {
                if (shooterName.Text == "" || string.IsNullOrWhiteSpace(shooterName.Text))
                {
                    Toast.MakeText(Activity, "Shooter name is empty!", ToastLength.Short).Show();
                }
                else
                {
                    string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "ClubClaysData.db3");
                    using SQLiteConnection db = new SQLiteConnection(dbPath);
                    affected = db.CreateCommand($"INSERT OR IGNORE INTO Shooters(Name, Class) VALUES ('{shooterName.Text}', '{shooterClass.Text}');").ExecuteNonQuery();

                    if (affected != 0)
                    {
                        shooters.Add(db.Table<Shooters>().Where(s => s.Name == shooterName.Text).First());
                        dialog.Dismiss();
                    }
                    else
                    {
                        Toast.MakeText(Activity, "Shooter already exists!", ToastLength.Short).Show();
                    }
                }
            };
        }
    }

    public class ShooterManagementRecyclerAdapter : RecyclerView.Adapter
    {
        private List<Shooters> shooters;
        private Context cont;

        // Provide a reference to the views for each data item
        public class MyView : RecyclerView.ViewHolder
        {
            public View MainView { get; set; }
            public TextView ShooterName { get; set; }
            public TextView ShooterClass { get; set; }
            public MyView(View view) : base(view)
            {
                MainView = view;
            }
        }

        // Return the size of data set (invoked by the layout manager)
        public override int ItemCount => shooters.Count;

        // Replace the contents of a view (invoked by layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            MyView myHolder = holder as MyView;
            myHolder.ShooterName.Text = shooters[position].Name;
            myHolder.ShooterClass.Text = shooters[position].Class;
        }

        // Create new views (invoked by layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View shooterCardView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.shootermanagement_item, parent, false);
            TextView shootersName = shooterCardView.FindViewById<TextView>(Resource.Id.shootersName);
            TextView shootersClass = shooterCardView.FindViewById<TextView>(Resource.Id.shootersClass);
            ImageButton editButton = shooterCardView.FindViewById<ImageButton>(Resource.Id.shooterEditButton);
            ImageButton deleteButton = shooterCardView.FindViewById<ImageButton>(Resource.Id.shooterDeleteButton);

            MyView view = new MyView(shooterCardView) { ShooterName = shootersName, ShooterClass = shootersClass };

            editButton.Click += delegate
            {
                MaterialAlertDialogBuilder builder = new MaterialAlertDialogBuilder(cont);
                builder.SetTitle("Edit Shooter");

                View layoutview = LayoutInflater.From(cont).Inflate(Resource.Layout.dialogfragment_addshooter, null);

                TextInputEditText shooterName = layoutview.FindViewById<TextInputEditText>(Resource.Id.newShootersName);
                TextInputEditText shooterClass = layoutview.FindViewById<TextInputEditText>(Resource.Id.newShooterClass);

                shooterName.Text = shooters[view.AbsoluteAdapterPosition].Name;
                shooterClass.Text = shooters[view.AbsoluteAdapterPosition].Class;

                int affected;
                builder.SetView(layoutview);
                builder.SetPositiveButton("Save", (EventHandler<DialogClickEventArgs>)null);
                builder.SetNegativeButton("Cancel", (c, ev) => { });

                var dialog = builder.Create();
                dialog.Show();

                dialog.GetButton((int)DialogButtonType.Positive).Click += (sender, args) =>
                {
                    if (shooterName.Text == "" || string.IsNullOrWhiteSpace(shooterName.Text))
                    {
                        Toast.MakeText(cont, "Shooter name is empty!", ToastLength.Short).Show();
                    }
                    else
                    {
                        string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "ClubClaysData.db3");
                        using (var db = new SQLiteConnection(dbPath))
                        {
                            affected = db.CreateCommand($"UPDATE OR IGNORE Shooters SET Name = '{shooterName.Text}', Class = '{shooterClass.Text}' WHERE Id = {shooters[view.AbsoluteAdapterPosition].Id};").ExecuteNonQuery();
                        }

                        if (affected != 0)
                        {
                            shooters[view.AbsoluteAdapterPosition].Name = shooterName.Text;
                            shooters[view.AbsoluteAdapterPosition].Class = shooterClass.Text;
                            NotifyDataSetChanged();
                            dialog.Dismiss();
                        }
                        else
                        {
                            Toast.MakeText(cont, "Shooter already exists!", ToastLength.Short).Show();
                        }
                    }
                };
            };
            
            deleteButton.Click += delegate
            {
                MaterialAlertDialogBuilder builder = new MaterialAlertDialogBuilder(cont);
                builder.SetTitle("Delete shooter?");
                builder.SetMessage("This will remove all their accompanying data from shoots.");
                builder.SetPositiveButton("Yes", (c, ev) =>
                {
                    string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "ClubClaysData.db3");
                    using (var db = new SQLiteConnection(dbPath))
                    {
                        int shooterId = shooters[view.AbsoluteAdapterPosition].Id;
                        db.Delete<Shooters>(shooterId);
                        var potentialShootsToDelete = db.Table<OverallScores>().Where(s => s.ShooterId == shooterId).ToList();
                        db.CreateCommand($"DELETE FROM OverallScores WHERE ShooterId = {shooterId};").ExecuteNonQuery();
                        var standScores = db.Table<StandScores>().Where(s => s.ShooterId == shooterId).ToList();
                        db.CreateCommand($"DELETE FROM StandScores WHERE ShooterId = {shooterId};").ExecuteNonQuery();
                        foreach (var standScore in standScores) 
                        {
                            db.CreateCommand($"DELETE FROM Shots WHERE StandScoreId = {standScore.Id};").ExecuteNonQuery();
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
                    shooters.Remove(shooters[view.AbsoluteAdapterPosition]);
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