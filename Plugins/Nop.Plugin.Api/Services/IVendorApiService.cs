using System;
using System.Collections.Generic;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Vendors;
using Nop.Plugin.Api.Constants;

namespace Nop.Plugin.Api.Services
{
    public interface IVendorApiService
    {
        IList<Vendor> GetVendors(IList<int> ids = null,
            DateTime? createdAtMin = null, DateTime? createdAtMax = null, DateTime? updatedAtMin = null, DateTime? updatedAtMax = null,
           int limit = Configurations.DefaultLimit, int page = Configurations.DefaultPageValue, int sinceId = Configurations.DefaultSinceId, 
           int? categoryId = null, string vendorName = null, bool? publishedStatus = null);

        int GetVendorsCount(DateTime? createdAtMin = null, DateTime? createdAtMax = null, 
            DateTime? updatedAtMin = null, DateTime? updatedAtMax = null, bool? publishedStatus = null, 
            string vendorName = null, int? categoryId = null);

        Vendor GetVendorById(int VendorId);

        Vendor GetVendorByIdNoTracking(int VendorId);
        Vendor GetVendorByName(string vendorName);
        //Vendor GetVendorBySKU(string sku);
    }
}