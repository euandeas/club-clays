using Android.OS;
using Android.Views;
using AndroidX.AppCompat.App;
using AndroidX.Fragment.App;
using AndroidX.Preference;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

namespace ClubClays.Fragments
{
    public class SettingsFragment : PreferenceFragmentCompat
    {      

        public override void OnCreatePreferences(Bundle savedInstanceState, string rootKey)
        {
            SetPreferencesFromResource(Resource.Xml.preferences, rootKey);
            Preference manageShooters = FindPreference("manage_shooters");
            Preference manageFormats = FindPreference("manage_formats");
            ListPreference theme = (ListPreference)FindPreference("theme_preference");

            manageShooters.PreferenceClick += ManageShooters_PreferenceClick;
            manageFormats.PreferenceClick += ManageFormats_PreferenceClick;
            theme.PreferenceChange += Theme_PreferenceChange;
        }

        private void Theme_PreferenceChange(object sender, Preference.PreferenceChangeEventArgs e)
        {
            switch (e.NewValue.ToString())
            {
                case "light":
                    ((AppCompatActivity)Activity).Delegate.SetLocalNightMode(AppCompatDelegate.ModeNightNo);
                    break;
                case "dark":
                    ((AppCompatActivity)Activity).Delegate.SetLocalNightMode(AppCompatDelegate.ModeNightYes);
                    break;
                case "sysdefault":
                    if (Build.VERSION.SdkInt >= BuildVersionCodes.Q)
                    {
                        ((AppCompatActivity)Activity).Delegate.SetLocalNightMode(AppCompatDelegate.ModeNightFollowSystem);
                    }
                    else
                    {
                        ((AppCompatActivity)Activity).Delegate.SetLocalNightMode(AppCompatDelegate.ModeNightAutoBattery);
                    }
                    break;
            }
        }

        private void ManageFormats_PreferenceClick(object sender, Preference.PreferenceClickEventArgs e)
        {
            FragmentTransaction fragmentTx = Activity.SupportFragmentManager.BeginTransaction();
            ShootFormatsFragment standSetupFragment = new ShootFormatsFragment();
            Bundle args = new Bundle();
            args.PutBoolean("selectable", false);
            standSetupFragment.Arguments = args;
            fragmentTx.Replace(Resource.Id.container, standSetupFragment);
            fragmentTx.AddToBackStack(null);
            fragmentTx.Commit();
        }

        private void ManageShooters_PreferenceClick(object sender, Preference.PreferenceClickEventArgs e)
        {
            FragmentTransaction fragmentTx = Activity.SupportFragmentManager.BeginTransaction();
            fragmentTx.Replace(Resource.Id.container, new ShooterManagementFragment());
            fragmentTx.AddToBackStack(null);
            fragmentTx.Commit();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment

            View root = base.OnCreateView(inflater, container, savedInstanceState);

            ((AppCompatActivity)Activity).SetSupportActionBar(root.FindViewById<Toolbar>(Resource.Id.toolbar));
            ActionBar supportBar = ((AppCompatActivity)Activity).SupportActionBar;
            supportBar.SetDisplayHomeAsUpEnabled(true);
            supportBar.SetDisplayShowHomeEnabled(true);

            return root;
        }

    }
}