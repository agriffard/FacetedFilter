using System.Collections.ObjectModel;

namespace FacetedFilter;

internal static class FacetedFilterEngine
{
    public static IReadOnlyList<TItem> FilterItems<TItem>(
        IReadOnlyList<TItem> items,
        IReadOnlyList<FacetDefinition<TItem>> facets,
        IReadOnlyDictionary<string, List<string>> selectedValues,
        string searchText,
        Func<TItem, string?>? searchTextSelector)
    {
        return items
            .Where(item => MatchesSearch(item, searchText, searchTextSelector))
            .Where(item => MatchesSelections(item, facets, selectedValues, excludedFacetKey: null))
            .ToList();
    }

    public static IReadOnlyList<FacetOption> GetOptions<TItem>(
        FacetDefinition<TItem> facet,
        IReadOnlyList<TItem> items,
        IReadOnlyDictionary<string, List<string>> selectedValues)
    {
        var options = facet.Options?.ToList() ?? items
            .SelectMany(item => NormalizeValues(facet.Values(item)))
            .Distinct(facet.Comparer)
            .OrderBy(value => value, facet.Comparer)
            .Select(value => new FacetOption { Value = value, Label = value })
            .ToList();

        if (selectedValues.TryGetValue(facet.Key, out var selected))
        {
            foreach (var value in selected)
            {
                if (!options.Any(option => facet.Comparer.Equals(option.Value, value)))
                {
                    options.Add(new FacetOption { Value = value, Label = value });
                }
            }
        }

        return new ReadOnlyCollection<FacetOption>(options);
    }

    public static int CountMatches<TItem>(
        IReadOnlyList<TItem> items,
        IReadOnlyList<FacetDefinition<TItem>> facets,
        IReadOnlyDictionary<string, List<string>> selectedValues,
        string searchText,
        Func<TItem, string?>? searchTextSelector,
        FacetDefinition<TItem> facet,
        string optionValue)
    {
        return items
            .Where(item => MatchesSearch(item, searchText, searchTextSelector))
            .Where(item => MatchesSelections(item, facets, selectedValues, facet.Key))
            .Count(item => NormalizeValues(facet.Values(item)).Any(value => facet.Comparer.Equals(value, optionValue)));
    }

    private static bool MatchesSelections<TItem>(
        TItem item,
        IReadOnlyList<FacetDefinition<TItem>> facets,
        IReadOnlyDictionary<string, List<string>> selectedValues,
        string? excludedFacetKey)
    {
        foreach (var facet in facets)
        {
            if (excludedFacetKey is not null && string.Equals(facet.Key, excludedFacetKey, StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            if (!selectedValues.TryGetValue(facet.Key, out var selected) || selected.Count == 0)
            {
                continue;
            }

            var itemValues = NormalizeValues(facet.Values(item));
            if (!itemValues.Any(itemValue => selected.Any(selectedValue => facet.Comparer.Equals(itemValue, selectedValue))))
            {
                return false;
            }
        }

        return true;
    }

    private static bool MatchesSearch<TItem>(TItem item, string searchText, Func<TItem, string?>? searchTextSelector)
    {
        if (string.IsNullOrWhiteSpace(searchText))
        {
            return true;
        }

        var haystack = searchTextSelector?.Invoke(item);
        return haystack?.Contains(searchText, StringComparison.OrdinalIgnoreCase) is true;
    }

    private static IEnumerable<string> NormalizeValues(IEnumerable<string>? values)
    {
        return values?
            .Where(value => !string.IsNullOrWhiteSpace(value))
            .Select(value => value.Trim())
            ?? [];
    }
}
