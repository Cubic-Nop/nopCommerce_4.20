using Nop.Core.Domain.Catalog;
using Nop.Plugin.Api.AutoMapper;
using Nop.Plugin.Api.DTOs.ProductAttributes;

namespace Nop.Plugin.Api.MappingExtensions
{
    public static class ProductAttributeDtoMappings
    {
        public static CustomeProductAttributes ToDto(this ProductAttribute productAttribute)
        {
            return productAttribute.MapTo<ProductAttribute, CustomeProductAttributes>();
        }
    }
}
