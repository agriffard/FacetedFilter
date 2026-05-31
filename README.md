# FacetedFilter

[![NuGet](https://img.shields.io/nuget/v/FacetedFilter?logo=nuget)](https://www.nuget.org/packages/FacetedFilter/)

FacetedFilter is a .NET 10 Blazor component library for the classic catalog/search-results experience: a sidebar of facets with live counts, multi-select filters, and removable active-filter chips.

## Repository layout

- `/src/FacetedFilter` — Razor class library packaged for NuGet
- `/samples/FacetedFilter.Demo` — Blazor WebAssembly sample app for local/manual testing
- `/docs` — repository documentation
- `/.github/workflows` — CI, NuGet packaging, and GitHub Pages deployment

## Features

- Multi-select facet groups
- Live counts that react to the other active filters
- Active-filter chips with clear-all support
- Generic `FacetDefinition<TItem>` model so you can adapt the component to your own catalog types
- Optional text search for the catalog/search-results pattern

## Install from NuGet

```bash
dotnet add package FacetedFilter
```

## Basic usage

```razor
@using FacetedFilter

<FacetedFilterPanel TItem="Product"
                    Items="Products"
                    Facets="Facets"
                    SearchTextSelector="SearchText"
                    ResultsLabel="matching products">
    <ItemTemplate Context="product">
        <article class="product-card">
            <h2>@product.Name</h2>
            <p>@product.Description</p>
        </article>
    </ItemTemplate>
</FacetedFilterPanel>

@code {
    private static string SearchText(Product product)
        => $"{product.Name} {product.Description}";

    private static readonly IReadOnlyList<Product> Products =
    [
        new("Trail Shell Jacket", "Apparel", "Northwind", "Blue", "$$$", "Next day", "Lightweight waterproof shell.")
    ];

    private static readonly IReadOnlyList<FacetDefinition<Product>> Facets =
    [
        new()
        {
            Key = "category",
            Title = "Category",
            Values = product => [product.Category]
        },
        new()
        {
            Key = "brand",
            Title = "Brand",
            Values = product => [product.Brand]
        }
    ];

    private sealed record Product(
        string Name,
        string Category,
        string Brand,
        string Color,
        string PriceBand,
        string ShippingSpeed,
        string Description);
}
```

## Run the sample app locally

```bash
dotnet restore FacetedFilter.slnx
dotnet run --project samples/FacetedFilter.Demo/FacetedFilter.Demo.csproj
```

Then open the local URL printed by the Blazor WebAssembly dev server.

## Packaging and deployment

- **CI** (`.github/workflows/ci.yml`) restores, builds, and tests the solution on pushes to `main` and pull requests.
- **NuGet** (`.github/workflows/nuget.yml`) packs the library on every tag matching `v*` and pushes the package when `NUGET_API_KEY` is configured.
- **GitHub Pages** (`.github/workflows/pages.yml`) publishes the sample app to Pages so the component has a live demo.

## API notes

`FacetDefinition<TItem>` lets you declare the filter groups for any item type:

- `Key` — stable identifier for the facet group
- `Title` — heading displayed in the sidebar
- `Values` — function that extracts one or more facet values from an item
- `Options` — optional explicit option list if you want to control labels/order

The `FacetedFilterPanel<TItem>` component renders the sidebar, chips, and filtered result list. Supply `ItemTemplate` to control how each result card is displayed.
