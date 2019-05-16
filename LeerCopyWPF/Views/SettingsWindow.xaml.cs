using LeerCopyWPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LeerCopyWPF.Views
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        #region Fields
        private readonly SettingsViewModel _viewModel;
        private readonly KeyConverter _keyConverter;
        #endregion // Fields

        #region Constructors
        public SettingsWindow()
        {
            InitializeComponent();

            _keyConverter = new KeyConverter();
            _viewModel = new SettingsViewModel(param => this.Close());
            DataContext = _viewModel;

            // This makes sure the first UserControl in the tab order gets focus when the window is opened
            // https://stackoverflow.com/a/818536
            Loaded += (sender, e) => MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
        }
        #endregion // Constructors

        #region EventHandlers
        /// <summary>
        /// Update the text within the text box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KeyBindingTxtBox_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            TextBox keyBindTxtBx = (TextBox)sender;
            Key newKey = e.Key;
            Key oldKey = (Key)_keyConverter.ConvertFromString(keyBindTxtBx.Text);

            if (newKey == oldKey)
            {
                return;
            }
            else if ((newKey < Key.Space || newKey > Key.Home) &&  // Space, PageUp, PageDown, Home, End
                    (newKey != Key.Insert && newKey != Key.Delete) &&
                    (newKey < Key.D0 || newKey > Key.Z) &&  // 0, 1, 2, ..., x, y, z
                    (newKey < Key.NumPad0 || newKey > Key.F24))         // 0, 1, 2, ..., *, +, ..., F22, F23, F24
            {
                return;
            }

            keyBindTxtBx.Text = _keyConverter.ConvertToString(newKey);
            e.Handled = true;
        } // KeyBindingTxtBox_KeyUp
        #endregion // EventHandlers
    }
}
