﻿using Microsoft.EntityFrameworkCore;

namespace Stellaway.DTOs.Pages;

public class PaginatedList<T> : List<T> where T : class
{
    public int PageIndex { get; private set; }
    public int PageSize { get; private set; }
    public int TotalPages { get; private set; }
    public int TotalItems { get; private set; }
    public bool HasPreviousPage => PageIndex > 1;
    public bool HasNextPage => PageIndex < TotalPages;

    public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
    {
        PageIndex = pageIndex;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        TotalItems = count;
        PageSize = pageSize;
        AddRange(items);
    }

    public static async Task<PaginatedList<T>> CreateAsync(
        IQueryable<T> source,
        int pageIndex,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        pageIndex = pageIndex < 1 ? 1 : pageIndex;
        pageSize = pageSize < 1 ? 10 : pageSize;
        var count = await source.CountAsync(cancellationToken);
        var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);
        return new PaginatedList<T>(items, count, pageIndex, pageSize);
    }
}

public static class PaginatedListExtensions
{
    public static Task<PaginatedList<TDestination>> PaginatedListAsync<TDestination>(
        this IQueryable<TDestination> queryable, int pageIndex, int pageSize, CancellationToken cancellationToken = default) where TDestination : class
      => PaginatedList<TDestination>.CreateAsync(queryable.AsNoTracking(), pageIndex, pageSize, cancellationToken);

    public static PaginatedList<U> MapTo<T, U>(this PaginatedList<T> source, Func<T, U> converter)
        where U : class
        where T : class
    {
        var convertedItems = source.Select(converter).ToList();
        return new PaginatedList<U>(convertedItems, source.TotalItems, source.PageIndex, source.PageSize);
    }
}