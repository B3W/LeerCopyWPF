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

        #endregion // Methods
    }
}