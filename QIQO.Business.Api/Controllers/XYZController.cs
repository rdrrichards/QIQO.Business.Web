using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Concurrent;

namespace QIQO.Business.Api.Controllers
{
    public class XYZController : Controller {
        private readonly IMemoryCache _memoryCache;
        private const string xyzCacheKey = "XYZ";

        public XYZController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        [HttpPost("XYZ")]
        public IActionResult AddXYZ([FromBody]ResourceMember[] resourceMembers)
        {
            try
            {
                if (!_memoryCache.TryGetValue(xyzCacheKey, out ConcurrentQueue<Object> xyz))
                {
                    xyz = new ConcurrentQueue<Object>();
                    _memoryCache.Set(xyzCacheKey, xyz, new MemoryCacheEntryOptions()
                    {
                        SlidingExpiration = new TimeSpan(24, 0, 0)
                    });
                }
                xyz.Enqueue(resourceMembers);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("api/devices")]
        public IActionResult Get(string query)
        {
            //var name = query.DeviceName;
            return Json(query);
        }
    }

    public class ResourceMember { }
    public class Device { public string DeviceName { get; set; } }
}
