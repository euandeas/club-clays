using Android.App;
using Android.OS;
using Android.Views;
using Fragment = AndroidX.Fragment.App.Fragment;
using AndroidX.AppCompat.Widget;
using AndroidX.AppCompat.App;

namespace ClubClays.Fragments
{
    public class MainFragment : Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // non-graphical initialisations (you can assign variables, get Intent extras, and anything else that doesn't involve the View hierarchy)
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // do any graphical initialisations (e.g. You can assign your View variables)
            View view = inflater.Inflate(Resource.Layout.fragment_main, container, false);

            ((AppCompatActivity)Activity).SetSupportActionBar(view.FindViewById<Toolbar>(Resource.Id.toolbar));
            ((AppCompatActivity)Activity).SupportActionBar.Title = "test";

            return view;
        }
    }
}