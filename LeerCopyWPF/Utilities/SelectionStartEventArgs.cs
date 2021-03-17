using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeerCopyWPF.Utilities
{
    public class SelectionStartEventArgs : EventArgs
    {
        #region Fields

        #region Public Fields
        #endregion

        #region Protected Fields
        #endregion

        #region Private Fields
        #endregion

        #endregion // Fields


        #region Properties

        #region Public Properties

        /// <summary>
        /// X coordinate of the main window
        /// </summary>
        public double MainWindowX { get; }

        /// <summary>
        /// Y coordinate of the main window
        /// </summary>
        public double MainWindowY { get; }

        #endregion

        #region Protected Properties
        #endregion

        #region Private Properties
        #endregion

        #endregion // Properties


        #region Methods

        #region Public Methods

        /// <summary>
        /// Constructs SelectionStartEventArgs instance
        /// </summary>
        /// <param name="x">X coordinate of the main window</param>
        /// <param name="y">Y coordinate of the main window</param>
        public SelectionStartEventArgs(double x, double y)
        {
            MainWindowX = x;
            MainWindowY = y;
        }

        #endregion

        #region Protected Methods
        #endregion

        #region Private Methods
        #endregion

        #endregion // Methods
    }
}
