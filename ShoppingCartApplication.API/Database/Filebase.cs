using Library.ShoppingCart.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCartApplication.API.Database
{
    public class Filebase
    {
        private string _root;
        private string _quantityRoot;
        private string _weightRoot;
        private string _cartRoot;
        private string _cartQuantity;
        private string _cartWeight;
        private static Filebase _instance;


        public static Filebase Current
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new Filebase();
                }

                return _instance;
            }
        }

        private Filebase()
        {
            _root = "C:\\temp";
            _quantityRoot = $"{_root}\\ProductByQuantity";
            _weightRoot = $"{_root}\\ProductByWeight";
            _cartRoot = $"{_root}\\Carts";
            _cartQuantity = "Quantity";
            _cartWeight = "Weight";
        }

        public Product AddOrUpdate(Product prod)
        {
            //set up a new Id if one doesn't already exist
            if (prod.ID <= 0)
            {
                // get a new ID
                prod.ID = NextId();
            }

            //go to the right place]
            string path;
            if (prod is ProductByWeight)
            {
                path = $"{_weightRoot}/{prod.ID}.json";
            } else
            {
                path = $"{_quantityRoot}/{prod.ID}.json";
            }

            //if the item has been previously persisted
            if(File.Exists(path))
            {
                //blow it up
                File.Delete(path);
            }

            //write the file
            File.WriteAllText(path, JsonConvert.SerializeObject(prod));

            //return the item, which now has an id
            return prod;
        }

        public List<ProductByWeight> WeightProducts
        {
            get
            {
                var root = new DirectoryInfo(_weightRoot);
                var _todos = new List<ProductByWeight>();
                foreach(var weightFile in root.GetFiles())
                {
                    var weight = JsonConvert.DeserializeObject<ProductByWeight>(File.ReadAllText(weightFile.FullName));
                    _todos.Add(weight);
                }
                return _todos;
            }
        }

        public List<ProductByQuantity> QuantityProducts
        {
            get
            {
                var root = new DirectoryInfo(_quantityRoot);
                var _apps = new List<ProductByQuantity>();
                foreach (var quantFile in root.GetFiles())
                {
                    var quant = JsonConvert.DeserializeObject<ProductByQuantity>(File.ReadAllText(quantFile.FullName));
                    _apps.Add(quant);
                }
                return _apps;
            }
        }

        public List<Product> getCartProducts(string cartName)
        {
            //var rootQuant = new DirectoryInfo($"{_cartsRoot}\\{cartName}\\{_cartQuantity}");
            //var rootWeight = new DirectoryInfo($"{_cartsRoot}\\{cartName}\\{_cartWeight}");
            var quantityProducts = Current.getCartQuantityProducts(cartName);
            var weightProducts = Current.getCartWeightProducts(cartName);

            //foreach (var quantFile in rootQuant.GetFiles())
            //{
            //    var quant = JsonConvert.DeserializeObject<ProductByQuantity>(File.ReadAllText(quantFile.FullName));
            //    quantityProducts.Add(quant);
            //}
            //foreach (var weightFile in rootWeight.GetFiles())
            //{
            //    var weight = JsonConvert.DeserializeObject<ProductByWeight>(File.ReadAllText(weightFile.FullName));
            //    weightProducts.Add(weight);
            //}

            var products = new List<Product>();
            quantityProducts.ForEach(products.Add);
            weightProducts.ForEach(products.Add);

            return products;
         }

        public List<ProductByQuantity> getCartQuantityProducts(string cartName)
        {
            var quantityProducts = new List<ProductByQuantity>();
            var root = $"{_cartRoot}\\{cartName}\\{_cartQuantity}";
            if (Directory.Exists(root))
            {
                var rootQuant = new DirectoryInfo($"{_cartRoot}\\{cartName}\\{_cartQuantity}");
                foreach (var quantFile in rootQuant.GetFiles())
                {
                    var quant = JsonConvert.DeserializeObject<ProductByQuantity>(File.ReadAllText(quantFile.FullName));
                    quantityProducts.Add(quant);
                }
            }
            return quantityProducts;
        }
        public List<ProductByWeight> getCartWeightProducts(string cartName)
        {
            var weightProducts = new List<ProductByWeight>();
            var root = $"{_cartRoot}\\{cartName}\\{_cartWeight}";
            if (Directory.Exists(root))
            {
                var rootWeight = new DirectoryInfo($"{_cartRoot}\\{cartName}\\{_cartWeight}");
                foreach (var weightFile in rootWeight.GetFiles())
                {
                    var weight = JsonConvert.DeserializeObject<ProductByWeight>(File.ReadAllText(weightFile.FullName));
                    weightProducts.Add(weight);
                }
            }
            return weightProducts;
        }

        public bool Delete(int id)
        {
            //TODO: refer to AddOrUpdate for an idea of how you can implement this.
            var productToDelete = Inventory.FirstOrDefault(p => p.ID == id);
            if (productToDelete != null)
            {
                string path;
                if (productToDelete is ProductByWeight)
                {
                    path = $"{_weightRoot}/{productToDelete.ID}.json";
                }
                else
                {
                    path = $"{_quantityRoot}/{productToDelete.ID}.json";
                }

                //if the item has been previously persisted
                if (File.Exists(path))
                {
                    //blow it up
                    File.Delete(path);
                    return true;
                }
            }

            return false;
        }
        public List<Product> Inventory
        {
            get
            {
                var returnList = new List<Product>();
                QuantityProducts.ForEach(returnList.Add);
                WeightProducts.ForEach(returnList.Add);

                return returnList;
            }
        }
        public int NextId()
        {
            if (!Inventory.Any())
            {
                return 1;
            }
            return Inventory.Select(p => p.ID).Max() + 1;
        }
        //List<string> files = new List<string>();
        //DirectoryInfo d = new DirectoryInfo(CartService.persistPath);
        //var filesDir = d.GetFiles("*.json");

        //for (int i = 0; i < filesDir.Length; i++)
        //{
        //    files.Add(filesDir[i].Name);
        //}
        //return files;
        public Product AddToCart(string cartName, Product prod)
        {
            var root = $"{_cartRoot}\\{cartName}";
            Directory.CreateDirectory(root);
            Directory.CreateDirectory($"{root}//{_cartQuantity}");
            Directory.CreateDirectory($"{root}//{_cartWeight}");

            string path;
            if (prod is ProductByQuantity)
            {
                // add product to the cart or increase a product quantity
                ProductByQuantity cartProductToAdd = (ProductByQuantity)prod;
                var productToDeleteQuantity = new ProductByQuantity(prod);

                var existingProduct = Current.getCartQuantityProducts(cartName).FirstOrDefault(p => p.ID == prod.ID);
                if (existingProduct != null)
                {
                    cartProductToAdd.Quantity += existingProduct.Quantity;
                    path = $"{root}\\{_cartQuantity}/{existingProduct.ID}.json";
                    File.Delete(path);
                }
                else
                {
                    path = $"{root}\\{_cartQuantity}/{cartProductToAdd.ID}.json";
                }
                File.WriteAllText(path, JsonConvert.SerializeObject(cartProductToAdd));

                // remove product from the inventory or decrease its quantity

                var inventoryProduct = Current.QuantityProducts.FirstOrDefault(p => p.ID == prod.ID);
                if (inventoryProduct != null)
                {
                    path = $"{_quantityRoot}/{prod.ID}.json";
                    File.Delete(path);

                    if (productToDeleteQuantity.Quantity != inventoryProduct.Quantity)
                    {
                        inventoryProduct.Quantity -= productToDeleteQuantity.Quantity;
                        File.WriteAllText(path, JsonConvert.SerializeObject(inventoryProduct));
                    }
                }
            }
            else if (prod is ProductByWeight)
            {
                // add product to the cart or increase a product quantity
                ProductByWeight cartProductToAdd = (ProductByWeight)prod;
                var productToDeleteWeight = new ProductByWeight(prod);

                var existingProduct = Current.getCartWeightProducts(cartName).FirstOrDefault(p => p.ID == prod.ID);
                if (existingProduct != null)
                {
                    cartProductToAdd.Weight += existingProduct.Weight;
                    path = $"{root}\\{_cartWeight}/{existingProduct.ID}.json";
                    File.Delete(path);
                }
                else
                {
                    path = $"{root}\\{_cartWeight}/{cartProductToAdd.ID}.json";
                }
                File.WriteAllText(path, JsonConvert.SerializeObject(cartProductToAdd));

                // remove product from the inventory or decrease its quantity

                var inventoryProduct = Current.WeightProducts.FirstOrDefault(p => p.ID == prod.ID);
                if (inventoryProduct != null)
                {
                    path = $"{_weightRoot}/{prod.ID}.json";
                    File.Delete(path);

                    if (productToDeleteWeight.Weight != inventoryProduct.Weight)
                    {
                        inventoryProduct.Weight -= productToDeleteWeight.Weight;
                        File.WriteAllText(path, JsonConvert.SerializeObject(inventoryProduct));
                    }
                }
            }
            return prod;
        }
        public Product DeleteProductFromCart(string cartName, Product prod)
        {
            var root = $"{_cartRoot}\\{cartName}";
            string path;
            if (prod is ProductByQuantity)
            {
                // remove product from the cart or decrease its quantity
                ProductByQuantity cartProductToDelete = (ProductByQuantity)prod;
                ProductByQuantity inventoryProductToAdd = new ProductByQuantity(prod);
                var existingProduct = Current.getCartQuantityProducts(cartName).FirstOrDefault(p => p.ID == prod.ID);
                if (existingProduct != null)
                {
                    path = $"{root}\\{_cartQuantity}/{existingProduct.ID}.json";
                    File.Delete(path);

                    if (existingProduct.Quantity != cartProductToDelete.Quantity)
                    {
                        existingProduct.Quantity -= cartProductToDelete.Quantity;
                        File.WriteAllText(path, JsonConvert.SerializeObject(existingProduct));
                    }
                }

                var existingInvProd = Current.QuantityProducts.FirstOrDefault(p => p.ID == prod.ID);
                if (existingInvProd != null)
                {
                    inventoryProductToAdd.Quantity += existingInvProd.Quantity;
                }
                path = $"{_quantityRoot}/{prod.ID}.json";
                File.WriteAllText(path, JsonConvert.SerializeObject(inventoryProductToAdd));
            }
            else if (prod is ProductByWeight)
            {
                // remove product from the cart or decrease its quantity
                ProductByWeight cartProductToDelete = (ProductByWeight)prod;
                ProductByWeight inventoryProductToAdd = new ProductByWeight(prod);
                var existingProduct = Current.getCartWeightProducts(cartName).FirstOrDefault(p => p.ID == prod.ID);
                if (existingProduct != null)
                {
                    path = $"{root}\\{_cartWeight}/{existingProduct.ID}.json";
                    File.Delete(path);

                    if (existingProduct.Weight != cartProductToDelete.Weight)
                    {
                        existingProduct.Weight -= cartProductToDelete.Weight;
                        File.WriteAllText(path, JsonConvert.SerializeObject(existingProduct));
                    }
                }

                var existingInvProd = Current.WeightProducts.FirstOrDefault(p => p.ID == prod.ID);
                if (existingInvProd != null)
                {
                    inventoryProductToAdd.Weight += existingInvProd.Weight;
                }
                path = $"{_weightRoot}/{prod.ID}.json";
                File.WriteAllText(path, JsonConvert.SerializeObject(inventoryProductToAdd));
            }
            return prod;
        }

        public string SaveCart(string cartName)
        {
            var root = $"{_cartRoot}\\{cartName}";
            var rootToCurrentCart = $"{_cartRoot}\\CurrentCart";
            if (Directory.Exists(rootToCurrentCart))
            {
                if (!Directory.Exists(root))
                {
                    Directory.CreateDirectory($"{root}//{_cartQuantity}");
                    Directory.CreateDirectory($"{root}//{_cartWeight}");

                    //List<ProductByQuantity> tempQuant = Current.getCartQuantityProducts(cartName);
                    foreach (var qp in Current.getCartQuantityProducts("CurrentCart"))
                    {
                        string path = $"{root}\\{_cartQuantity}/{qp.ID}.json";
                        File.WriteAllText(path, JsonConvert.SerializeObject(qp));
                    }

                    foreach (var wp in Current.getCartWeightProducts("CurrentCart"))
                    {
                        string path = $"{root}\\{_cartWeight}/{wp.ID}.json";
                        File.WriteAllText(path, JsonConvert.SerializeObject(wp));
                    }
                }
            }
            return cartName;
        }

        public List<string> getCartNames()
        {
            DirectoryInfo d = new DirectoryInfo(_cartRoot);
            var folders = d.GetDirectories();
            List<string> names = new List<string>();


            for (int i = 0; i < folders.Length; i++)
            {
                names.Add(folders[i].Name);
            }
            if (names.Contains("CurrentCart"))
            {
                int index = names.IndexOf("CurrentCart");
                names.RemoveAt(index);
            }

            return names;


        }
    }
}
