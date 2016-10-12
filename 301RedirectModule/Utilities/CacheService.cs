using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;

namespace SharedSource.RedirectModule.Utilities
{
    public class CacheService
    {
        private readonly Cache _cache;

        public CacheService()
        {
            _cache = HttpRuntime.Cache;
        }

        public Cache Cache
        {
            get { return _cache; }
        }
    }
}
