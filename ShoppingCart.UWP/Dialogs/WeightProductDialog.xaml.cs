using Library.ShoppingCart.Models;
using Library.ShoppingCart.Services;
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
    public sealed partial class WeightProductDialog : ContentDialog
    {
        public WeightProductDialog()
        {
            this.InitializeComponent();
            this.DataContext = new ProductByWeight();
        }
        public WeightProductDialog(Product selectedProduct)
        {
            this.InitializeComponent();
            this.DataContext = selectedProduct;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            var viewModel = DataContext as ProductByWeight;
            InventoryService.Current.AddOrUpdate(DataContext as ProductByWeight);
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
