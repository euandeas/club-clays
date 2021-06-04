using Android.OS;
using AndroidX.Fragment.App;
using AndroidX.ViewPager2.Adapter;
using ClubClays.Fragments;
using Google.Android.Material.Tabs;

namespace ClubClays
{
    public class ScoreViewPagerAdapter : FragmentStateAdapter
    {
        int numOfStands;
        string accessType;
        public ScoreViewPagerAdapter(Fragment fa, int numOfStands, string accessType) : base(fa)
        {
            this.numOfStands = numOfStands;
            this.accessType = accessType;
        }
        public override int ItemCount => numOfStands + 1;

        public override Fragment CreateFragment(int p0)
        {
            Fragment fragment;
            Bundle args;

            if (p0 == 0)
            {
                fragment = new OverallScoreFragment();
                args = new Bundle();
                args.PutString("accessType", accessType);
                fragment.Arguments = args;
            }
            else
            {
                fragment = new StandScoreFragment();
                args = new Bundle();
                args.PutInt("standNum", p0);
                args.PutString("accessType", accessType);
                fragment.Arguments = args;
            }
            return fragment;
        }

        public int AddStand()
        {
            numOfStands += 1;
            return numOfStands;
        }
    }

    public class TabConfigStrat : Java.Lang.Object, TabLayoutMediator.ITabConfigurationStrategy
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