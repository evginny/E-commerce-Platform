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
    public sealed partial class CartPage : Page
    {
        public CartPage()
        {
            this.InitializeComponent();
            DataContext = new CartViewModel();
        }

        private void Return_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(CustomerPage));
        }

        private async void CheckOut_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as CartViewModel;
            if (vm != null)
            {
                await vm.CheckOut();
            }
            Frame.Navigate(typeof(CustomerPage));
        }

        private void SignOut_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private async void Delete_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as CartViewModel;
            if (vm != null)
            {
                await vm.Remove();
            }
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as CartViewModel).Refresh();
        }

        private void CartSortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var vm = DataContext as CartViewModel;
            string sortName = e.AddedItems[0].ToString();
            if (vm != null)
            {
                switch (sortName)
                {
                    case "By Name ↑":
                        vm.Sort(SortType.ByNameAsc);
                        break;
                    case "By Name ↓":
                        vm.Sort(SortType.ByNameDesc);
                        break;
                    case "By Total Price ↑":
                        vm.Sort(SortType.ByTotalPriceAsc);
                        break;
                    case "By Total Price ↓":
                        vm.Sort(SortType.ByTotalPriceDesc);
                        break;
                }
                vm.Refresh();
                vm.Sort(SortType.Unordered);
            }
        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            //(DataContext as CartViewModel).Save();
            var vm = DataContext as CartViewModel;
            if (vm != null)
            {
                await vm.Save();
            }
        }

        private async void Load_Click(object sender, RoutedEventArgs e)
        {
            //(DataContext as CartViewModel).Load();
            var vm = DataContext as CartViewModel;
            if (vm != null)
            {
                await vm.Load();
            }

        }
    }
  
}
