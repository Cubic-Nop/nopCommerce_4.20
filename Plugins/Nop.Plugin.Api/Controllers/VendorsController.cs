using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Discounts;
using Nop.Plugin.Api.Attributes;
using Nop.Plugin.Api.Constants;
using Nop.Plugin.Api.Delta;
using Nop.Plugin.Api.DTOs.Images;
using Nop.Plugin.Api.DTOs.Vendors;
using Nop.Plugin.Api.Factories;
using Nop.Plugin.Api.JSON.ActionResults;
using Nop.Plugin.Api.ModelBinders;
using Nop.Plugin.Api.Models.VendorsParameters;
using Nop.Plugin.Api.Services;
using Nop.Services.Catalog;
using Nop.Services.Customers;
using Nop.Services.Discounts;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Media;
using Nop.Services.Security;
using Nop.Services.Seo;
using Nop.Services.Stores;
using Nop.Plugin.Api.Helpers;

namespace Nop.Plugin.Api.Controllers
{
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Mvc;
    using DTOs.Errors;
    using JSON.Serializers;
    using Nop.Services.Vendors;
    using Nop.Core.Domain.Vendors;
    using Nop.Core.Domain.Media;

    //[ApiAuthorize(Policy = JwtBearerDefaults.AuthenticationScheme, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class VendorsController : BaseApiController
    {
        private readonly IVendorApiService _vendorApiService;
        private readonly IVendorService _vendorService;
        private readonly IUrlRecordService _urlRecordService;
        private readonly IManufacturerService _manufacturerService;
        private readonly IFactory<Vendor> _factory;
        //private readonly IVendorTagService _VendorTagService;
        private readonly IVendorAttributeService _vendorAttributeService;
        private readonly IDTOHelper _dtoHelper;

        public VendorsController(IVendorApiService vendorApiService,
                                  IJsonFieldsSerializer jsonFieldsSerializer,
                                  IVendorService vendorService,
                                  IUrlRecordService urlRecordService,
                                  ICustomerActivityService customerActivityService,
                                  ILocalizationService localizationService,
                                  IFactory<Vendor> factory,
                                  IAclService aclService,
                                  IStoreMappingService storeMappingService,
                                  IStoreService storeService,
                                  ICustomerService customerService,
                                  IDiscountService discountService,
                                  IPictureService pictureService,
                                  IManufacturerService manufacturerService,
                                  //IVendorTagService VendorTagService,
                                  IVendorAttributeService vendorAttributeService,
                                  IDTOHelper dtoHelper) : base(jsonFieldsSerializer, aclService, customerService, storeMappingService, storeService, discountService, customerActivityService, localizationService, pictureService)
        {
            _vendorApiService = vendorApiService;
            _factory = factory;
            _manufacturerService = manufacturerService;
            //_VendorTagService = VendorTagService;
            _urlRecordService = urlRecordService;
            _vendorService = vendorService;
            _vendorAttributeService = vendorAttributeService;
            _dtoHelper = dtoHelper;
        }

        /// <summary>
        /// Receive a list of all Vendors
        /// </summary>
        /// <response code="200">OK</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        [HttpGet]
        [Route("/api/Vendors")]
        [ProducesResponseType(typeof(VendorsRootObjectDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(ErrorsRootObject), (int)HttpStatusCode.BadRequest)]
        [GetRequestsErrorInterceptorActionFilter]
        public IActionResult GetVendors(VendorsParametersModel parameters)
        {
            if (parameters.Limit < Configurations.MinLimit || parameters.Limit > Configurations.MaxLimit)
            {
                return Error(HttpStatusCode.BadRequest, "limit", "invalid limit parameter");
            }

            if (parameters.Page < Configurations.DefaultPageValue)
            {
                return Error(HttpStatusCode.BadRequest, "page", "invalid page parameter");
            }

            var allVendors = _vendorApiService.GetVendors(parameters.Ids, parameters.CreatedAtMin, parameters.CreatedAtMax, parameters.UpdatedAtMin,
                                                                        parameters.UpdatedAtMax, parameters.Limit, parameters.Page, parameters.SinceId, parameters.CategoryId,
                                                                        parameters.VendorName, parameters.PublishedStatus)
                                                .Where(p => StoreMappingService.Authorize(p));

            IList<VendorDto> vendorsAsDtos = allVendors.Select(vendor => _dtoHelper.PrepareVendorDTO(vendor)).ToList();

            var vendorsRootObject = new VendorsRootObjectDto()
            {
                Vendors = vendorsAsDtos
            };

            var json = JsonFieldsSerializer.Serialize(vendorsRootObject, parameters.Fields);

            return new RawJsonActionResult(json);
        }

        /// <summary>
        /// Receive a count of all Vendors
        /// </summary>
        /// <response code="200">OK</response>
        /// <response code="401">Unauthorized</response>
        [HttpGet]
        [Route("/api/Vendors/count")]
        [ProducesResponseType(typeof(VendorsCountRootObject), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(ErrorsRootObject), (int)HttpStatusCode.BadRequest)]
        [GetRequestsErrorInterceptorActionFilter]
        public IActionResult GetVendorsCount(VendorsCountParametersModel parameters)
        {
            var allVendorsCount = _vendorApiService.GetVendorsCount(parameters.CreatedAtMin, parameters.CreatedAtMax, parameters.UpdatedAtMin,
                                                                       parameters.UpdatedAtMax, parameters.PublishedStatus, parameters.VendorName,
                                                                       parameters.CategoryId);

            var vendorsCountRootObject = new VendorsCountRootObject()
            {
                Count = allVendorsCount
            };

            return Ok(vendorsCountRootObject);
        }

        /// <summary>
        /// Retrieve Vendor by spcified id
        /// </summary>
        /// <param name="id">Id of the Vendor</param>
        /// <param name="fields">Fields from the Vendor you want your json to contain</param>
        /// <response code="200">OK</response>
        /// <response code="404">Not Found</response>
        /// <response code="401">Unauthorized</response>
        [HttpGet]
        [Route("/api/Vendors/{id}")]
        [ProducesResponseType(typeof(VendorsRootObjectDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(ErrorsRootObject), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        [GetRequestsErrorInterceptorActionFilter]
        public IActionResult GetVendorById(int id, string fields = "")
        {
            if (id <= 0)
            {
                return Error(HttpStatusCode.BadRequest, "id", "invalid id");
            }

            var vendor = _vendorApiService.GetVendorById(id);

            if (vendor == null)
            {
                return Error(HttpStatusCode.NotFound, "Vendor", "not found");
            }

            var vendorDto = _dtoHelper.PrepareVendorDTO(vendor);

            var vendorsRootObject = new VendorsRootObjectDto();

            vendorsRootObject.Vendors.Add(vendorDto);

            var json = JsonFieldsSerializer.Serialize(vendorsRootObject, fields);

            return new RawJsonActionResult(json);
        }

        [HttpPost]
        [Route("/api/Vendors")]
        [ProducesResponseType(typeof(VendorsRootObjectDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(ErrorsRootObject), 422)]
        public IActionResult CreateVendor([ModelBinder(typeof(JsonModelBinder<VendorDto>))] Delta<VendorDto> vendorDelta)
        {
            // Here we display the errors if the validation has failed at some point.
            if (!ModelState.IsValid)
            {
                return Error();
            }
            //If the validation has passed the manufacturerDelta object won't be null for sure so we don't need to check for this.

            Picture insertedPicture = null;

            // We need to insert the picture before the manufacturer so we can obtain the picture id and map it to the manufacturer.
            if (vendorDelta.Dto.Image != null && vendorDelta.Dto.Image.Binary != null)
            {
                insertedPicture = PictureService.InsertPicture(vendorDelta.Dto.Image.Binary, vendorDelta.Dto.Image.MimeType, string.Empty);
            }

            var vendor = _factory.Initialize();
            // Inserting the new Vendor
            vendorDelta.Merge(vendor);

            // Inserting the new manufacturer

            if (insertedPicture != null)
            {
                vendor.PictureId = insertedPicture.Id;
            }

            vendor.Id = 0;
            _vendorService.InsertVendor(vendor);



            //UpdateVendorTags(Vendor, VendorDelta.Dto.Tags);

            //UpdateVendorManufacturers(Vendor, VendorDelta.Dto.ManufacturerIds);

            //UpdateAssociatedVendors(Vendor, VendorDelta.Dto.AssociatedVendorIds);

            //search engine name
            //var seName = _urlRecordService.ValidateSeName(Vendor, VendorDelta.Dto.SeName, Vendor.Name, true);
            //_urlRecordService.SaveSlug(Vendor, seName, 0);

            //UpdateAclRoles(Vendor, VendorDelta.Dto.RoleIds);

            //UpdateDiscountMappings(Vendor, VendorDelta.Dto.DiscountIds);

            //UpdateStoreMappings(Vendor, VendorDelta.Dto.StoreIds);

            //_vendorService.UpdateVendor(vendor);

            CustomerActivityService.InsertActivity("AddNewVendor",
                LocalizationService.GetResource("ActivityLog.AddNewVendor"), vendor);

            // Preparing the result dto of the new Vendor
            var vendorDto = _dtoHelper.PrepareVendorDTO(vendor);

            var vendorsRootObject = new VendorsRootObjectDto();

            vendorsRootObject.Vendors.Add(vendorDto);

            var json = JsonFieldsSerializer.Serialize(vendorsRootObject, string.Empty);

            return new RawJsonActionResult(json);
        }

        [HttpPut]
        [Route("/api/Vendors/{id}")]
        [ProducesResponseType(typeof(VendorsRootObjectDto), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorsRootObject), 422)]
        [ProducesResponseType(typeof(ErrorsRootObject), (int)HttpStatusCode.BadRequest)]
        public IActionResult UpdateVendor([ModelBinder(typeof(JsonModelBinder<VendorDto>))] Delta<VendorDto> vendorDelta)
        {
            // Here we display the errors if the validation has failed at some point.
            if (!ModelState.IsValid)
            {
                return Error();
            }

            var vendor = _vendorApiService.GetVendorById(vendorDelta.Dto.Id);

            if (vendor == null)
            {
                return Error(HttpStatusCode.NotFound, "Vendor", "not found");
            }

            vendorDelta.Merge(vendor);

            //Vendor.UpdatedOnUtc = DateTime.UtcNow;
            _vendorService.UpdateVendor(vendor);
            UpdatePicture(vendor, vendorDelta.Dto.Image);

            //UpdateVendorAttributes(Vendor, VendorDelta);

            //UpdateVendorPictures(Vendor, VendorDelta.Dto.Images);

            //UpdateVendorTags(Vendor, VendorDelta.Dto.Tags);

            //UpdateVendorManufacturers(Vendor, VendorDelta.Dto.ManufacturerIds);

            //UpdateAssociatedVendors(Vendor, VendorDelta.Dto.AssociatedVendorIds);

            // Update the SeName if specified
            //if (VendorDelta.Dto.SeName != null)
            //{
            //    var seName = _urlRecordService.ValidateSeName(Vendor, VendorDelta.Dto.SeName, Vendor.Name, true);
            //    _urlRecordService.SaveSlug(Vendor, seName, 0);
            //}

            //UpdateDiscountMappings(Vendor, VendorDelta.Dto.DiscountIds);

            //UpdateStoreMappings(Vendor, VendorDelta.Dto.StoreIds);

            //UpdateAclRoles(Vendor, VendorDelta.Dto.RoleIds);

            _vendorService.UpdateVendor(vendor);

            CustomerActivityService.InsertActivity("UpdateVendor",
               LocalizationService.GetResource("ActivityLog.UpdateVendor"), vendor);

            // Preparing the result dto of the new Vendor
            var vendorDto = _dtoHelper.PrepareVendorDTO(vendor);

            var vendorsRootObject = new VendorsRootObjectDto();

            vendorsRootObject.Vendors.Add(vendorDto);

            var json = JsonFieldsSerializer.Serialize(vendorsRootObject, string.Empty);

            return new RawJsonActionResult(json);
        }
        [HttpDelete]
        [Route("/api/Vendors/{id}")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorsRootObject), (int)HttpStatusCode.BadRequest)]
        [GetRequestsErrorInterceptorActionFilter]
        public IActionResult DeleteVendor(int id)
        {
            if (id <= 0)
            {
                return Error(HttpStatusCode.BadRequest, "id", "invalid id");
            }

            var vendor = _vendorApiService.GetVendorById(id);

            if (vendor == null)
            {
                return Error(HttpStatusCode.NotFound, "Vendor", "not found");
            }

            _vendorService.DeleteVendor(vendor);

            //activity log
            CustomerActivityService.InsertActivity("DeleteVendor",
                string.Format(LocalizationService.GetResource("ActivityLog.DeleteVendor"), vendor.Name), vendor);

            return new RawJsonActionResult("{}");
        }

        //private void UpdateVendorPictures(Vendor entityToUpdate, ImageDto setPic)
        //{
        //    // If no pictures are specified means we don't have to update anything
        //    if (setPic == null)
        //        return;

        //    if (setPic.Id > 0)
        //    {
        //        // update existing Vendor picture
        //        var vendorPictureToUpdate = entityToUpdate.VendorPictures.FirstOrDefault(x => x.Id == imageDto.Id);
        //        if (VendorPictureToUpdate != null && imageDto.Position > 0)
        //        {
        //            VendorPictureToUpdate.DisplayOrder = imageDto.Position;
        //            _VendorService.UpdateVendorPicture(VendorPictureToUpdate);
        //        }
        //    }
        //    else
        //    {
        //        // add new Vendor picture
        //        var newPicture = PictureService.InsertPicture(imageDto.Binary, imageDto.MimeType, string.Empty);
        //        _VendorService.InsertVendorPicture(new VendorPicture()
        //        {
        //            PictureId = newPicture.Id,
        //            VendorId = entityToUpdate.Id,
        //            DisplayOrder = imageDto.Position
        //        });
        //    }

        //}

        //private void UpdateVendorAttributes(Vendor entityToUpdate, Delta<VendorDto> VendorDtoDelta)
        //{
        //    // If no Vendor attribute mappings are specified means we don't have to update anything
        //    if (VendorDtoDelta.Dto.VendorAttributeMappings == null)
        //        return;

        //    // delete unused Vendor attribute mappings
        //    var toBeUpdatedIds = VendorDtoDelta.Dto.VendorAttributeMappings.Where(y => y.Id != 0).Select(x => x.Id);

        //    var unusedVendorAttributeMappings = entityToUpdate.VendorAttributeMappings.Where(x => !toBeUpdatedIds.Contains(x.Id)).ToList();

        //    foreach (var unusedVendorAttributeMapping in unusedVendorAttributeMappings)
        //    {
        //        _VendorAttributeService.DeleteVendorAttributeMapping(unusedVendorAttributeMapping);
        //    }

        //    foreach (var VendorAttributeMappingDto in VendorDtoDelta.Dto.VendorAttributeMappings)
        //    {
        //        if (VendorAttributeMappingDto.Id > 0)
        //        {
        //            // update existing Vendor attribute mapping
        //            var VendorAttributeMappingToUpdate = entityToUpdate.VendorAttributeMappings.FirstOrDefault(x => x.Id == VendorAttributeMappingDto.Id);
        //            if (VendorAttributeMappingToUpdate != null)
        //            {
        //                VendorDtoDelta.Merge(VendorAttributeMappingDto,VendorAttributeMappingToUpdate,false);

        //                _VendorAttributeService.UpdateVendorAttributeMapping(VendorAttributeMappingToUpdate);

        //                UpdateVendorAttributeValues(VendorAttributeMappingDto, VendorDtoDelta);
        //            }
        //        }
        //        else
        //        {
        //            var newVendorAttributeMapping = new VendorAttributeMapping {VendorId = entityToUpdate.Id};

        //            VendorDtoDelta.Merge(VendorAttributeMappingDto, newVendorAttributeMapping);

        //            // add new Vendor attribute
        //            _VendorAttributeService.InsertVendorAttributeMapping(newVendorAttributeMapping);
        //        }
        //    }
        //}

        //private void UpdateVendorAttributeValues(VendorAttributeMappingDto VendorAttributeMappingDto, Delta<VendorDto> VendorDtoDelta)
        //{
        //    // If no Vendor attribute values are specified means we don't have to update anything
        //    if (VendorAttributeMappingDto.VendorAttributeValues == null)
        //        return;

        //    // delete unused Vendor attribute values
        //    var toBeUpdatedIds = VendorAttributeMappingDto.VendorAttributeValues.Where(y => y.Id != 0).Select(x => x.Id);

        //    var unusedVendorAttributeValues =
        //        _VendorAttributeService.GetVendorAttributeValues(VendorAttributeMappingDto.Id).Where(x => !toBeUpdatedIds.Contains(x.Id)).ToList();

        //    foreach (var unusedVendorAttributeValue in unusedVendorAttributeValues)
        //    {
        //        _VendorAttributeService.DeleteVendorAttributeValue(unusedVendorAttributeValue);
        //    }

        //    foreach (var VendorAttributeValueDto in VendorAttributeMappingDto.VendorAttributeValues)
        //    {
        //        if (VendorAttributeValueDto.Id > 0)
        //        {
        //            // update existing Vendor attribute mapping
        //            var VendorAttributeValueToUpdate =
        //                _VendorAttributeService.GetVendorAttributeValueById(VendorAttributeValueDto.Id);
        //            if (VendorAttributeValueToUpdate != null)
        //            {
        //                VendorDtoDelta.Merge(VendorAttributeValueDto, VendorAttributeValueToUpdate, false);

        //                _VendorAttributeService.UpdateVendorAttributeValue(VendorAttributeValueToUpdate);
        //            }
        //        }
        //        else
        //        {
        //            var newVendorAttributeValue = new VendorAttributeValue();
        //            VendorDtoDelta.Merge(VendorAttributeValueDto, newVendorAttributeValue);

        //            newVendorAttributeValue.VendorAttributeMappingId = VendorAttributeMappingDto.Id;
        //            // add new Vendor attribute value
        //            _VendorAttributeService.InsertVendorAttributeValue(newVendorAttributeValue);
        //        }
        //    }
        //}

        //private void UpdateVendorTags(Vendor Vendor, IReadOnlyCollection<string> VendorTags)
        //{
        //    if (VendorTags == null)
        //        return;

        //    if (Vendor == null)
        //        throw new ArgumentNullException(nameof(Vendor));

        //    //Copied from UpdateVendorTags method of VendorTagService
        //    //Vendor tags
        //    var existingVendorTags = _VendorTagService.GetAllVendorTagsByVendorId(Vendor.Id);
        //    var VendorTagsToRemove = new List<VendorTag>();
        //    foreach (var existingVendorTag in existingVendorTags)
        //    {
        //        var found = false;
        //        foreach (var newVendorTag in VendorTags)
        //        {
        //            if (!existingVendorTag.Name.Equals(newVendorTag, StringComparison.InvariantCultureIgnoreCase))
        //                continue;

        //            found = true;
        //            break;
        //        }

        //        if (!found)
        //        {
        //            VendorTagsToRemove.Add(existingVendorTag);
        //        }
        //    }

        //    foreach (var VendorTag in VendorTagsToRemove)
        //    {
        //        //Vendor.VendorTags.Remove(VendorTag);
        //        Vendor.VendorVendorTagMappings
        //            .Remove(Vendor.VendorVendorTagMappings.FirstOrDefault(mapping => mapping.VendorTagId == VendorTag.Id));
        //        _VendorService.UpdateVendor(Vendor);
        //    }

        //    foreach (var VendorTagName in VendorTags)
        //    {
        //        VendorTag VendorTag;
        //        var VendorTag2 = _VendorTagService.GetVendorTagByName(VendorTagName);
        //        if (VendorTag2 == null)
        //        {
        //            //add new Vendor tag
        //            VendorTag = new VendorTag
        //            {
        //                Name = VendorTagName
        //            };
        //            _VendorTagService.InsertVendorTag(VendorTag);
        //        }
        //        else
        //        {
        //            VendorTag = VendorTag2;
        //        }

        //        if (!_VendorService.VendorTagExists(Vendor, VendorTag.Id))
        //        {
        //            Vendor.VendorVendorTagMappings.Add(new VendorVendorTagMapping { VendorTag = VendorTag });
        //            _VendorService.UpdateVendor(Vendor);
        //        }

        //        var seName = _urlRecordService.ValidateSeName(VendorTag, string.Empty, VendorTag.Name, true);
        //        _urlRecordService.SaveSlug(VendorTag, seName, 0);
        //    }
        //}

        //private void UpdateDiscountMappings(Vendor Vendor, List<int> passedDiscountIds)
        //{
        //    if (passedDiscountIds == null)
        //        return;

        //    var allDiscounts = DiscountService.GetAllDiscounts(DiscountType.AssignedToSkus, showHidden: true);

        //    foreach (var discount in allDiscounts)
        //    {
        //        if (passedDiscountIds.Contains(discount.Id))
        //        {
        //            //new discount
        //            if (Vendor.AppliedDiscounts.Count(d => d.Id == discount.Id) == 0)
        //                Vendor.AppliedDiscounts.Add(discount);
        //        }
        //        else
        //        {
        //            //remove discount
        //            if (Vendor.AppliedDiscounts.Count(d => d.Id == discount.Id) > 0)
        //                Vendor.AppliedDiscounts.Remove(discount);
        //        }
        //    }

        //    _VendorService.UpdateVendor(Vendor);
        //    _VendorService.UpdateHasDiscountsApplied(Vendor);
        //}

        //private void UpdateVendorManufacturers(Vendor Vendor, List<int> passedManufacturerIds)
        //{
        //    // If no manufacturers specified then there is nothing to map 
        //    if (passedManufacturerIds == null)
        //        return;

        //    var unusedVendorManufacturers = Vendor.VendorManufacturers.Where(x => !passedManufacturerIds.Contains(x.ManufacturerId)).ToList();

        //    // remove all manufacturers that are not passed
        //    foreach (var unusedVendorManufacturer in unusedVendorManufacturers)
        //    {
        //        _manufacturerService.DeleteVendorManufacturer(unusedVendorManufacturer);
        //    }

        //    foreach (var passedManufacturerId in passedManufacturerIds)
        //    {
        //        // not part of existing manufacturers so we will create a new one
        //        if (Vendor.VendorManufacturers.All(x => x.ManufacturerId != passedManufacturerId))
        //        {
        //            // if manufacturer does not exist we simply ignore it, otherwise add it to the Vendor
        //            var manufacturer = _manufacturerService.GetManufacturerById(passedManufacturerId);
        //            if (manufacturer != null)
        //            {
        //                _manufacturerService.InsertVendorManufacturer(new VendorManufacturer()
        //                { VendorId = Vendor.Id, ManufacturerId = manufacturer.Id });
        //            }
        //        }
        //    }
        //}

        //private void UpdateAssociatedVendors(Vendor Vendor, List<int> passedAssociatedVendorIds)
        //{
        //    // If no associated Vendors specified then there is nothing to map 
        //    if (passedAssociatedVendorIds == null)
        //        return;

        //    var noLongerAssociatedVendors =
        //        _VendorService.GetAssociatedVendors(Vendor.Id, showHidden: true)
        //            .Where(p => !passedAssociatedVendorIds.Contains(p.Id));

        //    // update all Vendors that are no longer associated with our Vendor
        //    foreach (var noLongerAssocuatedVendor in noLongerAssociatedVendors)
        //    {
        //        noLongerAssocuatedVendor.ParentGroupedVendorId = 0;
        //        _VendorService.UpdateVendor(noLongerAssocuatedVendor);
        //    }

        //    var newAssociatedVendors = _VendorService.GetVendorsByIds(passedAssociatedVendorIds.ToArray());
        //    foreach (var newAssociatedVendor in newAssociatedVendors)
        //    {
        //        newAssociatedVendor.ParentGroupedVendorId = Vendor.Id;
        //        _VendorService.UpdateVendor(newAssociatedVendor);
        //    }
        //}
        private void UpdatePicture(Vendor vendor, ImageDto imageDto)
        {
            // no image specified then do nothing
            if (imageDto == null)
                return;

            Picture updatedPicture;
            var currentManufacturerPicture = PictureService.GetPictureById(vendor.PictureId);

            // when there is a picture set for the manufacturer
            if (currentManufacturerPicture != null)
            {
                PictureService.DeletePicture(currentManufacturerPicture);

                // When the image attachment is null or empty.
                if (imageDto.Binary == null)
                {
                    vendor.PictureId = 0;
                }
                else
                {
                    updatedPicture = PictureService.InsertPicture(imageDto.Binary, imageDto.MimeType, string.Empty);
                    vendor.PictureId = updatedPicture.Id;
                }
            }
            // when there isn't a picture set for the manufacturer
            else
            {
                if (imageDto.Binary != null)
                {
                    updatedPicture = PictureService.InsertPicture(imageDto.Binary, imageDto.MimeType, string.Empty);
                    vendor.PictureId = updatedPicture.Id;
                }
            }
        }
    }
}