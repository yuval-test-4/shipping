using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shipping.APIs;
using Shipping.APIs.Common;
using Shipping.APIs.Dtos;
using Shipping.APIs.Errors;

namespace Shipping.APIs;

[Route("api/[controller]")]
[ApiController()]
public abstract class DestinationsControllerBase : ControllerBase
{
    protected readonly IDestinationsService _service;

    public DestinationsControllerBase(IDestinationsService service)
    {
        _service = service;
    }

    /// <summary>
    /// Create one Destination
    /// </summary>
    [HttpPost()]
    [Authorize(Roles = "user")]
    public async Task<ActionResult<Destination>> CreateDestination(DestinationCreateInput input)
    {
        var destination = await _service.CreateDestination(input);

        return CreatedAtAction(nameof(Destination), new { id = destination.Id }, destination);
    }

    /// <summary>
    /// Delete one Destination
    /// </summary>
    [HttpDelete("{Id}")]
    [Authorize(Roles = "user")]
    public async Task<ActionResult> DeleteDestination(
        [FromRoute()] DestinationWhereUniqueInput uniqueId
    )
    {
        try
        {
            await _service.DeleteDestination(uniqueId);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Find many Destinations
    /// </summary>
    [HttpGet()]
    [Authorize(Roles = "user")]
    public async Task<ActionResult<List<Destination>>> Destinations(
        [FromQuery()] DestinationFindManyArgs filter
    )
    {
        return Ok(await _service.Destinations(filter));
    }

    /// <summary>
    /// Meta data about Destination records
    /// </summary>
    [HttpPost("meta")]
    public async Task<ActionResult<MetadataDto>> DestinationsMeta(
        [FromQuery()] DestinationFindManyArgs filter
    )
    {
        return Ok(await _service.DestinationsMeta(filter));
    }

    /// <summary>
    /// Get one Destination
    /// </summary>
    [HttpGet("{Id}")]
    [Authorize(Roles = "user")]
    public async Task<ActionResult<Destination>> Destination(
        [FromRoute()] DestinationWhereUniqueInput uniqueId
    )
    {
        try
        {
            return await _service.Destination(uniqueId);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Update one Destination
    /// </summary>
    [HttpPatch("{Id}")]
    [Authorize(Roles = "user")]
    public async Task<ActionResult> UpdateDestination(
        [FromRoute()] DestinationWhereUniqueInput uniqueId,
        [FromQuery()] DestinationUpdateInput destinationUpdateDto
    )
    {
        try
        {
            await _service.UpdateDestination(uniqueId, destinationUpdateDto);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Connect multiple Items records to Destination
    /// </summary>
    [HttpPost("{Id}/items")]
    [Authorize(Roles = "user")]
    public async Task<ActionResult> ConnectItems(
        [FromRoute()] DestinationWhereUniqueInput uniqueId,
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
    /// Disconnect multiple Items records from Destination
    /// </summary>
    [HttpDelete("{Id}/items")]
    [Authorize(Roles = "user")]
    public async Task<ActionResult> DisconnectItems(
        [FromRoute()] DestinationWhereUniqueInput uniqueId,
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
    /// Find multiple Items records for Destination
    /// </summary>
    [HttpGet("{Id}/items")]
    [Authorize(Roles = "user")]
    public async Task<ActionResult<List<Item>>> FindItems(
        [FromRoute()] DestinationWhereUniqueInput uniqueId,
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
    /// Update multiple Items records for Destination
    /// </summary>
    [HttpPatch("{Id}/items")]
    [Authorize(Roles = "user")]
    public async Task<ActionResult> UpdateItems(
        [FromRoute()] DestinationWhereUniqueInput uniqueId,
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
