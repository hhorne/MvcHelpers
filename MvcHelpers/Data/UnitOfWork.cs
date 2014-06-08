using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;

namespace MvcHelpers.Data
{
	public interface IUnitOfWork : IDisposable
	{
		T Find<T>(params object[] keyValues) where T : class;
		IEnumerable<T> DataSet<T>() where T : class;
		void Add<T>(T entity) where T : class;
		void Delete<T>(T entity) where T : class;
		void Edit<T>(T entity) where T : class;
		bool Any<T>(Expression<Func<T, bool>> predicate) where T : class;
		T SingleOrDefault<T>(Expression<Func<T, bool>> predicate) where T : class;
		int SaveChanges();
	}

	public class UnitOfWork<TContext> : DynamicObject, IUnitOfWork where TContext : DbContext, IDisposable, new()
	{
		private TContext context = new TContext();

		public TContext Context
		{
			get { return context; }
			set { context = value; }
		}

		public T Find<T>(params object[] keyValues) where T : class
		{
			return context.Set<T>().Find(keyValues);
		}

		public IEnumerable<T> DataSet<T>() where T : class
		{
			return context.Set<T>().ToArray();
		}

		public IEnumerable<T> DataSet<T>(Expression<Func<T, bool>> predicate) where T : class
		{
			return context.Set<T>().Where(predicate).ToArray();
		}

		public void Add<T>(T entity) where T : class
		{
			context.Set<T>().Add(entity);
		}

		public void Delete<T>(T entity) where T : class
		{
			context.Set<T>().Remove(entity);
		}

		public void Edit<T>(T entity) where T : class
		{
			context.Entry(entity).State = EntityState.Modified;
		}

		public bool Any<T>(Expression<Func<T, bool>> predicate) where T : class
		{
			return context.Set<T>().Any(predicate);
		}

		public T SingleOrDefault<T>(Expression<Func<T, bool>> predicate) where T : class
		{
			var result = context.Set<T>().SingleOrDefault(predicate);
			return result;
		}

		public int SaveChanges()
		{
			var result = context.SaveChanges();
			return result;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				context.Dispose();
			}
		}
	}
}