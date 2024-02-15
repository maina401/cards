using Cards.Enums;
using Cards.Models;

public class CardParameters
{
    public CardFilter? Filter { get; set; }
    public Pagination? Pagination { get; set; }
    public Sort? Sort { get; set; }
}

public class Sort
{
    public string? Field { get; set; }
    public SortDirection Direction { get; set; } = SortDirection.Asc;

    public IQueryable<T> ApplyTo<T>(IQueryable<T> query)
    {
        if (Field != null)
        {
            query = Field.ToLower() switch
            {
                "name" => Direction == SortDirection.Asc
                    ? query.OrderBy(c => ((Card)(object)c).Name)
                    : query.OrderByDescending(c => ((Card)(object)c).Name),
                "color" => Direction == SortDirection.Asc
                    ? query.OrderBy(c => ((Card)(object)c).Color)
                    : query.OrderByDescending(c => ((Card)(object)c).Color),
                "status" => Direction == SortDirection.Asc
                    ? query.OrderBy(c => ((Card)(object)c).Status)
                    : query.OrderByDescending(c => ((Card)(object)c).Status),
                "createdat" => Direction == SortDirection.Asc
                    ? query.OrderBy(c => ((Card)(object)c).CreatedAt)
                    : query.OrderByDescending(c => ((Card)(object)c).CreatedAt),
                _ => Direction == SortDirection.Asc
                    ? query.OrderBy(c => ((Card)(object)c).Id)
                    : query.OrderByDescending(c => ((Card)(object)c).Id)
            };
        }

        return query;
    }
}

public enum SortDirection
{
    Asc,
    Desc,
}

public class Pagination
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;

    public IQueryable<T> ApplyTo<T>(IQueryable<T> query)
    {
        return query.Skip((PageNumber - 1) * PageSize).Take(PageSize);
    }
}

public class CardFilter
{
    public string? Name { get; set; }
    public string? Color { get; set; }
    public CardStatus? Status { get; set; }
    public DateTime? CreatedAt { get; set; }

    public IQueryable<Card> ApplyTo(IQueryable<Card> query)
    {
        if (!string.IsNullOrEmpty(Name))
        {
            query = query.Where(c => c.Name.Contains(Name));
        }

        if (!string.IsNullOrEmpty(Color))
        {
            query = query.Where(c => c.Color == Color);
        }

        if (Status.HasValue)
        {
            query = query.Where(c => c.Status == Status.Value);
        }

        if (CreatedAt.HasValue)
        {
            query = query.Where(c => c.CreatedAt.Date == CreatedAt.Value.Date);
        }

        return query;
    }
}