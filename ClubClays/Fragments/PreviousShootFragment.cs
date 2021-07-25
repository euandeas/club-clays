using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.Activity.Result;
using AndroidX.Activity.Result.Contract;
using AndroidX.AppCompat.App;
using AndroidX.AppCompat.View.Menu;
using AndroidX.Core.Content;
using AndroidX.Lifecycle;
using AndroidX.ViewPager2.Widget;
using ClubClays.DatabaseModels;
using Google.Android.Material.AppBar;
using Google.Android.Material.Dialog;
using Google.Android.Material.Tabs;
using SQLite;
using System;
using System.IO;
using Fragment = AndroidX.Fragment.App.Fragment;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

namespace ClubClays.Fragments
{
    public class PreviousShootFragment : Fragment, IActivityResultCallback
    {
        private ActivityResultLauncher launcher;
        private Shoot previousShootModel;
        int shootId;
        Java.IO.File file;
        Toolbar toolbar;
        View titleTextView;
        AppBarLayout appBarLayout;
        RelativeLayout collapsingRelativeLayout;
        ViewPager2 viewPager;
        TabLayout tabLayout;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            View view = inflater.Inflate(Resource.Layout.fragment_previous_shoot, container, false);

            launcher = RegisterForActivityResult(new ActivityResultContracts.StartActivityForResult(), this);

            shootId = Arguments.GetInt("ShootID");

            previousShootModel = new ViewModelProvider(Activity).Get(Java.Lang.Class.FromType(typeof(Shoot))) as Shoot;
            previousShootModel.InitialisePreviousShoot(shootId);

            toolbar = view.FindViewById<Toolbar>(Resource.Id.toolbar);
            ((AppCompatActivity)Activity).SetSupportActionBar(toolbar);
            ActionBar supportBar = ((AppCompatActivity)Activity).SupportActionBar;
            if (previousShootModel.Title == "")
            {
                supportBar.Title = "Previous Shoot";
            }
            else
            {
                supportBar.Title = $"{previousShootModel.Title}";
            }
            titleTextView = ToolbarTitle();
            supportBar.SetDisplayHomeAsUpEnabled(true);
            supportBar.SetDisplayShowHomeEnabled(true);
            HasOptionsMenu = true;

            view.FindViewById<TextView>(Resource.Id.mainTitleText).Text = supportBar.Title;
            view.FindViewById<TextView>(Resource.Id.dateText).Text = previousShootModel.Date.ToLongDateString().Replace(", ", " ");

            appBarLayout = view.FindViewById<AppBarLayout>(Resource.Id.appBar);
            appBarLayout.OffsetChanged += AppBarLayout_OffsetChanged;

            var collapsingToolbar = view.FindViewById<CollapsingToolbarLayout>(Resource.Id.collapsingToolbar);
            AppBarLayout.LayoutParams lp = (AppBarLayout.LayoutParams)collapsingToolbar.LayoutParameters;
            lp.Height = (Resources.DisplayMetrics.HeightPixels / 8) * 3;

            collapsingRelativeLayout = view.FindViewById<RelativeLayout>(Resource.Id.collapsingRelativeLayout);

            viewPager = view.FindViewById<ViewPager2>(Resource.Id.view_pager);
            viewPager.Adapter = new ScoreViewPagerAdapter(this, previousShootModel.NumStands, false);

            tabLayout = view.FindViewById<TabLayout>(Resource.Id.tab_layout);
            new TabLayoutMediator(tabLayout, viewPager, new TabConfigStrat()).Attach();

            return view;
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            inflater.Inflate(Resource.Menu.previous_shoot_toolbar_menu, menu);

            if (menu is MenuBuilder)
            {
                MenuBuilder m = (MenuBuilder)menu;
                m.SetOptionalIconsVisible(true);
            }

            base.OnCreateOptionsMenu(menu, inflater);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Resource.Id.share_csv)
            {
                file = previousShootModel.ShootToCSV();
                var uri = FileProvider.GetUriForFile(Context, "com.euandeas.clubclays", file);

                Intent shareIntent = new Intent();
                shareIntent.SetAction(Intent.ActionSend);
                shareIntent.PutExtra(Intent.ExtraStream, uri);
                shareIntent.SetType("text/csv");
                launcher.Launch(Intent.CreateChooser(shareIntent, "Share Shoot as CSV"));
            }
            else if (item.ItemId == Resource.Id.delete_shoot)
            {
                MaterialAlertDialogBuilder builder = new MaterialAlertDialogBuilder(Activity);
                builder.SetTitle("Delete shoot?");
                builder.SetMessage("This will remove the shoot and its data from your device.");
                builder.SetPositiveButton("Yes", (c, ev) =>
                {
                    string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "ClubClaysData.db3");
                    using (var db = new SQLiteConnection(dbPath))
                    {
                        db.Delete<Shoots>(shootId);
                        db.CreateCommand($"DELETE FROM OverallScores WHERE ShootId = {shootId};").ExecuteNonQuery();

                        var standsToDelete = db.Table<Stands>().Where(s => s.ShootId == shootId).ToList();
                        db.CreateCommand($"DELETE FROM Stands WHERE ShootId = {shootId};").ExecuteNonQuery();
                        foreach (var stand in standsToDelete)
                        {
                            db.CreateCommand($"DELETE FROM StandShots WHERE StandId = {stand.Id};").ExecuteNonQuery();

                            var standsScoresToDelete = db.Table<StandScores>().Where(s => s.StandId == stand.Id).ToList();
                            db.CreateCommand($"DELETE FROM StandScores WHERE StandId = {stand.Id};").ExecuteNonQuery();
                            foreach (var standScore in standsScoresToDelete)
                            {
                                db.CreateCommand($"DELETE FROM Shots WHERE StandScoreId = {standScore.Id};").ExecuteNonQuery();
                            }
                        }
                    }

                    Activity.OnBackPressed();
                });

                builder.SetNegativeButton("No", (c, ev) => { });
                builder.Show();
            }

            return base.OnOptionsItemSelected(item);
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

        public void OnActivityResult(Java.Lang.Object p0)
        {
            file.Delete();
        }
    }
}