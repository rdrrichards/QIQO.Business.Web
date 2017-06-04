using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
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
    }

    public class ResourceMember { }
}
