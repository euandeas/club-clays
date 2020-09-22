using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.Fragment.App;
using System;
using DialogFragment = AndroidX.Fragment.App.DialogFragment;
using Fragment = AndroidX.Fragment.App.Fragment;

namespace ClubClays.Fragments
{
    public class GeneralDataFragment : Fragment, DatePickerDialog.IOnDateSetListener
    {
        private Spinner dateSpinner;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            View view = inflater.Inflate(Resource.Layout.fragment_general_data, container, false);

            Spinner dateSpinner = view.FindViewById<Spinner>(Resource.Id.datePicker);
            dateSpinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(dateSpinner_ItemSelected); ;

            return view;
        }

        private void dateSpinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            DialogFragment datePicker = new DatePickerFragment();
            datePicker.Show(Activity.SupportFragmentManager, "date picker");
        }

        public void OnDateSet(DatePicker view, int year, int month, int dayOfMonth)
        {
            dateSpinner.TooltipText = "Test";
        }

    }

    public class DatePickerFragment : DialogFragment
    {
        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            return new DatePickerDialog(Activity, (DatePickerDialog.IOnDateSetListener)this, DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        }
    }
}