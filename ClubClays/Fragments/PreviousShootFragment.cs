using Android.OS;
using Android.Views;
using AndroidX.Activity;
using AndroidX.AppCompat.App;
using AndroidX.Fragment.App;
using AndroidX.Lifecycle;
using AndroidX.ViewPager2.Widget;
using Google.Android.Material.Tabs;
using System;
using Fragment = AndroidX.Fragment.App.Fragment;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

namespace ClubClays.Fragments
{
    public class PreviousShootFragment : Fragment
    {
        private PreviousShoot previousShootModel;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            View view = inflater.Inflate(Resource.Layout.fragment_previous_shoot, container, false);

            previousShootModel = new ViewModelProvider(Activity).Get(Java.Lang.Class.FromType(typeof(PreviousShoot))) as PreviousShoot;
            previousShootModel.InitialisePreviousShoot(Arguments.GetInt("ShootID", 1));

            Toolbar toolbar = view.FindViewById<Toolbar>(Resource.Id.toolbar);
            ((AppCompatActivity)Activity).SetSupportActionBar(toolbar);
            ActionBar supportBar = ((AppCompatActivity)Activity).SupportActionBar;
            supportBar.Title = $"{previousShootModel.EventType} on {previousShootModel.Date.ToShortDateString()}";
            supportBar.SetDisplayHomeAsUpEnabled(true);
            supportBar.SetDisplayShowHomeEnabled(true);

            ViewPager2 viewPager = view.FindViewById<ViewPager2>(Resource.Id.view_pager);
            viewPager.Adapter = new ScoreViewPagerAdapter(this, previousShootModel.NumStands, "previousShoot");

            TabLayout tabLayout = view.FindViewById<TabLayout>(Resource.Id.tab_layout);
            new TabLayoutMediator(tabLayout, viewPager, new TabConfigStrat()).Attach();

            return view;
        }

        public override void OnStop()
        {
            base.OnStop();
            Activity.ViewModelStore.Clear();
        }
    }
}