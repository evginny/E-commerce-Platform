using Library.ShoppingCart.Services;
using ShoppingCart.UWP.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace ShoppingCart.UWP.Dialogs
{
    public sealed partial class ProductDialog : ContentDialog
    {
        public ProductDialog()
        {
            this.InitializeComponent();
            this.DataContext = new ProductViewModel();
        }
        public ProductDialog(ProductViewModel ivm)
        {
            this.InitializeComponent();
            this.DataContext = ivm;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            var viewModel = DataContext as ProductViewModel;
            InventoryService.Current.AddOrUpdate(viewModel.BoundProduct);
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
        private void isBoGoCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as ProductViewModel;
            viewModel.IsBogo = true;

        }

        private void isBoGoCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as ProductViewModel;
            viewModel.IsBogo = false;
        }
    }
}
