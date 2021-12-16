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
            this.context = _context;
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
                context.Set<T>().Add(data);
                return new RepositoryResult(ResultConstants.Success, ResultConstants.CreateSucceeded);
            }
            catch (Exception)
            {
                throw new Exception(ResultConstants.CreateFailed);
            }
        }
        public async Task<RepositoryResult> CreateAsync<T>(T data) where T : class
        {
            try
            {
                await context.Set<T>().AddAsync(data); 
                return new RepositoryResult(ResultConstants.Success,ResultConstants.CreateSucceeded);
            }
            catch(Exception)
            {
               
                throw new Exception(ResultConstants.CreateFailed);
            }
        }

        public RepositoryResult Delete<T>(T data) where T : class
        {
            try
            {
                context.Set<T>().Remove(data);
                return new RepositoryResult(ResultConstants.Success, ResultConstants.DeleteSucceeded);
            }
            catch (Exception)
            {
                throw new Exception(ResultConstants.DeleteFailed);
            }
        }

        public RepositoryResult Update<T>(T data) where T : class
        {
            try
            {
                context.Set<T>().Update(data);
                return new RepositoryResult(ResultConstants.Success, ResultConstants.UpdateSucceeded);
            }
            catch (Exception)
            {
                throw new Exception(ResultConstants.UpdateFailed);
            }
        }
        public int Savechanges()
        {
            try
            {
                return context.SaveChanges();
            }
            catch (Exception)
            {
                throw new Exception(ResultConstants.SaveFailed);
            }
        }

        public async Task<int> SavechangesAsync()
        {
            try
            {
                return await context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new Exception(ResultConstants.SaveFailed);
            }
        }

    }
}
