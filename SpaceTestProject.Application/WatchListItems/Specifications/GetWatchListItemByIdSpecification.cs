using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using SpaceTestProject.Domain;
using SpaceTestProject.Persistence.Abstractions;

namespace SpaceTestProject.Application.WatchListItems.Specifications
{
    public class GetWatchListItemByIdSpecification : ISpecification<WatchListItem>
    {
        public Expression<Func<WatchListItem, bool>> Predicate { get; }
        public bool AsNoTracking { get; }
        public Func<IQueryable<WatchListItem>, IIncludableQueryable<WatchListItem, object>>[] Includes { get; }

        public GetWatchListItemByIdSpecification(Guid id)
        {
            Predicate = item => item.Id == id;
            AsNoTracking = false;
            Includes = null;
        }
    }
}
