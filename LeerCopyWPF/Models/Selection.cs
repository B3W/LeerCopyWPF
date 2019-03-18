using System;
using System.Windows;
using System.Collections.Generic;
using System.Text;

namespace LeerCopyWPF.Models
{
    /// <summary>
    /// Data structure representing a selection of the screen
    /// </summary>
    public class Selection
    {
        /// <summary>
        /// Point at which user began selection
        /// </summary>
        public Point StartPt { get; private set; }
        /// <summary>
        /// Point at which user ended selection
        /// </summary>
        public Point EndPt { get; private set; }

        public Selection()
        {
            StartPt = new Point();
            EndPt = new Point();
        }

        public void SetStart(Point point)
        {
            StartPt = point;
            EndPt = point;
        }

        public void SetStart(double x, double y)
        {
            StartPt = new Point(x, y);
            EndPt = new Point(x, y);
        }

        public void Update(Point point)
        {
            EndPt = point;
        }

        public void Update(double x, double y, bool offset)
        {
            if (offset)
            {
                EndPt.Offset(x, y);
            }
            else
            {
                EndPt = new Point(x, y);
            }
        }

        public void Reset()
        {
            StartPt = new Point();
            EndPt = new Point();
        }
    }
}
