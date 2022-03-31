using System;
using System.Linq.Expressions;
using SpaceTestProject.Persistence.Abstractions.Enums;

namespace SpaceTestProject.Persistence.Abstractions
{
    public interface ISorting<T>
    {
        Expression<Func<T, object>> Selector { get; }

        SortingType SortingType { get; }
    }
}
