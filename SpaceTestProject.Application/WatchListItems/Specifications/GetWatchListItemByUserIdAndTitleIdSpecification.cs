using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using SpaceTestProject.Domain;
using SpaceTestProject.Persistence.Abstractions;

namespace SpaceTestProject.Application.WatchListItems.Specifications
{
    class GetWatchListItemByUserIdAndTitleIdSpecification : ISpecification<WatchListItem>
    {
        public Expression<Func<WatchListItem, bool>> Predicate { get; }
        public bool AsNoTracking { get; }
        public Func<IQueryable<WatchListItem>, IIncludableQueryable<WatchListItem, object>>[] Includes { get; }

        public GetWatchListItemByUserIdAndTitleIdSpecification(int userId, string titleId)
        {
            Predicate = item => item.UserId == userId && item.TitleId == titleId;
            AsNoTracking = false;
            Includes = null;
        }
    }
}
