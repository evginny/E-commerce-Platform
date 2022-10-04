using Library.ShoppingCart.Services;
using ShoppingCart.UWP.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace ShoppingCart.UWP.ViewModels
{
    public class CartViewModel : INotifyPropertyChanged
    {
        public string FileName { get; set; }
        public string Query { get; set; }
        public double Subtotal
        {
            get
            {
                double total = 0;
                if (cartService != null)
                {
                    for (int i = 0; i < cartService.CartProducts.Count; i++)
                    {
                        total += cartService.CartProducts[i].TotalPrice;
                    }
                }
                return total;
            }
        }
        public double TaxAmount
        {
            get
            {
                if (cartService != null)
                {
                    return Subtotal * 0.07;
                }
                return 0;
            }
        }
        public double Total
        {
            get
            {
                if (cartService != null)
                {
                    return Subtotal + TaxAmount;
                }
                return 0;
            }
        }
        public string Country { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int ZipCode { get; set; }
        public int CardNumber { get; set; }
        public string CardName { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public int SecurityCode { get; set; }
    

        public ProductViewModel SelectedProduct { get; set; }
        public string SelectedFileName { get; set; }

        private CartService cartService;
        public int sortType { get; set; }
        public bool isAscending { get; set; }
        public List<string> Files
        {
            get
            {
                return cartService.returnCartNames();
                //List<string> files = new List<string>();
                //DirectoryInfo d = new DirectoryInfo(CartService.persistPath);
                //var filesDir = d.GetFiles("*.json");

                //for (int i = 0; i < filesDir.Length; i++)
                //{
                //    files.Add(filesDir[i].Name);
                //}
                //return files;
            }
        }
        public string FileToDeletePath
        {
            get
            {
                return CartService.deletePath;
            }
        }
        public ObservableCollection<ProductViewModel> CartProducts
        {
            get
            {
                if (cartService == null)
                {
                    return new ObservableCollection<ProductViewModel>();
                }
                // filtered, sorted by name, ascending order
                if (!string.IsNullOrEmpty(Query) && sortType == 1 && isAscending)
                {
                    return new ObservableCollection<ProductViewModel>(
                        cartService.CartProducts.Where(p => p.Name.ToUpper().Contains(Query.ToUpper())
                            || p.Description.ToUpper().Contains(Query.ToUpper()))
                        .OrderBy(p => p.Name)
                        .Select(p => new ProductViewModel(p)));
                }
                // unfiltered, sorted by name, ascending order
                if (string.IsNullOrEmpty(Query) && sortType == 1 && isAscending)
                {
                    return new ObservableCollection<ProductViewModel>(
                        cartService.CartProducts.OrderBy(p => p.Name)
                        .Select(p => new ProductViewModel(p)));
                }
                //filtered, sorted by name, descending order
                if (!string.IsNullOrEmpty(Query) && sortType == 1 && !isAscending)
                {
                    return new ObservableCollection<ProductViewModel>(
                        cartService.CartProducts.Where(p => p.Name.ToUpper().Contains(Query.ToUpper())
                            || p.Description.ToUpper().Contains(Query.ToUpper()))
                        .OrderByDescending(p => p.Name)
                        .Select(p => new ProductViewModel(p)));
                }
                //unfiltered, sorted by name, descending order
                if (string.IsNullOrEmpty(Query) && sortType == 1 && !isAscending)
                {
                    return new ObservableCollection<ProductViewModel>(
                        cartService.CartProducts.OrderByDescending(p => p.Name)
                        .Select(p => new ProductViewModel(p)));
                }

                // filtered, sorted by total price, ascending order
                if (!string.IsNullOrEmpty(Query) && sortType == 2 && isAscending)
                {
                    return new ObservableCollection<ProductViewModel>(
                        cartService.CartProducts.Where(p => p.Name.ToUpper().Contains(Query.ToUpper())
                            || p.Description.ToUpper().Contains(Query.ToUpper()))
                        .OrderBy(p => p.TotalPrice)
                        .Select(p => new ProductViewModel(p)));
                }
                // unfiltered, sorted by total price, ascending order
                if (string.IsNullOrEmpty(Query) && sortType == 2 && isAscending)
                {
                    return new ObservableCollection<ProductViewModel>(
                        cartService.CartProducts.OrderBy(p => p.TotalPrice)
                        .Select(p => new ProductViewModel(p)));
                }
                //filtered, sorted by total price, descending order
                if (!string.IsNullOrEmpty(Query) && sortType == 2 && !isAscending)
                {
                    return new ObservableCollection<ProductViewModel>(
                        cartService.CartProducts.Where(p => p.Name.ToUpper().Contains(Query.ToUpper())
                            || p.Description.ToUpper().Contains(Query.ToUpper()))
                        .OrderByDescending(p => p.TotalPrice)
                        .Select(p => new ProductViewModel(p)));
                }
                //unfiltered, sorted by total price, descending order
                if (string.IsNullOrEmpty(Query) && sortType == 2 && !isAscending)
                {
                    return new ObservableCollection<ProductViewModel>(
                        cartService.CartProducts.OrderByDescending(p => p.TotalPrice)
                        .Select(p => new ProductViewModel(p)));
                }
                // unfiltered unsorted list
                if (string.IsNullOrEmpty(Query))
                {
                    return new ObservableCollection<ProductViewModel>(cartService.CartProducts.Select(p => new ProductViewModel(p)));
                }
                //filtered list
                else
                {
                    return new ObservableCollection<ProductViewModel>(
                        cartService.CartProducts.Where(p => p.Name.ToUpper().Contains(Query.ToUpper())
                        || p.Description.ToUpper().Contains(Query.ToUpper()))
                        .Select(p => new ProductViewModel(p)));
                }
            }
        }
        public CartViewModel()
        {
            cartService = CartService.Current;

        }
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task CheckOut()
        {
            ContentDialog diag = new CheckOutDialog();
            await diag.ShowAsync();
        }

        public async Task Remove()
        {
            if (SelectedProduct != null)
            {
                var id = SelectedProduct?.ID ?? -1;
                if (id >= 1)
                {
                    ContentDialog diag = new DeleteProductFromCartDialog(SelectedProduct);
                    await diag.ShowAsync();
                    NotifyPropertyChanged("CartProducts");
                }
            }
            
        }
        public void Sort(SortType st)
        {
            switch (st)
            {
                case SortType.ByNameAsc:
                    sortType = 1;
                    isAscending = true;
                    break;
                case SortType.ByNameDesc:
                    sortType = 1;
                    isAscending = false;
                    break;
                case SortType.ByTotalPriceAsc:
                    sortType = 2;
                    isAscending = true;
                    break;
                case SortType.ByTotalPriceDesc:
                    sortType = 2;
                    isAscending = false;
                    break;
                case SortType.Unordered:
                    sortType = 0;
                    break;
            }
        }
        public void Refresh()
        {
            NotifyPropertyChanged("CartProducts");
        }
        public async Task Save()
        {
            ContentDialog diag = new SaveCartDialog();
            await diag.ShowAsync();
        }
        public async Task Load()
        {
            ContentDialog diag = new LoadCartDialog();
            await diag.ShowAsync();
            NotifyPropertyChanged("CartProducts");
        }
        public void Delete()
        {
            if (FileToDeletePath != null)
            {
                File.Delete(FileToDeletePath);
            }

           // File.Delete(FileToDeletePath);           
        }
        public void RemoveProducts()
        {
            cartService.RemoveAllProducts();
        }
        public void RemoveProductsFromCart()
        {
            cartService.RemoveAllProducts();
            NotifyPropertyChanged("CartProducts");
        }

    }

}
