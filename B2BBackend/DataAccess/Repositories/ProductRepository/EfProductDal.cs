using Core.DataAccess.EntityFramework;
using DataAccess.Context.EntityFramework;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories.ProductRepository
{
    public class EfProductDal : EfEntityRepositoryBase<Product, SimpleContextDb>, IProductDal
    {
        public async Task<List<ProductListDto>> GetProductList(int customerId)
        {
            using (var context = new SimpleContextDb())
            {
                var customerRelationship = context.CustomerRelationships.Where(x => x.CustomerId == customerId).SingleOrDefault();
                var result = from product in context.Products
                             select new ProductListDto
                             {
                                 Id = product.Id,
                                 Name = product.Name,
                                 Discount = customerRelationship.Discount,
                                 Price = context.PriceListDetails.Where(x => x.PriceListId == customerRelationship.PriceListId && x.ProductId == product.Id).Count() > 0
                                 ?
                                 context.PriceListDetails.Where(x => x.PriceListId == customerRelationship.PriceListId && x.ProductId == product.Id).Select(x => x.Price).FirstOrDefault()
                                 : 0,
                             };
                return await result.OrderBy(x => x.Name).ToListAsync();
            }
        }
    }
}