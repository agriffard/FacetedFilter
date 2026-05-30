# FacetedFilter docs

FacetedFilter is a .NET 10 Blazor component library for the classic catalog/search-results layout.

## What it gives you

- Sidebar facet groups with live result counts
- Multi-select filtering inside each facet group
- Active-filter chips that can be removed one-by-one
- Optional free-text search alongside facet filters

## Projects in this repository

- `src/FacetedFilter` — Razor class library and NuGet package source
- `samples/FacetedFilter.Demo` — Blazor WebAssembly demo app used for manual verification and GitHub Pages deployment

## Local workflow

```bash
dotnet restore FacetedFilter.slnx
dotnet build FacetedFilter.slnx
dotnet test FacetedFilter.slnx
dotnet run --project samples/FacetedFilter.Demo/FacetedFilter.Demo.csproj
```
