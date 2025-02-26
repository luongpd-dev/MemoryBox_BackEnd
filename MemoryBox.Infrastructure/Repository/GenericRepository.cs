using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MemoryBox.Application.Utils.Pagination;


using MemoryBox.Domain.Interfaces;
using MemoryBox.Infrastructure.Data;

using Microsoft.EntityFrameworkCore;

namespace MemoryBox.Infrastructure.Repository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        internal MyDbContext context;
        internal DbSet<TEntity> dbSet;
        public IQueryable<TEntity> Entities => dbSet;

        //public IQueryable<TEntity> Entities => throw new NotImplementedException();

        public GenericRepository(MyDbContext context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }

        public virtual IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "",
            int? pageIndex = null,
            int? pageSize = null)
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (pageIndex.HasValue && pageSize.HasValue)
            {
                int validPageIndex = pageIndex.Value > 0 ? pageIndex.Value - 1 : 0;
                int validPageSize = pageSize.Value > 0 ? pageSize.Value : 10;

                query = query.Skip(validPageIndex * validPageSize).Take(validPageSize);
            }

            return query.ToList();
        }

        public virtual TEntity GetByID(object id)
        {
            return dbSet.Find(id);
        }

        public virtual void Insert(TEntity entity)
        {
            dbSet.Add(entity);
            context.SaveChanges();
        }

        public virtual void Delete(object id)
        {
            TEntity entityToDelete = dbSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            dbSet.Attach(entityToUpdate);
            context.Entry(entityToUpdate).State = EntityState.Modified;
        }


        public bool Exists(Expression<Func<TEntity, bool>> filter)
        {
            return dbSet.Any(filter);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return dbSet.AsEnumerable();
        }

        public async Task<IList<TEntity>> GetAllAsync()
        {
            return await dbSet.ToListAsync();
        }

        public async Task<TEntity?> GetByIdAsync(object id)
        {
            return await dbSet.FindAsync(id);
        }

        public async Task<IPaginatedList<TEntity>> GetPagging(IQueryable<TEntity> query, int index, int pageSize)
        {
            query = query.AsNoTracking();
            int count = await query.CountAsync();
            IReadOnlyCollection<TEntity> items = await query.Skip((pageSize - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PaginatedList<TEntity>(items, count, index, pageSize);
        }
        public async Task InsertAsync(TEntity obj)
        {
            await dbSet.AddAsync(obj);
        }

        public void InsertRange(IList<TEntity> obj)
        {
            dbSet.AddRange(obj);
        }
        public void Save()
        {
            context.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }
        public Task UpdateAsync(TEntity obj)
        {
            return Task.FromResult(dbSet.Update(obj));
        }

        public async Task DeleteAsync(object id)
        {
            TEntity entity = await dbSet.FindAsync(id) ?? throw new Exception();
            dbSet.Remove(entity);
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>>? filter = null)
        {
            return filter == null ? await dbSet.CountAsync() : await dbSet.CountAsync(filter);
        }

        public async Task<TEntity?> FirstOrDefaultAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        string includeProperties = ""
)
        {
            IQueryable<TEntity> query = dbSet;

            // Apply the filter if provided
            if (filter != null)
            {
                query = query.Where(filter);
            }

            // Include related properties using Eager Loading
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty.Trim());
                }
            }

            // Return the first entity or default (null) if none match
            return await query.FirstOrDefaultAsync();
        }


    }
}
