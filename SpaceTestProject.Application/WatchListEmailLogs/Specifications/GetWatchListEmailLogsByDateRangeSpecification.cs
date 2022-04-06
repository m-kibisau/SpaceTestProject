using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using SpaceTestProject.Domain;
using SpaceTestProject.Persistence.Abstractions;

namespace SpaceTestProject.Application.WatchListEmailLogs.Specifications
{
    public class GetWatchListEmailLogsByDateRangeSpecification : ISpecification<WatchListEmailLog>
    {
        public Expression<Func<WatchListEmailLog, bool>> Predicate { get; }
        public bool AsNoTracking { get; }
        public Func<IQueryable<WatchListEmailLog>, IIncludableQueryable<WatchListEmailLog, object>>[] Includes { get; }

        public GetWatchListEmailLogsByDateRangeSpecification(DateTime startTime, DateTime endTime)
        {
            Predicate = log => log.SendingTime <= startTime && log.SendingTime >= endTime;
            AsNoTracking = false;
            Includes = null;
        }
    }
}
