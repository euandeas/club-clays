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
using System;
using System.Collections.Generic;
using Fragment = AndroidX.Fragment.App.Fragment;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

namespace ClubClays.Fragments
{
    public class StandScoreFragment : Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            View view = inflater.Inflate(Resource.Layout.fragment_stand_score, container, false);

            LinearLayoutManager LayoutManager = new LinearLayoutManager(Activity);
            RecyclerView StandScoresRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerView);
            StandScoresRecyclerView.SetLayoutManager(LayoutManager);
            StandScoresRecyclerView.SetAdapter(new StandScoresRecyclerAdapter(Activity, Context, Arguments.GetInt("standNum")));

            return view;
        }
    }

    public class StandScoresRecyclerAdapter : RecyclerView.Adapter
    {
        ShootScoreManagement scoreManagementModel;
        Context context;
        int standNum;
        public class MyView : RecyclerView.ViewHolder
        {
            public View mMainView { get; set; }
            public TextView mShooterName { get; set; }
            public TextView mShooterStandTotal { get; set; }
            public LinearLayout mStandHits { get; set; }
            public MyView(View view) : base(view)
            {
                mMainView = view;
            }
        }
        public override int ItemCount => scoreManagementModel.NumberOfShooters;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            MyView myHolder = holder as MyView;

            scoreManagementModel.ShooterStandData(position, standNum, out string name, out int standTotal, out List<Tuple<string, int[]>> shots);
            myHolder.mShooterName.Text = name;
            myHolder.mShooterStandTotal.Text = $"{standTotal}";
            foreach (Tuple<string, int[]> shot in shots)
            {
                if(shot.Item1 == "Pair")
                {
                    Button view1 = new Button(context);
                    Button view2 = new Button(context);
                    view1.Tag = "";
                    view2.Tag = "";

                    myHolder.mStandHits.AddView(view1);
                    myHolder.mStandHits.AddView(view2);
                }
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View standCardView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.stand_score_item, parent, false);
            TextView shooterName = standCardView.FindViewById<TextView>(Resource.Id.shooterName);
            TextView shooterStandTotal = standCardView.FindViewById<TextView>(Resource.Id.overallTotal);
            LinearLayout standHits = standCardView.FindViewById<LinearLayout>(Resource.Id.hitsLayout);

            MyView view = new MyView(standCardView) { mShooterName = shooterName, mShooterStandTotal = shooterStandTotal, mStandHits = standHits };

            return view;
        }

        public StandScoresRecyclerAdapter(FragmentActivity activity, Context context, int standNum)
        {
            scoreManagementModel = new ViewModelProvider(activity).Get(Java.Lang.Class.FromType(typeof(ShootScoreManagement))) as ShootScoreManagement;
            this.context = context;
            this.standNum = standNum;
        }
    }
}