using LeerCopyWPF.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;

namespace LeerCopyWPF.Models
{
    public class SelectionRefactor
    {
        #region Members
        public Point StartPt { get; set; }
        public Point EndPt { get; set; }
        public BitmapSource Bitmap { get; }
        public Rect ScreenBounds { get; }
        #endregion // Members

        #region Constructors
        public SelectionRefactor(BitmapSource bitmap, Rect screenBounds)
        {
            Bitmap = bitmap;
            ScreenBounds = screenBounds;
            StartPt = new Point();
            EndPt = new Point();
        }
        #endregion // Constructors

        #region Methods
        public void Reset()
        {
            StartPt = new Point();
            EndPt = new Point();
        }


        /// <summary>
        /// Resizes selection if the resized selection fits within the bounds of the screens
        /// and it does not overlap edges (i.e. left edge passes over right edge)
        /// </summary>
        /// <param name="offsetX"></param>
        /// <param name="offsetY"></param>
        /// <param name="dir"></param>
        public void Resize(double offsetX, double offsetY, KeyActions.KeyDown dir)
        {
            Point tmpPt;

            switch (dir)
            {
                case KeyActions.KeyDown.Up:
                    if (StartPt.Y > EndPt.Y)
                    {
                        tmpPt = new Point(EndPt.X, (EndPt.Y + offsetY));
                        if (ScreenBounds.Top <= tmpPt.Y && StartPt.Y > tmpPt.Y)
                        {
                            EndPt = tmpPt;
                        }
                    }
                    else if (StartPt.Y < EndPt.Y)
                    {
                        tmpPt = new Point(StartPt.X, (StartPt.Y + offsetY));
                        if (ScreenBounds.Top <= tmpPt.Y && EndPt.Y > tmpPt.Y)
                        {
                            StartPt = tmpPt;
                        }
                    }
                    break;
                case KeyActions.KeyDown.Down:
                    if (StartPt.Y > EndPt.Y)
                    {
                        tmpPt = new Point(StartPt.X, (StartPt.Y + offsetY));
                        if (ScreenBounds.Bottom >= tmpPt.Y && EndPt.Y < tmpPt.Y)
                        {
                            StartPt = tmpPt;
                        }
                    }
                    else if (StartPt.Y < EndPt.Y)
                    {
                        tmpPt = new Point(EndPt.X, (EndPt.Y + offsetY));
                        if (ScreenBounds.Bottom >= tmpPt.Y && StartPt.Y < tmpPt.Y)
                        {
                            EndPt = tmpPt;
                        }
                    }
                    break;
                case KeyActions.KeyDown.Left:
                    if (StartPt.X > EndPt.X)
                    {
                        tmpPt = new Point((EndPt.X + offsetX), EndPt.Y);
                        if (ScreenBounds.Left <= tmpPt.X && StartPt.X > tmpPt.X)
                        {
                            EndPt = tmpPt;
                        }
                    }
                    else if (StartPt.X < EndPt.X)
                    {
                        tmpPt = new Point((StartPt.X + offsetX), StartPt.Y);
                        if (ScreenBounds.Left <= tmpPt.X && EndPt.X > tmpPt.X)
                        {
                            StartPt = tmpPt;
                        }
                    }
                    break;
                case KeyActions.KeyDown.Right:
                    if (StartPt.X > EndPt.X)
                    {
                        tmpPt = new Point((StartPt.X + offsetX), StartPt.Y);
                        if (ScreenBounds.Right >= tmpPt.X && EndPt.X < tmpPt.X)
                        {
                            StartPt = tmpPt;
                        }
                    }
                    else if (StartPt.X < EndPt.X)
                    {
                        tmpPt = new Point((EndPt.X + offsetX), EndPt.Y);
                        if (ScreenBounds.Right >= tmpPt.X && StartPt.X < tmpPt.X)
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
        #endregion // Methods
    }
}