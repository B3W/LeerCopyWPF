using LeerCopyWPF.Enums;
using System.Windows;
using System.Windows.Media.Imaging;

namespace LeerCopyWPF.Models
{
    public class Selection
    {
        #region Fields
        #endregion // Fields


        #region Properties

        /// <summary>
        /// Starting point of the selection
        /// </summary>
        public Point StartPt { get; set; }

        /// <summary>
        /// Ending point of the selection
        /// </summary>
        public Point EndPt { get; set; }

        /// <summary>
        /// Image being selected
        /// </summary>
        public BitmapSource Bitmap { get; }

        /// <summary>
        /// Max bounds for the selection
        /// </summary>
        public Rect ScreenBounds { get; }

        #endregion // Properties


        #region Methods

        /// <summary>
        /// Constructs a selection
        /// </summary>
        /// <param name="bitmap">Image for selection</param>
        /// <param name="screenBounds">Bounds of the image</param>
        public Selection(BitmapSource bitmap, Rect screenBounds)
        {
            Bitmap = bitmap;
            ScreenBounds = screenBounds;
            StartPt = new Point();
            EndPt = new Point();
        } // Selection


        /// <summary>
        /// Resets the selection
        /// </summary>
        public void Reset()
        {
            StartPt = new Point();
            EndPt = new Point();
        } // Reset


        /// <summary>
        /// Resizes selection if the resized selection fits within the bounds of the screens
        /// and it does not overlap edges (i.e. left edge passes over right edge)
        /// </summary>
        /// <param name="offsetX"></param>
        /// <param name="offsetY"></param>
        /// <param name="dir"></param>
        public void Resize(double offsetX, double offsetY, ResizeDirection dir)
        {
            Point tmpPt;

            switch (dir)
            {
                case ResizeDirection.Up:
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
                case ResizeDirection.Down:
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
                case ResizeDirection.Left:
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
                case ResizeDirection.Right:
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
                case ResizeDirection.Invalid:
                default:
                    break;
            }
        } // Resize

        #endregion // Methods
    }
}