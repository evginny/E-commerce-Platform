using Library.ShoppingCart.Models;
using Library.ShoppingCart.Services;
using ShoppingCart.UWP.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ShoppingCart.UWP.Dialogs
{
    public sealed partial class QuantityProductDialog : ContentDialog
    {
        public QuantityProductDialog()
        {
            this.InitializeComponent();
            DataContext = new ProductByQuantity();
        }

        public QuantityProductDialog(Product selectedProduct)
        {
            this.InitializeComponent();
            this.DataContext = selectedProduct;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            var viewModel = DataContext as ProductByQuantity;

            InventoryService.Current.AddOrUpdate(DataContext as ProductByQuantity);
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void isBoGoCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as ProductByQuantity;
            viewModel.IsBogo = true;
        }

        private void isBoGoCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as ProductByQuantity;
            viewModel.IsBogo = false;
        }
    }
}
