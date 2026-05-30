namespace FacetedFilter;

public sealed class FacetOption
{
    public required string Value { get; init; }

    public string? Label { get; init; }

    public string DisplayLabel => string.IsNullOrWhiteSpace(Label) ? Value : Label;
}

public sealed class FacetDefinition<TItem>
{
    public required string Key { get; init; }

    public required string Title { get; init; }

    public required Func<TItem, IEnumerable<string>> Values { get; init; }

    public IReadOnlyList<FacetOption>? Options { get; init; }

    public StringComparer Comparer { get; init; } = StringComparer.OrdinalIgnoreCase;
}
