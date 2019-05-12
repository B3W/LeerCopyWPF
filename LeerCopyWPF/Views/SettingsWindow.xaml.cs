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

        public SettingsWindow()
        {
            InitializeComponent();

            _viewModel = new SettingsViewModel(param => this.Close());
            DataContext = _viewModel;
        }


        /// <summary>
        /// Handles key binding validation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KeyBindingTxtBox_KeyUp(object sender, KeyEventArgs e)
        {
            TextBox keyBindTxtBx = (TextBox)sender;
            BindingExpression txtBindingExpr = keyBindTxtBx.GetBindingExpression(TextBox.TextProperty);
            // Get the property name for raising NotifyPropertyChanged
            Binding txtBinding = txtBindingExpr.ParentBinding;
            string propertyName = txtBinding.Path.Path;
            Key key = e.Key;

            // Validate key press against valid keys
            if ((key >= Key.Cancel && key <= Key.Return)  ||  // Cancel, Backspace, Tab, Linefeed, Clear, Enter, Return
                (key >= Key.Space && key <= Key.Home)     ||  // Space, PageUp, PageDown, Home, End
                (key == Key.Insert || key == Key.Delete)  ||  
                (key >= Key.D0 && key <= Key.Z)           ||  // 0, 1, 2, ..., x, y, z
                (key >= Key.NumPad0 && key <= Key.F24))       // 0, 1, 2, ..., *, +, ..., F22, F23, F24
            {
                // Validate against current bindings
            }

            
            throw new NotImplementedException();
        }
    }
}
