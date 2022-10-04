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
    public sealed partial class AddProductToCartDialog : ContentDialog
    {
        public AddProductToCartDialog()
        {
            this.InitializeComponent();
            this.DataContext = new ProductViewModel();
        }

        public AddProductToCartDialog(ProductViewModel pvm)
        {
            this.InitializeComponent();
            this.DataContext = pvm;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            var viewModel = DataContext as ProductViewModel;
            if (viewModel.isQuantityProduct)
            {
                CartService.Current.CreateProductByQuantity(viewModel.BoundProduct.ID, viewModel.quantity);
            }
            else if (viewModel.isWeightProduct)
            {
                CartService.Current.CreateProductByWeight(viewModel.BoundProduct.ID, viewModel.weight);
            }
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
