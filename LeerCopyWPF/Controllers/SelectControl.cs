using LeerCopyWPF.Models;
using System.Windows;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace LeerCopyWPF.Controllers
{
    /// <summary>
    /// Controller logic between the UI and underlying selection data structure
    /// </summary>
    public class SelectControl
    {
        /// <summary>
        /// Bitmap of the screen
        /// </summary>
        public BitmapSource Bitmap { get; private set; }
        /// <summary>
        /// Tracks user's selection area
        /// </summary>
        public Selection Selection { get; private set; }
        /// <summary>
        /// Determines if user has made a selection
        /// </summary>
        public bool IsSelected { get; private set; }
        /// <summary>
        /// Determines if user is currently making a selection
        /// </summary>
        public bool IsSelecting { get; private set; }


        public SelectControl(BitmapSource bm)
        {
            Bitmap = bm;
            Selection = new Selection();
            IsSelecting = false;
            IsSelected = false;
        }


        public void StartSelection(Point start)
        {
            if (IsSelected)
            {
                Selection.Update(start);
            } else
            {
                Selection.SetStart(start);
            }
            IsSelecting = true;
        } // StartSelection


        public void UpdateSelection(Point point)
        {
            Selection.Update(point);
        } // UpdateSelection


        public void UpdateSelection(double x, double y)
        {
            if (IsSelected && !IsSelecting)
            {
                Selection.Update(x, y, true);
            }
        } // UpdateSelection


        public void StopSelection(Point point)
        {
            if (IsSelecting && !Selection.StartPt.Equals(point))
            {
                UpdateSelection(point);
                IsSelected = true;
            } else
            {
                IsSelected = false;
            }
            IsSelecting = false;
        } // StopSelection


        public Rect GetSelectionRect()
        {
            return new Rect(Selection.StartPt, Selection.EndPt);
        } // GetSelectionRect


        public RectangleGeometry GetSelectionGeometry()
        {
            return new RectangleGeometry(new Rect(Selection.StartPt, Selection.EndPt));
        } // GetSelectionGeometry


        public void CopySelection()
        {

        } // CopySelection


        public void EditSelection()
        {

        } // EditSelection


        public void SaveSelection()
        {

        } // SaveSelection


        public void MaximizeSelection()
        {

        } // MaximizeSelection


        public void ClearSelection()
        {
            IsSelecting = false;
            IsSelected = false;
            Selection.Reset();
        } // ClearSelection
    }
}
