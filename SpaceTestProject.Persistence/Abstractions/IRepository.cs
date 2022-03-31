using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpaceTestProject.Persistence.Abstractions
{
    public interface IRepository<TEntity> where TEntity: class
    {
        Task<long> CountAsync();
        Task<long> CountAsync(ISpecification<TEntity> specification);

        Task<bool> AnyAsync();
        Task<bool> AnyAsync(ISpecification<TEntity> specification);

        Task<TEntity> GetAsync(ISpecification<TEntity> specification);
        Task<List<TEntity>> GetAllAsync();
        Task<List<TEntity>> GetAllAsync(ISpecification<TEntity> specification);
        Task<List<TEntity>> GetAllAsync(ISpecification<TEntity> specification, ISorting<TEntity> sorting);

        Task<List<TEntity>> GetAllAsync(ISpecification<TEntity> specification, ISorting<TEntity> sorting,
            Limiting limit);

        Task<bool> CreateAsync(TEntity entity);
        Task<bool> UpdateAsync(TEntity entity);

        Task<bool> DeleteAsync(ISpecification<TEntity> specification);
    }
}
