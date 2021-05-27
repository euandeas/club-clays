using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.Lifecycle;
using AndroidX.RecyclerView.Widget;
using AndroidX.ViewPager2.Widget;
using Google.Android.Material.Tabs;
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
            StandScoresRecyclerView.SetAdapter(new StandScoresRecyclerAdapter(Context));

            return view;
        }
    }

    public class StandScoresRecyclerAdapter : RecyclerView.Adapter
    {
        Context context;
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
        public override int ItemCount => 5;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            MyView myHolder = holder as MyView;
            //myHolder.mShooterName.Text = $"{allShoots[position].EventType} on {allShoots[position].Date.ToShortDateString()}";
            //myHolder.mShooterOverallTotal.Text = $"{allShoots[position].NumStands} Stand(s), {allShoots[position].ClayAmount} Clays";
            //myHolder.mShooterStandTotals.Text = $"{allShoots[position].Location}";
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View standCardView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.stand_score_item, parent, false);
            TextView shooterName = standCardView.FindViewById<TextView>(Resource.Id.shooterName);
            TextView shooterStandTotal = standCardView.FindViewById<TextView>(Resource.Id.overallTotal);
            LinearLayout standHits = standCardView.FindViewById<LinearLayout>(Resource.Id.hitsLayout);

            MyView view = new MyView(standCardView) { mShooterName = shooterName, mShooterStandTotal = shooterStandTotal, mStandHits = standHits };

            standCardView.Click += delegate
            {
            };

            return view;
        }

        public StandScoresRecyclerAdapter(Context context)
        {
            this.context = context;
        }


    }
}