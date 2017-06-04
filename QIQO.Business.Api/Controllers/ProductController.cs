using QIQO.Business.Client.Contracts;
using QIQO.Business.Client.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Mvc;
using QIQO.Business.Core;
using QIQO.Business.Services;
using QIQO.Business.ViewModels.Api;
using Microsoft.Extensions.Caching.Distributed;

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
        public JsonResult Get(int page = 0, int psize = 10, string orderby = "productName", string category = "all")
        {
            var route = "api/products";
            List<Product> prods;

            if (psize <= 0) psize = 10;

            try
            {
                if (!_memoryCache.TryGetValue(prodCacheKey, out List<ProductViewModel> pvms))
                {
                    var company = new Company() { CompanyKey = 1 };
                    pvms = new List<ProductViewModel>();

                    using (var proxy = _serviceFactory.CreateClient<IProductService>())
                    {
                        var products = proxy.GetProductsAsync(company);
                        prods = products.Result;
                    }

                    foreach (var prod in prods)
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

                var filteredQuery = baseQuery.Where(p => p.ProductType == (category == "all" ? p.ProductType : category)).ToList();

                var totalCount = filteredQuery.Count;
                var totalPages = Math.Ceiling((decimal)totalCount / psize);

                //var prevUrl = page > 0 ? _urlHelper.Link("Product", new { controller = "product", page = page - 1, orderby = orderby, category = category }) : "";
                //var nextUrl = page < totalPages - 1 ? _urlHelper.Link("Product", new { page = page + 1, orderby = orderby, category = category }) : "";
                var prevUrl = page > 0 ? $"{baseUrl}{route}?{nameof(category)}={category}&{nameof(page)}={page-1}&{nameof(orderby)}={orderby}" : "";
                var nextUrl = page <= totalPages - 1 ? $"{baseUrl}{route}?{nameof(category)}={category}&{nameof(page)}={page+1}&{nameof(orderby)}={orderby}" : "";

                var results = filteredQuery.Skip(psize * (page-1))
                                       .Take(psize)
                                       .ToList();

                return Json(new { TotalCount = totalCount, CurrentPage = page, TotalPages = totalPages,
                    Category = category, OrderBy = orderby, PageSize = psize,
                    PrevPageUrl = prevUrl, NextPageUrl = nextUrl, Results = results });
            }
            catch (Exception ex)
            {
                return Json(ex);
            }

            //List<Product> products = new List<Product>();
            //return Json(products);
        }

        [HttpGet("api/products/{product_key}")]
        public JsonResult Get(int product_key)
        {
            try
            {
                Product prod;

                using (var proxy = _serviceFactory.CreateClient<IProductService>())
                {
                    var product = proxy.GetProductAsync(product_key);
                    prod = product.Result;
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
        public JsonResult Post([FromBody] ProductViewModel product)
        {
            if (product != null)
            {
                Debug.WriteLine(product.ProductLongDesc);
                try
                {
                    var pvm = _entityService.Map(product);
                    using (var proxy = _serviceFactory.CreateClient<IProductService>())
                    {
                        var return_val = proxy.CreateProductAsync(pvm);
                        return Json(return_val.Result);
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
        public JsonResult Put([FromBody] ProductViewModel product)
        {
            //return Json(new Exception(null));
            if (product != null)
            {
                Debug.WriteLine(product.ProductShortDesc);
                return Post(product);
            }
            return Json(new BadRequestResult());
        }

        [HttpDelete("api/products/{product_key}")]
        public JsonResult Delete(int product_key)
        {
            try
            {
                var pvm = new Product() { ProductKey = product_key };
                
                using (var proxy = _serviceFactory.CreateClient<IProductService>())
                {
                    var return_val = proxy.DeleteProductAsync(pvm);
                    return Json(return_val.Result);
                }
            }
            catch (Exception ex)
            {
                return Json(ex);
            }
        }

        [HttpGet("api/products&q={q}")]
        public JsonResult Get(string q = "")
        {
            if (q == "") return Json(new List<AccountViewModel>());

            Debug.WriteLine(q);
            List<Product> prods;
            int psize = 100, page = 0;

            try
            {
                if (!_memoryCache.TryGetValue(prodCacheKey, out List<ProductViewModel> pvms))
                {
                    var company = new Company() { CompanyKey = 1 };
                    pvms = new List<ProductViewModel>();

                    using (var proxy = _serviceFactory.CreateClient<IProductService>())
                    {
                        var products = proxy.GetProductsAsync(company);
                        prods = products.Result;
                    }

                    foreach (var prod in prods)
                    {
                        pvms.Add(_entityService.Map(prod));
                    }
                    _memoryCache.Set(prodCacheKey, pvms, new MemoryCacheEntryOptions() { SlidingExpiration = new TimeSpan(0, 20, 0) });
                }

                //Debug.WriteLine(pvms.Count);
                var baseQuery = pvms.OrderBy(p => p.ProductName).ToList();
                //Debug.WriteLine(baseQuery.Count);
                var filteredQuery = baseQuery.Where(p => p.ProductName.ToLower().Contains(q.ToLower()) || p.ProductDesc.ToLower().Contains(q.ToLower())).ToList();
                //Debug.WriteLine(filteredQuery.Count);

                var totalCount = filteredQuery.Count;
                var totalPages = Math.Ceiling((decimal)totalCount / psize);

                //var prevUrl = page > 0 ? _urlHelper.Link("Product", new { controller = "product", page = page - 1, orderby = orderby, category = category }) : "";
                //var nextUrl = page < totalPages - 1 ? _urlHelper.Link("Product", new { page = page + 1, orderby = orderby, category = category }) : "";
                var prevUrl = ""; // page > 0 ? $"{baseUrl}{route}?{nameof(category)}={category}&{nameof(page)}={page - 1}&{nameof(orderby)}={orderby}" : "";
                var nextUrl = ""; // page < totalPages - 1 ? $"{baseUrl}{route}?{nameof(category)}={category}&{nameof(page)}={page + 1}&{nameof(orderby)}={orderby}" : "";

                var results = filteredQuery.Skip(psize * page)
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


        [HttpGet("api/products/recent")]
        public JsonResult GetRecent()
        {
            List<Product> prods;
            List<ProductViewModel> pvms;

            int psize = 100, page = 0;

            try
            {
                if (!_memoryCache.TryGetValue(prodCacheKey, out pvms))
                {
                    var company = new Company() { CompanyKey = 1 };
                    pvms = new List<ProductViewModel>();

                    using (var proxy = _serviceFactory.CreateClient<IProductService>())
                    {
                        var products = proxy.GetProductsAsync(company);
                        prods = products.Result;
                    }

                    foreach (var prod in prods)
                    {
                        pvms.Add(_entityService.Map(prod));
                    }
                    _memoryCache.Set(prodCacheKey, pvms, new MemoryCacheEntryOptions() { SlidingExpiration = new TimeSpan(0, 20, 0) });
                }

                //Debug.WriteLine(pvms.Count);
                var baseQuery = pvms.OrderBy(p => p.ProductName).ToList();
                //Debug.WriteLine(baseQuery.Count);
                var filteredQuery = baseQuery.Where(p => p.ProductLastUpdated >= DateTime.Now.AddDays(-90)).ToList();
                //Debug.WriteLine(filteredQuery.Count);

                var totalCount = filteredQuery.Count;
                var totalPages = Math.Ceiling((decimal)totalCount / psize);

                //var prevUrl = page > 0 ? _urlHelper.Link("Product", new { controller = "product", page = page - 1, orderby = orderby, category = category }) : "";
                //var nextUrl = page < totalPages - 1 ? _urlHelper.Link("Product", new { page = page + 1, orderby = orderby, category = category }) : "";
                var prevUrl = ""; // page > 0 ? $"{baseUrl}{route}?{nameof(category)}={category}&{nameof(page)}={page - 1}&{nameof(orderby)}={orderby}" : "";
                var nextUrl = ""; // page < totalPages - 1 ? $"{baseUrl}{route}?{nameof(category)}={category}&{nameof(page)}={page + 1}&{nameof(orderby)}={orderby}" : "";

                var results = filteredQuery.Skip(psize * page)
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
