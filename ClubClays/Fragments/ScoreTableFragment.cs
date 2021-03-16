using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fragment = AndroidX.Fragment.App.Fragment;

namespace ClubClays.Fragments
{
    public class ScoreTableFragment : Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            View view = inflater.Inflate(Resource.Layout.fragment_score_table, container, false);

            TableLayout tableLayout = view.FindViewById<TableLayout>(Resource.Id.scoreTable);

            TableRow tableRow0 = new TableRow(Context);
            TextView tv0 = new TextView(Context);
            tv0.Text = "Test0";
            tableRow0.AddView(tv0);
            TextView tv1 = new TextView(Context);
            tv1.Text = "Test1";
            tableRow0.AddView(tv1);
            tableLayout.AddView(tableRow0);

            return view;
        }
    }
}