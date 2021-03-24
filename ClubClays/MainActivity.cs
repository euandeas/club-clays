using Android.App;
using Android.OS;
using Android.Runtime;
using AndroidX.AppCompat.App;
using FragmentTransaction = AndroidX.Fragment.App.FragmentTransaction;
using ClubClays.Fragments;
using SQLite;
using System.IO;

namespace ClubClays
{
    [Activity(Label = "@string/app_name", Theme = "@style/SplashTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "ClubClaysData.db3");
            using(var db = new SQLiteConnection(dbPath))
            {
                db.CreateTable<DatabaseModels.Shoots>();
                db.CreateTable<DatabaseModels.Stands>();
                db.CreateTable<DatabaseModels.StandScores>();
                db.CreateTable<DatabaseModels.Shots>();
                db.CreateTable<DatabaseModels.OverallScores>();
                db.CreateTable<DatabaseModels.Shooters>();
                db.CreateTable<DatabaseModels.ShootFormats>();
                db.CreateTable<DatabaseModels.StandFormats>();
                db.CreateTable<DatabaseModels.SavedFormatsLink>();
                db.CreateTable<DatabaseModels.StandShotsLink>();

                db.Insert(new DatabaseModels.Shots())
            }

            SetTheme(Resource.Style.AppTheme);
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            if (savedInstanceState == null)
            {
                FragmentTransaction fragmentTx = SupportFragmentManager.BeginTransaction();
                fragmentTx.Replace(Resource.Id.container, new MainFragment());
                fragmentTx.Commit();
            }
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public override bool OnSupportNavigateUp()
        {
            base.OnBackPressed();
            return true;
        }
    }
}