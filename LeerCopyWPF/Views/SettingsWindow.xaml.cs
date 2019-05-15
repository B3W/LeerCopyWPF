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
        private readonly SettingsViewModel _viewModel;
        private readonly KeyConverter _keyConverter;

        public SettingsWindow()
        {
            InitializeComponent();

            _viewModel = new SettingsViewModel(param => this.Close());
            DataContext = _viewModel;
            _keyConverter = new KeyConverter();
        }


        /// <summary>
        /// Handles key binding validation to prevent unwanted characters entering control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KeyBindingTxtBox_PreviewTextInput(object sender, KeyEventArgs e)
        {
            TextBox keyBindTxtBx = (TextBox)sender;
            Key newKey = e.Key;
            Key oldKey = (Key)_keyConverter.ConvertFromString(keyBindTxtBx.Text);

            if (newKey == oldKey)
            {
                e.Handled = true;
            }
            else if ((newKey < Key.Cancel || newKey > Key.Return)   &&  // Cancel, Backspace, Tab, Linefeed, Clear, Enter, Return
                    (newKey < Key.Space || newKey > Key.Home)       &&  // Space, PageUp, PageDown, Home, End
                    (newKey != Key.Insert && newKey != Key.Delete)  &&  
                    (newKey < Key.D0 || newKey > Key.Z)             &&  // 0, 1, 2, ..., x, y, z
                    (newKey < Key.NumPad0 || newKey > Key.F24))         // 0, 1, 2, ..., *, +, ..., F22, F23, F24
            {
                e.Handled = true;
            }
            else
            {
                // Get the property name for raising NotifyPropertyChanged
                BindingExpression txtBindingExpr = keyBindTxtBx.GetBindingExpression(TextBox.TextProperty);
                Binding txtBinding = txtBindingExpr.ParentBinding;
                string propertyName = txtBinding.Path.Path;

                /*
                 * How to manually add errors to Validation.Errors (https://stackoverflow.com/a/3660863)
                 * ValidationError validationError = new ValidationError( ? , txtBindingExpr);
                 * validationError.ErrorContent = "This is not a valid e-mail address";
                 * Validation.MarkInvalid(txtBindingExpr, validationError);
                 */
            }
            
            throw new NotImplementedException();
        } // KeyBindingTxtBox_KeyDown
    }
}
