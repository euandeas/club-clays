﻿using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using System;
using Fragment = AndroidX.Fragment.App.Fragment;

namespace ClubClays.Fragments
{
    public class StandFormatFragment : Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            View view = inflater.Inflate(Resource.Layout.fragment_stand_format, container, false);

            Toolbar toolbar = view.FindViewById<Toolbar>(Resource.Id.toolbar);
            ((AppCompatActivity)Activity).SetSupportActionBar(toolbar);
            ActionBar supportBar = ((AppCompatActivity)Activity).SupportActionBar;
            supportBar.SetDisplayHomeAsUpEnabled(true);
            supportBar.SetDisplayShowHomeEnabled(true);

            return view;
        }
    }

    public class StandFormatRecyclerAdapter : RecyclerView.Adapter
    {
        //private List<Shooters> shooters;

        // Provide a reference to the views for each data item
        public class MyView : RecyclerView.ViewHolder
        {
            public View mMainView { get; set; }
            public MyView(View view) : base(view)
            {
                mMainView = view;
            }
        }

        // Return the size of data set (invoked by the layout manager)
        public override int ItemCount
        {
            get { return 0; }
        }

        // Replace the contents of a view (invoked by layout manager)
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            throw new NotImplementedException();
        }

        // Create new views (invoked by layout manager)
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            throw new NotImplementedException();
        }

        // Provide a suitable constructor (depends on the kind of dataset)
        public StandFormatRecyclerAdapter()
        {

        }
    }
}