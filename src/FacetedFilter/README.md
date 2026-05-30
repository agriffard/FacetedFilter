# FacetedFilter

`FacetedFilter` is a Blazor component library for the catalog/search-results pattern.

## Features

- Sidebar facets with live counts
- Multi-select facet groups
- Active-filter chips with one-click removal
- Generic item model support through `FacetDefinition<TItem>`

## Quick start

```razor
@using FacetedFilter

<FacetedFilterPanel TItem="Product"
                    Items="Products"
                    Facets="Facets"
                    SearchTextSelector="SearchText">
    <ItemTemplate Context="product">
        <article>@product.Name</article>
    </ItemTemplate>
</FacetedFilterPanel>

@code {
    private static string SearchText(Product product)
        => $"{product.Name} {product.Description}";
}
```
