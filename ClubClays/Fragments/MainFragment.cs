﻿using Android.OS;
using Android.Views;
using Fragment = AndroidX.Fragment.App.Fragment;
using FragmentTransaction = AndroidX.Fragment.App.FragmentTransaction;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;
using Android.Widget;
using Google.Android.Material.AppBar;
using System;
using AndroidX.RecyclerView.Widget;
using AndroidX.AppCompat.View.Menu;
using AndroidX.AppCompat.App;
using Google.Android.Material.FloatingActionButton;
using System.IO;
using SQLite;
using System.Collections.Generic;
using AndroidX.Fragment.App;
using Android.Gms.Ads;

namespace ClubClays.Fragments
{
    public class MainFragment : Fragment
    {
        Toolbar toolbar;
        AppBarLayout appBarLayout;
        View titleTextView;
        RelativeLayout collapsingRelativeLayout;
        string currentDate;
        string mainTitle = "Welcome Back!";

        public override void OnCreate(Bundle savedInstanceState)
        {
            // non-graphical initialisations (you can assign variables, get Intent extras, and anything else that doesn't involve the View hierarchy)
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // do any graphical initialisations (e.g. You can assign your View variables)
            View view = inflater.Inflate(Resource.Layout.fragment_main, container, false);

            Activity.ViewModelStore.Clear();
            Activity.SupportFragmentManager.PopBackStack(null, FragmentManager.PopBackStackInclusive);

            currentDate = DateTime.Now.ToLongDateString().Replace(", ", " ");
            string timeOfDay = DateTime.Now.ToString("tt").ToLower();
            if (timeOfDay == "am")
            {
                mainTitle = "Good Morning!";
            }
            else if (timeOfDay == "pm")
            {
                mainTitle = "Good Afternoon!";
            }

            AdView adView = view.FindViewById<AdView>(Resource.Id.adView);
            AdRequest adRequest = new AdRequest.Builder().Build();
            adView.LoadAd(adRequest);

            toolbar = view.FindViewById<Toolbar>(Resource.Id.toolbar);
            toolbar.Title = mainTitle;
            titleTextView = ToolbarTitle();
            ((AppCompatActivity)Activity).SetSupportActionBar(toolbar);
            HasOptionsMenu = true;

            view.FindViewById<TextView>(Resource.Id.dateText).Text = currentDate;
            view.FindViewById<TextView>(Resource.Id.mainTitleText).Text = mainTitle;

            appBarLayout = view.FindViewById<AppBarLayout>(Resource.Id.appBar);
            appBarLayout.OffsetChanged += AppBarLayout_OffsetChanged;

            var collapsingToolbar = view.FindViewById<CollapsingToolbarLayout>(Resource.Id.collapsingToolbar);
            AppBarLayout.LayoutParams lp = (AppBarLayout.LayoutParams)collapsingToolbar.LayoutParameters;
            lp.Height = (Resources.DisplayMetrics.HeightPixels / 8)*3;

            collapsingRelativeLayout = view.FindViewById<RelativeLayout>(Resource.Id.collapsingRelativeLayout);

            FloatingActionButton fab = view.FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.Click += Fab_Click;

            List<DatabaseModels.Shoots> shoots;

            string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "ClubClaysData.db3");
            using (var db = new SQLiteConnection(dbPath))
            {
                shoots = db.Table<DatabaseModels.Shoots>().OrderByDescending(s => s.Date).ToList();
            }

            LinearLayoutManager LayoutManager = new LinearLayoutManager(Activity);
            RecyclerView shootsRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerView);
            shootsRecyclerView.SetLayoutManager(LayoutManager);
            shootsRecyclerView.SetAdapter(new PreviousShootsRecyclerAdapter(shoots, Activity));

