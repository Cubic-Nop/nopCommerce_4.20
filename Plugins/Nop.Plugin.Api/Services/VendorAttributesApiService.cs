using System.Collections.Generic;
using System.Linq;
using Nop.Core.Data;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Vendors;
using Nop.Plugin.Api.Constants;
using Nop.Plugin.Api.DataStructures;

namespace Nop.Plugin.Api.Services
{
    public class VendorAttributesApiService : IVendorAttributesApiService
    {
        private readonly IRepository<VendorAttribute> _VendorAttributesRepository;

        public VendorAttributesApiService(IRepository<VendorAttribute> VendorAttributesRepository)
        {
            _VendorAttributesRepository = VendorAttributesRepository;
        }

        public IList<VendorAttribute> GetVendorAttributes(int limit = Configurations.DefaultLimit,
             int page = Configurations.DefaultPageValue, int sinceId = Configurations.DefaultSinceId)
        {
            var query = GetVendorAttributesQuery(sinceId);

            return new ApiList<VendorAttribute>(query, page - 1, limit);
        }

        public int GetVendorAttributesCount()
        {
            return GetVendorAttributesQuery().Count();
        }

        VendorAttribute IVendorAttributesApiService.GetById(int id)
        {
            if (id <= 0)
                return null;

            return _VendorAttributesRepository.GetById(id);
        }

        private IQueryable<VendorAttribute> GetVendorAttributesQuery(int sinceId = Configurations.DefaultSinceId)
        {
            var query = _VendorAttributesRepository.Table;

            if (sinceId > 0)
            {
                query = query.Where(VendorAttribute => VendorAttribute.Id > sinceId);
            }

            query = query.OrderBy(VendorAttribute => VendorAttribute.Id);

            return query;
        }
    }
}