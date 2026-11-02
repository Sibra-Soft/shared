namespace Shared.Extensions
{
    public static class ListviewExt
    {
        public static void MoveSelectedItemUp(this ListView listView)
        {
            listView.MoveSelectedItem(-1);
        }

        public static void MoveSelectedItemDown(this ListView listView)
        {
            listView.MoveSelectedItem(1);
        }

        private static void MoveSelectedItem(this ListView listView, int direction)
        {
            if (listView.SelectedItems.Count == 0)
                return;

            ListViewItem selectedItem = listView.SelectedItems[0];
            int oldIndex = selectedItem.Index;
            int newIndex = oldIndex + direction;

            if (newIndex < 0 || newIndex >= listView.Items.Count)
                return;

            listView.BeginUpdate();

            try
            {
                listView.Items.RemoveAt(oldIndex);
                listView.Items.Insert(newIndex, selectedItem);

                selectedItem.Selected = true;
                selectedItem.Focused = true;
                listView.Focus();
            }
            finally
            {
                listView.EndUpdate();
            }
        }

        public static void ResizeColumns(this ListView listView)
        {
            if (listView.Columns.Count == 0)
                return;

            int totalWidth = listView.ClientSize.Width;

            // Houd rekening met scrollbar
            if (listView.Items.Count > 0)
            {
                totalWidth -= SystemInformation.VerticalScrollBarWidth;
            }

            int fixedWidth = 0;
            int fillColumnCount = 0;

            // Eerste pass: bereken fixed widths
            foreach (ColumnHeader col in listView.Columns)
            {
                if (IsFixed(col))
                {
                    fixedWidth += col.Width;
                }
                else
                {
                    fillColumnCount++;
                }
            }

            if (fillColumnCount == 0)
                return;

            int remainingWidth = totalWidth - fixedWidth;
            if (remainingWidth <= 0)
                return;

            int widthPerColumn = remainingWidth / fillColumnCount;

            // Tweede pass: toepassen
            foreach (ColumnHeader col in listView.Columns)
            {
                if (!IsFixed(col))
                {
                    col.Width = widthPerColumn;
                }
            }
        }

        private static bool IsFixed(ColumnHeader col)
        {
            return col.Tag != null && col.Tag.ToString() == "fixed";
        }
    }
}