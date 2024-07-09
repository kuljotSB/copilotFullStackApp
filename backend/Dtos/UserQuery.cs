namespace backend.api.Dtos;

public record class UserQuery(
    int id,
    Boolean WebSearcherPluginChoice,
    Boolean AISearchPluginChoice,
    Boolean GraphPluginChoice,
    string? userQueryString

);