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
    /// Connect multiple Items records to Shipment
    /// </summary>
    [HttpPost("{Id}/items")]
    [Authorize(Roles = "user")]
    public async Task<ActionResult> ConnectItems(
        [FromRoute()] ShipmentWhereUniqueInput uniqueId,
        [FromQuery()] ItemWhereUniqueInput[] itemsId
    )
    {
        try
        {
            await _service.ConnectItems(uniqueId, itemsId);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Disconnect multiple Items records from Shipment
    /// </summary>
    [HttpDelete("{Id}/items")]
    [Authorize(Roles = "user")]
    public async Task<ActionResult> DisconnectItems(
        [FromRoute()] ShipmentWhereUniqueInput uniqueId,
        [FromBody()] ItemWhereUniqueInput[] itemsId
    )
    {
        try
        {
            await _service.DisconnectItems(uniqueId, itemsId);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Find multiple Items records for Shipment
    /// </summary>
    [HttpGet("{Id}/items")]
    [Authorize(Roles = "user")]
    public async Task<ActionResult<List<Item>>> FindItems(
        [FromRoute()] ShipmentWhereUniqueInput uniqueId,
        [FromQuery()] ItemFindManyArgs filter
    )
    {
        try
        {
            return Ok(await _service.FindItems(uniqueId, filter));
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Update multiple Items records for Shipment
    /// </summary>
    [HttpPatch("{Id}/items")]
    [Authorize(Roles = "user")]
    public async Task<ActionResult> UpdateItems(
        [FromRoute()] ShipmentWhereUniqueInput uniqueId,
        [FromBody()] ItemWhereUniqueInput[] itemsId
    )
    {
        try
        {
            await _service.UpdateItems(uniqueId, itemsId);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }
}
