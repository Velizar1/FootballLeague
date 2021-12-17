using FootballLeague.Core.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FootballLeague.Core.Repositories
{
    public interface IRepository
    {
        RepositoryResult Create<T>(T data) where T : class;
        RepositoryResult Update<T>(T data) where T:class;
        RepositoryResult Delete<T>(T data) where T:class;
        Task<RepositoryResult> CreateAsync<T>(T data) where T : class;
        int Savechanges();
        Task<int> SavechangesAsync();
        IQueryable<T> All<T>() where T: class;
        IQueryable<T> AllReadOnly<T>() where T : class;
        IQueryable<T> All<T>(Expression<Func<T, bool>> expression) where T : class;
        IQueryable<T> AllReadOnly<T>(Expression<Func<T, bool>> expression) where T : class;
        
    }
}
