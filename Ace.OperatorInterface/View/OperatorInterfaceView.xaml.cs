// Copyright © Omron Robotics and Safety Technologies, Inc. All rights reserved.
//

using Ace.OperatorInterface.ViewModel;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Ace.OperatorInterface.View {
    /// <summary>
    /// Interaction logic for OperatorInterface.xaml
    /// </summary>
    public partial class OperatorInterfaceView : UserControl, IOperatorInterfaceView {
        private OperatorInterfaceViewModel OperatorInterfaceVM {
            get {
                return this.DataContext as OperatorInterfaceViewModel;
            }
        }

        public OperatorInterfaceView() {
            InitializeComponent();
            this.DataContextChanged += OperatorInterfaceView_DataContextChanged;
        }

        private void OperatorInterfaceView_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e) {
            this.OperatorInterfaceVM.UpdateControllerCollection();

            // Get the DataContext for this XAML-UserControl.
            // Assign the OnViewModel_ReportError() method below to the Action<string> ReportError delegate of the ViewModel base class
            ((OperatorInterfaceViewModel) DataContext).ControllerItems.ReportError = OnViewModel_ReportError;
        }

        // Popup a MessageBox in case of an error thrown in the ViewModel classes.
        private void OnViewModel_ReportError(string obj) {
            System.Windows.MessageBox.Show(obj.ToString());
        }

        private void TextBox_GotFocus(object sender, System.Windows.RoutedEventArgs e) {

            UpdateSelectedItem(sender);

            var textBox = sender as TextBox;
            if (textBox == null) {
                return;
            }

            Binding binding = BindingOperations.GetBinding(textBox, TextBox.TextProperty);
            if (binding == null) {
                return;
            }

            var selection = OperatorInterfaceVM.SelectedControllerItem;
            if (selection != null) {
                selection.SelectedPropertyName = binding.Path.Path;
            }

        }

        private void TextBox_LostFocus(object sender, System.Windows.RoutedEventArgs e) {
            UpdateSelectedItem(sender);

            var selection = OperatorInterfaceVM.SelectedControllerItem;
            if (selection != null) {
                selection.SelectedPropertyName = string.Empty;
            }
        }

        private void UpdateSelectedItem(object sender) {
            ListBoxItem selectedItem = (ListBoxItem) listBoxControllers.ItemContainerGenerator.
                  ContainerFromItem(((FrameworkElement) sender).DataContext);
            selectedItem.IsSelected = true;

        }
    }
}


