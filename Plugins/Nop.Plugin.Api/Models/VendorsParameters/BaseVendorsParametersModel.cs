using System;
using Newtonsoft.Json;

namespace Nop.Plugin.Api.Models.VendorsParameters
{
    // JsonProperty is used only for swagger
    public class BaseVendorsParametersModel
    {
        public BaseVendorsParametersModel()
        {
            CreatedAtMin = null;
            CreatedAtMax = null;
            UpdatedAtMin = null;
            UpdatedAtMax = null;
            PublishedStatus = null;
            VendorName = null;
            CategoryId = null;
        }

        /// <summary>
        /// Show Vendors created after date (format: 2008-12-31 03:00)
        /// </summary>
        [JsonProperty("created_at_min")]
        public DateTime? CreatedAtMin { get; set; }

        /// <summary>
        /// Show Vendors created before date (format: 2008-12-31 03:00)
        /// </summary>
        [JsonProperty("created_at_max")]
        public DateTime? CreatedAtMax { get; set; }

        /// <summary>
        /// Show Vendors last updated after date (format: 2008-12-31 03:00)
        /// </summary>
        [JsonProperty("updated_at_min")]
        public DateTime? UpdatedAtMin { get; set; }

        /// <summary>
        /// Show Vendors last updated before date (format: 2008-12-31 03:00)
        /// </summary>
        [JsonProperty("updated_at_max")]
        public DateTime? UpdatedAtMax { get; set; }

        /// <summary>
        /// <ul>
        /// <li>published - Show only published Vendors</li>
        /// <li>unpublished - Show only unpublished Vendors</li>
        /// <li>any - Show all Vendors (default)</li>
        /// </ul>
        /// </summary>
        [JsonProperty("published_status")]
        public bool? PublishedStatus { get; set; }

        /// <summary>
        /// Filter by Vendor vendor
        /// </summary>
        [JsonProperty("vendor_name")]
        public string VendorName { get; set; }

        /// <summary>
        /// Show only the Vendors mapped to the specified category
        /// </summary>
        [JsonProperty("category_id")]
        public int? CategoryId { get; set; }
    }
}