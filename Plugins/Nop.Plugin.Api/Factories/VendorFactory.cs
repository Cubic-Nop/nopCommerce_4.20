using System;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Directory;
using Nop.Core.Domain.Vendors;
using Nop.Services.Directory;

namespace Nop.Plugin.Api.Factories
{
    public class VendorFactory : IFactory<Vendor>
    {
        private readonly IMeasureService _measureService;
        private readonly MeasureSettings _measureSettings;

        public VendorFactory(IMeasureService measureService, MeasureSettings measureSettings)
        {
            _measureService = measureService;
            _measureSettings = measureSettings;
        }

        public Vendor Initialize()
        {
            var defaultVendor = new Vendor()
            {
                Active = true,
                AdminComment = "",
                AllowCustomersToSelectPageSize = false,
                Description = "",
                DisplayOrder = 0,
                Deleted = false,
                Email = "",
                Id = 0,
                LimitedToStores = false,
                MetaDescription = "",
                MetaKeywords = "",
                MetaTitle = "",
                Name = "",
                PageSize = 6,
                PageSizeOptions = "6, 3, 9",
            };
            return defaultVendor;
        }
    }
}