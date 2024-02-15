using Cards.Core.DTOs;
using Cards.Interfaces;
using Cards.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cards.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class CardController(ICardService cardService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] CardParameters cardParameters)
        {
            var cards = await cardService.GetAllAsync(cardParameters);
            return Ok(cards);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var card = await cardService.GetByIdAsync(id);
            return Ok(card);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CardDto card)
        {
            var createdCard = await cardService.CreateAsync(card);
            return Ok(createdCard);
        }

        [HttpPut]
        public async Task<IActionResult> Update(CardDto card)
        {
            await cardService.UpdateAsync(card);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await cardService.DeleteAsync(id);
            return NoContent();
        }
    }
}