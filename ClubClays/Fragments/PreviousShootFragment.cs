using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.Activity;
using AndroidX.AppCompat.App;
using AndroidX.AppCompat.View.Menu;
using AndroidX.Fragment.App;
using AndroidX.Lifecycle;
using AndroidX.ViewPager2.Widget;
using Google.Android.Material.AppBar;
using Google.Android.Material.Tabs;
using System;
using Fragment = AndroidX.Fragment.App.Fragment;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

namespace ClubClays.Fragments
{
    public class PreviousShootFragment : Fragment
    {
        private PreviousShoot previousShootModel;
        Toolbar toolbar;
        View titleTextView;
        AppBarLayout appBarLayout;
        RelativeLayout collapsingRelativeLayout;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            View view = inflater.Inflate(Resource.Layout.fragment_previous_shoot, container, false);

            previousShootModel = new ViewModelProvider(Activity).Get(Java.Lang.Class.FromType(typeof(PreviousShoot))) as PreviousShoot;
            previousShootModel.InitialisePreviousShoot(Arguments.GetInt("ShootID", 1));

            toolbar = view.FindViewById<Toolbar>(Resource.Id.toolbar);
            ((AppCompatActivity)Activity).SetSupportActionBar(toolbar);
            ActionBar supportBar = ((AppCompatActivity)Activity).SupportActionBar;
            supportBar.Title = $"{previousShootModel.EventType} on {previousShootModel.Date.ToShortDateString()}";
            titleTextView = ToolbarTitle();
            supportBar.SetDisplayHomeAsUpEnabled(true);
            supportBar.SetDisplayShowHomeEnabled(true);
            HasOptionsMenu = true;

            appBarLayout = view.FindViewById<AppBarLayout>(Resource.Id.appBar);
            appBarLayout.OffsetChanged += AppBarLayout_OffsetChanged;

            collapsingRelativeLayout = view.FindViewById<RelativeLayout>(Resource.Id.collapsingRelativeLayout);

            ViewPager2 viewPager = view.FindViewById<ViewPager2>(Resource.Id.view_pager);
            viewPager.Adapter = new ScoreViewPagerAdapter(this, previousShootModel.NumStands, "previousShoot");

            TabLayout tabLayout = view.FindViewById<TabLayout>(Resource.Id.tab_layout);
            new TabLayoutMediator(tabLayout, viewPager, new TabConfigStrat()).Attach();

            return view;
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

        public override void OnStop()
        {
            base.OnStop();
            Activity.ViewModelStore.Clear();
        }
    }
}