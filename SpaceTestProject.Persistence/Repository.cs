using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SpaceTestProject.Persistence.Abstractions;
using SpaceTestProject.Persistence.Abstractions.Enums;
using SpaceTestProject.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using SpaceTestProject.Persistence.Extensions;

namespace SpaceTestProject.Persistence
{
    public class Repository<TEntity>: IRepository<TEntity> where TEntity: class
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly DbSet<TEntity> _dbSet;

        public Repository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<TEntity>();
        }

        public async Task<long> CountAsync() => await _dbSet.CountAsync();

        public async Task<long> CountAsync(ISpecification<TEntity> specification) =>
            await _dbSet.CountAsync(specification.Predicate);

        public async Task<bool> AnyAsync() => await _dbSet.AnyAsync();

        public async Task<bool> AnyAsync(ISpecification<TEntity> specification) =>
            await _dbSet.AnyAsync(specification.Predicate);

        public async Task<TEntity> GetAsync(ISpecification<TEntity> specification)
        {
            var query = _dbSet.Include(specification.Includes);

            return specification.AsNoTracking
                ? await query.AsNoTracking().FirstOrDefaultAsync(specification.Predicate)
                : await query.FirstOrDefaultAsync(specification.Predicate);
        }

        public async Task<List<TEntity>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<List<TEntity>> GetAllAsync(ISpecification<TEntity> specification)
        {
            var query = _dbSet.Include(specification.Includes);

            if (specification.Predicate != null)
            {
                query = query.Where(specification.Predicate);
            }

            return specification.AsNoTracking ?
                await query.AsNoTracking().ToListAsync() :
                await query.ToListAsync();
        }

        public async Task<List<TEntity>> GetAllAsync(ISpecification<TEntity> specification, ISorting<TEntity> sorting)
        {
            var query = _dbSet.Include(specification.Includes);

            if (specification.Predicate != null)
            {
                query = query.Where(specification.Predicate);
            }

            query = sorting.SortingType == SortingType.Ascending
                ? query.OrderBy(sorting.Selector)
                : query.OrderByDescending(sorting.Selector);

            return specification.AsNoTracking ?
                await query.AsNoTracking().ToListAsync() :
                await query.ToListAsync();
        }

        public async Task<List<TEntity>> GetAllAsync(ISpecification<TEntity> specification, ISorting<TEntity> sorting, Limiting limit)
        {
            var query = _dbSet.Include(specification.Includes);

            if (specification.Predicate != null)
            {
                query = query.Where(specification.Predicate);
            }

            query = sorting.SortingType == SortingType.Ascending
                ? query.OrderBy(sorting.Selector)
                : query.OrderByDescending(sorting.Selector);

            return specification.AsNoTracking ?
                await query.Take(limit.CountOfRecords).AsNoTracking().ToListAsync() :
                await query.Take(limit.CountOfRecords).ToListAsync();
        }

        public async Task<bool> CreateAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            var result = await _dbContext.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> UpdateAsync(TEntity entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            var result = await _dbContext.SaveChangesAsync();

            return result > 0;
        }

        public async Task<bool> DeleteAsync(ISpecification<TEntity> specification)
        {
            var entitiesToDelete = await GetAllAsync(specification);
            _dbSet.RemoveRange(entitiesToDelete);

            var result = await _dbContext.SaveChangesAsync();

            return result > 0;
        }
    }
}
