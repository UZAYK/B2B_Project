using Core.Utilities.Result.Abstract;
using Entities.Concrete;
using Entities.Dtos;

namespace Business.Repositories.ProductImageRepository
{
    public interface IProductImageService
    {
        Task<IResult> Add(ProductImageAddDto productImage);

        Task<IResult> Update(ProductImageUpdateDto productImage);

        Task<IResult> SetMainImage(int id);

        Task<IResult> Delete(ProductImage productImage);

        Task<IDataResult<List<ProductImage>>> GetList();

        Task<List<ProductImage>> GetListByProductId(int productId);

        Task<IDataResult<ProductImage>> GetById(int id);
    }
}