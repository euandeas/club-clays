using AndroidX.RecyclerView.Widget;

namespace ClubClays
{
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

        public override void OnSwiped(RecyclerView.ViewHolder p0, int p1) { }

        public override void OnSelectedChanged(RecyclerView.ViewHolder viewHolder, int actionState)
        {
            if (actionState != ItemTouchHelper.ActionStateIdle)
            {
                if (viewHolder.GetType() != typeof(RecyclerView.ViewHolder))
                {
                    mAdapter.onRowSelected(viewHolder);
                }
            }
            base.OnSelectedChanged(viewHolder, actionState);
        }

        public override void ClearView(RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder)
        {
            base.ClearView(recyclerView, viewHolder);

            if (viewHolder.GetType() != typeof(RecyclerView.ViewHolder))
            {
                RecyclerView.ViewHolder myViewHolder = viewHolder;
                mAdapter.onRowClear(myViewHolder);
            }
        }
    }

    public class ItemMoveSwipeCallback : ItemTouchHelper.Callback
    {
        private ItemTouchHelperContract mAdapter;

        public ItemMoveSwipeCallback(ItemTouchHelperContract adapter)
        {
            mAdapter = adapter;
        }

        public interface ItemTouchHelperContract
        {
            void onRowMoved(int fromPosition, int toPosition);
            void onRowSelected(RecyclerView.ViewHolder myViewHolder);
            void onRowClear(RecyclerView.ViewHolder myViewHolder);
            void onSwiped(RecyclerView.ViewHolder myViewHolder, int pos);
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

        public override void OnSwiped(RecyclerView.ViewHolder p0, int p1)
        {
            mAdapter.onSwiped(p0, p1);
        }

        public override void OnSelectedChanged(RecyclerView.ViewHolder viewHolder, int actionState)
        {
            if (actionState != ItemTouchHelper.ActionStateIdle)
            {
                if (viewHolder.GetType() != typeof(RecyclerView.ViewHolder))
                {
                    mAdapter.onRowSelected(viewHolder);
                }
            }
            base.OnSelectedChanged(viewHolder, actionState);
        }

        public override void ClearView(RecyclerView recyclerView, RecyclerView.ViewHolder viewHolder)
        {
            base.ClearView(recyclerView, viewHolder);

            if (viewHolder.GetType() != typeof(RecyclerView.ViewHolder))
            {
                RecyclerView.ViewHolder myViewHolder = viewHolder;
                mAdapter.onRowClear(myViewHolder);
            }
        }
    }
}