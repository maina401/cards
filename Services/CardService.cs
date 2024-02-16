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
        private readonly string role;
        private IQueryable<Card> query;

        public CardService(AppDbContext context, IAuthService authService)
        {
            this.context = context;
            userId = authService.UserId!;
            role = authService.Role!;
            query = context.Cards.AsQueryable();
            if (role == AccessLevel.Member.ToString())
            {
                query = query.Where(c => c.UserId.ToString() == authService.UserId);
            }
        }

        public async Task<PagedResponse<Card>> GetAllAsync(CardParameters cardParameters)
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
            // Get the total count before applying the pagination, after applying the filters
            var totalCount = await query.CountAsync();


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

            var pagedList = await PagedList<Card>.ToPagedList(query, cardParameters.Pagination?.PageNumber ?? 1,
                cardParameters.Pagination?.PageSize ?? 1000);

            return new PagedResponse<Card>
            {
                Data = pagedList,
                CurrentPage = pagedList.CurrentPage,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pagedList.PageSize), // Calculate the total pages (rounded up to the nearest whole number
                PageSize = pagedList.PageSize,
                TotalCount = totalCount
            };
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
                Color = input.Color,
                Status = input.Status,
                CreatedAt = DateTime.UtcNow,
                UserId = userId != null ? Guid.Parse(userId) : throw new InvalidOperationException("Invalid user id")
            };
            context.Cards.Add(card);
            await context.SaveChangesAsync();
            return card;
        }

        public async Task UpdateAsync(CardDto input)
        {
            //id is required
            if (input.Id == Guid.Empty) throw new InvalidOperationException("Id is required");
            var card = await context.Cards.FindAsync(input.Id);
            if (card == null) throw new InvalidOperationException("Card not found");
            
            if (card.UserId.ToString() != userId &&
               role != AccessLevel.Admin.ToString())
            {
                throw new InvalidOperationException("Unauthorized");
            }
            
            card.Name = input.Name;
            card.Description = input.Description ?? string.Empty;
            card.Color = input.Color;
            card.Status = input.Status;
            card.UpdatedAt = DateTime.UtcNow;
            
            context.Entry(card).State = EntityState.Modified;
            
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var card = await context.Cards.FindAsync(id);
            //only admin can delete cards not owned by them
            if (card == null) throw new InvalidOperationException("Card not found");

            if (card.UserId.ToString() != userId &&
               role != AccessLevel.Admin.ToString())
            {
                throw new InvalidOperationException("Unauthorized");
            }

            context.Cards.Remove(card);
            await context.SaveChangesAsync();
        }
    }
}