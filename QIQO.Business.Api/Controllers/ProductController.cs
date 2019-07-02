using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using QIQO.Business.Client.Contracts;
using QIQO.Business.Client.Entities;
using QIQO.Business.Core;
using QIQO.Business.Services;
using QIQO.Business.ViewModels.Api;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace QIQO.Business.Api.Controllers
{
    public class ProductController : Controller
    {
        private readonly IServiceFactory _serviceFactory;
        private readonly IEntityService _entityService;
        private readonly IMemoryCache _memoryCache;

        private const string baseUrl = "http://localhost:34479/";
        private const string prodCacheKey = "ProductList";

        public ProductController(IServiceFactory serviceFactory, IEntityService entityService, IMemoryCache memoryCache)
        {
            _serviceFactory = serviceFactory;
            _entityService = entityService;
            _memoryCache = memoryCache;
        }

        [HttpGet("api/products")]
        public async Task<IActionResult> Get(int page = 0, int psize = 10, string orderby = "productName", string category = "all")
        {
            string route = "api/products";
            List<Product> prods;

            if (psize <= 0) psize = 10;

            try
            {
                if (!_memoryCache.TryGetValue(prodCacheKey, out List<ProductViewModel> pvms))
                {
                    Company company = new Company() { CompanyKey = 1 };
                    pvms = new List<ProductViewModel>();

                    using (IProductService proxy = _serviceFactory.CreateClient<IProductService>())
                    {
                        prods = await proxy.GetProductsAsync(company);
                    }

                    foreach (Product prod in prods)
                    {
                        pvms.Add(_entityService.Map(prod));
                    }
                    _memoryCache.Set(prodCacheKey, pvms, new MemoryCacheEntryOptions() { SlidingExpiration = new TimeSpan(0, 20, 0) });
                }
                IEnumerable<ProductViewModel> baseQuery;
                switch (orderby)
                {
                    case "productName":
                        baseQuery = pvms.OrderBy(p => p.ProductName);
                        break;
                    case "productType":
                        baseQuery = pvms.OrderBy(p => p.ProductType);
                        break;
                    case "priceAscending":
                        baseQuery = pvms.OrderBy(p => p.ProductBasePrice);
                        break;
                    case "priceDescending":
                        baseQuery = pvms.OrderByDescending(p => p.ProductBasePrice);
                        break;
                    default:
                        baseQuery = pvms.OrderBy(p => p.ProductName);
                        break;
                }

                List<ProductViewModel> filteredQuery = baseQuery.Where(p => p.ProductType == (category == "all" ? p.ProductType : category)).ToList();

                int totalCount = filteredQuery.Count;
                decimal totalPages = Math.Ceiling((decimal)totalCount / psize);

                //var prevUrl = page > 0 ? _urlHelper.Link("Product", new { controller = "product", page = page - 1, orderby = orderby, category = category }) : "";
                //var nextUrl = page < totalPages - 1 ? _urlHelper.Link("Product", new { page = page + 1, orderby = orderby, category = category }) : "";
                string prevUrl = page > 0 ? $"{baseUrl}{route}?{nameof(category)}={category}&{nameof(page)}={page - 1}&{nameof(orderby)}={orderby}" : "";
                string nextUrl = page <= totalPages - 1 ? $"{baseUrl}{route}?{nameof(category)}={category}&{nameof(page)}={page + 1}&{nameof(orderby)}={orderby}" : "";

                List<ProductViewModel> results = filteredQuery.Skip(psize * (page - 1))
                                       .Take(psize)
                                       .ToList();

                return Json(new
                {
                    TotalCount = totalCount,
                    CurrentPage = page,
                    TotalPages = totalPages,
                    Category = category,
                    OrderBy = orderby,
                    PageSize = psize,
                    PrevPageUrl = prevUrl,
                    NextPageUrl = nextUrl,
                    Results = results
                });
            }
            catch (Exception ex)
            {
                return Json(ex);
            }

            //List<Product> products = new List<Product>();
            //return Json(products);
        }

        [HttpGet("api/products/{product_key}")]
        public async Task<IActionResult> Get(int product_key)
        {
            try
            {
                Product prod;

                using (IProductService proxy = _serviceFactory.CreateClient<IProductService>())
                {
                    prod = await proxy.GetProductAsync(product_key);
                }

                Console.WriteLine(prod.ProductDesc);
                return Json(_entityService.Map(prod));
            }
            catch (Exception ex)
            {
                return Json(ex);
            }
        }

        //[Authorize]
        [HttpPost("api/products")]
        public async Task<IActionResult> Post([FromBody] ProductViewModel product)
        {
            if (product != null)
            {
                Debug.WriteLine(product.ProductLongDesc);
                try
                {
                    Product pvm = _entityService.Map(product);
                    using (IProductService proxy = _serviceFactory.CreateClient<IProductService>())
                    {
                        int return_val = await proxy.CreateProductAsync(pvm);
                        return Json(return_val);
                    }
                }
                catch (Exception ex)
                {
                    return Json(ex);
                }
            }
            return Json(new BadRequestResult());
        }

        [HttpPut("api/products")]
        public async Task<IActionResult> Put([FromBody] ProductViewModel product)
        {
            //return Json(new Exception(null));
            if (product != null)
            {
                Debug.WriteLine(product.ProductShortDesc);
                return await Post(product);
            }
            return Json(new BadRequestResult());
        }

        [HttpDelete("api/products/{product_key}")]
        public async Task<IActionResult> Delete(int product_key)
        {
            try
            {
                Product pvm = new Product() { ProductKey = product_key };

                using (IProductService proxy = _serviceFactory.CreateClient<IProductService>())
                {
                    bool return_val = await proxy.DeleteProductAsync(pvm);
                    return Json(return_val);
                }
            }
            catch (Exception ex)
            {
                return Json(ex);
            }
        }

        [HttpGet("api/products&q={q}")]
        public async Task<IActionResult> Get(string q = "")
        {
            if (q == "") return Json(new List<AccountViewModel>());

            Debug.WriteLine(q);
            List<Product> prods;
            int psize = 50, page = 0;
            string route = "api/products";
            string category = "all";
            string orderby = "productName";

            try
            {
                if (!_memoryCache.TryGetValue(prodCacheKey, out List<ProductViewModel> pvms))
                {
                    Company company = new Company() { CompanyKey = 1 };
                    pvms = new List<ProductViewModel>();

                    using (IProductService proxy = _serviceFactory.CreateClient<IProductService>())
                    {
                        prods = await proxy.GetProductsAsync(company);
                    }

                    foreach (Product prod in prods)
                    {
                        pvms.Add(_entityService.Map(prod));
                    }
                    _memoryCache.Set(prodCacheKey, pvms, new MemoryCacheEntryOptions() { SlidingExpiration = new TimeSpan(0, 20, 0) });
                }

                //Debug.WriteLine(pvms.Count);
                List<ProductViewModel> baseQuery = pvms.OrderBy(p => p.ProductName).ToList();
                //Debug.WriteLine(baseQuery.Count);
                List<ProductViewModel> filteredQuery = baseQuery.Where(p => p.ProductName.ToLower().Contains(q.ToLower()) || p.ProductDesc.ToLower().Contains(q.ToLower())).ToList();
                //Debug.WriteLine(filteredQuery.Count);

                int totalCount = filteredQuery.Count;
                decimal totalPages = Math.Ceiling((decimal)totalCount / psize);

                string prevUrl = page > 0 ? $"{baseUrl}{route}?{nameof(category)}={category}&{nameof(page)}={page - 1}&{nameof(orderby)}={orderby}" : "";
                string nextUrl = page <= totalPages - 1 ? $"{baseUrl}{route}?{nameof(category)}={category}&{nameof(page)}={page + 1}&{nameof(orderby)}={orderby}" : "";

                List<ProductViewModel> results = filteredQuery.Skip(psize * page)
                                       .Take(psize)
                                       .ToList();

                return Json(new
                {
                    TotalCount = totalCount,
                    CurrentPage = page,
                    TotalPages = totalPages,
                    Category = category,
                    OrderBy = orderby,
                    PageSize = psize,
                    PrevPageUrl = prevUrl,
                    NextPageUrl = nextUrl,
                    Results = results
                });
            }
            catch (Exception ex)
            {
                return Json(ex);
            }
        }


        [HttpGet("api/products/recent")]
        public async Task<IActionResult> GetRecent()
        {
            List<Product> prods;

            int psize = 100, page = 0;

            try
            {
                if (!_memoryCache.TryGetValue(prodCacheKey, out List<ProductViewModel> pvms))
                {
                    Company company = new Company() { CompanyKey = 1 };
                    pvms = new List<ProductViewModel>();

                    using (IProductService proxy = _serviceFactory.CreateClient<IProductService>())
                    {
                        prods = await proxy.GetProductsAsync(company);
                    }

                    foreach (Product prod in prods)
                    {
                        pvms.Add(_entityService.Map(prod));
                    }
                    _memoryCache.Set(prodCacheKey, pvms, new MemoryCacheEntryOptions() { SlidingExpiration = new TimeSpan(0, 20, 0) });
                }

                //Debug.WriteLine(pvms.Count);
                List<ProductViewModel> baseQuery = pvms.OrderBy(p => p.ProductName).ToList();
                //Debug.WriteLine(baseQuery.Count);
                List<ProductViewModel> filteredQuery = baseQuery.Where(p => p.ProductLastUpdated >= DateTime.Now.AddDays(-90)).ToList();
                //Debug.WriteLine(filteredQuery.Count);

                int totalCount = filteredQuery.Count;
                decimal totalPages = Math.Ceiling((decimal)totalCount / psize);

                //var prevUrl = page > 0 ? _urlHelper.Link("Product", new { controller = "product", page = page - 1, orderby = orderby, category = category }) : "";
                //var nextUrl = page < totalPages - 1 ? _urlHelper.Link("Product", new { page = page + 1, orderby = orderby, category = category }) : "";
                string prevUrl = "";
                string nextUrl = "";

                List<ProductViewModel> results = filteredQuery.Skip(psize * page)
                                       .Take(psize)
                                       .ToList();

                return Json(new
                {
                    TotalCount = totalCount,
                    CurrentPage = page,
                    TotalPages = totalPages,
                    Category = "",
                    OrderBy = "",
                    PageSize = psize,
                    PrevPageUrl = prevUrl,
                    NextPageUrl = nextUrl,
                    Results = results
                });
            }
            catch (Exception ex)
            {
                return Json(ex);
            }

            //List<Product> products = new List<Product>();
            //return Json(products);
        }
    }
}
