﻿namespace Etk.Tests.Data.Shops
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Xml.Serialization;
    using Etk.Tests.Data.Shops.DataType;
    
    public static class ProductsManager
    {
        #region attributes and properties
        static private ProductList productList;
        #endregion

        #region .ctors
        static ProductsManager()
        {
            CreateDefaultData();
        }
        #endregion

        #region public methods
        /// <summary>Return aall managed orders</summary>
        public static IEnumerable<Product> GetAllProducts()
        {
            if (productList == null || productList.Products == null)
                return null;
            return productList.Products;
        }

        /// <summary>Return an product given its id</summary>
        /// <param name="id">Product id to retrieve</param>
        public static Product GetProduct(int id)
        {
            if (productList == null && productList.Products != null)
                return null;
            return productList.Products.FirstOrDefault(o => o.Id == id);
        }

        /// <summary>Return a list of specific products</summary>
        /// <param name="ids">the product ids to retrieve</param>
        public static IEnumerable<Product> GetProducts(IEnumerable<int> ids)
        {
            if (productList == null && productList.Products != null)
                return null;
            if (ids == null || !ids.Any())
                return null;
            return productList.Products.Where(o => ids.Contains(o.Id));
        }
        #endregion

        #region private methods
        static private void CreateDefaultData()
        {
            XmlSerializer xs = new XmlSerializer(typeof(ProductList));
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Etk.Tests.Data.Shops.Data.Products.xml"))
            {
                productList = xs.Deserialize(stream) as ProductList;
            }
        }
        #endregion
    }
}