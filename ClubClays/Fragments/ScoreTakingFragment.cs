using Android.OS;
using Android.Views;
using AndroidX.AppCompat.App;
using AndroidX.Lifecycle;
using AndroidX.ViewPager2.Widget;
using Google.Android.Material.Tabs;
using Fragment = AndroidX.Fragment.App.Fragment;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

namespace ClubClays.Fragments
{
    public class ScoreTakingFragment : Fragment
    {
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

            ShootScoreManagement scoreManagementModel = new ViewModelProvider(Activity).Get(Java.Lang.Class.FromType(typeof(ShootScoreManagement))) as ShootScoreManagement;

            ViewPager2 viewPager = view.FindViewById<ViewPager2>(Resource.Id.view_pager);
            viewPager.Adapter = new ScoreViewPagerAdapter(this, scoreManagementModel.NumStands);

            TabLayout tabLayout = view.FindViewById<TabLayout>(Resource.Id.tab_layout);
            new TabLayoutMediator(tabLayout, viewPager, new TabConfigStrat()).Attach();

            return view;
        }
    }
}