using Cards.Core.DTOs;
using Cards.Models;

namespace Cards.Interfaces
{
    public interface ICardService
    {
        Task<PagedResponse<Card>> GetAllAsync(CardParameters cardParameters);
        Task<Card> GetByIdAsync(Guid id);
        Task<Card> CreateAsync(CardDto input);
        Task UpdateAsync(CardDto input);
        Task DeleteAsync(Guid id);
    }
}