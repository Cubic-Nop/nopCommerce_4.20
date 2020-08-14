using System.Collections.Generic;
using Newtonsoft.Json;
using Nop.Plugin.Api.DTOs.Base;

namespace Nop.Plugin.Api.DTOs.ProductCategoryMappings
{
    [JsonObject(Title = "product_category_mapping")]
    public class ProductCategoryMappingDto : BaseDto
    {
        public ProductCategoryMappingDto()
        {
            Categories = new List<int>();
        }
        [JsonProperty("categories")]
        public List<int> Categories { get; set; }
        /// <summary>
        /// Gets or sets the SKU
        /// </summary>
        [JsonProperty("product_sku")]
        public string ProductSku { get; set; }
        /// <summary>
        /// Gets or sets the product identifier
        /// </summary>
        [JsonProperty("product_id")]
        public int? ProductId { get; set; }
        /// <summary>
        /// Gets or sets the category identifier in 365
        /// </summary>
        [JsonProperty("category_id_dynamics")]
        public int? CategoryIdDynamics { get; set; }
        /// <summary>
        /// Gets or sets the category identifier
        /// </summary>
        [JsonProperty("category_id")]
        public int? CategoryId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the product is featured
        /// </summary>
        [JsonProperty("is_featured_product")]
        public bool? IsFeaturedProduct { get; set; }

        /// <summary>
        /// Gets or sets the display order
        /// </summary>
        [JsonProperty("display_order")]
        public int? DisplayOrder { get; set; }
    }
}