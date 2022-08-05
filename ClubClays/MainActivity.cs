using Android.Content;
using Android.OS;
using AndroidX.AppCompat.App;
using AndroidX.Core.View;
using AndroidX.Preference;
using SplashScreenX = AndroidX.Core.SplashScreen.SplashScreen;

namespace ClubClays
{
    [Activity(Label = "@string/app_name", Theme = "@style/Theme.App.Starting", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            if (savedInstanceState == null)
            {
                ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(Application.Context)!;
                switch (prefs.GetString("theme_preference", "System Default"))
                {
                    case "Light":
                        base.Delegate.SetLocalNightMode(AppCompatDelegate.ModeNightNo);
                        break;
                    case "Dark":
                        base.Delegate.SetLocalNightMode(AppCompatDelegate.ModeNightYes);
                        break;
                    case "System Default":
                        base.Delegate.SetLocalNightMode((Build.VERSION.SdkInt >= BuildVersionCodes.Q) ? AppCompatDelegate.ModeNightFollowSystem : AppCompatDelegate.ModeNightAutoBattery);
                        break;
                }
            }

            SplashScreenX.InstallSplashScreen(this);

            base.OnCreate(savedInstanceState);

            WindowCompat.SetDecorFitsSystemWindows(Window!, false);

            SetContentView(Resource.Layout.activity_main);
        }
    }
}