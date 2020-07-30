using System.Collections.Generic;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Vendors;
using Nop.Plugin.Api.Constants;

namespace Nop.Plugin.Api.Services
{
    public interface IVendorAttributesApiService
    {
        IList<VendorAttribute> GetVendorAttributes(int limit = Configurations.DefaultLimit,
            int page = Configurations.DefaultPageValue, int sinceId = Configurations.DefaultSinceId);

        int GetVendorAttributesCount();

        VendorAttribute GetById(int id);
    }
}