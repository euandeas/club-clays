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
        public ScoreViewPagerAdapter(Fragment fa, int numOfStands) : base(fa)
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