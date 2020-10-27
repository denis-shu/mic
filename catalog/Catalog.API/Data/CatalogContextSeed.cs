using Catalog.API.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.API.Data
{
    public class CatalogContextSeed
    {
        public static void SeedData(IMongoCollection<Product> prodCollection)
        {
            var isExist = prodCollection.Find(p => true).Any();
            if (!isExist)
            {
                prodCollection.InsertManyAsync(GetPreConfigProducts());
            }
        }

        private static IEnumerable<Product> GetPreConfigProducts()
        {
            return new List<Product>()
            {
                new Product()
                {
                    Name= "IPhone x",
                    Summary = "asd",
                    Description="asd",
                    ImageFile ="product-1.png",
                    Price = 980.00M,
                    Category = "Smart"

                },
                new Product()
                {
                    Name= "IPhone x1",
                    Summary = "asd1",
                    Description="asd1",
                    ImageFile ="product-2.png",
                    Price = 970.00M,
                    Category = "Smart"

                },
                new Product()
                {
                    Name= "IPhone x2",
                    Summary = "asd2",
                    Description="asd2",
                    ImageFile ="product-3.png",
                    Price = 960.00M,
                    Category = "Smart"

                },new Product()
                {
                    Name= "IPhone x3",
                    Summary = "asd3",
                    Description="asd3",
                    ImageFile ="product-4.png",
                    Price = 940.00M,
                    Category = "Smart1"

                }
            };
        }
    }
}
