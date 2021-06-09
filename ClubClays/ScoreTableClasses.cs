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
        bool editable;
        public ScoreViewPagerAdapter(Fragment fa, int numOfStands, bool editable) : base(fa)
        {
            this.numOfStands = numOfStands;
            this.editable = editable;
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
                fragment.Arguments = args;
            }
            else
            {
                fragment = new StandScoreFragment();
                args = new Bundle();
                args.PutInt("standNum", p0);
                args.PutBoolean("editable", editable);
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