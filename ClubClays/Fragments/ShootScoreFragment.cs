using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using AndroidX.AppCompat.App;
using AndroidX.AppCompat.Widget;
using AndroidX.Fragment.App;
using AndroidX.Lifecycle;
using AndroidX.ViewPager2.Adapter;
using AndroidX.ViewPager2.Widget;
using Google.Android.Material.Tabs;
using Java.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fragment = AndroidX.Fragment.App.Fragment;

namespace ClubClays.Fragments
{
    public class ShootScoreFragment : Fragment
    {
        private ShootScoreManagement scoreManagementModel;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            View view = inflater.Inflate(Resource.Layout.fragment_shoot_score, container, false);

            Toolbar toolbar = view.FindViewById<Toolbar>(Resource.Id.toolbar);
            ((AppCompatActivity)Activity).SetSupportActionBar(toolbar);
            ActionBar supportBar = ((AppCompatActivity)Activity).SupportActionBar;

            scoreManagementModel = new ViewModelProvider(Activity).Get(Java.Lang.Class.FromType(typeof(ShootScoreManagement))) as ShootScoreManagement;

            ViewPager2 viewPager = view.FindViewById<ViewPager2>(Resource.Id.view_pager);
            viewPager.Adapter = new ViewPagerAdapter(this, scoreManagementModel.CurrentNumStands);

            TabLayout tabLayout = view.FindViewById<TabLayout>(Resource.Id.tab_layout);
            new TabLayoutMediator(tabLayout, viewPager, new tabConfigStrat()).Attach();

            return view;
        }

        public class tabConfigStrat : Java.Lang.Object, TabLayoutMediator.ITabConfigurationStrategy
        {
            public void OnConfigureTab(TabLayout.Tab p0, int p1)
            {
                if (p1 == 0)
                {
                    p0.SetText("Overall");
                }
                else
                {
                    p0.SetText($"Stand {p1}");
                }
            }
        }
    }

    public class ViewPagerAdapter : FragmentStateAdapter
    {
        int numOfStands;
        public ViewPagerAdapter(Fragment fa, int numOfStands) : base(fa)
        {
            this.numOfStands = numOfStands;
        }
        public override int ItemCount => numOfStands + 1;

        public override Fragment CreateFragment(int p0)
        {
            Fragment fragment = new ScoreTableFragment();
            Bundle args = new Bundle();
            args.PutInt("standNum", p0);
            fragment.Arguments = args;
            return fragment;
        }
    }
}