using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shipping.APIs;
using Shipping.APIs.Common;
using Shipping.APIs.Dtos;
using Shipping.APIs.Errors;

namespace Shipping.APIs;

[Route("api/[controller]")]
[ApiController()]
public abstract class PackageModelsControllerBase : ControllerBase
{
    protected readonly IPackageModelsService _service;

    public PackageModelsControllerBase(IPackageModelsService service)
    {
        _service = service;
    }

    /// <summary>
    /// Create one Package
    /// </summary>
    [HttpPost()]
    [Authorize(Roles = "user")]
    public async Task<ActionResult<PackageModel>> CreatePackageModel(PackageModelCreateInput input)
    {
        var packageModel = await _service.CreatePackageModel(input);

        return CreatedAtAction(nameof(PackageModel), new { id = packageModel.Id }, packageModel);
    }

    /// <summary>
    /// Delete one Package
    /// </summary>
    [HttpDelete("{Id}")]
    [Authorize(Roles = "user")]
    public async Task<ActionResult> DeletePackageModel(
        [FromRoute()] PackageModelWhereUniqueInput uniqueId
    )
    {
        try
        {
            await _service.DeletePackageModel(uniqueId);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Find many Packages
    /// </summary>
    [HttpGet()]
    [Authorize(Roles = "user")]
    public async Task<ActionResult<List<PackageModel>>> PackageModels(
        [FromQuery()] PackageModelFindManyArgs filter
    )
    {
        return Ok(await _service.PackageModels(filter));
    }

    /// <summary>
    /// Meta data about Package records
    /// </summary>
    [HttpPost("meta")]
    public async Task<ActionResult<MetadataDto>> PackageModelsMeta(
        [FromQuery()] PackageModelFindManyArgs filter
    )
    {
        return Ok(await _service.PackageModelsMeta(filter));
    }

    /// <summary>
    /// Get one Package
    /// </summary>
    [HttpGet("{Id}")]
    [Authorize(Roles = "user")]
    public async Task<ActionResult<PackageModel>> PackageModel(
        [FromRoute()] PackageModelWhereUniqueInput uniqueId
    )
    {
        try
        {
            return await _service.PackageModel(uniqueId);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Update one Package
    /// </summary>
    [HttpPatch("{Id}")]
    [Authorize(Roles = "user")]
    public async Task<ActionResult> UpdatePackageModel(
        [FromRoute()] PackageModelWhereUniqueInput uniqueId,
        [FromQuery()] PackageModelUpdateInput packageModelUpdateDto
    )
    {
        try
        {
            await _service.UpdatePackageModel(uniqueId, packageModelUpdateDto);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Get a Destination record for Package
    /// </summary>
    [HttpGet("{Id}/destination")]
    public async Task<ActionResult<List<Destination>>> GetDestination(
        [FromRoute()] PackageModelWhereUniqueInput uniqueId
    )
    {
        var destination = await _service.GetDestination(uniqueId);
        return Ok(destination);
    }

    /// <summary>
    /// Connect multiple Shipments records to Package
    /// </summary>
    [HttpPost("{Id}/shipments")]
    [Authorize(Roles = "user")]
    public async Task<ActionResult> ConnectShipments(
        [FromRoute()] PackageModelWhereUniqueInput uniqueId,
        [FromQuery()] ShipmentWhereUniqueInput[] shipmentsId
    )
    {
        try
        {
            await _service.ConnectShipments(uniqueId, shipmentsId);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Disconnect multiple Shipments records from Package
    /// </summary>
    [HttpDelete("{Id}/shipments")]
    [Authorize(Roles = "user")]
    public async Task<ActionResult> DisconnectShipments(
        [FromRoute()] PackageModelWhereUniqueInput uniqueId,
        [FromBody()] ShipmentWhereUniqueInput[] shipmentsId
    )
    {
        try
        {
            await _service.DisconnectShipments(uniqueId, shipmentsId);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Find multiple Shipments records for Package
    /// </summary>
    [HttpGet("{Id}/shipments")]
    [Authorize(Roles = "user")]
    public async Task<ActionResult<List<Shipment>>> FindShipments(
        [FromRoute()] PackageModelWhereUniqueInput uniqueId,
        [FromQuery()] ShipmentFindManyArgs filter
    )
    {
        try
        {
            return Ok(await _service.FindShipments(uniqueId, filter));
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Update multiple Shipments records for Package
    /// </summary>
    [HttpPatch("{Id}/shipments")]
    [Authorize(Roles = "user")]
    public async Task<ActionResult> UpdateShipments(
        [FromRoute()] PackageModelWhereUniqueInput uniqueId,
        [FromBody()] ShipmentWhereUniqueInput[] shipmentsId
    )
    {
        try
        {
            await _service.UpdateShipments(uniqueId, shipmentsId);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }
}
