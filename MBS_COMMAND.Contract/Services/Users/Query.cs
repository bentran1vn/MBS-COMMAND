using MBS_COMMAND.Contract.Abstractions.Messages;
using MBS_COMMAND.Contract.Enumerations;

namespace MBS_COMMAND.Contract.Services.Users;

public static class Query
{
    public record GetProductQuery(string? SearchTerm, string? SortColumn, SortOrder? SortOrder, IDictionary<string, SortOrder>? SortColumnAndOrder) : IQuery<List<Response.ProductResponse>>;

    public record GetProductById(Guid Id) : IQuery<List<Response.ProductResponse>>;
}
