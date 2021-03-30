using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.Lifecycle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fragment = AndroidX.Fragment.App.Fragment;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

namespace ClubClays.Fragments
{
    public class ShootEndFragment : Fragment
    {
        private ShootScoreManagement scoreManagementModel;
        private EditText usernotes;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            View view = inflater.Inflate(Resource.Layout.fragment_shoot_end, container, false);

            scoreManagementModel = new ViewModelProvider(Activity).Get(Java.Lang.Class.FromType(typeof(ShootScoreManagement))) as ShootScoreManagement;

            usernotes = view.FindViewById<EditText>(Resource.Id.shootNotes);
            view.FindViewById<TextView>(Resource.Id.saveButton).Click += Save_Click;

            return view;
        }

        private void Save_Click(object sender, EventArgs e)
        {
            scoreManagementModel.UserNotes = usernotes.Text;
            scoreManagementModel.SaveShootData();
            scoreManagementModel.Dispose();
            Context.StartActivity(new Intent(Context, typeof(MainActivity)));
            Activity.Finish();
        }
    }
}