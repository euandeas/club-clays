using Android.App;
using Android.OS;
using Android.Runtime;
using AndroidX.AppCompat.App;
using FragmentTransaction = AndroidX.Fragment.App.FragmentTransaction;
using ClubClays.Fragments;

namespace ClubClays
{
    [Activity(Label = "@string/app_name", Theme = "@style/SplashTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private MainFragment mainFragment = new MainFragment();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            SetTheme(Resource.Style.AppTheme);
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            if (savedInstanceState == null)
            {
                FragmentTransaction fragmentTx = SupportFragmentManager.BeginTransaction();
                fragmentTx.Replace(Resource.Id.container, mainFragment);
                fragmentTx.Commit();
            }
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}