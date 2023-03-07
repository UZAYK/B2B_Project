using Core.Utilities.Result.Abstract;
using Entities.Concrete;

namespace Business.Repositories.ProductImageRepository
{
    public interface IProductImageService
    {
        Task<IResult> Add(ProductImageAddDto productImage);

        Task<IResult> Update(ProductImage productImage);

        Task<IResult> Delete(ProductImage productImage);

        Task<IDataResult<List<ProductImage>>> GetList();

        Task<IDataResult<ProductImage>> GetById(int id);
    }
}