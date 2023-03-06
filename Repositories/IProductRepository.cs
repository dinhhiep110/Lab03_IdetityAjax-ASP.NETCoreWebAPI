using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using BusinessObjects.Dtos;

namespace Repositories
{
    public interface IProductRepository
    {
        void SaveProduct(Product product);

        Product GetProductById(int id);

        void DeleteProduct(Product product);

        List<Category> GetCategories();
        List<Product> GetProducts();
        void UpdateProduct(Product product);
    }
}
