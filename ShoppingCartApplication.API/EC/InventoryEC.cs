using Library.ShoppingCart.Models;
using ShoppingCartApplication.API.Database;

namespace ShoppingCartApplication.API.EC
{
    public class InventoryEC
    {
        public List<Product> Get()
        {
            //return FakeDatabase.Inventory;
            return Filebase.Current.Inventory;
        }
        public Product AddOrUpdate(Product prod)
        {
            //if (!FakeDatabase.Inventory.Any(p => p.ID == prod.ID))
            //if (prod.ID <= 0)
            //{
            //    prod.ID = FakeDatabase.NextId();
            //    if (prod is ProductByQuantity)
            //    {
            //        ProductByQuantity productByQuantity = (ProductByQuantity)prod;
            //        FakeDatabase.QuantityProducts.Add(productByQuantity);
            //    }
            //    else if (prod is ProductByWeight)
            //    {
            //        ProductByWeight productByWeight = (ProductByWeight)prod;
            //        FakeDatabase.WeightProducts.Add(productByWeight);
            //    }
            //    //FakeDatabase.Inventory.Add(prod);
            //}
            //else
            //{
            //    var productToUpdate = FakeDatabase.Inventory.FirstOrDefault(p => p.ID == prod.ID);
            //    if (productToUpdate is ProductByQuantity)
            //    {
            //        ProductByQuantity productToUpdateQuantity = (ProductByQuantity)productToUpdate;
            //        ProductByQuantity updatedProduct = (ProductByQuantity)prod;
            //        if (productToUpdateQuantity != null)
            //        {
            //            FakeDatabase.QuantityProducts.Remove(productToUpdateQuantity);
            //            FakeDatabase.QuantityProducts.Add(updatedProduct);
            //        }
                    
            //    }
            //    else if (productToUpdate is ProductByWeight)
            //        {
            //            ProductByWeight productToUpdateWeight = (ProductByWeight)productToUpdate;
            //            ProductByWeight updatedProduct = (ProductByWeight)prod;
            //            if (productToUpdateWeight != null)
            //            {
            //                FakeDatabase.WeightProducts.Remove(productToUpdateWeight);
            //                FakeDatabase.WeightProducts.Add(updatedProduct);
            //        }
            //    }
            //}
            return Filebase.Current.AddOrUpdate(prod);
        }

        public bool Delete(int id)
        {
            //var productToDelete = FakeDatabase.Inventory.FirstOrDefault(i => i.ID == id);
            //if (productToDelete != null)
            //{
            //    if ( productToDelete is ProductByQuantity)
            //    {
            //        var productToDeleteQuantity = productToDelete as ProductByQuantity;
            //        if ( productToDeleteQuantity != null)
            //        {
            //            FakeDatabase.QuantityProducts.Remove(productToDeleteQuantity);
            //        }
            //    }
            //    else if ( productToDelete is ProductByWeight)
            //    {
            //        var productToDeleteWeight = productToDelete as ProductByWeight;
            //        if (productToDeleteWeight != null)
            //        {
            //            FakeDatabase.WeightProducts.Remove(productToDeleteWeight);  
            //        }
            //    }

            //}

            return Filebase.Current.Delete(id);
        }

        public int RemoveUnits(Product prod)
        {
            int index = -1;
            for (int i = 0; i < FakeDatabase.QuantityProducts.Count; i++)
            {
                if (FakeDatabase.QuantityProducts[i].ID == prod.ID)
                {
                    index = i;
                }
            }
            if (prod is ProductByQuantity)
            {
                ProductByQuantity prodToRemove = (ProductByQuantity)prod;

                var inventoryProduct = FakeDatabase.QuantityProducts.FirstOrDefault(i => i.ID == prod.ID);
                if (inventoryProduct != null)
                {
                    if (inventoryProduct is ProductByQuantity)
                    {
                        ProductByQuantity invProd = (ProductByQuantity)inventoryProduct;
                        if (invProd.Quantity == prod.Quantity)
                        {
                            FakeDatabase.QuantityProducts.Remove(invProd);
                        }
                        else if (invProd.Quantity > prod.Quantity)
                        {
                            if (index != -1)
                            {
                                FakeDatabase.QuantityProducts[index].Quantity -= prod.Quantity;
                            }
                        }

                    }
                }
            }
            return prod.ID;
        }

        public int RemoveWeight(Product prod)
        {
            int index = -1;
            for (int i = 0; i < FakeDatabase.WeightProducts.Count; i++)
            {
                if (FakeDatabase.WeightProducts[i].ID == prod.ID)
                {
                    index = i;
                }
            }
            if (prod is ProductByWeight)
            {
                ProductByWeight prodToRemove = (ProductByWeight)prod;

                var inventoryProduct = FakeDatabase.WeightProducts.FirstOrDefault(i => i.ID == prod.ID);
                if (inventoryProduct != null)
                {
                    if (inventoryProduct is ProductByWeight)
                    {
                        ProductByWeight invProd = (ProductByWeight)inventoryProduct;
                        if (invProd.Weight == prod.Weight)
                        {
                            FakeDatabase.WeightProducts.Remove(invProd);
                        }
                        else if (invProd.Weight > prod.Weight)
                        {
                            if (index != -1)
                            {
                                FakeDatabase.WeightProducts[index].Weight -= prod.Weight;
                            }
                        }

                    }
                }
            }
            return prod.ID;
        }

        public int AddUnits(Product prod)
        {
            int index = -1;
            for (int i = 0; i < FakeDatabase.QuantityProducts.Count; i++)
            {
                if (FakeDatabase.QuantityProducts[i].ID == prod.ID)
                {
                    index = i;
                }
            }
            if (prod is ProductByQuantity)
            {
                ProductByQuantity prodToAdd = (ProductByQuantity)prod;

                var inventoryProduct = FakeDatabase.QuantityProducts.FirstOrDefault(i => i.ID == prod.ID);
                if (inventoryProduct != null)
                {
                    if (index != -1)
                    {
                        FakeDatabase.QuantityProducts[index].Quantity += prodToAdd.Quantity;
                    }
                }
                else
                {
                    FakeDatabase.QuantityProducts.Add(prodToAdd);
                }
            }
            return prod.ID;
        }

        public int AddWeight(Product prod)
        {
            int index = -1;
            for (int i = 0; i < FakeDatabase.WeightProducts.Count; i++)
            {
                if (FakeDatabase.WeightProducts[i].ID == prod.ID)
                {
                    index = i;
                }
            }
            if (prod is ProductByWeight)
            {
                ProductByWeight prodToAdd = (ProductByWeight)prod;

                var inventoryProduct = FakeDatabase.WeightProducts.FirstOrDefault(i => i.ID == prod.ID);
                if (inventoryProduct != null)
                {
                    if (index != -1)
                    {
                        FakeDatabase.WeightProducts[index].Weight += prodToAdd.Weight;
                    }
                }
                else
                {
                    FakeDatabase.WeightProducts.Add(prodToAdd);
                }
            }
            return prod.ID;

        }
    }
}
