using Library.ShoppingCart.Models;
using Library.ShoppingCart.Services;
using ShoppingCart.UWP.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace ShoppingCart.UWP.ViewModels
{
    public class EmployeeViewModel : INotifyPropertyChanged
    {
        public string Query { get; set; }
        public int quantity { get; set; }
        public double weight { get; set; }
        public ProductViewModel SelectedProduct { get; set; }
        private InventoryService inventoryService;
        private int sortType { get; set; }
        private bool isAscending { get; set; }
        public ObservableCollection<ProductViewModel> InvProducts
        {
            get
            {
                // if the list is empty
                if (inventoryService == null)
                {
                    return new ObservableCollection<ProductViewModel>();
                }
                // filtered, sorted by name, ascending order
                if (!string.IsNullOrEmpty(Query) && sortType == 1 && isAscending)
                {
                    return new ObservableCollection<ProductViewModel>(
                        inventoryService.InvProducts.Where(p => p.Name.ToUpper().Contains(Query.ToUpper())
                            || p.Description.ToUpper().Contains(Query.ToUpper()))
                        .OrderBy(p => p.Name)
                        .Select(p => new ProductViewModel(p)));
                }
                // unfiltered, sorted by name, ascending order
                if (string.IsNullOrEmpty(Query) && sortType == 1 && isAscending)
                {
                    return new ObservableCollection<ProductViewModel>(
                        inventoryService.InvProducts.OrderBy(p => p.Name)
                        .Select(p => new ProductViewModel(p)));
                }
                //filtered, sorted by name, descending order
                if (!string.IsNullOrEmpty(Query) && sortType == 1 && !isAscending)
                {
                    return new ObservableCollection<ProductViewModel>(
                        inventoryService.InvProducts.Where(p => p.Name.ToUpper().Contains(Query.ToUpper())
                            || p.Description.ToUpper().Contains(Query.ToUpper()))
                        .OrderByDescending(p => p.Name)
                        .Select(p => new ProductViewModel(p)));
                }
                //unfiltered, sorted by name, descending order
                if (string.IsNullOrEmpty(Query) && sortType == 1 && !isAscending)
                {
                    return new ObservableCollection<ProductViewModel>(
                        inventoryService.InvProducts.OrderByDescending(p => p.Name)
                        .Select(p => new ProductViewModel(p)));
                }

                // filtered, sorted by price, ascending order
                if (!string.IsNullOrEmpty(Query) && sortType == 2 && isAscending)
                {
                    return new ObservableCollection<ProductViewModel>(
                        inventoryService.InvProducts.Where(p => p.Name.ToUpper().Contains(Query.ToUpper())
                            || p.Description.ToUpper().Contains(Query.ToUpper()))
                        .OrderBy(p => p.Price)
                        .Select(p => new ProductViewModel(p)));
                }
                // unfiltered, sorted by price, ascending order
                if (string.IsNullOrEmpty(Query) && sortType == 2 && isAscending)
                {
                    return new ObservableCollection<ProductViewModel>(
                        inventoryService.InvProducts.OrderBy(p => p.Price)
                        .Select(p => new ProductViewModel(p)));
                }
                //filtered, sorted by price, descending order
                if (!string.IsNullOrEmpty(Query) && sortType == 2 && !isAscending)
                {
                    return new ObservableCollection<ProductViewModel>(
                        inventoryService.InvProducts.Where(p => p.Name.ToUpper().Contains(Query.ToUpper())
                            || p.Description.ToUpper().Contains(Query.ToUpper()))
                        .OrderByDescending(p => p.Price)
                        .Select(p => new ProductViewModel(p)));
                }
                //unfiltered, sorted by price, descending order
                if (string.IsNullOrEmpty(Query) && sortType == 2 && !isAscending)
                {
                    return new ObservableCollection<ProductViewModel>(
                        inventoryService.InvProducts.OrderByDescending(p => p.Price)
                        .Select(p => new ProductViewModel(p)));
                }

                // unfiltered unsorted list
                if (string.IsNullOrEmpty(Query))
                {
                    return new ObservableCollection<ProductViewModel>(inventoryService.InvProducts.Select(p => new ProductViewModel(p)));
                }
                else
                {
                    // filtered list
                    return new ObservableCollection<ProductViewModel>(
                        inventoryService.InvProducts.Where(p => p.Name.ToUpper().Contains(Query.ToUpper())
                            || p.Description.ToUpper().Contains(Query.ToUpper()))
                        .Select(p => new ProductViewModel(p)));
                }

            }
        }

        public EmployeeViewModel()
        {
            inventoryService = InventoryService.Current;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task Add(ProductType pType)
        {
            Query = string.Empty;
            ContentDialog diag = null;
            if (pType == ProductType.QuantityProduct)
            {
                diag = new QuantityProductDialog();
            }
            else if (pType == ProductType.WeightProduct)
            {
                diag = new WeightProductDialog();
            }
            else if (pType == ProductType.Product)
            {
                diag = new ProductDialog();
            }
            else
            {
                throw new NotImplementedException();
            }

            await diag.ShowAsync();
            NotifyPropertyChanged("InvProducts");
        }

        public async Task AddProductCart()
        {
            if (SelectedProduct != null)
            {
                ContentDialog diag = new AddProductToCartDialog(SelectedProduct);
                await diag.ShowAsync();
                NotifyPropertyChanged("InvProducts");
            }
        }

        public void Remove()
        {
            var id = SelectedProduct?.ID ?? -1;
            if (id >= 1)
            {
                inventoryService.Delete(SelectedProduct.ID);
            }
            NotifyPropertyChanged("InvProducts");
        }

        public async Task Update()
        {
            if (SelectedProduct != null)
            {
                ContentDialog diag = new EditDialog(SelectedProduct);
                await diag.ShowAsync();
                NotifyPropertyChanged("InvProducts");
            }

        }

        public void Save()
        {
            inventoryService.Save();
        }

        public void Load()
        {
            inventoryService.Load();
            NotifyPropertyChanged("InvProducts");
        }

        public void Refresh()
        {
            NotifyPropertyChanged("InvProducts");
        }

        // sort by name 
        // sort type : 1 - by name, 2 - by price
   /*     public void Sort(int _sortType, bool _isAscending)
        {
            if (_sortType == 1 && _isAscending)
            {
                sortType = 1;
                isAscending = true;
            }
            else if (_sortType == 1 && !_isAscending)
            {
                sortType = 1;
                isAscending = false;
            }
            else if (_sortType == 2 && _isAscending)
            {
                sortType = 2;
                isAscending = true;
            }
            else if (_sortType == 2 && !_isAscending)
            {
                sortType = 2;
                isAscending = false;
            }
            else 
            {
                return;
            }
        }
   */
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
                case SortType.ByPriceAsc:
                    sortType = 2;
                    isAscending = true;
                    break;
                case SortType.ByPriceDesc:
                    sortType = 2;
                    isAscending = false;
                    break;
                case SortType.Unordered:
                    sortType = 0;
                    break;
            }
        }
        public void ClearSort()
        {
            sortType = 0;
        }
    }

    public enum ProductType
    {
        QuantityProduct, WeightProduct, Product
    }

    public enum SortType
    {
        ByNameAsc, ByNameDesc, ByPriceAsc, ByPriceDesc, ByTotalPriceAsc, ByTotalPriceDesc, Unordered
    }
}
