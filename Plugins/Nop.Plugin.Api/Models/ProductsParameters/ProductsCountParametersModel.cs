using Nop.Plugin.Api.ModelBinders;

namespace Nop.Plugin.Api.Models.ProductsParameters
{
    using Microsoft.AspNetCore.Mvc;

    [ModelBinder(typeof(ParametersModelBinder<VendorsCountParametersModel>))]
    public class VendorsCountParametersModel : BaseVendorsParametersModel
    {
        // Nothing special here, created just for clarity.
    }
}