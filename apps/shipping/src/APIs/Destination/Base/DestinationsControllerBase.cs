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
    /// Connect multiple Packages records to Destination
    /// </summary>
    [HttpPost("{Id}/packages")]
    [Authorize(Roles = "user")]
    public async Task<ActionResult> ConnectPackages(
        [FromRoute()] DestinationWhereUniqueInput uniqueId,
        [FromQuery()] PackageModelWhereUniqueInput[] packageModelsId
    )
    {
        try
        {
            await _service.ConnectPackages(uniqueId, packageModelsId);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Disconnect multiple Packages records from Destination
    /// </summary>
    [HttpDelete("{Id}/packages")]
    [Authorize(Roles = "user")]
    public async Task<ActionResult> DisconnectPackages(
        [FromRoute()] DestinationWhereUniqueInput uniqueId,
        [FromBody()] PackageModelWhereUniqueInput[] packageModelsId
    )
    {
        try
        {
            await _service.DisconnectPackages(uniqueId, packageModelsId);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Find multiple Packages records for Destination
    /// </summary>
    [HttpGet("{Id}/packages")]
    [Authorize(Roles = "user")]
    public async Task<ActionResult<List<PackageModel>>> FindPackages(
        [FromRoute()] DestinationWhereUniqueInput uniqueId,
        [FromQuery()] PackageModelFindManyArgs filter
    )
    {
        try
        {
            return Ok(await _service.FindPackages(uniqueId, filter));
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Update multiple Packages records for Destination
    /// </summary>
    [HttpPatch("{Id}/packages")]
    [Authorize(Roles = "user")]
    public async Task<ActionResult> UpdatePackages(
        [FromRoute()] DestinationWhereUniqueInput uniqueId,
        [FromBody()] PackageModelWhereUniqueInput[] packageModelsId
    )
    {
        try
        {
            await _service.UpdatePackages(uniqueId, packageModelsId);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }
}
