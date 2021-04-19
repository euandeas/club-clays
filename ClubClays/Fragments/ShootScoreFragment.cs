using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.Fragment.App;
using AndroidX.Lifecycle;
using AndroidX.ViewPager2.Widget;
using ClubClays.DatabaseModels;
using Google.Android.Material.Dialog;
using Google.Android.Material.FloatingActionButton;
using Google.Android.Material.Tabs;
using System;
using Fragment = AndroidX.Fragment.App.Fragment;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

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
            viewPager.Adapter = new ScoreViewPagerAdapter(this, scoreManagementModel.NumStands, "currentShoot");

            TabLayout tabLayout = view.FindViewById<TabLayout>(Resource.Id.tab_layout);
            new TabLayoutMediator(tabLayout, viewPager, new TabConfigStrat()).Attach();

            FloatingActionButton fab = view.FindViewById<FloatingActionButton>(Resource.Id.nextButton);
            fab.Click += NextButton_Click;

            return view;
        }

        private void NextButton_Click(object sender, EventArgs e)
        {
            FragmentTransaction fragmentTx = Activity.SupportFragmentManager.BeginTransaction();
            if (scoreManagementModel.TrackingType == "Unknown")
            {
                MaterialAlertDialogBuilder builder = new MaterialAlertDialogBuilder(Activity);
                builder.SetTitle("Add Next Stand");

                View view = LayoutInflater.From(Activity).Inflate(Resource.Layout.dialogfragment_addstand, null);

                EditText standType = view.FindViewById<EditText>(Resource.Id.newStandType);
                EditText standFormat = view.FindViewById<EditText>(Resource.Id.newStandFormat);
                EditText numOfPairs = view.FindViewById<EditText>(Resource.Id.newNumOfPairs);

                builder.SetView(view);
                builder.SetPositiveButton("Add", (c, ev) =>
                {
                    scoreManagementModel.AddStand( new StandFormats { StandType = standType.Text, StandFormat = standFormat.Text, NumPairs = int.Parse(numOfPairs.Text) });
                    scoreManagementModel.NextStand();
                    fragmentTx.Replace(Resource.Id.container, new ScoreTakingFragment());
                    fragmentTx.Commit();
                });
                builder.SetNegativeButton("Finish Shoot", (c, ev) => 
                {
                    fragmentTx.Replace(Resource.Id.container, new ShootEndFragment());
                    fragmentTx.Commit();
                });

                builder.Show();
            }
            else
            {
                if (scoreManagementModel.LastStand)
                {
                    fragmentTx.Replace(Resource.Id.container, new ShootEndFragment());
                }
                else
                {
                    scoreManagementModel.NextStand();
                    fragmentTx.Replace(Resource.Id.container, new ScoreTakingFragment());
                }
                fragmentTx.Commit();
            
            }
        }

    }
}