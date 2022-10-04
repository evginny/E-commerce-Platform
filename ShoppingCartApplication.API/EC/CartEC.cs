using Library.ShoppingCart.Models;
using ShoppingCartApplication.API.Database;

namespace ShoppingCartApplication.API.EC
{
    public class CartEC
    {
        public List<Product> Get(string fileName)
        {

            //if (FakeDatabase.Carts.ContainsKey(fileName))
            //{

            //    return FakeDatabase.Carts[fileName];
            //}
            //return new List<Product>();
            return Filebase.Current.getCartProducts(fileName);
        }

        public Product Add(string fileName, Product prod)
        {
        //    int index = -1;
        //    if (prod is ProductByQuantity)
        //    {
            //    ProductByQuantity cartProdToAdd = (ProductByQuantity)prod;
            //    if (!FakeDatabase.Carts.ContainsKey(fileName))
            //    {
            //        FakeDatabase.Carts.Add(fileName, new List<Product>());
            //    }
                
            //    var cartProduct = FakeDatabase.Carts[fileName].FirstOrDefault(p => p.ID == prod.ID);
            //    for (int i = 0; i < FakeDatabase.Carts[fileName].Count; i++)
            //    {
            //        if (FakeDatabase.Carts[fileName][i].ID == prod.ID)
            //        {
            //            index = i;
            //        }
            //    }
            //    if (cartProduct != null && index != -1)
            //    {
            //        FakeDatabase.Carts[fileName][index].Quantity += prod.Quantity;
            //    }
            //    else
            //    {
            //        FakeDatabase.Carts[fileName].Add(cartProdToAdd);
            //    }

            //}
            //else if (prod is ProductByWeight)
            //{
            //    ProductByWeight cartProdToAdd = (ProductByWeight)prod;
            //    if (!FakeDatabase.Carts.ContainsKey(fileName))
            //    {
            //        FakeDatabase.Carts.Add(fileName, new List<Product>());
            //    }

            //    var cartProduct = FakeDatabase.Carts[fileName].FirstOrDefault(p => p.ID == prod.ID);
            //    for (int i = 0; i < FakeDatabase.Carts[fileName].Count; i++)
            //    {
            //        if (FakeDatabase.Carts[fileName][i].ID == prod.ID)
            //        {
            //            index = i;
            //        }
            //    }

            //    if (cartProduct != null && index != -1)
            //    {
            //        FakeDatabase.Carts[fileName][index].Weight += prod.Weight;
            //    }
            //    else
            //    {
            //        FakeDatabase.Carts[fileName].Add(cartProdToAdd);
                    
            //    }
            //}
            //return prod;
            return Filebase.Current.AddToCart(fileName, prod);
        }

        public Product Delete(String fileName, Product prod)
        {
            //int index = -1;

            //if (FakeDatabase.Carts.ContainsKey(fileName))
            //{
            //    var cartProduct = FakeDatabase.Carts[fileName].FirstOrDefault(p => p.ID == prod.ID);
            //    for (int i = 0; i < FakeDatabase.Carts[fileName].Count; i++)
            //    {
            //        if (FakeDatabase.Carts[fileName][i].ID == prod.ID)
            //        {
            //            index = i;
            //        }
            //    }
            //    if (prod is ProductByQuantity)
            //    {
            //        if (index != -1 && cartProduct != null)
            //        {
            //            if (prod.Quantity < cartProduct.Quantity)
            //            {
            //                FakeDatabase.Carts[fileName][index].Quantity -= prod.Quantity;
            //            }
            //            else
            //            {
            //                FakeDatabase.Carts[fileName].RemoveAt(index);
            //            }
            //        }
            //    }
            //    else if (prod is ProductByWeight)
            //    {
            //        if (index != -1 && cartProduct != null)
            //        {
            //            if (prod.Weight < cartProduct.Weight)
            //            {
            //                FakeDatabase.Carts[fileName][index].Weight -= prod.Weight;
            //            }
            //            else
            //            {
            //                FakeDatabase.Carts[fileName].RemoveAt(index);
            //            }
            //        }
            //    }
            //}
            //return prod;
            return Filebase.Current.DeleteProductFromCart(fileName, prod);
        }

        public string SaveCart(string fileName)
        {
            //if (FakeDatabase.Carts != null)
            //{
            //    //string lastKey = FakeDatabase.Carts.ElementAt(FakeDatabase.Carts.Count - 1).Key;
            //    if (FakeDatabase.Carts.ContainsKey("CurrentCart"))
            //    {
            //        if (!FakeDatabase.Carts.ContainsKey(fileName))
            //        {
            //            FakeDatabase.Carts.Add(fileName, new List<Product>());

            //            for (int i = 0; i < FakeDatabase.Carts["CurrentCart"].Count; i++)
            //            {
            //                if (FakeDatabase.Carts["CurrentCart"][i] is ProductByQuantity)
            //                {
            //                    FakeDatabase.Carts[fileName].Add( new ProductByQuantity((FakeDatabase.Carts["CurrentCart"][i])));
            //                }
            //                else if (FakeDatabase.Carts["CurrentCart"][i] is ProductByWeight)
            //                {
            //                    FakeDatabase.Carts[fileName].Add(new ProductByWeight((FakeDatabase.Carts["CurrentCart"][i])));
            //                }

            //            }

            //        }
            //    }
            //}
            //return fileName;
            return Filebase.Current.SaveCart(fileName);
        }

        public List<string> ReturnCartNames()
        {
            //return FakeDatabase.Carts.Keys.ToList();
            return Filebase.Current.getCartNames();
        }
    }
}
