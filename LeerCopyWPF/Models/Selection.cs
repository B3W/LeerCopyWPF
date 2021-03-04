using LeerCopyWPF.Enums;
using LeerCopyWPF.Utilities;
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
        /// Max bounds for the selection. Relative application's selection window.
        /// </summary>
        public Rect SelectionBounds { get; }

        /// <summary>
        /// Bounds for screen selection is occuring on. Relative to other screens.
        /// </summary>
        public Rect ScreenBounds { get; }

        #endregion // Properties


        #region Methods

        /// <summary>
        /// Constructs a selection
        /// </summary>
        /// <param name="bitmap">Image for selection</param>
        /// <param name="screenBounds">Bounds of the screen</param>
        public Selection(BitmapSource bitmap, Rect screenBounds)
        {
            Bitmap = bitmap;
            ScreenBounds = screenBounds;
            StartPt = new Point();
            EndPt = new Point();

            // Selection bounds are derived from screen bounds but always start at (0, 0) because screen
            // coordinates are global where as selection coordinates are relative to the selection windows.
            SelectionBounds = new Rect(0, 0, ScreenBounds.Width, ScreenBounds.Height);
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
            switch (dir)
            {
                case ResizeDirection.Up:
                    if (StartPt.Y > EndPt.Y)
                    {
                        double modifiedY = EndPt.Y + offsetY;
                        double clampedY = modifiedY.Clamp(SelectionBounds.Top, StartPt.Y);

                        EndPt = new Point(EndPt.X, clampedY);
                    }
                    else
                    {
                        double modifiedY = StartPt.Y + offsetY;
                        double clampedY = modifiedY.Clamp(SelectionBounds.Top, EndPt.Y);

                        StartPt = new Point(StartPt.X, clampedY);
                    }
                    break;
                case ResizeDirection.Down:
                    if (StartPt.Y > EndPt.Y)
                    {
                        double modifiedY = StartPt.Y + offsetY;
                        double clampedY = modifiedY.Clamp(EndPt.Y, SelectionBounds.Bottom);

                        StartPt = new Point(StartPt.X, clampedY);
                    }
                    else
                    {
                        double modifiedY = EndPt.Y + offsetY;
                        double clampedY = modifiedY.Clamp(StartPt.Y, SelectionBounds.Bottom);

                        EndPt = new Point(EndPt.X, clampedY);
                    }
                    break;
                case ResizeDirection.Left:
                    if (StartPt.X > EndPt.X)
                    {
                        double modifiedX = EndPt.X + offsetX;
                        double clampedX = modifiedX.Clamp(SelectionBounds.Left, StartPt.X);

                        EndPt = new Point(clampedX, EndPt.Y);
                    }
                    else
                    {
                        double modifiedX = StartPt.X + offsetX;
                        double clampedX = modifiedX.Clamp(SelectionBounds.Left, EndPt.X);

                        StartPt = new Point(clampedX, StartPt.Y);
                    }
                    break;
                case ResizeDirection.Right:
                    if (StartPt.X > EndPt.X)
                    {
                        double modifiedX = StartPt.X + offsetX;
                        double clampedX = modifiedX.Clamp(EndPt.X, SelectionBounds.Right);

                        StartPt = new Point(clampedX, StartPt.Y);
                    }
                    else
                    {
                        double modifiedX = EndPt.X + offsetX;
                        double clampedX = modifiedX.Clamp(StartPt.X, SelectionBounds.Right);

                        EndPt = new Point(clampedX, EndPt.Y);
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