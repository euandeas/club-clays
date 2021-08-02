using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.Fragment.App;
using AndroidX.Lifecycle;
using AndroidX.RecyclerView.Widget;
using System;
using System.Collections.Generic;
using Fragment = AndroidX.Fragment.App.Fragment;

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

            LinearLayoutManager layoutManager = new LinearLayoutManager(Activity);
            RecyclerView standScoresRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.recyclerView);
            standScoresRecyclerView.SetLayoutManager(layoutManager);
            standScoresRecyclerView.SetAdapter(new StandScoresRecyclerAdapter(Activity, Context, Arguments.GetInt("standNum"), Arguments.GetBoolean("editable")));

            return view;
        }
    }

    public class StandScoresRecyclerAdapter : RecyclerView.Adapter
    {
        Shoot scoreManagementModel;
        Context context;
        int standNum;
        bool editable;
        public class MyView : RecyclerView.ViewHolder
        {
            public View MainView { get; set; }
            public TextView ShooterName { get; set; }
            public TextView ShooterStandTotal { get; set; }
            public LinearLayout StandHits { get; set; }
            public MyView(View view) : base(view)
            {
                MainView = view;
            }
        }
        public override int ItemCount => scoreManagementModel.NumberOfShooters;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            MyView myHolder = holder as MyView;

            scoreManagementModel.ShooterStandData(position, standNum, out string name, out int standTotal, out List<Tuple<string, int[]>> shots);
            myHolder.ShooterName.Text = name;
            myHolder.ShooterStandTotal.Text = $"{standTotal}";

            for (int x = 0; x <= shots.Count - 1; x++)
            {
                if (shots[x].Item1 == "Pair")
                {
                    UpdateButton(shots[x].Item2[0], (ImageButton)myHolder.StandHits.FindViewWithTag($"{x}.1"));
                    UpdateButton(shots[x].Item2[1], (ImageButton)myHolder.StandHits.FindViewWithTag($"{x}.2"));
                }
                if (shots[x].Item1 == "Single")
                {
                    UpdateButton(shots[x].Item2[0], (ImageButton)myHolder.StandHits.FindViewWithTag($"{x}"));
                }
            }
        }

        public void ButtonClicked(ImageButton view, int position, int shotsNum, int shotNum, TextView total)
        {
            UpdateButton(scoreManagementModel.UpdateScore(position, standNum, shotsNum, shotNum), view);
            total.Text = $"{scoreManagementModel.ShooterStandTotal(position, standNum)}";
        }

        public void UpdateButton(int updateTo, ImageButton view)
        {
            switch (updateTo)
            {
                case 0:
                    view.SetImageResource(0);
                    view.SetBackgroundResource(Resource.Drawable.default_hit_miss_button);
                    break;

                case 1:
                    view.SetImageResource(Resource.Drawable.outline_cross);
                    view.SetBackgroundResource(Resource.Drawable.hit_hit_miss_button);
                    break;

                case 2:
                    view.SetImageResource(Resource.Drawable.outline_circle);
                    view.SetBackgroundResource(Resource.Drawable.miss_hit_miss_button);
                    break;
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View standCardView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.stand_score_item, parent, false);
            TextView shooterName = standCardView.FindViewById<TextView>(Resource.Id.shooterName);
            TextView shooterStandTotal = standCardView.FindViewById<TextView>(Resource.Id.overallTotal);
            LinearLayout standHits = standCardView.FindViewById<LinearLayout>(Resource.Id.hitsLayout);

            MyView view = new MyView(standCardView) { ShooterName = shooterName, ShooterStandTotal = shooterStandTotal, StandHits = standHits };

            List<string> shotsFormat = scoreManagementModel.StandShots(standNum);
            for (int x = 0; x <= shotsFormat.Count - 1; x++)
            {
                if (shotsFormat[x] == "Pair")
                {
                    ImageButton view1 = new ImageButton(context);
                    ImageButton view2 = new ImageButton(context);

                    int size = (int)context.Resources.GetDimension(Resource.Dimension.button_side);
                    LinearLayout.LayoutParams lp1 = new LinearLayout.LayoutParams(size, size);
                    LinearLayout.LayoutParams lp2 = new LinearLayout.LayoutParams(size, size);
                    
                    lp1.SetMargins((int)context.Resources.GetDimension(Resource.Dimension.button_left_long), (int)context.Resources.GetDimension(Resource.Dimension.button_top_bottom), 0, (int)context.Resources.GetDimension(Resource.Dimension.button_top_bottom));
                    lp2.SetMargins((int)context.Resources.GetDimension(Resource.Dimension.button_left_short), (int)context.Resources.GetDimension(Resource.Dimension.button_top_bottom), 0, (int)context.Resources.GetDimension(Resource.Dimension.button_top_bottom));

                    view1.Tag = $"{x}.1";
                    view2.Tag = $"{x}.2";

                    if (editable)
                    {
                        view1.Click += (s, e) =>
                        {
                            ButtonClicked((ImageButton)s, view.AbsoluteAdapterPosition, (int)char.GetNumericValue(((string)((ImageButton)s).Tag)[0]), 0, view.ShooterStandTotal);
                        };

                        view2.Click += (s, e) =>
                        {
                            ButtonClicked((ImageButton)s, view.AbsoluteAdapterPosition, (int)char.GetNumericValue(((string)((ImageButton)s).Tag)[0]), 1, view.ShooterStandTotal);
                        };
                    }

                    standHits.AddView(view1, lp1);
                    standHits.AddView(view2, lp2);
                }
                if (shotsFormat[x] == "Single")
                {
                    ImageButton view1 = new ImageButton(context);

                    int size = (int)context.Resources.GetDimension(Resource.Dimension.button_side);
                    LinearLayout.LayoutParams lp = new LinearLayout.LayoutParams(size, size);
                    lp.SetMargins((int)context.Resources.GetDimension(Resource.Dimension.button_left_long), (int)context.Resources.GetDimension(Resource.Dimension.button_top_bottom), 0, (int)context.Resources.GetDimension(Resource.Dimension.button_top_bottom));

                    view1.Tag = $"{x}";

                    if (editable)
                    {
                        view1.Click += (s, e) =>
                        {
                            ButtonClicked((ImageButton)s, view.AbsoluteAdapterPosition, (int)char.GetNumericValue(((string)((ImageButton)s).Tag)[0]), 0, view.ShooterStandTotal);
                        };
                    }

                    standHits.AddView(view1, lp);
                }
            }

            return view;
        }

        public StandScoresRecyclerAdapter(FragmentActivity activity, Context context, int standNum, bool editable)
        {
            scoreManagementModel = new ViewModelProvider(activity).Get(Java.Lang.Class.FromType(typeof(Shoot))) as Shoot;
            this.context = context;
            this.standNum = standNum;
            this.editable = editable;
        }
    }
}