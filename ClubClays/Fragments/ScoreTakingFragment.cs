using Android.OS;
using Android.Views;
using AndroidX.Activity;
using AndroidX.AppCompat.App;
using AndroidX.AppCompat.View.Menu;
using AndroidX.Fragment.App;
using AndroidX.Lifecycle;
using AndroidX.ViewPager2.Widget;
using Google.Android.Material.Dialog;
using Google.Android.Material.FloatingActionButton;
using Google.Android.Material.Tabs;
using System;
using Fragment = AndroidX.Fragment.App.Fragment;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

namespace ClubClays.Fragments
{
    public class ScoreTakingFragment : Fragment, IFragmentResultListener
    {
        Shoot scoreManagementModel;
        FloatingActionButton fab;
        ScoreViewPagerAdapter scoreViewPagerAdapter;
        ViewPager2 viewPager;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here

        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            View view =  inflater.Inflate(Resource.Layout.fragment_score_taking, container, false);

            Toolbar toolbar = view.FindViewById<Toolbar>(Resource.Id.toolbar);
            ((AppCompatActivity)Activity).SetSupportActionBar(toolbar);
            HasOptionsMenu = true;

            scoreManagementModel = new ViewModelProvider(Activity).Get(Java.Lang.Class.FromType(typeof(Shoot))) as Shoot;

            viewPager = view.FindViewById<ViewPager2>(Resource.Id.view_pager);
            scoreViewPagerAdapter = new ScoreViewPagerAdapter(this, scoreManagementModel.NumStands, true);
            viewPager.Adapter = scoreViewPagerAdapter;

            TabLayout tabLayout = view.FindViewById<TabLayout>(Resource.Id.tab_layout);
            new TabLayoutMediator(tabLayout, viewPager, new TabConfigStrat()).Attach();
            tabLayout.TabSelected += TabLayout_TabSelected;

            fab = view.FindViewById<FloatingActionButton>(Resource.Id.addButton);
            FABVisibility(tabLayout.SelectedTabPosition);
            fab.Click += Fab_Click;

            Activity.OnBackPressedDispatcher.AddCallback(this, new BackPress(this));

            return view;
        }

        private void Fab_Click(object sender, EventArgs e)
        {
            FragmentTransaction fragmentTx = Activity.SupportFragmentManager.BeginTransaction();
            fragmentTx.Replace(Resource.Id.container, new AddStandFormatFragment());
            fragmentTx.AddToBackStack(null);
            fragmentTx.Commit();
            Activity.SupportFragmentManager.SetFragmentResultListener("1", this, this);
        }

        private void TabLayout_TabSelected(object sender, TabLayout.TabSelectedEventArgs e)
        {
            FABVisibility(e.Tab.Position);
        }

        private void FABVisibility(int pos)
        {
            fab.Visibility = (pos == scoreManagementModel.NumStands) ? ViewStates.Visible : ViewStates.Gone;
        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            inflater.Inflate(Resource.Menu.score_taking_toolbar_menu, menu);

            if (menu is MenuBuilder)
            {
                MenuBuilder m = (MenuBuilder)menu;
                m.SetOptionalIconsVisible(true);
            }

            base.OnCreateOptionsMenu(menu, inflater);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Resource.Id.finish_shoot)
            {
                MaterialAlertDialogBuilder builder = new MaterialAlertDialogBuilder(Activity);
                builder.SetTitle("Finish shoot?");
                builder.SetMessage("Are you sure that you would like to end this shoot?");
                builder.SetPositiveButton("Yes", (c, ev) =>
                {
                    FragmentTransaction fragmentTx = Activity.SupportFragmentManager.BeginTransaction();
                    fragmentTx.Replace(Resource.Id.container, new ShootEndFragment());
                    fragmentTx.AddToBackStack(null);
                    fragmentTx.Commit();
                });

                builder.SetNegativeButton("No", (c, ev) => { });
                builder.Show();
            }

            return base.OnOptionsItemSelected(item);
        }

        public void OnFragmentResult(string p0, Bundle p1)
        {
            if (p0 == "1")
            {
                if(p1.GetBoolean("StandAdded", false))
                {
                    viewPager.SetCurrentItem(scoreViewPagerAdapter.ItemCount, false);
                }
            }
        }

        public class BackPress : OnBackPressedCallback
        {
            ScoreTakingFragment context;

            public BackPress(ScoreTakingFragment cont) : base(true)
            {
                context = cont;
            }
            public override void HandleOnBackPressed()
            {
                MaterialAlertDialogBuilder builder = new MaterialAlertDialogBuilder(context.Activity);
                builder.SetTitle("Exit Shoot?");
                builder.SetMessage("Are you sure that you would like to exit this shoot without saving?");
                builder.SetPositiveButton("Yes", (c, ev) =>
                {
                    context.Activity.SupportFragmentManager.PopBackStack(null, FragmentManager.PopBackStackInclusive);
                    FragmentTransaction fragmentTx = context.Activity.SupportFragmentManager.BeginTransaction();
                    fragmentTx.Replace(Resource.Id.container, new MainFragment());
                    fragmentTx.Commit();
                });

                builder.SetNegativeButton("No", (c, ev) => { });
                builder.Show();
            }
        }
    }
}