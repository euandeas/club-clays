using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.Lifecycle;
using Fragment = AndroidX.Fragment.App.Fragment;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

namespace ClubClays.Fragments
{
    public class ScoreTakingFragment : Fragment
    {
        private ShootScoreManagement scoreManagementModel;
        private string trackingType;
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

            
            scoreManagementModel = new ViewModelProvider(Activity).Get(Java.Lang.Class.FromType(typeof(ShootScoreManagement))) as ShootScoreManagement;
            trackingType = scoreManagementModel.TrackingType;

            TextView shooterNameLabel = view.FindViewById<TextView>(Resource.Id.shooterNameText);
            TextView standNumLabel = view.FindViewById<TextView>(Resource.Id.standNumberText);
            TextView pairNumLabel = view.FindViewById<TextView>(Resource.Id.pairNumberText);
            TextView currentStandScoreLabel = view.FindViewById<TextView>(Resource.Id.currentScoreText);

            shooterNameLabel.Text = scoreManagementModel.CurrentShooterName;
            standNumLabel.Text = $"{scoreManagementModel.CurrentStand}";
            pairNumLabel.Text = $"{scoreManagementModel.CurrentPair}";
            currentStandScoreLabel.Text = scoreManagementModel.CurrentStandScore;



            return view;
        }
    }
}