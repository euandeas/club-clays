using Android.OS;
using Android.Views;
using AndroidX.AppCompat.App;
using AndroidX.AppCompat.View.Menu;
using AndroidX.Fragment.App;
using AndroidX.Lifecycle;
using AndroidX.ViewPager2.Widget;
using Google.Android.Material.FloatingActionButton;
using Google.Android.Material.Tabs;
using System;
using Fragment = AndroidX.Fragment.App.Fragment;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

namespace ClubClays.Fragments
{
    public class ScoreTakingFragment : Fragment
    {
        ShootScoreManagement scoreManagementModel;
        FloatingActionButton fab;
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
            ActionBar supportBar = ((AppCompatActivity)Activity).SupportActionBar;
            HasOptionsMenu = true;

            scoreManagementModel = new ViewModelProvider(Activity).Get(Java.Lang.Class.FromType(typeof(ShootScoreManagement))) as ShootScoreManagement;

            ViewPager2 viewPager = view.FindViewById<ViewPager2>(Resource.Id.view_pager);
            viewPager.Adapter = new ScoreViewPagerAdapter(this, scoreManagementModel.NumStands);

            TabLayout tabLayout = view.FindViewById<TabLayout>(Resource.Id.tab_layout);
            new TabLayoutMediator(tabLayout, viewPager, new TabConfigStrat()).Attach();
            tabLayout.TabSelected += TabLayout_TabSelected;

            fab = view.FindViewById<FloatingActionButton>(Resource.Id.addButton);
            fab.Visibility = ViewStates.Gone;
            fab.Click += Fab_Click;

            return view;
        }

        private void Fab_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void TabLayout_TabSelected(object sender, TabLayout.TabSelectedEventArgs e)
        {
            if (e.Tab.Position == scoreManagementModel.NumStands)
            {
                fab.Visibility = ViewStates.Visible;
            }
            else
            {
                fab.Visibility = ViewStates.Gone;
            }
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
                FragmentTransaction fragmentTx = Activity.SupportFragmentManager.BeginTransaction();
                fragmentTx.Replace(Resource.Id.container, new ShootEndFragment());
                fragmentTx.AddToBackStack(null);
                fragmentTx.Commit();
            }

            return base.OnOptionsItemSelected(item);
        }
    }
}