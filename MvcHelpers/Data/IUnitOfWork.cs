using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace MvcHelpers.Data
{
    public interface IUnitOfWork : IDisposable
    {
    	T Find<T>(params object[] keyValues) where T : class;
		IQueryable<T> GetAll<T>() where T : class;
		IQueryable<T> FindBy<T>(Expression<Func<T, bool>> predicate) where T : class;
		void Add<T>(T entity) where T : class;
		void Delete<T>(T entity) where T : class;
		void Edit<T>(T entity) where T : class;
		void SaveChanges();
	
		//IEnumerable<T> Get<T>() where T : class;

		//IEnumerable<T> Get<T>(
		//	Expression<Func<T, bool>> predicate = null,
		//	Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
		//	string includeProperties = ""
		//) where T : class;

		//T Find<T>(params object[] keyValues) where T : class;

		//T Add<T>(T item) where T : class;

		//T Remove<T>(T item) where T : class;

		//void Update<T>(T item) where T : class;

		//int Count<T>(Func<T, bool> predicate = null) where T : class;

		//int SaveChanges();
    }
}
