using System;
using System.Windows;
using System.Collections.Generic;
using System.Text;

namespace LeerCopyWPF.Models
{
    public class Selection
    {
        /// <summary>
        /// Point at which user began selection
        /// </summary>
        public Point startPt { get; private set; }
        /// <summary>
        /// Point at which user ended selection
        /// </summary>
        public Point endPt { get; private set; }

        public Selection()
        {
            startPt = new Point();
            endPt = new Point();
        }

        public void SetStart(Point point)
        {
            startPt = point;
            endPt = point;
        }

        public void SetStart(double x, double y)
        {
            startPt = new Point(x, y);
            endPt = new Point(x, y);
        }

        public void Update(Point point)
        {
            endPt = point;
        }

        public void Update(double x, double y, bool offset)
        {
            if (offset)
            {
                endPt.Offset(x, y);
            }
            else
            {
                endPt = new Point(x, y);
            }
        }

        public void Reset()
        {
            startPt = new Point();
            endPt = new Point();
        }
    }
}
