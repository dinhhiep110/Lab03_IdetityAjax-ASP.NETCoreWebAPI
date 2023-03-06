using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BusinessObjects;
using BusinessObjects.Dtos;
using DataAccess;

namespace Repositories
{
    public class ProductRepository : IProductRepository
    {

        public void SaveProduct(Product product)
        {
            ProductDAO.SaveProduct(product);
        }

        public void UpdateProduct(Product product)
        {
            ProductDAO.UpdateProduct(product);
        }

        public Product GetProductById(int id)
        {
            return ProductDAO.FindProductById(id);
        }

        public void DeleteProduct(Product product)
        {
            ProductDAO.DeleteProduct(product);
        }

        public List<Category> GetCategories()
        {
            return CategoryDAO.GetCategories();
        }
        public List<Product> GetProducts()
        {
            return ProductDAO.GetProducts();
        }
    }
}
