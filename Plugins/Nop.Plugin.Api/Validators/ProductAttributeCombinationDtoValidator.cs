﻿using Microsoft.AspNetCore.Http;
using Nop.Plugin.Api.DTOs.Products;
using Nop.Plugin.Api.Helpers;
using System.Collections.Generic;

namespace Nop.Plugin.Api.Validators
{
    public class ProductAttributeCombinationDtoValidator : BaseDtoValidator<VendorAttributeCombinationDto>
    {

        #region Constructors

        public ProductAttributeCombinationDtoValidator(IHttpContextAccessor httpContextAccessor, IJsonHelper jsonHelper, Dictionary<string, object> requestJsonDictionary) : base(httpContextAccessor, jsonHelper, requestJsonDictionary)
        {
            SetAttributesXmlRule();
            SetProductIdRule();
        }

        #endregion

        #region Private Methods

        private void SetAttributesXmlRule()
        {
            SetNotNullOrEmptyCreateOrUpdateRule(p => p.AttributesXml, "invalid attributes xml", "attributes_xml");
        }

        private void SetProductIdRule()
        {
            SetGreaterThanZeroCreateOrUpdateRule(p => p.ProductId, "invalid product id", "product_id");
        }

        #endregion

    }
}