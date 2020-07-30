using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Nop.Core.Domain.Catalog;
using Nop.Plugin.Api.Attributes;
using Nop.Plugin.Api.DTOs.Base;
using Nop.Plugin.Api.DTOs.Images;
using Nop.Plugin.Api.DTOs.Languages;
using Nop.Plugin.Api.DTOs.SpecificationAttributes;
using Nop.Plugin.Api.Validators;

namespace Nop.Plugin.Api.DTOs.Vendors
{
    [JsonObject(Title = "vendor")]
    public class VendorDto : BaseDto
    {
        private ICollection<VendorNoteDto> _vendorNotes;
        private ICollection<AddressDto> _addresses;
        private ImageDto _imageDto;

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the email
        /// </summary>
        [JsonProperty("email")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the description
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the parent picture identifier
        /// </summary>
        [JsonProperty("picture_id")]
        public int PictureId { get; set; }

        [JsonProperty("image")]
        public ImageDto Image
        {
            get
            {
                return _imageDto;
            }
            set
            {
                _imageDto = value;
            }
        }
        /// <summary>
        /// Gets or sets vendor notes
        /// </summary>
        [JsonProperty("notes")]
        public virtual ICollection<VendorNoteDto> VendorNotes
        {
            get
            {
                if (_vendorNotes == null)
                {
                    _vendorNotes = new List<VendorNoteDto>();
                }

                return _vendorNotes;
            }
            set { _vendorNotes = value; }
        }
        /// <summary>
        /// Gets or sets customer addresses
        /// </summary>
        [JsonProperty("addresses")]
        public ICollection<AddressDto> Addresses
        {
            get
            {
                if (_addresses == null)
                {
                    _addresses = new List<AddressDto>();
                }

                return _addresses;
            }
            set { _addresses = value; }
        }

        /// <summary>
        /// Gets or sets the admin comment
        /// </summary>
        [JsonProperty("admin_comment")]
        public string AdminComment { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity is active
        /// </summary>
        [JsonProperty("active")]
        public bool Active { get; set; }


        /// <summary>
        /// Gets or sets the display order
        /// </summary>
        [JsonProperty("display_order")]
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Gets or sets the meta keywords
        /// </summary>
        [JsonProperty("meta_keywords")]
        public string MetaKeywords { get; set; }

        /// <summary>
        /// Gets or sets the meta description
        /// </summary>
        [JsonProperty("meta_description")]
        public string MetaDescription { get; set; }

        /// <summary>
        /// Gets or sets the meta title
        /// </summary>
        [JsonProperty("meta_title")]
        public string MetaTitle { get; set; }

        /// <summary>
        /// Gets or sets the page size
        /// </summary>
        [JsonProperty("page_size")]
        public int PageSize { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether customers can select the page size
        /// </summary>
        [JsonProperty("allow_customers_to_select_pagesize")]
        public bool AllowCustomersToSelectPageSize { get; set; }

        /// <summary>
        /// Gets or sets the available customer selectable page size options
        /// </summary>
        [JsonProperty("pagesize_options")]
        public string PageSizeOptions
        {
            get; set;
        }
    }
}