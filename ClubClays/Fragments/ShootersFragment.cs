using Android.OS;
using Android.Views;
using AndroidX.RecyclerView.Widget;
using ClubClays.DatabaseModels;
using System.Collections.Generic;
using Fragment = AndroidX.Fragment.App.Fragment;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;
using Android.Widget;
using AndroidX.Lifecycle;
using AndroidX.Activity;
using Google.Android.Material.FloatingActionButton;
using AndroidX.AppCompat.App;
using SQLite;
using System.IO;
using Color = Android.Graphics.Color;
using Google.Android.Material.Dialog;

namespace ClubClays.Fragments
{
    public class ShootersFragment : Fragment
    {
        private ShooterStandData selectedShootersModel;

        public RecyclerView allRecyclerView;
        public RecyclerView selectedRecyclerView;
        private RecyclerView.LayoutManager allLayoutManager;
        private RecyclerView.LayoutManager selectedLayoutManager;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            View view = inflater.Inflate(Resource.Layout.fragment_shooters, container, false);

            Toolbar toolbar = view.FindViewById<Toolbar>(Resource.Id.toolbar);
            ((AppCompatActivity)Activity).SetSupportActionBar(toolbar);
            ActionBar supportBar = ((AppCompatActivity)Activity).SupportActionBar;
            supportBar.SetDisplayHomeAsUpEnabled(true);
            supportBar.SetDisplayShowHomeEnabled(true);

            selectedShootersModel = new ViewModelProvider(Activity).Get(Java.Lang.Class.FromType(typeof(ShooterStandData))) as ShooterStandData;

            allLayoutManager = new LinearLayoutManager(Activity);
            selectedLayoutManager = new LinearLayoutManager(Activity);

            allRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.allRecyclerView);
            allRecyclerView.SetLayoutManager(allLayoutManager);

            RecyclerView.Adapter standsAdapter = new ShootersRecyclerAdapter(ref selectedShootersModel, ref allRecyclerView, "selected");
            ItemTouchHelper.Callback callback = new ItemMoveCallback((ItemMoveCallback.ItemTouchHelperContract)standsAdapter);
            ItemTouchHelper touchHelper = new ItemTouchHelper(callback);

            selectedRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.selectedRecyclerView);
            selectedRecyclerView.SetLayoutManager(selectedLayoutManager);
            touchHelper.AttachToRecyclerView(selectedRecyclerView);

            allRecyclerView.SetAdapter(new ShootersRecyclerAdapter(ref selectedShootersModel, ref selectedRecyclerView, "all"));
            selectedRecyclerView.SetAdapter(standsAdapter);

            FloatingActionButton fab = view.FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.Click += Fab_Click;

            Activity.OnBackPressedDispatcher.AddCallback(new BackPress(this));

            return view;
        }

        private void Fab_Click(object sender, System.EventArgs e)
        {
            MaterialAlertDialogBuilder builder = new MaterialAlertDialogBuilder(Activity);
            builder.SetTitle("Add New Shooter");

            View view = LayoutInflater.From(Activity).Inflate(Resource.Layout.dialogfragment_addshooter, null);

            EditText shooterName = view.FindViewById<EditText>(Resource.Id.newShootersName);
            EditText shooterClass = view.FindViewById<EditText>(Resource.Id.newShooterClass);

            builder.SetView(view);
            builder.SetPositiveButton("Add", (c, ev) => 
            {
                //Shooters newShooter = new Shooters { Name = shooterName.Text, Class = shooterClass.Text };

                string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "ClubClaysData.db3");
                using SQLiteConnection db = new SQLiteConnection(dbPath);
                db.CreateCommand($"INSERT OR IGNORE INTO Shooters(Name, Class) VALUES ('{shooterName.Text}', '{shooterClass.Text}');").ExecuteNonQuery();
                var shooter = db.Table<Shooters>().Where(s => s.Name == shooterName.Text).ToList();
                db.Close();

                selectedShootersModel.selectedShooters.Add(shooter[0]);
                
            });
            builder.SetNegativeButton("Cancel", (c, ev) => { });

            builder.Show();
        }

        public class BackPress : OnBackPressedCallback
        {
            ShootersFragment context;

            public BackPress(ShootersFragment cont) : base(true)
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
            result.PutInt("numSelected", selectedShootersModel.selectedShooters.Count);
            Activity.SupportFragmentManager.SetFragmentResult("1", result);
        }  
    }

    public class ShootersRecyclerAdapter : RecyclerView.Adapter, ItemMoveCallback.ItemTouchHelperContract
    {
        private List<Shooters> shooters;
        private ShooterStandData shootersModel;
        private RecyclerView otherRecyclerView;
        private string type;

        // Provide a reference to the views for each data item
        public class MyView : RecyclerView.ViewHolder
        {
            public View mMainView { get; set; }
            public TextView mShooterName { get; set; }
            public MyView(View view) : base(view)
            {
                mMainView = view;
            }
        }

        // Return the size of data set (invoked by the layout manager)
        public override int ItemCount => shooters.Count;

        // Replace the contents of a view (invoked by layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            MyView myHolder = holder as MyView;
            myHolder.mShooterName.Text = shooters[position].Name;
        }

        // Create new views (invoked by layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View shooterCardView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.shooters_item, parent, false);
            TextView shootersName = shooterCardView.FindViewById<TextView>(Resource.Id.shootersName);

            MyView view = new MyView(shooterCardView) { mShooterName = shootersName };

            shooterCardView.Click += delegate
            {
                if (type == "all")
                {
                    shootersModel.selectedShooters.Add(shootersModel.allShooters[view.AdapterPosition]);
                    shootersModel.allShooters.RemoveAt(view.AdapterPosition);
                    NotifyDataSetChanged();
                    otherRecyclerView.GetAdapter().NotifyDataSetChanged();
                }
                else if (type == "selected")
                {
                    shootersModel.allShooters.Add(shootersModel.selectedShooters[view.AdapterPosition]);
                    shootersModel.selectedShooters.RemoveAt(view.AdapterPosition);
                    NotifyDataSetChanged();
                    otherRecyclerView.GetAdapter().NotifyDataSetChanged();
                }
            };

            return view;
        }

        public void onRowMoved(int fromPosition, int toPosition)
        {
            if (fromPosition < toPosition)
            {
                for (int i = fromPosition; i < toPosition; i++)
                {
                    Swap(shooters, i, i + 1);
                }
            }
            else
            {
                for (int i = fromPosition; i > toPosition; i--)
                {
                    Swap(shooters, i, i - 1);
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
        public ShootersRecyclerAdapter(ref ShooterStandData viewModel, ref RecyclerView otherRecycler, string recylerType)
        {
            shootersModel = viewModel;
            type = recylerType;
            otherRecyclerView = otherRecycler;

            if (type == "all")
            {
                shooters = shootersModel.allShooters;
            }
            else if (type == "selected")
            {
                shooters = shootersModel.selectedShooters;
            }
        }
    }
}