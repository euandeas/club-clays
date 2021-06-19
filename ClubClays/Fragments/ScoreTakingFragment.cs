using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.AppCompat.View.Menu;
using AndroidX.Fragment.App;
using AndroidX.Lifecycle;
using AndroidX.ViewPager2.Widget;
using Google.Android.Material.Dialog;
using Google.Android.Material.FloatingActionButton;
using Google.Android.Material.Tabs;
using System;
using System.Collections.Generic;
using Fragment = AndroidX.Fragment.App.Fragment;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

namespace ClubClays.Fragments
{
    public class ScoreTakingFragment : Fragment
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
            ActionBar supportBar = ((AppCompatActivity)Activity).SupportActionBar;
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

            return view;
        }

        private void Fab_Click(object sender, EventArgs e)
        {
            FragmentTransaction fragmentTx = Activity.SupportFragmentManager.BeginTransaction();
            fragmentTx.Replace(Resource.Id.container, new AddStandFormatFragment());
            fragmentTx.AddToBackStack(null);
            fragmentTx.Commit();
        }

        private void TabLayout_TabSelected(object sender, TabLayout.TabSelectedEventArgs e)
        {
            FABVisibility(e.Tab.Position);
        }

        private void FABVisibility(int pos)
        {
            if (pos == scoreManagementModel.NumStands)
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
    }
}