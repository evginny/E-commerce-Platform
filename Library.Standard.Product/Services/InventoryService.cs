using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.ShoppingCart.Models;
using Library.ShoppingCart.Utility;
using Library.Standard.Product.Utility;
using Newtonsoft.Json;

namespace Library.ShoppingCart.Services
{
    public class InventoryService
    {
        private string persistPath
            = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\Inventory";
        private ListNavigator<Product> listNavigatorProcessed;
        private List<Product> invProducts;
        private List<Product> invProcessedList;
        public IEnumerable<Product> invProductsProcessed
        {
            get
            {
                return ProcessedList;
            }
        }

        public List<Product> InvProducts
        {
            get
            {
                return invProducts;
            }
        }
        public ListNavigator<Product> ListNavigatorProcessed
        {
            get
            {
                return listNavigatorProcessed;
            }
        }

        private static InventoryService current;

        public static InventoryService Current
        {
            get
            {
                if (current == null)
                {
                    current = new InventoryService();
                }

                return current;
            }
        }

        private InventoryService()
        {
            var quantityJson = new WebRequestHandler().Get("http://localhost:5211/Inventory").Result;
            invProducts = JsonConvert.DeserializeObject<List<Product>>(quantityJson);

            listNavigatorProcessed = new ListNavigator<Product>(ProcessedList);

            if (!Directory.Exists($"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\Inventory"))
            {
                Directory.CreateDirectory($"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\Inventory");
            }
        }

        public void setIsBogo(Product prod, bool bogo)
        {
            prod.IsBogo = bogo;
        }

        // creates a new product with a new unique ID
        // if the user tries to create an alreay existing product
        // (with the same name, description, and price)
        // increments the quantity of that product without 
        // creating a totally new one 

        public int NextId
        {
            get
            {
                if (!InvProducts.Any())
                {
                    return 1;
                }
                return InvProducts.Select(p => p.ID).Max() + 1;
            }
        }
        public void AddOrUpdate(Product prod)
        {
            var response = new WebRequestHandler().Post("http://localhost:5211/Inventory/AddOrUpdate", prod).Result;
            var newProd = JsonConvert.DeserializeObject<Product>(response);

            var oldVersion = invProducts.FirstOrDefault(p => p.ID == newProd.ID);
            if (oldVersion != null)
            {
                var index = invProducts.IndexOf(oldVersion);
                invProducts.RemoveAt(index);
                invProducts.Insert(index, newProd);
            }
            else
            {
                invProducts.Add(newProd);
            }
        }


        // increments the quantity of the product by arbitrary number
        public void AddUnit(int id, int quantity) /////////////////////////////////////////////
        {
            Product productUnit = invProducts.FirstOrDefault(p => p.ID == id);
            if (productUnit == null)
            {
                return;
            }
            if (productUnit is ProductByWeight)
            {
                return;
            }

            productUnit.Quantity += quantity;
            //productUnit.TotalPrice = productUnit.Price * productUnit.Quantity;
        }

        // decrements the quantity of the product by arbitrary number
        // if there is only one item, deletes the whole product
        public void RemoveUnit(int id, int quant)
        {
            var unitToRemove = invProducts.FirstOrDefault(p => p.ID == id);
            if (unitToRemove == null)
            {
                return;
            }
            if ((unitToRemove.Quantity - quant) == 0)
            {
                invProducts.Remove(unitToRemove);
                return;
            }

            unitToRemove.Quantity -= quant;
        }
        public void RemoveWeight(int id, double weight)
        {
            var unitToRemove = invProducts.FirstOrDefault(p => p.ID == id);
            if (unitToRemove == null)
            {
                return;
            }
            if ((unitToRemove.Weight - weight) == 0)
            {
                invProducts.Remove(unitToRemove);
                return;
            }

            unitToRemove.Weight -= weight;
        }

        public void Delete(int id)
        {
            var productToDelete = invProducts.FirstOrDefault(p => p.ID == id);

            var response = new WebRequestHandler().Get($"http://localhost:5211/Inventory/Delete/{id}");
            if (productToDelete == null)
            {
                return;
            }
            InvProducts.Remove(productToDelete);
        }

