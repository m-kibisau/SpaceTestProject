using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query;
using SpaceTestProject.Domain;
using SpaceTestProject.Persistence.Abstractions;

namespace SpaceTestProject.Application.WatchListItems.Specifications
{
    class GetWatchListItemByUserIdAndWatchedStatusSpecification : ISpecification<WatchListItem>
    {
        public Expression<Func<WatchListItem, bool>> Predicate { get; }
        public bool AsNoTracking { get; }
        public Func<IQueryable<WatchListItem>, IIncludableQueryable<WatchListItem, object>>[] Includes { get; }

        public GetWatchListItemByUserIdAndWatchedStatusSpecification(int userId, bool isWatched)
        {
            Predicate = item => item.UserId == userId && item.IsWatched == isWatched;
            AsNoTracking = false;
            Includes = null;
        }
    }
}
