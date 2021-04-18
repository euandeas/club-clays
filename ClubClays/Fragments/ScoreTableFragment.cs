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
        private Shoot scoreManagementModel;
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
            string viewAccessingScore = Arguments.GetString("ViewAccessingScore", "previousShoot");

            if (viewAccessingScore == "currentShoot")
            {
                scoreManagementModel = new ViewModelProvider(Activity).Get(Java.Lang.Class.FromType(typeof(ShootScoreManagement))) as ShootScoreManagement;
            }
            else if (viewAccessingScore == "previousShoot")
            {
                scoreManagementModel = new ViewModelProvider(Activity).Get(Java.Lang.Class.FromType(typeof(PreviousShoot))) as PreviousShoot;
            }

            TableLayout tableLayout = view.FindViewById<TableLayout>(Resource.Id.scoreTable);

            if (whichStand == 0)
            {
                TopRowOfTable(tableLayout, scoreManagementModel.NumStands, scoreManagementModel.NumClays);
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
                TopRowOfTable(tableLayout, scoreManagementModel.PairsInStand(whichStand), scoreManagementModel.PairsInStand(whichStand)*2);
                for (int x = 1; x <= scoreManagementModel.NumberOfShooters; x++)
                {
            
                    scoreManagementModel.ShooterStandData(x, whichStand, out string name, out string total, out SortedList<int, int[]> hits);
                    
                    TableRow tableRow = new TableRow(Context); 
                    AddViewToRow(tableRow, name);

                    for (int y = 1; y <= hits.Count; y++)
                    {
                        AddDoubleViewToRow(tableRow, hits[y]);
                    }

                    AddViewToRow(tableRow, total);
                    tableLayout.AddView(tableRow);
                }
            }

            return view;
        }

        public void TopRowOfTable(TableLayout tableLayout, int numSections, int totalNumClays)
        {
            TableRow tableRow = new TableRow(Context);
            tableRow.SetBackgroundColor(Color.Rgb(96, 125, 139));
            AddViewToRow(tableRow, "Name");
            for (int x = 1; x <= numSections; x++)
            {
                AddViewToRow(tableRow, $"{x}");
            }
            AddViewToRow(tableRow, $"Total /{totalNumClays}");
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

        public void AddDoubleViewToRow(TableRow tableRow, int[] text)
        {
            LinearLayout linearLayout = new LinearLayout(Context);
            linearLayout.Orientation = Orientation.Horizontal;
            
            TextView tv0 = new TextView(Context);
            tv0.Text = TranslateHitMiss(text[0]);
            tv0.SetPadding((int)(5 * Resources.DisplayMetrics.Density + 0.5f), 0, 0, 0);

            TextView tv1 = new TextView(Context);
            tv1.Text = TranslateHitMiss(text[1]);
            tv1.SetPadding((int)(5 * Resources.DisplayMetrics.Density + 0.5f), 0, 0, 0);

            linearLayout.AddView(tv0);
            linearLayout.AddView(tv1);

            tableRow.AddView(linearLayout);
        }

        public string TranslateHitMiss(int value)
        {
            switch (value)
            {
                default:
                    return " ";
                
                case 1:
                    return "X";
                
                case 2:
                    return "O";

            }              
        }
    }
}