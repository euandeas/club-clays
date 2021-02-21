using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.Fragment.App;
using AndroidX.Lifecycle;
using Fragment = AndroidX.Fragment.App.Fragment;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

namespace ClubClays.Fragments
{
    public class ScoreTakingFragment : Fragment
    {
        private ShootScoreManagement scoreManagementModel;
        private string trackingType;

        int shot1Val = 0;
        int shot2Val = 0;

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
            standNumLabel.Text = $"Stand: {scoreManagementModel.CurrentStand}";
            pairNumLabel.Text = $"Pair: {scoreManagementModel.CurrentPair}";
            currentStandScoreLabel.Text = $"Current Score: {scoreManagementModel.CurrentStandScore}";

            Button shot1Button = view.FindViewById<Button>(Resource.Id.button1);
            Button shot2Button = view.FindViewById<Button>(Resource.Id.button2);

            shot1Button.Click += Shot1Button_Click;
            shot2Button.Click += Shot2Button_Click;

            TextView nextButton = view.FindViewById<TextView>(Resource.Id.nextButton);
            nextButton.Click += NextButton_Click;

            return view;
        }

        private void NextButton_Click(object sender, System.EventArgs e)
        {
            scoreManagementModel.AddScore(shot1Val, shot2Val);

            FragmentTransaction fragmentTx = Activity.SupportFragmentManager.BeginTransaction();

            if (scoreManagementModel.LastPair)
            {
                if (scoreManagementModel.LastShooter)
                {
                    scoreManagementModel.NextShooter();
                }
                else
                {
                    
                }
            }
            else
            {
                fragmentTx.Replace(Resource.Id.container, new ScoreTakingFragment());
                fragmentTx.Commit();
            }
        }

        private void Shot1Button_Click(object sender, System.EventArgs e)
        {
            ShotButton_Click(sender as Button, ref shot1Val);
        }

        private void Shot2Button_Click(object sender, System.EventArgs e)
        {
            ShotButton_Click(sender as Button, ref shot2Val);
        }

        private void ShotButton_Click(Button sender, ref int shotCounter)
        {
            if(shotCounter == 2)
            {
                shotCounter = 0;
            }
            else
            {
                shotCounter++;
            }

            switch (shotCounter)
            {
                case 0:
                    sender.Text = "";
                    sender.SetBackgroundResource(Resource.Drawable.default_hit_miss_button);
                    break;

                case 1:
                    sender.Text = "Hit";
                    sender.SetBackgroundResource(Resource.Drawable.hit_hit_miss_button);
                    break;

                case 2:
                    sender.Text = "Miss";
                    sender.SetBackgroundResource(Resource.Drawable.miss_hit_miss_button);
                    break;
                      
            }
        }
    }
}