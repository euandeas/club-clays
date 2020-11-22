using Android.OS;
using Android.Views;
using System;
using AndroidX.RecyclerView.Widget;
using Fragment = AndroidX.Fragment.App.Fragment;

namespace ClubClays.Fragments
{
    public class StandSetupFragment : Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            View view = inflater.Inflate(Resource.Layout.fragment_standsetup, container, false);

            return view;
        }
    }

    public class StandsRecyclerAdapter : RecyclerView.Adapter
    {
        public override int ItemCount => throw new NotImplementedException();

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            throw new NotImplementedException();
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            throw new NotImplementedException();
        }
    }
}