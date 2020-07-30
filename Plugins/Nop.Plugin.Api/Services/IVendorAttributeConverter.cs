using Nop.Plugin.Api.DTOs;
using System.Collections.Generic;

namespace Nop.Plugin.Api.Services
{
    public interface IVendorAttributeConverter
    {
        List<VendorItemAttributeDto> Parse(string attributesXml);
        string ConvertToXml(List<VendorItemAttributeDto> attributeDtos, int VendorId);
    }
}
