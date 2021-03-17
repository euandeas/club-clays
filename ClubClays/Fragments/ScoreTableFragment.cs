using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.Lifecycle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fragment = AndroidX.Fragment.App.Fragment;

namespace ClubClays.Fragments
{
    public class ScoreTableFragment : Fragment
    {
        private ShootScoreManagement scoreManagementModel;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            View view = inflater.Inflate(Resource.Layout.fragment_score_table, container, false);

            int whichStand = Arguments.GetInt("standNum", 0);

            scoreManagementModel = new ViewModelProvider(Activity).Get(Java.Lang.Class.FromType(typeof(ShootScoreManagement))) as ShootScoreManagement;
            
            TableLayout tableLayout = view.FindViewById<TableLayout>(Resource.Id.scoreTable);

            if (whichStand == 0)
            {
                TopRowOfTable(tableLayout, scoreManagementModel.CurrentNumStands);
                for (int x = 1; x <= scoreManagementModel.NumberOfShooters; x++)
                {
                    List<string> shooterData = scoreManagementModel.ShooterOverallData(x);
                    TableRow tableRow = new TableRow(Context);
                    foreach (string text in shooterData)
                    {
                        AddViewToRow(tableRow, text);
                    }
                    tableLayout.AddView(tableRow);
                }
            }
            else
            {
                TopRowOfTable(tableLayout, scoreManagementModel.PairsInStand(whichStand));
                for (int x = 1; x <= scoreManagementModel.NumberOfShooters; x++)
                {
                    List<string> shooterData = scoreManagementModel.ShooterOverallData(x);
                    TableRow tableRow = new TableRow(Context);
                    foreach (string text in shooterData)
                    {
                        AddViewToRow(tableRow, text);
                    }
                    tableLayout.AddView(tableRow);
                }
            }

            return view;
        }

        public void TopRowOfTable(TableLayout tableLayout, int numSections)
        {
            TableRow tableRow = new TableRow(Context);
            tableRow.SetBackgroundColor(Color.Rgb(96, 125, 139));
            AddViewToRow(tableRow, "Name");
            for (int x = 1; x <= numSections; x++)
            {
                AddViewToRow(tableRow, $"{x}");
            }
            AddViewToRow(tableRow, "Total");
            tableLayout.AddView(tableRow);
        }

        public void AddViewToRow(TableRow tableRow, string text)
        {
            TextView tv = new TextView(Context);
            tv.Text = text;
            tv.SetPadding((int)(5 * Resources.DisplayMetrics.Density + 0.5f), 0, 0, 0);
            tableRow.AddView(tv);
            tv.Dispose();
        }
    }
}