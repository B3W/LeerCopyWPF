using LeerCopyWPF.Commands;
using LeerCopyWPF.Controller;
using LeerCopyWPF.Enums;
using LeerCopyWPF.Models;
using LeerCopyWPF.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Input;

namespace LeerCopyWPF.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        #region Constants

        private const string ConstDisplayName = "Leer Copy";

        #endregion // Constants


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

        public override string DisplayName { get => ConstDisplayName; }

        #endregion

        #region Protected Properties
        #endregion

        #region Private Properties
        #endregion

        #endregion // Properties


        #region Fields

        #region Public Methods

        public MainWindowViewModel()
        {
        }

        #endregion

        #region Protected Methods
        #endregion

        #region Private Methods
        #endregion

        #endregion // Methods
    }
}
