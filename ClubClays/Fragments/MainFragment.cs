using Android.App;
using Android.OS;
using Android.Views;
using Fragment = AndroidX.Fragment.App.Fragment;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;
using Android.Widget;
using Google.Android.Material.AppBar;
using System;

namespace ClubClays.Fragments
{
    public class MainFragment : Fragment
    {
        Toolbar toolbar;
        AppBarLayout appBarLayout;
        View titleTextView;
        RelativeLayout collapsingRelativeLayout;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // non-graphical initialisations (you can assign variables, get Intent extras, and anything else that doesn't involve the View hierarchy)
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // do any graphical initialisations (e.g. You can assign your View variables)
            View view = inflater.Inflate(Resource.Layout.fragment_main, container, false);

            toolbar = view.FindViewById<Toolbar>(Resource.Id.toolbar);
            titleTextView = ToolbarTitle();
            
            appBarLayout = view.FindViewById<AppBarLayout>(Resource.Id.appBar);
            appBarLayout.OffsetChanged += AppBarLayout_OffsetChanged;

            collapsingRelativeLayout = view.FindViewById<RelativeLayout>(Resource.Id.collapsingRelativeLayout);

            return view;
        }

        private void AppBarLayout_OffsetChanged(object sender, AppBarLayout.OffsetChangedEventArgs e)
        {
            titleTextView.Alpha = Math.Abs(e.VerticalOffset / (appBarLayout.TotalScrollRange - ((float)appBarLayout.TotalScrollRange / 2)));
            collapsingRelativeLayout.Alpha = 1.0f - Math.Abs(e.VerticalOffset / (appBarLayout.TotalScrollRange-((float)appBarLayout.TotalScrollRange / 2)));
        }

        private View ToolbarTitle()
        {
            int childCount = toolbar.ChildCount;
            for (int i = 0; i < childCount; i++)
            {
                View child = toolbar.GetChildAt(i);
                if (child is TextView)
                {
                    return child;
                }
            }

            return new View(Activity);
        }
    }
}