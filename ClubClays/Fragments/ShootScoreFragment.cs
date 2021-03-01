using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using AndroidX.AppCompat.App;
using AndroidX.AppCompat.Widget;
using AndroidX.Fragment.App;
using AndroidX.ViewPager2.Adapter;
using AndroidX.ViewPager2.Widget;
using Google.Android.Material.Tabs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fragment = AndroidX.Fragment.App.Fragment;

namespace ClubClays.Fragments
{
    public class ShootScoreFragment : Fragment
    {
        ViewPager2 viewPager;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            View view = inflater.Inflate(Resource.Layout.fragment_score_taking, container, false);

            Toolbar toolbar = view.FindViewById<Toolbar>(Resource.Id.toolbar);
            ((AppCompatActivity)Activity).SetSupportActionBar(toolbar);
            ActionBar supportBar = ((AppCompatActivity)Activity).SupportActionBar;

            viewPager = view.FindViewById<ViewPager2>(Resource.Id.viewpager);
            viewPager.Adapter = new ViewPagerAdapter();

            TabLayout tabLayout = view.FindViewById<TabLayout>(Resource.Id.tab_layout);
            new TabLayoutMediator(tabLayout, viewPager,
                    (tab, position)->tab.setText("OBJECT " + (position + 1))
            ).attach();

            return view;
        }
    }

    public class ViewPagerAdapter : FragmentStateAdapter
    {
        public override int ItemCount => throw new NotImplementedException();

        public override Fragment CreateFragment(int p0)
        {
            throw new NotImplementedException();
        }
    }
}