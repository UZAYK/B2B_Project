using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Repositories.ProductImageRepository;
using Entities.Concrete;
using Business.Aspects.Secured;
using Core.Aspects.Validation;
using Core.Aspects.Caching;
using Core.Aspects.Performance;
using Business.Repositories.ProductImageRepository.Validation;
using Business.Repositories.ProductImageRepository.Constants;
using Core.Utilities.Result.Abstract;
using Core.Utilities.Result.Concrete;
using DataAccess.Repositories.ProductImageRepository;
using Business.Abstract;
using Core.Utilities.Business;
using Microsoft.Win32;

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
            IResult result = BusinessRules.Run(
CheckIfImageExtesionsAllow(productImageModel.Image.FileName),
CheckIfImageSizeIsLessThanOneMb(productImageModel.Image.Length)
);
            if (result != null)
                return result;
            string fileName = _fileService.FileSaveToServer(productImageModel.Image, "./Content/img");
            ProductImage productImage = new()
            {
                ImageUrl = fileName,
                ProductId = productImageModel.ProductId
            };

            await _productImageDal.Add(productImage);
            return new SuccessResult(ProductImageMessages.Added);
        }

        [SecuredAspect()]
        [ValidationAspect(typeof(ProductImageValidator))]
        [RemoveCacheAspect("IProductImageService.Get")]

        public async Task<IResult> Update(ProductImage productImage)
        {
            await _productImageDal.Update(productImage);
            return new SuccessResult(ProductImageMessages.Updated);
        }

        [SecuredAspect()]
        [RemoveCacheAspect("IProductImageService.Get")]

        public async Task<IResult> Delete(ProductImage productImage)
        {
            await _productImageDal.Delete(productImage);
            return new SuccessResult(ProductImageMessages.Deleted);
        }

        [SecuredAspect()]
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

            decimal ingMbSize = Convert.ToDecimal(ingSize + 0.000001);
            if (ingMbSize > 5)
            {
                return new ErrorResult("Y�kledi�iniz resmi boyutu en fazla 1mb olmal�d�r");
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
                return new ErrorResult("Ekledi�iniz resim .jpg, jpeg, gif, .png t�rlerinden biri olmal�d�r!");
            }
            return new SuccessResult();
        }
    }
}
