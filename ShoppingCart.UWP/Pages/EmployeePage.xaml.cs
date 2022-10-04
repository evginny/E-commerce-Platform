using ShoppingCart.UWP.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ShoppingCart.UWP.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EmployeePage : Page  
    {
        public EmployeePage()
        {
            this.InitializeComponent();
            DataContext = new EmployeeViewModel();
        }
        private void  Search_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as EmployeeViewModel).Refresh();
        }

        private async void Add_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as EmployeeViewModel;
            if (vm != null)
            {
                await vm.Add(ProductType.Product);
            }
        }

        private async void Edit_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as EmployeeViewModel;
            if (vm != null)
            {
                await vm.Update();
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as EmployeeViewModel;
            if (vm != null)
            {
                vm.Remove();
            }
        }

        private void SignOut_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as EmployeeViewModel).Save();
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as EmployeeViewModel).Load();
        }

        private async void AddQuantity_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as EmployeeViewModel;
            if (vm != null)
            {
                await vm.Add(ProductType.QuantityProduct);
            }
        }

        private async void AddWeight_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as EmployeeViewModel;
            if (vm != null)
            {
                await vm.Add(ProductType.WeightProduct);
            }
        }

        private void SortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var vm = DataContext as EmployeeViewModel;
            string sortName = e.AddedItems[0].ToString();
            if (vm != null)
            {
                /* switch (sortName)
                 {
                     case "By Name ↑":
                         vm.Sort(1, true);
                         break;
                     case "By Name ↓":
                         vm.Sort(1, false);
                         break;
                     case "By Price ↑":
                         vm.Sort(2, true);
                         break;
                     case "By Price ↓":
                         vm.Sort(2, false);
                         break;
                 }
                */
                switch (sortName)
                {
                    case "By Name ↑":
                        vm.Sort(SortType.ByNameAsc);
                        break;
                    case "By Name ↓":
                        vm.Sort(SortType.ByNameDesc);
                        break;
                    case "By Price ↑":
                        vm.Sort(SortType.ByPriceAsc);
                        break;
                    case "By Price ↓":
                        vm.Sort(SortType.ByPriceDesc);
                        break;
                }
                vm.Refresh();
                vm.Sort(SortType.Unordered);
               // vm.ClearSort(); 
            }

        }
    }
}
