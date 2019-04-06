using System;
using System.Windows;
using System.Collections.Generic;
using System.Text;
using LeerCopyWPF.Enums;

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
        } // SetStart


        public void SetStart(double x, double y)
        {
            StartPt = new Point(x, y);
            EndPt = new Point(x, y);
        } // SetStart


        public void UpdateStart(Point point)
        {
            StartPt = point;
        } // UpdateStart


        public void UpdateEnd(Point point)
        {
            EndPt = point;
        } // UpdateEnd
        

        public void UpdateEnd(double x, double y, bool offset)
        {
            if (offset)
            {
                EndPt = new Point(EndPt.X + x, EndPt.Y + y);
            }
            else
            {
                EndPt = new Point(x, y);
            }
        } // UpdateEnd


        /// <summary>
        /// Resizes selection if the resized selection fits within the bounds of the screens
        /// and it does not overlap edges (i.e. left edge passes over right edge)
        /// </summary>
        /// <param name="offsetX"></param>
        /// <param name="offsetY"></param>
        /// <param name="dir"></param>
        /// <param name="bounds"></param>
        public void Resize(double offsetX, double offsetY, KeyActions.KeyDown dir, Rect bounds)
        {
            Point tmpPt;

            switch (dir)
            {
                case KeyActions.KeyDown.Up:
                    if (StartPt.Y > EndPt.Y)
                    {
                        tmpPt = new Point(EndPt.X, (EndPt.Y + offsetY));
                        if (bounds.Top <= tmpPt.Y && StartPt.Y > tmpPt.Y)
                        {
                            EndPt = tmpPt;
                        }
                    }
                    else if (StartPt.Y < EndPt.Y)
                    {
                        tmpPt = new Point(StartPt.X, (StartPt.Y + offsetY));
                        if (bounds.Top <= tmpPt.Y && EndPt.Y > tmpPt.Y)
                        {
                            StartPt = tmpPt;
                        }
                    }
                    break;
                case KeyActions.KeyDown.Down:
                    if (StartPt.Y > EndPt.Y)
                    {
                        tmpPt = new Point(StartPt.X, (StartPt.Y + offsetY));
                        if (bounds.Bottom >= tmpPt.Y && EndPt.Y < tmpPt.Y)
                        {
                            StartPt = tmpPt;
                        }
                    }
                    else if (StartPt.Y < EndPt.Y)
                    {
                        tmpPt = new Point(EndPt.X, (EndPt.Y + offsetY));
                        if (bounds.Bottom >= tmpPt.Y && StartPt.Y < tmpPt.Y)
                        {
                            EndPt = tmpPt;
                        }
                    }
                    break;
                case KeyActions.KeyDown.Left:
                    if (StartPt.X > EndPt.X)
                    {
                        tmpPt = new Point((EndPt.X + offsetX), EndPt.Y);
                        if (bounds.Left <= tmpPt.X && StartPt.X > tmpPt.X)
                        {
                            EndPt = tmpPt;
                        }
                    }
                    else if (StartPt.X < EndPt.X)
                    {
                        tmpPt = new Point((StartPt.X + offsetX), StartPt.Y);
                        if (bounds.Left <= tmpPt.X && EndPt.X > tmpPt.X)
                        {
                            StartPt = tmpPt;
                        }
                    }
                    break;
                case KeyActions.KeyDown.Right:
                    if (StartPt.X > EndPt.X)
                    {
                        tmpPt = new Point((StartPt.X + offsetX), StartPt.Y);
                        if (bounds.Right >= tmpPt.X && EndPt.X < tmpPt.X)
                        {
                            StartPt = tmpPt;
                        }
                    }
                    else if (StartPt.X < EndPt.X)
                    {
                        tmpPt = new Point((EndPt.X + offsetX), EndPt.Y);
                        if (bounds.Right >= tmpPt.X && StartPt.X < tmpPt.X)
                        {
                            EndPt = tmpPt;
                        }
                    }
                    break;
                case KeyActions.KeyDown.Invalid:
                default:
                    break;
            }
        } // Resize

        public void Reset()
        {
            StartPt = new Point();
            EndPt = new Point();
        } // Reset
    }
}
