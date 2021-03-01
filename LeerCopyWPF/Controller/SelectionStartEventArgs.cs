using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LeerCopyWPF.Controller
{
    /// <summary>
    /// Custom EventArgs for the SelectionStarted event.
    /// </summary>
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
        /// Coordinate contained within active screen.
        /// </summary>
        public Point ActiveScreenLocation { get; }

        #endregion

        #region Protected Properties
        #endregion

        #region Private Properties
        #endregion

        #endregion // Properties


        #region Methods

        #region Public Methods

        /// <summary>
        /// Constructs instance of SelectionStartEventArgs.
        /// </summary>
        /// <param name="activeScreenLocation">Coordinate contained within active screen</param>
        public SelectionStartEventArgs(Point activeScreenLocation)
        {
            ActiveScreenLocation = activeScreenLocation;
        }

        #endregion

        #region Protected Methods
        #endregion

        #region Private Methods
        #endregion

        #endregion // Methods
    }
}
