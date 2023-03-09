using Business.Aspects.Secured;
using Business.Repositories.BasketRepository;
using Business.Repositories.OrderDetailRepository;
using Business.Repositories.PriceListDetailRepository;
using Business.Repositories.ProductImageRepository;
using Business.Repositories.ProductRepository.Constants;
using Business.Repositories.ProductRepository.Validation;
using Core.Aspects.Caching;
using Core.Aspects.Performance;
using Core.Aspects.Validation;
using Core.Utilities.Business;
using Core.Utilities.Result.Abstract;
using Core.Utilities.Result.Concrete;
using DataAccess.Repositories.ProductRepository;
using Entities.Concrete;
using Entities.Dtos;

namespace Business.Repositories.ProductRepository
{
    public class ProductManager : IProductService
    {
        private readonly IProductDal _productDal;
        private readonly IProductImageService _productImageService;
        private readonly IPriceListDetailService _priceListDetailService;
        private readonly IBasketService _basketService;
        private readonly IOrderDetailService _orderDetailService;
        public ProductManager(IProductDal productDal, IProductImageService productImageService, IPriceListDetailService priceListDetailService, IBasketService basketService, IOrderDetailService orderDetailService)
        {
            _productDal = productDal;
            _productImageService = productImageService;
            _priceListDetailService = priceListDetailService;
            _basketService = basketService;
            _orderDetailService = orderDetailService;
        }

        ////[SecuredAspect("Admin,Product.add")]
        [ValidationAspect(typeof(ProductValidator))]
        [RemoveCacheAspect("IProductService.Get")]
        public async Task<IResult> Add(Product product)
        {
            await _productDal.Add(product);
            return new SuccessResult(ProductMessages.Added);
        }

        ////[SecuredAspect("Admin,Product.update")]
        [ValidationAspect(typeof(ProductValidator))]
        [RemoveCacheAspect("IProductService.Get")]
        public async Task<IResult> Update(Product product)
        {
            await _productDal.Update(product);
            return new SuccessResult(ProductMessages.Updated);
        }

        ////[SecuredAspect("Admin,Product.delete")]
        [RemoveCacheAspect("IProductService.Get")]
        public async Task<IResult> Delete(Product product)
        {
            IResult result = BusinessRules.Run(
             await CheckIfProductExistToBaskets(product.Id),
             await CheckIfProductExistToOrderDetails(product.Id));
            
            if (result is not null) return result;

            var images = await _productImageService.GetListByProductId(product.Id);
            images.ForEach(async x => { await _productImageService.Delete(x); });

            var productListProduct = await _priceListDetailService.GetListByProductId(product.Id);
            productListProduct.ForEach(async x => { await _priceListDetailService.Delete(x); });

            await _productDal.Delete(product);
            return new SuccessResult(ProductMessages.Deleted);
        }

        ////[SecuredAspect("Admin,Product.get")]
        //[CacheAspect()]
        [PerformanceAspect()]
        public async Task<IDataResult<List<Product>>> GetList()
        {
            return new SuccessDataResult<List<Product>>(await _productDal.GetAll());
        }

        ////[SecuredAspect("Admin,Product.get")]
        [CacheAspect()]
        [PerformanceAspect()]
        public async Task<IDataResult<List<ProductListDto>>> GetProductList(int customerId)
        {
            return new SuccessDataResult<List<ProductListDto>>(await _productDal.GetProductList(customerId));
        }

        ////[SecuredAspect("Admin,Product.get")]
        public async Task<IDataResult<Product>> GetById(int id)
        {
            return new SuccessDataResult<Product>(await _productDal.Get(p => p.Id == id));
        }
        public async Task<IResult> CheckIfProductExistToBaskets(int productId)
        {
            var result = await _basketService.GetListByProductId(productId);
            if (result.Count > 0)
                return new ErrorResult("Silmeye çalýþýlan ürün en az bir sepette bulunuyor");
            return new SuccessResult();
        }
        public async Task<IResult> CheckIfProductExistToOrderDetails(int productId)
        {
            var result = await _basketService.GetListByProductId(productId);
            if (result.Count > 0)
                return new ErrorResult("Silmeye çalýþýlan ürünün en az bir sipariþi var.");
            return new SuccessResult();
        }
    }
}