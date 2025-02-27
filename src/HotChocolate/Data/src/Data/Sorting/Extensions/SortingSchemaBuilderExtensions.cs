using System;
using HotChocolate.Data;
using HotChocolate.Data.Sorting;

// ReSharper disable once CheckNamespace
namespace HotChocolate;

/// <summary>
/// Provides Sorting extensions for the <see cref="ISchemaBuilder"/>.
/// </summary>
public static class SortingSchemaBuilderExtensions
{
    /// <summary>
    /// Adds Sorting support.
    /// </summary>
    /// <param name="builder">
    /// The <see cref="ISchemaBuilder"/>.
    /// </param>
    /// <param name="name">
    /// The sort convention name.
    /// </param>
    /// <returns>
    /// Returns the <see cref="ISchemaBuilder"/>.
    /// </returns>
    public static ISchemaBuilder AddSorting(
        this ISchemaBuilder builder,
        string? name = null) =>
        AddSorting(builder, x => x.AddDefaults(), name);

    /// <summary>
    /// Adds Sorting support.
    /// </summary>
    /// <param name="builder">
    /// The <see cref="ISchemaBuilder"/>.
    /// </param>
    /// <param name="configure">
    /// Configures the convention.
    /// </param>
    /// <param name="name">
    /// The sort convention name.
    /// </param>
    /// <returns>
    /// Returns the <see cref="ISchemaBuilder"/>.
    /// </returns>
    public static ISchemaBuilder AddSorting(
        this ISchemaBuilder builder,
        Action<ISortConventionDescriptor> configure,
        string? name = null) =>
        builder
            .TryAddConvention<ISortConvention>(_ => new SortConvention(configure), name)
            .TryAddTypeInterceptor<SortTypeInterceptor>();

    /// <summary>
    /// Adds Sorting support.
    /// </summary>
    /// <param name="builder">
    /// The <see cref="ISchemaBuilder"/>.
    /// </param>
    /// <param name="name">
    /// The sort convention name.
    /// </param>
    /// <typeparam name="TConvention">
    /// The concrete sort convention type.
    /// </typeparam>
    /// <returns>
    /// Returns the <see cref="ISchemaBuilder"/>.
    /// </returns>
    public static ISchemaBuilder AddSorting<TConvention>(
        this ISchemaBuilder builder,
        string? name = null)
        where TConvention : class, ISortConvention =>
        builder
            .TryAddConvention<ISortConvention, TConvention>(name)
            .TryAddTypeInterceptor<SortTypeInterceptor>();
}
