using System.Security.Claims;
using Cards.Core.DTOs;
using Cards.Enums;
using Cards.Interfaces;
using Cards.Models;
using Microsoft.EntityFrameworkCore;

namespace Cards.Services
{
    public class CardService : ICardService
    {
        private readonly AppDbContext context;
        private readonly string userId;
        private IQueryable<Card> query;
        private readonly IHttpContextAccessor httpContextAccessor;

        public CardService(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            this.context = context;
            this.httpContextAccessor = httpContextAccessor;
            userId = httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var role = httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.Role);
            query = this.context.Cards.AsQueryable();
            if (role == AccessLevel.Member.ToString())
            {
                query = query.Where(c => c.UserId.ToString() == userId);
            }
        }

        public async Task<PagedList<Card>> GetAllAsync(CardParameters cardParameters)
        {
            // Get the user id and role
            if (!Guid.TryParse(userId, out var id))
            {
                throw new InvalidOperationException("Invalid user id");
            }


            // Apply the filters
            if (cardParameters.Filter != null)
            {
                query = cardParameters.Filter.ApplyTo(query);
            }

            // Apply the sorting
            if (cardParameters.Sort != null)
            {
                query = cardParameters.Sort.ApplyTo(query);
            }

            // Apply the pagination
            if (cardParameters.Pagination != null)
            {
                query = cardParameters.Pagination.ApplyTo(query);
            }


            return await PagedList<Card>.ToPagedList(query, cardParameters.Pagination?.PageNumber ?? 1,
                cardParameters.Pagination?.PageSize ?? 1000);
        }

        public async Task<Card> GetByIdAsync(Guid id)
        {
            return await query.Where(card => card.Id == id).FirstOrDefaultAsync() ??
                   throw new InvalidOperationException("Card not found");
        }

        public async Task<Card> CreateAsync(CardDto input)
        {
            var card = new Card
            {
                Name = input.Name,
                Description = input.Description ?? string.Empty,
                UserId = userId != null ? Guid.Parse(userId) : throw new InvalidOperationException("Invalid user id")
            };
            context.Cards.Add(card);
            await context.SaveChangesAsync();
            return card;
        }

        public async Task UpdateAsync(CardDto card)
        {
            context.Entry(card).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var card = await context.Cards.FindAsync(id);
            //only admin can delete cards not owned by them
            if (card == null) throw new InvalidOperationException("Card not found");

            if (card.UserId.ToString() != userId &&
                !httpContextAccessor.HttpContext!.User.IsInRole(AccessLevel.Admin.ToString()))
            {
                throw new InvalidOperationException("Unauthorized");
            }

            context.Cards.Remove(card);
            await context.SaveChangesAsync();
        }
    }
}