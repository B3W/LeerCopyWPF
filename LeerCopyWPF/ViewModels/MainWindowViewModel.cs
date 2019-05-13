using LeerCopyWPF.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace LeerCopyWPF.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        #region Fields

        #endregion // Fields

        #region Constants
        private const string ConstDisplayName = "Leer Copy";
        #endregion // Constants

        #region Properties
        public override string DisplayName { get => ConstDisplayName; }

        public ICommand CloseCommand { get; }
        #endregion // Properties

        #region Constructors
        public MainWindowViewModel(Action<object> closeAction)
        {

            CloseCommand = new RelayCommand(closeAction);
        }
        #endregion // Constructors
    }
}
