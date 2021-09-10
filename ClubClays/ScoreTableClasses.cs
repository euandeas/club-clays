using Android.OS;
using AndroidX.Fragment.App;
using AndroidX.Lifecycle;
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
    }

    public class TabConfigStrat : Java.Lang.Object, TabLayoutMediator.ITabConfigurationStrategy
    {
        Shoot scoreManagementModel;
        public TabConfigStrat(FragmentActivity activity)
        {
            scoreManagementModel = new ViewModelProvider(activity).Get(Java.Lang.Class.FromType(typeof(Shoot))) as Shoot;
        }
        
        public void OnConfigureTab(TabLayout.Tab p0, int p1)
        {
            p0.SetText(p1 == 0 ? "Overall" : $"Stand {scoreManagementModel.StandNum(p1)}");
        }
    }
}