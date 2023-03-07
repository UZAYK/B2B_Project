using Business.Abstract;
using Business.Aspects.Secured;
using Business.Repositories.ProductImageRepository.Constants;
using Business.Repositories.ProductImageRepository.Validation;
using Core.Aspects.Caching;
using Core.Aspects.Performance;
using Core.Aspects.Transaction;
using Core.Aspects.Validation;
using Core.Utilities.Business;
using Core.Utilities.Result.Abstract;
using Core.Utilities.Result.Concrete;
using DataAccess.Repositories.ProductImageRepository;
using Entities.Concrete;
using Entities.Dtos;

namespace Business.Repositories.ProductImageRepository
{
    public class ProductImageManager : IProductImageService
    {
        private readonly IProductImageDal _productImageDal;
        private readonly IFileService _fileService;

        public ProductImageManager(IProductImageDal productImageDal, IFileService fileService)
        {
            _productImageDal = productImageDal;
            _fileService = fileService;
        }

        //[SecuredAspect()]
        [ValidationAspect(typeof(ProductImageValidator))]
        [RemoveCacheAspect("IProductImageService.Get")]
        public async Task<IResult> Add(ProductImageAddDto productImageModel)
        {
            foreach (var image in productImageModel.Image)
            {
                IResult result = BusinessRules.Run(CheckIfImageExtesionsAllow(image.FileName),
                                                   CheckIfImageSizeIsLessThanOneMb(productImageModel.Image.Length));
                if (result is null)
                {
                    string fileName = _fileService.FileSaveToServer(image, "./Content/img");
                    ProductImage productImage = new()
                    {
                        ImageUrl = fileName,
                        ProductId = productImageModel.ProductId
                    };
                    await _productImageDal.Add(productImage);
                }
                else
                    return new ErrorResult(ProductImageMessages.Error_Added);
            }
            return new SuccessResult(ProductImageMessages.Added);
        }

        //[SecuredAspect()]
        [ValidationAspect(typeof(ProductImageValidator))]
        [RemoveCacheAspect("IProductImageService.Get")]
        [TransactionAspect]
        public async Task<IResult> Update(ProductImageUpdateDto productUpdateImage)
        {
            IResult result = BusinessRules.Run(CheckIfImageExtesionsAllow(productUpdateImage.Image.FileName),
                                              CheckIfImageSizeIsLessThanOneMb(productUpdateImage.Image.Length));
            if (result is not null) return result;
            string path = @"./Content/img/" + productUpdateImage.ImageUrl; ;
            _fileService.FileDeleteToServer(path);

            string fileName = _fileService.FileSaveToServer(productUpdateImage.Image, "./Content/img/");
            var model = await _productImageDal.Get(x => x.Id == productUpdateImage.Id);
            model.ImageUrl = fileName;

            await _productImageDal.Update(model);
            return new SuccessResult(ProductImageMessages.Updated);
        }

        [SecuredAspect()]
        [RemoveCacheAspect("IProductImageService.Get")]
        public async Task<IResult> Delete(ProductImage productImage)
        {
            await _productImageDal.Delete(productImage);
            return new SuccessResult(ProductImageMessages.Deleted);
        }

        //[SecuredAspect()]
        [CacheAspect()]
        [PerformanceAspect()]
        public async Task<IDataResult<List<ProductImage>>> GetList()
        {
            return new SuccessDataResult<List<ProductImage>>(await _productImageDal.GetAll());
        }

        [SecuredAspect()]
        public async Task<IDataResult<ProductImage>> GetById(int id)
        {
            return new SuccessDataResult<ProductImage>(await _productImageDal.Get(p => p.Id == id));
        }

        private IResult CheckIfImageSizeIsLessThanOneMb(long ingSize)
        {
            decimal ingMbSize = Convert.ToDecimal(ingSize * 0.000001);
            if (ingMbSize > 5)
            {
                return new ErrorResult("Yüklediðiniz resmi boyutu en fazla 1mb olmalýdýr");
            }
            return new SuccessResult();
        }

        private IResult CheckIfImageExtesionsAllow(string fileName)
        {
            var ext = fileName.Substring(fileName.LastIndexOf('.'));
            var extension = ext.ToLower();
            List<string> AllowFileExtensions = new List<string> { ".jpg", ".jpeg", ".gif", ".png" };
            if (!AllowFileExtensions.Contains(extension))
            {
                return new ErrorResult("Eklediðiniz resim .jpg, jpeg, gif, .png türlerinden biri olmalýdýr!");
            }
            return new SuccessResult();
        }
    }
}