        public IEnumerable<Product> GetFilteredList(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return InvProducts;
            }
            return InvProducts.Where(p =>
                    (p?.Name?.ToUpper()?.Contains(query.ToUpper()) ?? false)
                    || (p?.Description?.ToUpper()?.Contains(query.ToUpper()) ?? false));
        }

        public Product RetriveProduct(int id)
        {
            var product = invProducts.FirstOrDefault(p => p.ID == id);
            return product;
        }

        private string query;
        private bool sort;
        private int sortType;
        private bool isAscending;


        public IEnumerable<Product> ReadSearch(string query, bool sort, int sortType, bool isAscending)
        {
            this.query = query;
            this.sort = sort;
            this.sortType = sortType;
            this.isAscending = isAscending;

            //CM: added some dark magic
            listNavigatorProcessed = new ListNavigator<Product>(ProcessedList);
            return ProcessedList;
        }
        // sort types: 1 - by name, 2 - by price
        public IEnumerable<Product> ProcessedList
        {
            get
            {
                // ordered by name / ascending / no filtered
                if (string.IsNullOrEmpty(query) && sort && sortType == 1 && isAscending)
                {
                    return invProducts.OrderBy(p => p.Name);
                }

                // ordered by name / ascending / filtered 
                if (sort && sortType == 1 && isAscending)
                {
                    return invProducts.Where(p =>
                        (p?.Name?.ToUpper()?.Contains(query.ToUpper()) ?? false)
                        || (p?.Description?.ToUpper()?.Contains(query.ToUpper()) ?? false))
                        .OrderBy(p => p.Name);
                }

                // ordered by name / descending / no filtered
                if (string.IsNullOrEmpty(query) && sort && sortType == 1 && !isAscending)
                {
                    return invProducts.OrderByDescending(p => p.Name);
                }

                // ordered by name / descending / filtered
                if (sort && sortType == 1 && !isAscending)
                {
                    return invProducts.Where(p =>
                        (p?.Name?.ToUpper()?.Contains(query.ToUpper()) ?? false)
                        || (p?.Description?.ToUpper()?.Contains(query.ToUpper()) ?? false))
                            .OrderByDescending(p => p.Name);
                }

                // ordered by price / ascending / no filtered
                if (string.IsNullOrEmpty(query) && sort && sortType == 3 && isAscending)
                {
                    return InvProducts.OrderBy(p => p.Price);
                }

                // ordered by price / ascending / filtered
                if (sort && sortType == 3 && isAscending)
                {
                    return invProducts.Where(p =>
                         (p?.Name?.ToUpper()?.Contains(query.ToUpper()) ?? false)
                         || (p?.Description?.ToUpper()?.Contains(query.ToUpper()) ?? false))
                                .OrderBy(p => p.Price);
                }

                // oredered by price / descending / no filtered
                if (string.IsNullOrEmpty(query) && sort && sortType == 3 && !isAscending)
                {
                    return invProducts.OrderByDescending(p => p.Price);
                }

                // orederd by price / descending / filtered
                if (sort && sortType == 3 && !isAscending)
                {
                    return invProducts.Where(p =>
                        (p?.Name?.ToUpper()?.Contains(query.ToUpper()) ?? false)
                        || (p?.Description?.ToUpper()?.Contains(query.ToUpper()) ?? false))
                            .OrderByDescending(p => p.Price);
                }


                // unordered / filtered
                if (!string.IsNullOrEmpty(query) && !sort)
                {
                    return invProducts.Where(p =>
                        (p?.Name?.ToUpper()?.Contains(query.ToUpper()) ?? false)
                        || (p?.Description?.ToUpper()?.Contains(query.ToUpper()) ?? false));
                }

                // unordered / no filtered
                return invProducts;
            }
        }
        public void Load(string fileName = null)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                fileName = $"{persistPath}\\SaveData.json";
            }
            else
            {
                fileName = $"{persistPath}\\{fileName}.json";
            }
            var prodJson = File.ReadAllText(fileName);
            invProducts = JsonConvert.DeserializeObject<List<Product>>
                (prodJson, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All })
                ?? new List<Product>();
        }
        public void Save(string fileName = null)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                fileName = $"{persistPath}\\SaveData.json";
            }
            else
            {
                fileName = $"{persistPath}\\{fileName}.json";
            }
            var prodJson = JsonConvert.SerializeObject(invProducts
                , new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
            File.WriteAllText(fileName, prodJson);
        }
    }
}
