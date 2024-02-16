using Cards.Core.DTOs;
using Cards.Interfaces;
using Cards.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cards.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(AuthenticationSchemes = "Bearer")]
public class CardController(ICardService cardService) : ControllerBase
{
    /// <summary>
    /// Gets all cards with optional pagination, sorting, and filtering.
    /// </summary>
    ///  <param name="cardParameters">The parameters for pagination, sorting, and filtering.</param>
    /// <response code="200">Returns the list of cards.</response>
    /// <response code="422">If there's an error processing the request.</response>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResponse<CardDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> GetAll([FromQuery] CardParameters cardParameters)
    {
        var cards = await cardService.GetAllAsync(cardParameters);
        return Ok(cards);
    }

    /// <summary>
    /// Gets a specific card by ID.
    /// </summary>
    /// <response code="200">Returns the card.</response>
    /// <response code="404">If the card is not found.</response>
    /// <response code="422">If there's an error processing the request.</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(CardDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var card = await cardService.GetByIdAsync(id);
        return Ok(card);
    }

    /// <summary>
    /// Creates a new card.
    /// </summary>
    /// <response code="200">Returns the created card.</response>
    /// <response code="422">If there's an error processing the request.</response>
    [HttpPost]
    [ProducesResponseType(typeof(CardDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Create(CardDto card)
    {
        var createdCard = await cardService.CreateAsync(card);
        return Ok(createdCard);
    }

    /// <summary>
    /// Updates an existing card.
    /// </summary>
    /// <response code="204">Returns no content.</response>
    /// <response code="422">If there's an error processing the request.</response>
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Update(CardDto card)
    {
        await cardService.UpdateAsync(card);
        return NoContent();
    }

    /// <summary>
    /// Deletes a card.
    /// </summary>
    /// <response code="204">Returns no content.</response>
    /// <response code="404">If the card is not found.</response>
    /// <response code="422">If there's an error processing the request.</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Delete(Guid id)
    {
        await cardService.DeleteAsync(id);
        return NoContent();
    }
}