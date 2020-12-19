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
            standsModel.standFormats.Add(new StandFormats {StandType = "Test1", StandFormat = "Test1", NumPairs = 5 });
            standsModel.standFormats.Add(new StandFormats { StandType = "Test2", StandFormat = "Test2", NumPairs = 5 });
            standsModel.standFormats.Add(new StandFormats { StandType = "Test3", StandFormat = "Test3", NumPairs = 5 });
            standsModel.standFormats.Add(new StandFormats { StandType = "Test4", StandFormat = "Test4", NumPairs = 5 });

            RecyclerView.Adapter standsAdapter = new StandsRecyclerAdapter(this, standsModel.standFormats);

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
            throw new NotImplementedException();
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
            intent.PutExtra("numSelected", standsModel.standFormats.Count);
            TargetFragment.OnActivityResult(TargetRequestCode, 2, intent);
        }
    }

    public class StandsRecyclerAdapter : RecyclerView.Adapter, ItemMoveCallback.ItemTouchHelperContract
    {
        private List<StandFormats> stands;
        private StandSetupFragment parentFragment;

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
        public StandsRecyclerAdapter(StandSetupFragment initialisingFragment, List<StandFormats> standsList)
        {
            stands = standsList;
            parentFragment = initialisingFragment;
        }
    }

    public class ItemMoveCallback : ItemTouchHelper.Callback
    {
        private ItemTouchHelperContract mAdapter;
        
        public ItemMoveCallback(ItemTouchHelperContract adapter)
        {
            mAdapter = adapter;
        }

        public interface ItemTouchHelperContract
        {
            void onRowMoved(int fromPosition, int toPosition);
            void onRowSelected(RecyclerView.ViewHolder myViewHolder);
            void onRowClear(RecyclerView.ViewHolder myViewHolder);
        }

        public override bool IsLongPressDragEnabled => true;
        public override bool IsItemViewSwipeEnabled => true;

        public override int GetMovementFlags(RecyclerView p0, RecyclerView.ViewHolder p1)
        {
            int dragFlags = ItemTouchHelper.Up | ItemTouchHelper.Down;
            return MakeMovementFlags(dragFlags, 0);
        }

        public override bool OnMove(RecyclerView p0, RecyclerView.ViewHolder p1, RecyclerView.ViewHolder p2)
        {
            mAdapter.onRowMoved(p1.AdapterPosition, p2.AdapterPosition);
            return true;
        }

        public override void OnSwiped(RecyclerView.ViewHolder p0, int p1){}

        public override void OnSelectedChanged(RecyclerView.ViewHolder viewHolder, int actionState)
        {
            if (actionState != ItemTouchHelper.ActionStateIdle)
            {
                if (viewHolder.GetType() == typeof(RecyclerView.ViewHolder))
                {                 
                    mAdapter.onRowSelected(viewHolder);
                }
            }
            base.OnSelectedChanged(viewHolder, actionState);
        }

        public override void ClearView(RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder)
        {
            base.ClearView(recyclerView, viewHolder);

            if (viewHolder.GetType() == typeof(RecyclerView.ViewHolder))
            {
                RecyclerView.ViewHolder myViewHolder = viewHolder;
                mAdapter.onRowClear(myViewHolder);
            }
        }
    }
}