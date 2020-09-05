using System;
using System.Collections.Generic;
using System.Linq;
using Nop.Core.Data;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Vendors;
using Nop.Plugin.Api.Constants;
using Nop.Plugin.Api.DataStructures;
using Nop.Services.Stores;

namespace Nop.Plugin.Api.Services
{
    public class VendorApiService : IVendorApiService
    {
        private readonly IStoreMappingService _storeMappingService;
        private readonly IRepository<Vendor> _vendorRepository;

        public VendorApiService(
            IRepository<Vendor> vendorRepository,
            IStoreMappingService storeMappingService)
        {
            _vendorRepository = vendorRepository;
            _storeMappingService = storeMappingService;
        }

        public IList<Vendor> GetVendors(IList<int> ids = null,
            DateTime? createdAtMin = null, DateTime? createdAtMax = null, DateTime? updatedAtMin = null, DateTime? updatedAtMax = null,
           int limit = Configurations.DefaultLimit, int page = Configurations.DefaultPageValue, int sinceId = Configurations.DefaultSinceId,
           int? categoryId = null, string vendorName = null, bool? publishedStatus = null)
        {
            var query = GetVendorsQuery(createdAtMin, createdAtMax, updatedAtMin, updatedAtMax, vendorName, publishedStatus, ids, categoryId);

            if (sinceId > 0)
            {
                query = query.Where(c => c.Id > sinceId);
            }

            return new ApiList<Vendor>(query, page - 1, limit);
        }

        public int GetVendorsCount(DateTime? createdAtMin = null, DateTime? createdAtMax = null,
            DateTime? updatedAtMin = null, DateTime? updatedAtMax = null, bool? publishedStatus = null, string vendorName = null,
            int? categoryId = null)
        {
            var query = GetVendorsQuery(createdAtMin, createdAtMax, updatedAtMin, updatedAtMax, vendorName,
                                         publishedStatus, categoryId: categoryId);

            return query.ToList().Count(p => _storeMappingService.Authorize(p));
        }

        public Vendor GetVendorById(int vendorId)
        {
            if (vendorId == 0)
                return null;

            return _vendorRepository.Table.FirstOrDefault(vendor => vendor.Id == vendorId && !vendor.Deleted);
        }
        //public Vendor GetVendorBySKU(string id)
        //{
        //    if (string.IsNullOrEmpty(id))
        //        return null;

        //    return _vendorRepository.Table.FirstOrDefault(Vendor => Vendor.Sku == id && !Vendor.Deleted);
        //}
        public Vendor GetVendorByIdNoTracking(int VendorId)
        {
            if (VendorId == 0)
                return null;

            return _vendorRepository.Table.FirstOrDefault(Vendor => Vendor.Id == VendorId && !Vendor.Deleted);
        }

        private IQueryable<Vendor> GetVendorsQuery(DateTime? createdAtMin = null, DateTime? createdAtMax = null,
            DateTime? updatedAtMin = null, DateTime? updatedAtMax = null, string vendorName = null,
            bool? publishedStatus = null, IList<int> ids = null, int? categoryId = null)

        {
            var query = _vendorRepository.Table;

            if (ids != null && ids.Count > 0)
            {
                query = query.Where(c => ids.Contains(c.Id));
            }



            // always return Vendors that are not deleted!!!
            query = query.Where(c => !c.Deleted);



            if (!string.IsNullOrEmpty(vendorName))
            {
                query = from vendor in _vendorRepository.Table
                        where vendor.Name == vendorName && !vendor.Deleted && vendor.Active
                        select vendor;
            }

            query = query.OrderBy(Vendor => Vendor.Id);

            return query;
        }

        public Vendor GetVendorByName(string vendorName)
        {
            if (string.IsNullOrEmpty(vendorName))
                return null;

            return _vendorRepository.Table.FirstOrDefault(vendor => vendor.Name == vendorName && !vendor.Deleted);
        }
    }
}