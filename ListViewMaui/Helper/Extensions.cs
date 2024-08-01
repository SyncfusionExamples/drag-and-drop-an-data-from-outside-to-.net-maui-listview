using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DragDropSample
{
    public static class ListViewExtHelper
    {
        internal static void GetPositionFrombounds(this ListViewExt listView, Point? dragPoint, Rect bounds, out double prevPosition, out double nextPosition)
        {
            var isVertical = listView.Orientation == ItemsLayoutOrientation.Vertical;
            prevPosition = isVertical ? dragPoint.Value.Y : dragPoint.Value.X;
            nextPosition = isVertical ? dragPoint.Value.Y + bounds.Height : dragPoint.Value.X + bounds.Width;

            if (listView.HasStickyHeader())
            {
                prevPosition -= listView.HeaderSize;
                nextPosition -= listView.HeaderSize;
            }
        }

        internal static bool PerformAutoScroll(this ListViewExt listView, double prevPosition, double nextPosition)
        {
            var scrollOffset = (double)listView.visualContainer.GetType().GetRuntimeProperties().FirstOrDefault(x => x.Name == "ScrollOffset").GetValue(listView.visualContainer);
            prevPosition += scrollOffset;
            nextPosition += scrollOffset;

            if (listView.CanAutoScroll(prevPosition, nextPosition, ref listView.IsDown))
            {
                listView.StartScrolling(listView.IsDown ? 50 : -50);
                return true;
            }
            else
            {
                listView.StopScrolling();
            }
            return false;
        }

        internal static void FindEndItemIndex(this ListViewExt listView, double prevPosition, double nextPosition)
        {
            listView.FindEndItemIndex(prevPosition, nextPosition);
        }
    }
}
