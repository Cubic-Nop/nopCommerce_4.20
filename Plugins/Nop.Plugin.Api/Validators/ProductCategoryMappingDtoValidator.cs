using Microsoft.AspNetCore.Http;
using Nop.Plugin.Api.DTOs.ProductCategoryMappings;
using Nop.Plugin.Api.Helpers;
using System.Collections.Generic;

namespace Nop.Plugin.Api.Validators
{
    public class ProductCategoryMappingDtoValidator : BaseDtoValidator<ProductCategoryMappingDto>
    {

        #region Constructors

        public ProductCategoryMappingDtoValidator(IHttpContextAccessor httpContextAccessor, IJsonHelper jsonHelper, Dictionary<string, object> requestJsonDictionary) : base(httpContextAccessor, jsonHelper, requestJsonDictionary)
        {
            //SetCategoryIdRule();
            SetProductIdRule();
        }

        #endregion

        #region Private Methods

        //private void SetCategoryIdRule()
        //{
        //    SetGreaterThanZeroCreateOrUpdateRule(p => p.CategoryIdDynamics, "invalid category_id", "category_id");
        //}

        private void SetProductIdRule()
        {
            SetNotNullOrEmptyCreateOrUpdateRule(p => p.ProductSku, "invalid product_sku", "product_sku");
        }

        #endregion

    }
}