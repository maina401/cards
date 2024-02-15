using Cards.Core.DTOs;
using Cards.Models;

namespace Cards.Interfaces
{
    public interface ICardService
    {
        Task<PagedList<Card>> GetAllAsync(CardParameters cardParameters);
        Task<Card> GetByIdAsync(Guid id);
        Task<Card> CreateAsync(CardDto card);
        Task UpdateAsync(CardDto input);
        Task DeleteAsync(Guid id);
    }
}