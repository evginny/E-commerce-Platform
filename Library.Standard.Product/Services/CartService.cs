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
    public class CartService
    {
        public static string persistPath
            = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\Cart";
        public static string FileName { get; set; }
        
        private ListNavigator<Product> listNavigator;
        private InventoryService inventoryService;
        private List<Product> cartProducts;
        private ListNavigator<Product> listNavigatorProcessed;

        public static string deletePath { get; set; }
        public ListNavigator<Product> ListNavigatorProcessed
        {
            get
            {
                return listNavigatorProcessed;
            }
        }
        public List<Product> CartProducts
        {
            get
            {
                return cartProducts;
            }
        }
        public ListNavigator<Product> ListNavigator
        {
            get
            {
                return listNavigator;
            }
        }
        private static CartService current;
        public static CartService Current
        {
            get
            {
                if (current == null)
                {
                    current = new CartService();
                }
                return current;
            }
        }
        private CartService()
        {
            if (string.IsNullOrEmpty(FileName))
            {
                var listJson = new WebRequestHandler().Get("http://localhost:5211/Cart/CurrentCart").Result;
                cartProducts = JsonConvert.DeserializeObject<List<Product>>(listJson);
            }
            else
            {
                var listJson = new WebRequestHandler().Get($"http://localhost:5211/Cart/{FileName}").Result;
                cartProducts = JsonConvert.DeserializeObject<List<Product>>(listJson);
            }

            //cartProducts = new List<Product>();
            inventoryService = InventoryService.Current;
            listNavigator = new ListNavigator<Product>(cartProducts);
            listNavigatorProcessed = new ListNavigator<Product>(ProcessedList);
        }
        public int NextIdCart
        {
            get
            {
                if (!CartProducts.Any())
                {
                    return 1;
                }
                return CartProducts.Select(p => p.ID).Max() + 1;
            }
        }



        // creates a new product/adds an item to the cart list by removing this product
        // or an item from the inventory list
        public void CreateProductByQuantity(int id, int quant, string fileName = null)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                fileName = "CurrentCart";
            }
            FileName = fileName;

            if (quant <= 0)
            {
                return;
            }
            int cartIndex = -1;
            bool isDublicate = false;
            var invProduct = inventoryService.InvProducts.FirstOrDefault(p => p.ID == id);

            if (invProduct == null)
            {
                return;
            }

            for (int i = 0; i < cartProducts.Count; i++)
            {
                if (invProduct.Name?.ToUpper() == cartProducts[i].Name?.ToUpper()
                    &&
                    (invProduct.Description?.ToUpper() == cartProducts[i].Description?.ToUpper()))
                {
                    cartIndex = i;
                    isDublicate = true;
                }
            }

            Product productToAdd = new ProductByQuantity(invProduct);

            var serverCartProduct = new ProductByQuantity(productToAdd);
            serverCartProduct.Quantity = quant;



            if (invProduct.Quantity < quant)
            {
                quant = invProduct.Quantity;
                serverCartProduct.Quantity = quant;
            }

            var response = new WebRequestHandler().Post($"http://localhost:5211/Cart/Add/{fileName}", serverCartProduct).Result;

            if (isDublicate)
            {
                cartProducts[cartIndex].Quantity += quant;

            }
            else
            {
                productToAdd.Quantity = quant;
                productToAdd.ID = invProduct.ID;
                cartProducts.Add(productToAdd);
            }

            if (quant < invProduct.Quantity)
            {
                //var responseInv = new WebRequestHandler().Post("http://localhost:5211/Inventory/DeleteUnit", serverInventoryProduct).Result;
                inventoryService.RemoveUnit(invProduct.ID, quant);
            }
            else if (quant == invProduct.Quantity)
            {
                ////var responseInv = new WebRequestHandler().Get($"http://localhost:5211/Inventory/Delete/{serverInventoryProduct.ID}");
                inventoryService.Delete(invProduct.ID);
            }
        }

        public void CreateProductByWeight(int id, double weight, string fileName = null)
        {
            if (weight <= 0)
            {
                return;
            }
            if (string.IsNullOrEmpty(fileName))
            {
                fileName = "CurrentCart";
            }
            FileName = fileName;

            int cartIndex = -1;
            bool isDublicate = false;

            var invProduct = inventoryService.InvProducts.FirstOrDefault(p => p.ID == id);

            if (invProduct == null)
            {
                return;
            }

            for (int i = 0; i < cartProducts.Count; i++)
            {
                if (invProduct.Name?.ToUpper() == cartProducts[i].Name?.ToUpper()
                    &&
                    (invProduct.Description?.ToUpper() == cartProducts[i].Description?.ToUpper()))
                {
                    cartIndex = i;
                    isDublicate = true;
                }
            }

            Product productToAdd = new ProductByWeight(invProduct);

            var serverCartProduct = new ProductByWeight(productToAdd);
            serverCartProduct.Weight = weight;


            if (invProduct.Weight < weight)
            {
                weight = invProduct.Weight;
                serverCartProduct.Weight = weight;
            }

            var response = new WebRequestHandler().Post($"http://localhost:5211/Cart/Add/{fileName}", serverCartProduct).Result;


            if (isDublicate)
            {
                cartProducts[cartIndex].Weight += weight;
            }
            else
            {
                productToAdd.Weight = weight;
                productToAdd.ID = invProduct.ID;
                cartProducts.Add(productToAdd);
            }

            if (weight < invProduct.Weight)
            {
                //var responseInv = new WebRequestHandler().Post("http://localhost:5211/Inventory/DeleteWeight", serverInventoryProduct).Result;
                inventoryService.RemoveWeight(invProduct.ID, weight);
            }
            else if (weight == invProduct.Weight)
            {
                //var responseInv = new WebRequestHandler().Get($"http://localhost:5211/Inventory/Delete/{serverInventoryProduct.ID}");
                inventoryService.Delete(invProduct.ID);
            }
        }

        public void RemoveAllProducts()
        {
            cartProducts.Clear();
        }
        public void RemoveProductQuantity(int id, int quant, string fileName = null)
        {

            if (quant <= 0)
            {
                return;
            }
            if (string.IsNullOrEmpty(fileName))
            {
                fileName = "CurrentCart";
            }
            FileName = fileName;

            int invIndex = 0;
            bool isDublicate = false;
            var cartProduct = cartProducts.FirstOrDefault(p => p.ID == id);
            if (cartProduct == null)
            {
                return;
            }

            for (int i = 0; i < inventoryService.InvProducts.Count; i++)
            {
                if (cartProduct.Name?.ToUpper()
                    == inventoryService.InvProducts[i].Name?.ToUpper()
                    &&
                    (cartProduct.Description?.ToUpper()
                    == inventoryService.InvProducts[i].Description?.ToUpper()))
                {
                    isDublicate = true;
                    invIndex = i;
                }
            }

            var invProduct = new ProductByQuantity(cartProduct);

            var serverInventoryProduct = new ProductByQuantity(invProduct);
            serverInventoryProduct.Quantity = quant;

            if (cartProduct.Quantity < quant)
            {
                quant = cartProduct.Quantity;
                serverInventoryProduct.Quantity = quant;
            }
            var cartResponse = new WebRequestHandler().Post($"http://localhost:5211/Cart/Delete/{fileName}", serverInventoryProduct).Result;
            //var responseInv = new WebRequestHandler().Post("http://localhost:5211/Inventory/AddUnits", serverInventoryProduct).Result;

            if (isDublicate)
            {
                inventoryService.InvProducts[invIndex].Quantity += quant;
            }
            else
            {
                invProduct.Quantity = quant;
                inventoryService.AddOrUpdate(invProduct);
            }

           // var cartResponse = new WebRequestHandler().Post($"http://localhost:5211/Cart/Delete/{fileName}", serverInventoryProduct).Result;

            if (quant < cartProduct.Quantity)
            {
                cartProduct.Quantity -= quant;

            }
            else if (quant == cartProduct.Quantity)
            {
                cartProducts.Remove(cartProduct);
            }

        }
        public void RemoveProductWeight(int id, double weight, string fileName = null)
        {
            if (weight <= 0)
            {
                return;
            }

            if (string.IsNullOrEmpty(fileName))
            {
                fileName = "CurrentCart";
            }
            FileName = fileName;

            int invIndex = 0;
            bool isDublicate = false;
            var cartProduct = cartProducts.FirstOrDefault(p => p.ID == id);

            if (cartProduct == null)
            {
                return;
            }

            for (int i = 0; i < inventoryService.InvProducts.Count; i++)
            {
                if (cartProduct.Name?.ToUpper()
                    == inventoryService.InvProducts[i].Name?.ToUpper()
                    &&
                    (cartProduct.Description?.ToUpper()
                    == inventoryService.InvProducts[i].Description?.ToUpper()))
                {
                    isDublicate = true;
                    invIndex = i;
                }
            }

            var invProduct = new ProductByWeight(cartProduct);

            var serverInventoryProduct = new ProductByWeight(invProduct);
            serverInventoryProduct.Weight = weight;


            if (invProduct.Weight < weight)
            {
                weight = invProduct.Weight;
                serverInventoryProduct.Weight = weight;
            }
            var cartResponse = new WebRequestHandler().Post($"http://localhost:5211/Cart/Delete/{fileName}", serverInventoryProduct).Result;

            //var responseInv = new WebRequestHandler().Post("http://localhost:5211/Inventory/AddWeight", serverInventoryProduct).Result;

            if (isDublicate)
            {
                inventoryService.InvProducts[invIndex].Weight += weight;
            }
            else
            {
                invProduct.Weight = weight;
                inventoryService.AddOrUpdate(invProduct);
            }

            //var cartResponse = new WebRequestHandler().Post($"http://localhost:5211/Cart/Delete/{fileName}", serverInventoryProduct).Result;


            if (weight < cartProduct.Weight)
            {
                cartProduct.Weight -= weight;
            }
            else if (weight == cartProduct.Weight)
            {
                cartProducts.Remove(cartProduct);
            }
        }
        public void Load(string fileName = null)
        {
            if (fileName == null)
            {
                return;
            }

            var listJson = new WebRequestHandler().Get($"http://localhost:5211/Cart/{fileName}").Result;
            cartProducts = JsonConvert.DeserializeObject<List<Product>>(listJson);



            //if (string.IsNullOrEmpty(fileName))
            //{
            //    fileName = "CurrentCart";
            //}
            //FileName = fileName;

            //var prodJson = File.ReadAllText(fileName);
            //cartProducts = JsonConvert.DeserializeObject<List<Product>>
            //    (prodJson, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All })
            //    ?? new List<Product>();
            //deletePath = Path.Combine(persistPath, fileName);
        }
        public List<string> returnCartNames()
        {
            var response = new WebRequestHandler().Get("http://localhost:5211/Cart/GetCartNames").Result;
            return JsonConvert.DeserializeObject<List<String>>(response);
        }
        public void Save(string fileName = null)
        {
            var Response = new WebRequestHandler().Get("http://localhost:5211/Cart/GetCartNames").Result;
        

            if (string.IsNullOrEmpty(fileName))
            {
                fileName = "CurrentCart";
            }
            
            FileName = fileName;

            var cartResponse = new WebRequestHandler().Get($"http://localhost:5211/Cart/SaveCart/{fileName}").Result;


            //if (string.IsNullOrEmpty(fileName))
            //{
            //    fileName = $"{persistPath}\\SaveData.json";
            //}
            //else
            //{
            //    fileName = $"{persistPath}\\{fileName}.json";
            //}
            //var prodJson = JsonConvert.SerializeObject(cartProducts
            //    , new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
            //File.WriteAllText(fileName, prodJson);
            //deletePath = Path.Combine(persistPath, fileName);
        }

        public IEnumerable<Product> GetFilteredList(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return CartProducts;
            }
            return CartProducts.Where(p =>
                    (p?.Name?.ToUpper()?.Contains(query.ToUpper()) ?? false)
                    || (p?.Description?.ToUpper()?.Contains(query.ToUpper()) ?? false));
        }

        public Product RetriveProduct(int id)
        {
            var product = cartProducts.FirstOrDefault(p => p.ID == id);
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

            listNavigatorProcessed = new ListNavigator<Product>(ProcessedList);
            return ProcessedList;
        }
        // sort types: 1 - by name, 2 - by price, 3 - by total price
        public IEnumerable<Product> ProcessedList
        {
            get
            {
                // ordered by name / ascending / no filtered
                if (string.IsNullOrEmpty(query) && sort && sortType == 1 && isAscending)
                {
                    return cartProducts.OrderBy(p => p.Name);
                }

                // ordered by name / ascending / filtered 
                if (sort && sortType == 1 && isAscending)
                {
                    return cartProducts.Where(p =>
                        (p?.Name?.ToUpper()?.Contains(query.ToUpper()) ?? false)
                        || (p?.Description?.ToUpper()?.Contains(query.ToUpper()) ?? false))
                        .OrderBy(p => p.Name);
                }

                // ordered by name / descending / no filtered
                if (string.IsNullOrEmpty(query) && sort && sortType == 1 && !isAscending)
                {
                    return cartProducts.OrderByDescending(p => p.Name);
                }

                // ordered by name / descending / filtered
                if (sort && sortType == 1 && !isAscending)
                {
                    return cartProducts.Where(p =>
                        (p?.Name?.ToUpper()?.Contains(query.ToUpper()) ?? false)
                        || (p?.Description?.ToUpper()?.Contains(query.ToUpper()) ?? false))
                            .OrderByDescending(p => p.Name);
                }

                // ordered by total price / ascending / no filtered
                if (string.IsNullOrEmpty(query) && sort && sortType == 2 && isAscending)
                {
                    return cartProducts.OrderBy(p => p.TotalPrice);
                }

                // ordered by total price / ascending / filtered
                if (sort && sortType == 2 && isAscending)
                {
                    return cartProducts.Where(p =>
                        (p?.Name?.ToUpper()?.Contains(query.ToUpper()) ?? false)
                        || (p?.Description?.ToUpper()?.Contains(query.ToUpper()) ?? false))
                            .OrderBy(p => p.TotalPrice);
                }

                // ordered by total price / descending / no filtered
                if (string.IsNullOrEmpty(query) && sort && sortType == 2 && !isAscending)
                {
                    return cartProducts.OrderByDescending(p => p.TotalPrice);
                }

                // ordered by total price / descending / filtered
                if (sort && sortType == 2 && !isAscending)
                {
                    return cartProducts.Where(p =>
                        (p?.Name?.ToUpper()?.Contains(query.ToUpper()) ?? false)
                        || (p?.Description?.ToUpper()?.Contains(query.ToUpper()) ?? false))
                        .OrderByDescending(p => p.TotalPrice);
                }

                // unordered / filtered
                if (!string.IsNullOrEmpty(query) && !sort)
                {
                    return cartProducts.Where(p =>
                    (p?.Name?.ToUpper()?.Contains(query.ToUpper()) ?? false)
                    || (p?.Description?.ToUpper()?.Contains(query.ToUpper()) ?? false));
                }

                // unordered / no filtered
                return cartProducts;
            }
        }

    }
}
