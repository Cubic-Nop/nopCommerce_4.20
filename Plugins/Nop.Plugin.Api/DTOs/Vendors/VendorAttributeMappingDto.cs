using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Nop.Core.Domain.Catalog;
using Nop.Plugin.Api.DTOs.Base;

namespace Nop.Plugin.Api.DTOs.Vendors
{
    [JsonObject(Title = "attribute")]
    //[Validator(typeof(VendorDtoValidator))]
    public class VendorAttributeMappingDto : BaseDto
    {
        private List<VendorAttributeValueDto> _VendorAttributeValues;

        /// <summary>
        /// Gets or sets the Vendor attribute identifier
        /// </summary>
        [JsonProperty("Vendor_attribute_id")]
        public int VendorAttributeId { get; set; }

        [JsonProperty("Vendor_attribute_name")]
        public string VendorAttributeName { get; set; }

        /// <summary>
        /// Gets or sets a value a text prompt
        /// </summary>
        [JsonProperty("text_prompt")]
        public string TextPrompt { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity is required
        /// </summary>
        [JsonProperty("is_required")]
        public bool IsRequired { get; set; }

        /// <summary>
        /// Gets or sets the attribute control type identifier
        /// </summary>
        [JsonProperty("attribute_control_type_id")]
        public int AttributeControlTypeId { get; set; }

        /// <summary>
        /// Gets or sets the display order
        /// </summary>
        [JsonProperty("display_order")]
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Gets or sets the default value (for textbox and multiline textbox)
        /// </summary>
        [JsonProperty("default_value")]
        public string DefaultValue { get; set; }

        /// <summary>
        /// Gets the attribute control type
        /// </summary>
        [JsonProperty("attribute_control_type_name")]
        public string AttributeControlType
        {
            get
            {
                return ((AttributeControlType)this.AttributeControlTypeId).ToString();
            }
            set
            {
                AttributeControlType attributeControlTypeId;
                if (Enum.TryParse(value, out attributeControlTypeId))
                {
                    this.AttributeControlTypeId = (int)attributeControlTypeId;
                }
            }
        }

        /// <summary>
        /// Gets the Vendor attribute values
        /// </summary>
        [JsonProperty("attribute_values")]
        public List<VendorAttributeValueDto> VendorAttributeValues
        {
            get { return _VendorAttributeValues; }
            set { _VendorAttributeValues = value; }
        }
    }
}