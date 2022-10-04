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
    public sealed partial class LoadCartDialog : ContentDialog
    {
        public LoadCartDialog()
        {
            this.InitializeComponent();
            this.DataContext = new CartViewModel();
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            var vm = DataContext as CartViewModel;
            if (vm != null)
            {
                if (vm.SelectedFileName != null)
                {
                    CartService.Current.Load(vm.SelectedFileName);
                    //vm.SetFileToDelete(vm.SelectedFileName);
                }
            }
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
