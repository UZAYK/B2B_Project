using Core.DataAccess;
using Entities.Concrete;

namespace DataAccess.Repositories.ProductRepository
{
    public interface IProductDal : IEntityRepository<Product>
    {
        Task<List<ProductListDto>> GetProductList(int customerId);
    }
}