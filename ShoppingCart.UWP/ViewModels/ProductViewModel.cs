using Library.ShoppingCart.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace ShoppingCart.UWP.ViewModels
{
    public class ProductViewModel : INotifyPropertyChanged
    {
        public string Name
        {
            get
            {
                return BoundProduct?.Name ?? string.Empty;
            }
            set
            {
                if (BoundProduct == null)
                {
                    return;
                }
                BoundProduct.Name = value;  
            }
        }
        public string Description
        {
            get
            {
                return BoundProduct?.Description ?? string.Empty;
            }
            set
            {
                if (BoundProduct == null)
                {
                    return;
                }
                BoundProduct.Description = value;
            }
        }
        public double Price
        {
            get
            {
                return BoundProduct?.Price ?? 0;
            }
            set
            {
                if (BoundProduct == null)
                {
                    return;
                }
                BoundProduct.Price = value;
            }
        }
        public virtual int Quantity
        {
            get
            {
                return BoundProduct?.Quantity ?? 0;
            }
            set
            {
                if (BoundProduct == null)
                {
                    return;
                }
                BoundProduct.Quantity = value;
            }
        }
        public virtual double TotalPrice
        {
            get
            {
                if (BoundProduct is ProductByWeight)
                {
                    if (BoundProduct.IsBogo)
                    {
                        if ((Math.Floor(Weight) % 2 == 0))
                        {
                            return (Price * (Math.Floor(Weight) / 2 + (Weight % 2)));
                        }
                        else if ((Math.Floor(Weight) - 1) % 2 == 0)
                        {
                            if ((Math.Floor(Weight) - 1) == 0)
                            {
                                return (Weight - 1) * Price;
                            }
                            return (Price * ((Math.Floor(Weight) - 1) / 2 + ((Weight - 1) % 2) + 1));
                        }
                        else
                        {
                            return Price;
                        }
                    }
                    return Weight * Price;
                }
                if (BoundProduct is ProductByQuantity)
                {
                    if (BoundProduct.IsBogo)
                    {
                        if (Quantity % 2 == 0)
                        {
                            return Price / 2 * Quantity;
                        }
                        else if ((Quantity % 2 == 1) && (Quantity > 1))
                        {
                            return (Price / 2 * (Quantity - 1) + Price);
                        }
                        else
                        {
                            return Price;
                        }
                    }
                    return Quantity * Price;
                }
                return 0;
            }
        }

        public virtual double Weight
        {
            get
            {
                return BoundProduct?.Weight ?? 0;
            }
            set
            {
                if (BoundProduct == null)
                {
                    return;
                }
                BoundProduct.Weight = value;
            }
        }
        public virtual bool IsBogo
        {
            get
            {
                return BoundProduct?.IsBogo ?? false;
            }
            set
            {
                if (BoundProduct == null)
                {
                    return;
                }
                BoundProduct.IsBogo = value;
            }
        }
        public int ID
        {
            get
            {
                return BoundProduct?.ID ?? 0;
            }
        }
        public int quantity { get; set; }
        public double weight { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public override string ToString()
        {
            return $"{ID} - {Name}: {Description}; ${Math.Round(Price, 2)}\n";
        }
        public Product BoundProduct
        {
            get
            {
                if (BoundQuantityProduct != null)
                {
                    return BoundQuantityProduct;
                }
                return BoundWeightProduct;
            }
        }
        public bool isQuantityProduct
        {
            get
            {
                return BoundQuantityProduct != null;
            }

            set
            {
                if (value)
                {
                    boundQuantityProduct = new ProductByQuantity();
                    boundWeightProduct = null;
                    NotifyPropertyChanged("IsWeightVisible");
                    NotifyPropertyChanged("IsQuantityVisible");
                }

            }
        }

        public bool isWeightProduct
        {
            get
            {
                return BoundWeightProduct != null;
            }

            set
            {
                if (value)
                {
                    boundWeightProduct = new ProductByWeight();
                    boundQuantityProduct = null;
                    NotifyPropertyChanged("IsWeightVisible");
                    NotifyPropertyChanged("IsQuantityVisible");
                }
            }
        }

        private ProductByQuantity boundQuantityProduct;
        public ProductByQuantity BoundQuantityProduct
        {
            get
            {
                return boundQuantityProduct;
            }
        }
        private ProductByWeight boundWeightProduct;
        public ProductByWeight BoundWeightProduct
        {
            get
            {
                return boundWeightProduct;
            }
        }
        public ProductViewModel()
        {
            boundQuantityProduct = new ProductByQuantity();
            boundWeightProduct = null;
        }
        public ProductViewModel(Product p)
        {
            if (p == null)
            {
                return;
            }
            if (p is ProductByQuantity)
            {
                boundQuantityProduct = p as ProductByQuantity;
            }
            else if (p is ProductByWeight)
            {
                boundWeightProduct = p as ProductByWeight;
            }
        }
        public Visibility isBoGoShow
        {
            get
            {
                return BoundProduct.IsBogo == true ? Visibility.Visible : Visibility.Collapsed;
            }
        }
        public Visibility IsQuantity
        {
            get
            {
                return isQuantityProduct == true ? Visibility.Visible : Visibility.Collapsed;
            }
        }
        public Visibility IsWeight
        {
            get
            {
                return isWeightProduct == true ? Visibility.Visible : Visibility.Collapsed;
            }
        }
        public Visibility IsQuantityVisible
        {
            get
            {
                return BoundQuantityProduct == null && BoundWeightProduct != null ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        public Visibility IsWeightVisible
        {
            get
            {
                return BoundWeightProduct == null && BoundQuantityProduct != null ? Visibility.Collapsed : Visibility.Visible;
            }
        }
    }
}
