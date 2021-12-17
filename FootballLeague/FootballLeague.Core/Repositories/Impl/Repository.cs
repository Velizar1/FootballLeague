using FootballLeague.Core.Commons;
using FootballLeague.Core.Constants;
using FootballLeague.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FootballLeague.Core.Repositories.Impl
{
    public class Repository : IRepository
    {
        protected ApplicationDbContext context;

        public Repository(ApplicationDbContext _context)
        {
            context = _context;
        }

        public IQueryable<T> All<T>() where T : class
        {
            return context.Set<T>().AsQueryable();
        }

        public IQueryable<T> All<T>(Expression<Func<T, bool>> expression) where T : class
        {
            return context.Set<T>().Where(expression).AsQueryable();
        }

        public IQueryable<T> AllReadOnly<T>() where T : class
        {
            return context.Set<T>().AsQueryable().AsNoTracking();
        }

        public IQueryable<T> AllReadOnly<T>(Expression<Func<T, bool>> expression) where T : class
        {
            return context.Set<T>().AsQueryable().Where(expression).AsNoTracking();
        }

        public RepositoryResult Create<T>(T data) where T : class
        {
            try
            {
                var entity = context.Set<T>().Add(data);
                if (entity == null)
                {
                    new RepositoryResult(ResultConstants.Success, ResultConstants.CreateFailed);
                }
                return new RepositoryResult(ResultConstants.Success, ResultConstants.CreateSucceeded);
            }
            catch (Exception ex)
            {
                throw new Exception(ResultConstants.CreateFailed + typeof(T).ToString(), ex);
            }
        }
        public async Task<RepositoryResult> CreateAsync<T>(T data) where T : class
        {
            try
            {
                var entity = await context.Set<T>().AddAsync(data);
                if (entity == null)
                {
                    new RepositoryResult(ResultConstants.Success, ResultConstants.CreateFailed);
                }

                return new RepositoryResult(ResultConstants.Success, ResultConstants.CreateSucceeded);
            }
            catch (Exception ex)
            {
                throw new Exception(ResultConstants.CreateFailed + typeof(T).ToString(), ex);
            }
        }

        public RepositoryResult Delete<T>(T data) where T : class
        {
            try
            {
                var entity = context.Set<T>().Remove(data);
                if (entity == null)
                {
                    new RepositoryResult(ResultConstants.Success, ResultConstants.CreateFailed);
                }

                return new RepositoryResult(ResultConstants.Success, ResultConstants.DeleteSucceeded);
            }
            catch (Exception ex)
            {
                throw new Exception(ResultConstants.DeleteFailed + typeof(T).ToString(), ex);
            }
        }

        public RepositoryResult Update<T>(T data) where T : class
        {
            try
            {
                var entity = context.Set<T>().Update(data);
                if (entity == null)
                {
                    new RepositoryResult(ResultConstants.Success, ResultConstants.CreateFailed);
                }

                return new RepositoryResult(ResultConstants.Success, ResultConstants.UpdateSucceeded);
            }
            catch (Exception ex)
            {
                throw new Exception(ResultConstants.UpdateFailed + typeof(T).ToString(), ex);
            }
        }
        public int Savechanges()
        {
            try
            {
                return context.SaveChanges();
            }
            catch (DbUpdateException dbEx)
            {
                throw new DbUpdateException(ResultConstants.SaveFailed,dbEx);
            }
            
        }

        public async Task<int> SavechangesAsync()
        {
            try
            {
                return await context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                throw new DbUpdateException(ResultConstants.SaveFailed,dbEx);
            }
        }

    }
}
