using Android.OS;
using Android.Views;
using System;
using AndroidX.RecyclerView.Widget;
using Fragment = AndroidX.Fragment.App.Fragment;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;
using AndroidX.AppCompat.App;
using AndroidX.Lifecycle;
using Google.Android.Material.FloatingActionButton;
using AndroidX.Activity;
using ClubClays.DatabaseModels;
using System.Collections.Generic;
using Android.Widget;
using Google.Android.Material.Dialog;

namespace ClubClays.Fragments
{
    public class StandSetupFragment : Fragment
    {
        public ShooterStandData standsModel;
        public RecyclerView standsRecyclerView;
        private RecyclerView.LayoutManager standsLayoutManager;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            View view = inflater.Inflate(Resource.Layout.fragment_standsetup, container, false);

            Toolbar toolbar = view.FindViewById<Toolbar>(Resource.Id.toolbar);
            ((AppCompatActivity)Activity).SetSupportActionBar(toolbar);
            ActionBar supportBar = ((AppCompatActivity)Activity).SupportActionBar;
            supportBar.SetDisplayHomeAsUpEnabled(true);
            supportBar.SetDisplayShowHomeEnabled(true);

            standsModel = new ViewModelProvider(Activity).Get(Java.Lang.Class.FromType(typeof(ShooterStandData))) as ShooterStandData;
            RecyclerView.Adapter standsAdapter = new StandsRecyclerAdapter(ref standsModel);
            ItemTouchHelper.Callback callback = new ItemMoveCallback((ItemMoveCallback.ItemTouchHelperContract)standsAdapter);
            ItemTouchHelper touchHelper = new ItemTouchHelper(callback);
            standsLayoutManager = new LinearLayoutManager(Activity);
            standsRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.standRecyclerView);
            touchHelper.AttachToRecyclerView(standsRecyclerView);
            standsRecyclerView.SetLayoutManager(standsLayoutManager);
            standsRecyclerView.SetAdapter(standsAdapter);

            FloatingActionButton fab = view.FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.Click += Fab_Click; ;

            Activity.OnBackPressedDispatcher.AddCallback(new BackPress(this));
            return view;
        }

        private void Fab_Click(object sender, EventArgs e)
        {
            MaterialAlertDialogBuilder builder = new MaterialAlertDialogBuilder(Activity);
            builder.SetTitle("Add New Stand");

            View view = LayoutInflater.From(Activity).Inflate(Resource.Layout.dialogfragment_addstand, null);

            EditText standType = view.FindViewById<EditText>(Resource.Id.newStandType);
            EditText numOfPairs = view.FindViewById<EditText>(Resource.Id.newNumOfPairs);

            builder.SetView(view);
            builder.SetPositiveButton("Add", (c, ev) =>
            {
                List<string> shotformat = new List<string>();
                for (int x = 1; x <= int.Parse(numOfPairs.Text); x++)
                {
                    shotformat.Add("Pair");
                }
                standsModel.standFormats.Add(new Stand(standType.Text, shotformat));
                standsRecyclerView.GetAdapter().NotifyDataSetChanged();
            });
            builder.SetNegativeButton("Cancel", (c, ev) => { });

            builder.Show();
        }

        public class BackPress : OnBackPressedCallback
        {
            StandSetupFragment context;
            public BackPress(StandSetupFragment cont) : base(true)
            {
                context = cont;
            }
            public override void HandleOnBackPressed()
            {
                context.SendResult();
                Remove();
                context.Activity.SupportFragmentManager.PopBackStack();
            }
        }

        public void SendResult()
        {
            Bundle result = new Bundle();
            result.PutInt("standsCreated", standsModel.standFormats.Count);
            Activity.SupportFragmentManager.SetFragmentResult("2", result);
        }
    }

    public class StandsRecyclerAdapter : RecyclerView.Adapter, ItemMoveCallback.ItemTouchHelperContract
    {
        private List<Stand> stands;

        // Provide a reference to the views for each data item
        public class MyView : RecyclerView.ViewHolder
        {
            public View mMainView { get; set; }
            public TextView mStandNumber { get; set; }
            public TextView mStandType { get; set; }
            public TextView mNumClays { get; set; }
            public MyView(View view) : base(view)
            {
                mMainView = view;
            }
        }

        // Return the size of data set (invoked by the layout manager)
        public override int ItemCount
        {
            get { return stands.Count; }
        }

        // Replace the contents of a view (invoked by layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            MyView myHolder = holder as MyView;
            myHolder.mStandNumber.Text = $"Stand {position + 1}";
            myHolder.mStandType.Text = stands[position].standType;
            myHolder.mNumClays.Text = stands[position].NumClays.ToString();
        }

        // Create new views (invoked by layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View standCardView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.stand_item, parent, false);
            TextView StandNum = standCardView.FindViewById<TextView>(Resource.Id.standNumber);
            TextView StandType = standCardView.FindViewById<TextView>(Resource.Id.standType);
            TextView NumClays = standCardView.FindViewById<TextView>(Resource.Id.numOfClays);

            MyView view = new MyView(standCardView) { mStandNumber = StandNum, mStandType = StandType, mNumClays = NumClays };

            return view;
        }

        public void onRowMoved(int fromPosition, int toPosition)
        {
            if (fromPosition < toPosition)
            {
                for (int i = fromPosition; i < toPosition; i++)
                {
                    Swap(stands, i, i + 1);
                }
            }
            else
            {
                for (int i = fromPosition; i > toPosition; i--)
                {
                    Swap(stands, i, i - 1);
                }
            }
            NotifyItemMoved(fromPosition, toPosition);
        }

        public void onRowSelected(RecyclerView.ViewHolder myViewHolder)
        {            
        }

        public void onRowClear(RecyclerView.ViewHolder myViewHolder)
        {
            NotifyDataSetChanged();
        }

        public static void Swap<T>(IList<T> list, int indexA, int indexB)
        {
            T tmp = list[indexA];
            list[indexA] = list[indexB];
            list[indexB] = tmp;
        }

        // Provide a suitable constructor (depends on the kind of dataset)
        public StandsRecyclerAdapter(ref ShooterStandData viewModel)
        {
            stands = viewModel.standFormats; 
        }
    }
}