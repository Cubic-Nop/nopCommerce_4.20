﻿using Nop.Core.Domain.Catalog;
using Nop.Plugin.Api.AutoMapper;
using Nop.Plugin.Api.DTOs.Products;

namespace Nop.Plugin.Api.MappingExtensions
{
    public static class ProductAttributeCombinationDtoMappings
    {
        public static VendorAttributeCombinationDto ToDto(this ProductAttributeCombination productAttributeCombination)
        {
            return productAttributeCombination.MapTo<ProductAttributeCombination, VendorAttributeCombinationDto>();
        }
    }
}