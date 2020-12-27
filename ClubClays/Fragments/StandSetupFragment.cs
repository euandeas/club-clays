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
using Android.Content;
using ClubClays.DatabaseModels;
using System.Collections.Generic;
using Android.Widget;
using Android.Graphics;

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
            AlertDialog.Builder builder = new AlertDialog.Builder(Activity);
            builder.SetTitle("Add New Stand");

            View view = LayoutInflater.From(Activity).Inflate(Resource.Layout.dialogfragment_addstand, null);

            EditText standType = view.FindViewById<EditText>(Resource.Id.newStandType);
            EditText standFormat = view.FindViewById<EditText>(Resource.Id.newStandFormat);
            EditText numOfPairs = view.FindViewById<EditText>(Resource.Id.newNumOfPairs);

            builder.SetView(view);
            builder.SetPositiveButton("Add", (c, ev) =>
            {
                standsModel.standFormats.Add(new StandFormats { StandType = standType.Text, StandFormat = standFormat.Text, NumPairs = int.Parse(numOfPairs.Text) });
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
            Intent intent = new Intent();
            intent.PutExtra("standsCreated", standsModel.standFormats.Count);
            TargetFragment.OnActivityResult(TargetRequestCode, 2, intent);
        }
    }

    public class StandsRecyclerAdapter : RecyclerView.Adapter, ItemMoveCallback.ItemTouchHelperContract
    {
        private List<StandFormats> stands;

        // Provide a reference to the views for each data item
        public class MyView : RecyclerView.ViewHolder
        {
            public View mMainView { get; set; }
            public TextView mStandType { get; set; }
            public TextView mStandFormat { get; set; }
            public TextView mNumPairs { get; set; }
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
            myHolder.mStandType.Text = stands[position].StandType;
            myHolder.mStandFormat.Text = stands[position].StandFormat;
            myHolder.mNumPairs.Text = stands[position].NumPairs.ToString();
        }

        // Create new views (invoked by layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View standCardView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.stand_item, parent, false);
            TextView StandType = standCardView.FindViewById<TextView>(Resource.Id.standType);
            TextView StandFormat = standCardView.FindViewById<TextView>(Resource.Id.standFormat);
            TextView NumPairs = standCardView.FindViewById<TextView>(Resource.Id.numOfPairs);

            MyView view = new MyView(standCardView) { mStandType = StandType, mStandFormat = StandFormat, mNumPairs = NumPairs };

            standCardView.Click += delegate
            {
            };

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
            myViewHolder.ItemView.SetBackgroundColor(Color.Gray);
        }

        public void onRowClear(RecyclerView.ViewHolder myViewHolder)
        {
            myViewHolder.ItemView.SetBackgroundColor(Color.White);
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