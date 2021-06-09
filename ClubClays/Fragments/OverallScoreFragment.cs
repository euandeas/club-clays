using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.Fragment.App;
using AndroidX.Lifecycle;
using AndroidX.RecyclerView.Widget;
using AndroidX.ViewPager2.Widget;
using Google.Android.Material.Tabs;
using System.Collections.Generic;
using static Android.Views.ViewGroup;
using Fragment = AndroidX.Fragment.App.Fragment;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

namespace ClubClays.Fragments
{
    public class OverallScoreFragment : Fragment
    {
        RecyclerView overallScoresRecyclerView;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            View view = inflater.Inflate(Resource.Layout.fragment_overall_score, container, false);

            LinearLayoutManager LayoutManager = new LinearLayoutManager(Activity);
            overallScoresRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerView);
            overallScoresRecyclerView.SetLayoutManager(LayoutManager);
            overallScoresRecyclerView.SetAdapter(new OverallScoresRecyclerAdapter(Activity, Context));

            return view;
        }

        public override void OnResume()
        {
            base.OnResume();
            overallScoresRecyclerView.SetAdapter(new OverallScoresRecyclerAdapter(Activity, Context));
        }
    }

    public class OverallScoresRecyclerAdapter : RecyclerView.Adapter
    {
        Shoot scoreManagementModel;
        Context context;

        public class MyView : RecyclerView.ViewHolder
        {
            public View mMainView { get; set; }
            public TextView mShooterName { get; set; }
            public TextView mShooterOverallTotal { get; set; }
            public LinearLayout mShooterStandTotals { get; set; }
            public MyView(View view) : base(view)
            {
                mMainView = view;
            }
        }
        public override int ItemCount => scoreManagementModel.NumberOfShooters;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            MyView myHolder = holder as MyView;

            scoreManagementModel.ShooterOverallData(position, out string name, out int overallTotal, out List<int> totals);
            myHolder.mShooterName.Text = name;
            myHolder.mShooterOverallTotal.Text = $"{overallTotal}";
            for (int x = 0; x <= scoreManagementModel.NumStands-1; x++)
            {
                TextView view = (TextView)myHolder.mShooterStandTotals.FindViewWithTag(x);
                view.Text = $"{totals[x]}";
            }     
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View overallCardView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.overall_score_item, parent, false);
            TextView shooterName = overallCardView.FindViewById<TextView>(Resource.Id.shooterName);
            TextView shooterOverallTotal = overallCardView.FindViewById<TextView>(Resource.Id.overallTotal);
            LinearLayout shooterStandTotals = overallCardView.FindViewById<LinearLayout>(Resource.Id.scoresLayout);

            MyView view = new MyView(overallCardView) { mShooterName = shooterName, mShooterOverallTotal = shooterOverallTotal, mShooterStandTotals = shooterStandTotals };

            for (int x = 0; x <= scoreManagementModel.NumStands-1; x++)
            {
                TextView tview = new TextView(context);
                tview.TextSize = 16;

                LinearLayout.LayoutParams lp = new LinearLayout.LayoutParams(LayoutParams.WrapContent, LayoutParams.WrapContent);
                lp.SetMargins((int)context.Resources.GetDimension(Resource.Dimension.button_left_long), (int)context.Resources.GetDimension(Resource.Dimension.button_top_bottom), 0, (int)context.Resources.GetDimension(Resource.Dimension.button_top_bottom));

                tview.Tag = x;
                shooterStandTotals.AddView(tview, lp);
            }

            return view;
        }

        public OverallScoresRecyclerAdapter(FragmentActivity activity, Context context)
        {
            scoreManagementModel = new ViewModelProvider(activity).Get(Java.Lang.Class.FromType(typeof(Shoot))) as Shoot;
            this.context = context;
        }
    }
}
