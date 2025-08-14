﻿namespace LazyLoading.Application.Common.Pagination;

public sealed record PagedResponse<T>(IReadOnlyList<T> Items, string? NextCursor);