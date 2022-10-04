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
    public sealed partial class DeleteProductFromCartDialog : ContentDialog
    {
        public DeleteProductFromCartDialog()
        {
            this.InitializeComponent();
            this.DataContext = new ProductViewModel();
        }

        public DeleteProductFromCartDialog(ProductViewModel pvm)
        {
            this.InitializeComponent();
            this.DataContext = pvm;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            var vm = DataContext as ProductViewModel;
            if (vm.isQuantityProduct)
            {
                CartService.Current.RemoveProductQuantity(vm.BoundProduct.ID, vm.quantity);
            }
            else if (vm.isWeightProduct)
            {
                CartService.Current.RemoveProductWeight(vm.BoundProduct.ID, vm.weight);
            }
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