            return view;
        }

        private void Fab_Click(object sender, EventArgs e)
        {
            FragmentTransaction fragmentTx = Activity.SupportFragmentManager.BeginTransaction();
            fragmentTx.Replace(Resource.Id.container, new GeneralDataFragment());
            fragmentTx.AddToBackStack(null);
            fragmentTx.Commit();
        }

        private void AppBarLayout_OffsetChanged(object sender, AppBarLayout.OffsetChangedEventArgs e)
        {
            titleTextView.Alpha = Math.Abs(e.VerticalOffset / (appBarLayout.TotalScrollRange - ((float)appBarLayout.TotalScrollRange / 2)));
            collapsingRelativeLayout.Alpha = 1.0f - Math.Abs(e.VerticalOffset / (appBarLayout.TotalScrollRange - ((float)appBarLayout.TotalScrollRange / 2)));
        }

        private View ToolbarTitle()
        {
            int childCount = toolbar.ChildCount;
            for (int i = 0; i < childCount; i++)
            {
                View child = toolbar.GetChildAt(i);
                if (child is TextView)
                {
                    return child;
                }
            }

            return new View(Activity);
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            inflater.Inflate(Resource.Menu.main_toolbar_menu, menu);

            if (menu is MenuBuilder)
            {
                MenuBuilder m = (MenuBuilder)menu;
                m.SetOptionalIconsVisible(true);
            }

            base.OnCreateOptionsMenu(menu, inflater);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Resource.Id.menu_settings)
            {
                FragmentTransaction fragmentTx = Activity.SupportFragmentManager.BeginTransaction();
                fragmentTx.Replace(Resource.Id.container, new SettingsFragment());
                fragmentTx.AddToBackStack(null);
                fragmentTx.Commit();
            }

            return base.OnOptionsItemSelected(item);
        }
    }

    public class PreviousShootsRecyclerAdapter : RecyclerView.Adapter
    {
        List<DatabaseModels.Shoots> allShoots;
        FragmentActivity activity;

        public class MyView : RecyclerView.ViewHolder
        {
            public View mMainView { get; set; }
            public TextView mShootDate { get; set; }
            public TextView mShootTitle { get; set; }
            public TextView mShootDiscipline { get; set; }
            public TextView mShootLocation { get; set; }
            public MyView(View view) : base(view)
            {
                mMainView = view;
            }
        }
        public override int ItemCount => allShoots.Count;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            MyView myHolder = holder as MyView;
            myHolder.mShootDate.Text = $"{allShoots[position].Date.ToLongDateString().Replace(", ", " ")}";
            myHolder.mShootDiscipline.Text = $"{allShoots[position].EventType}";

            if (allShoots[position].Title == "")
            {
                myHolder.mShootTitle.Visibility = ViewStates.Gone;
            }
            else
            {
                myHolder.mShootTitle.Text = $"{allShoots[position].Title}";
            }

            if (allShoots[position].Location == "")
            {
                myHolder.mShootLocation.Visibility = ViewStates.Gone;
            }
            else
            {
                myHolder.mShootLocation.Text = $"{allShoots[position].Location}";
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View shootCardView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.shoot_item, parent, false);
            TextView shootDate = shootCardView.FindViewById<TextView>(Resource.Id.shootDate);
            TextView shootTitle = shootCardView.FindViewById<TextView>(Resource.Id.shootTitle);
            TextView shootDiscipline = shootCardView.FindViewById<TextView>(Resource.Id.shootDiscipline);
            TextView shootLocation = shootCardView.FindViewById<TextView>(Resource.Id.shootLocation);

            MyView view = new MyView(shootCardView) { mShootDate = shootDate ,mShootTitle = shootTitle, mShootDiscipline = shootDiscipline, mShootLocation = shootLocation };

            shootCardView.Click += delegate
            {
                Fragment fragment = new PreviousShootFragment();
                Bundle args = new Bundle();
                args.PutInt("ShootID", allShoots[view.AbsoluteAdapterPosition].Id);
                fragment.Arguments = args;

                FragmentTransaction fragmentTx = activity.SupportFragmentManager.BeginTransaction();
                fragmentTx.Replace(Resource.Id.container, fragment);
                fragmentTx.AddToBackStack(null);
                fragmentTx.Commit();
            };

            return view;
        }

        public PreviousShootsRecyclerAdapter(List<DatabaseModels.Shoots> shoots, FragmentActivity activity)
        {
            allShoots = shoots;
            this.activity = activity;
        }
    }
}