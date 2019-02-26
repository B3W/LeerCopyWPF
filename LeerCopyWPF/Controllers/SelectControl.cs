using LeerCopyWPF.Models;
using System.Windows;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Imaging;

namespace LeerCopyWPF.Controllers
{
    public class SelectControl
    {
        /// <summary>
        /// Bitmap of the screen
        /// </summary>
        public BitmapSource bitmap { get; private set; }
        /// <summary>
        /// Tracks user's selection area
        /// </summary>
        public Selection selection { get; private set; }
        /// <summary>
        /// Determines if user has made a selection
        /// </summary>
        public bool isSelected { get; private set; }
        /// <summary>
        /// Determines if user is currently making a selection
        /// </summary>
        public bool isSelecting { get; private set; }

        public SelectControl(BitmapSource bm)
        {
            bitmap = bm;
            selection = new Selection();
            isSelecting = false;
            isSelected = false;
        }

        public void StartSelection(Point start)
        {
            selection.SetStart(start);
            isSelecting = true;
        }

        public void UpdateSelection(Point point)
        {
            if (isSelecting)
            {
                selection.Update(point);
            }
        }

        public void UpdateSelection(double x, double y)
        {
            if (isSelected && !isSelecting)
            {
                selection.Update(x, y, true);
            }
        }

        public void StopSelection(Point point)
        {
            if (isSelecting && !selection.startPt.Equals(point))
            {
                UpdateSelection(point);
                isSelected = true;
            } else
            {
                isSelected = false;
            }
            isSelecting = false;
        }

        public void ClearSelection()
        {
            isSelecting = false;
            isSelected = false;
            selection.Reset();
        }
    }
}
