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
    public class OverallScoreFragment : Fragment
    {
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
            RecyclerView overallScoresRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerView);
            overallScoresRecyclerView.SetLayoutManager(LayoutManager);
            overallScoresRecyclerView.SetAdapter(new OverallScoresRecyclerAdapter());

            return view;
        }
    }

    public class OverallScoresRecyclerAdapter : RecyclerView.Adapter
    {

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
            View overallCardView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.overall_score_item, parent, false);
            TextView shooterName = overallCardView.FindViewById<TextView>(Resource.Id.shooterName);
            TextView shooterOverallTotal = overallCardView.FindViewById<TextView>(Resource.Id.overallTotal);
            LinearLayout shooterStandTotals = overallCardView.FindViewById<LinearLayout>(Resource.Id.scoresLayout);

            MyView view = new MyView(overallCardView) { mShooterName = shooterName, mShooterOverallTotal = shooterOverallTotal, mShooterStandTotals = shooterStandTotals };

            overallCardView.Click += delegate
            {
            };

            return view;
        }

        public OverallScoresRecyclerAdapter()
        {          
        }
    }
}
