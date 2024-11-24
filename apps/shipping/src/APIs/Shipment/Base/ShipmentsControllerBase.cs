using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shipping.APIs;
using Shipping.APIs.Common;
using Shipping.APIs.Dtos;
using Shipping.APIs.Errors;

namespace Shipping.APIs;

[Route("api/[controller]")]
[ApiController()]
public abstract class ShipmentsControllerBase : ControllerBase
{
    protected readonly IShipmentsService _service;

    public ShipmentsControllerBase(IShipmentsService service)
    {
        _service = service;
    }

    /// <summary>
    /// Create one Shipment
    /// </summary>
    [HttpPost()]
    [Authorize(Roles = "user")]
    public async Task<ActionResult<Shipment>> CreateShipment(ShipmentCreateInput input)
    {
        var shipment = await _service.CreateShipment(input);

        return CreatedAtAction(nameof(Shipment), new { id = shipment.Id }, shipment);
    }

    /// <summary>
    /// Delete one Shipment
    /// </summary>
    [HttpDelete("{Id}")]
    [Authorize(Roles = "user")]
    public async Task<ActionResult> DeleteShipment([FromRoute()] ShipmentWhereUniqueInput uniqueId)
    {
        try
        {
            await _service.DeleteShipment(uniqueId);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Find many Shipments
    /// </summary>
    [HttpGet()]
    [Authorize(Roles = "user")]
    public async Task<ActionResult<List<Shipment>>> Shipments(
        [FromQuery()] ShipmentFindManyArgs filter
    )
    {
        return Ok(await _service.Shipments(filter));
    }

    /// <summary>
    /// Meta data about Shipment records
    /// </summary>
    [HttpPost("meta")]
    public async Task<ActionResult<MetadataDto>> ShipmentsMeta(
        [FromQuery()] ShipmentFindManyArgs filter
    )
    {
        return Ok(await _service.ShipmentsMeta(filter));
    }

    /// <summary>
    /// Get one Shipment
    /// </summary>
    [HttpGet("{Id}")]
    [Authorize(Roles = "user")]
    public async Task<ActionResult<Shipment>> Shipment(
        [FromRoute()] ShipmentWhereUniqueInput uniqueId
    )
    {
        try
        {
            return await _service.Shipment(uniqueId);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Update one Shipment
    /// </summary>
    [HttpPatch("{Id}")]
    [Authorize(Roles = "user")]
    public async Task<ActionResult> UpdateShipment(
        [FromRoute()] ShipmentWhereUniqueInput uniqueId,
        [FromQuery()] ShipmentUpdateInput shipmentUpdateDto
    )
    {
        try
        {
            await _service.UpdateShipment(uniqueId, shipmentUpdateDto);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Get a Package record for Shipment
    /// </summary>
    [HttpGet("{Id}/packageField")]
    public async Task<ActionResult<List<PackageModel>>> GetPackageField(
        [FromRoute()] ShipmentWhereUniqueInput uniqueId
    )
    {
        var packageModel = await _service.GetPackageField(uniqueId);
        return Ok(packageModel);
    }
}
