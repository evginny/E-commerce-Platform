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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ShoppingCart.UWP.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CustomerPage : Page
    {
        public CustomerPage()
        {
            this.InitializeComponent();
            DataContext = new EmployeeViewModel();
        }

        private void SignOut_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private void Cart_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(CartPage));
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as EmployeeViewModel).Refresh();
        }

        private async void AddToCart_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as EmployeeViewModel;
            if (vm != null)
            {
                await vm.AddProductCart();
            }
        }
    }
}
