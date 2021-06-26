using Android.OS;
using Android.Views;
using AndroidX.Fragment.App;
using AndroidX.Lifecycle;
using Google.Android.Material.FloatingActionButton;
using Google.Android.Material.TextField;
using System;
using Fragment = AndroidX.Fragment.App.Fragment;

namespace ClubClays.Fragments
{
    public class ShootEndFragment : Fragment
    {
        private Shoot scoreManagementModel;
        private TextInputEditText usernotes;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            View view = inflater.Inflate(Resource.Layout.fragment_shoot_end, container, false);

            scoreManagementModel = new ViewModelProvider(Activity).Get(Java.Lang.Class.FromType(typeof(Shoot))) as Shoot;

            usernotes = view.FindViewById<TextInputEditText>(Resource.Id.shootNotes);
            view.FindViewById<FloatingActionButton>(Resource.Id.saveButton).Click += Save_Click;

            return view;
        }

        private void Save_Click(object sender, EventArgs e)
        {
            scoreManagementModel.UserNotes = usernotes.Text;
            scoreManagementModel.SaveShootData();
            Activity.SupportFragmentManager.PopBackStack(null, FragmentManager.PopBackStackInclusive);
            FragmentTransaction fragmentTx = Activity.SupportFragmentManager.BeginTransaction();
            fragmentTx.Replace(Resource.Id.container, new MainFragment());
            fragmentTx.Commit();
        }
    }
}