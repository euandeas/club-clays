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

            manageShooters.PreferenceClick += ManageShooters_PreferenceClick;
            manageFormats.PreferenceClick += ManageFormats_PreferenceClick;
        }

        private void ManageFormats_PreferenceClick(object sender, Preference.PreferenceClickEventArgs e)
        {
            FragmentTransaction fragmentTx = Activity.SupportFragmentManager.BeginTransaction();
            fragmentTx.Replace(Resource.Id.container, new StandFormatsManagmentFragment());
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
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            View root = base.OnCreateView(inflater, container, savedInstanceState);

            ((AppCompatActivity)Activity).SetSupportActionBar(root.FindViewById<Toolbar>(Resource.Id.toolbar));
            ActionBar supportBar = ((AppCompatActivity)Activity).SupportActionBar;
            supportBar.SetDisplayHomeAsUpEnabled(true);
            supportBar.SetDisplayShowHomeEnabled(true);

            return root;
        }
    }
}