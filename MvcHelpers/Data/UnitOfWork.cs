using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace MvcHelpers.Data
{
	public class UnitOfWork<C> : IUnitOfWork where C : DbContext, new()
	{
		private C _Context = new C();

		public C Context
		{
			get { return _Context; }
			set { _Context = value; }
		}

		public T Find<T>(params object[] keyValues) where T : class
		{
			return _Context.Set<T>().Find(keyValues);
		}

		public IQueryable<T> GetAll<T>() where T : class
    	{
    		return _Context.Set<T>().AsQueryable();
    	}

    	public IQueryable<T> FindBy<T>(Expression<Func<T, bool>> predicate) where T : class
    	{
    		return _Context.Set<T>().Where(predicate);
    	}

    	public void Add<T>(T entity) where T : class
    	{
    		_Context.Set<T>().Add(entity);
    	}

    	public void Delete<T>(T entity) where T : class
    	{
    		_Context.Set<T>().Remove(entity);
    	}

    	public void Edit<T>(T entity) where T : class
    	{
    		_Context.Entry(entity).State = EntityState.Modified;
    	}

    	public void SaveChanges()
    	{
    		_Context.SaveChanges();
    	}

		public void Dispose()
		{
			_Context.Dispose();
		}

		#region
		//private bool _Disposed = false;

		//private readonly DbContext _DbContext;

		//public UnitOfWork(DbContext dbContext)
		//{
		//	_DbContext = dbContext;
		//}

		//public int SaveChanges()
		//{
		//	int retVal = 0;
		//	bool saveFailed = false;

		//	do // An implementation of "Client Wins" concurrency. Essentially, the last person to click save wins.
		//	{
		//		try
		//		{
		//			retVal = _DbContext.SaveChanges();
		//		}
		//		catch (DbUpdateConcurrencyException e)
		//		{
		//			saveFailed = true;
		//			var entry = e.Entries.Single();
		//			entry.OriginalValues.SetValues(entry.GetDatabaseValues());
		//		}

		//	} while (saveFailed);

		//	return retVal;
		//}

		//protected virtual void Dispose(bool disposing)
		//{
		//	if (!_Disposed)
		//	{
		//		if (disposing)
		//		{
		//			_DbContext.Dispose();
		//		}
		//	}

		//	_Disposed = true;
		//}

		//public void Dispose()
		//{
		//	Dispose(true);
		//	GC.SuppressFinalize(this);
		//}

		//public IEnumerable<T> Get<T>() where T : class
		//{
		//	return Get<T>(null, null, "");
		//}

		//// IUnitOfWork
		//public IEnumerable<T> Get<T>(
		//	Expression<Func<T, bool>> predicate = null,
		//	Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
		//	string includeProperties = ""
		//) where T : class
		//{
		//	IQueryable<T> query = _DbContext.Set<T>();

		//	if (predicate != null)
		//	{
		//		query = query.Where(predicate);
		//	}

		//	query = includeProperties.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries)
		//		.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

		//	if (orderBy != null)
		//	{
		//		return orderBy(query).ToList();
		//	}

		//	return query.ToList();
		//}

		//public T Find<T>(params object[] keyValues) where T : class
		//{
		//	return _DbContext.Set<T>().Find(keyValues);
		//}

		//public T Add<T>(T item) where T : class
		//{
		//	return _DbContext.Set<T>().Add(item);
		//}

		//public T Remove<T>(T item) where T : class
		//{
		//	if (_DbContext.Entry(item).State == EntityState.Detached)
		//	{
		//		_DbContext.Set<T>().Attach(item);
		//	}

		//	return _DbContext.Set<T>().Remove(item);
		//}

		//public void Update<T>(T item) where T : class
		//{
		//	_DbContext.Set<T>().Attach(item);
		//	_DbContext.Entry(item).State = EntityState.Modified;
		//}

		//public int Count<T>(Func<T, bool> predicate = null) where T : class
		//{
		//	if (predicate == null)
		//		return _DbContext.Set<T>().Count();
		//	return _DbContext.Set<T>().Count(predicate);
		//}
		#endregion
	}
}
