using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo.Data;
using Todo.Data.Entities;
using Todo.Services.Impl;
using Todo.Web.ViewModels;

namespace Todo.Tests
{
    public class MockFactory
    {
        public static Mock<IDbSet<TEntity>> CreateMockDbset<TEntity>(IList<TEntity> sourceList) where TEntity : class
        {
            var queryable = sourceList.AsQueryable();
            var dbsetMock = new Mock<IDbSet<TEntity>>();
            dbsetMock.As<IDbAsyncEnumerable<TEntity>>()
                .Setup(m => m.GetAsyncEnumerator())
                .Returns(new TestDbAsyncEnumerator<TEntity>(queryable.GetEnumerator()));

            dbsetMock.As<IQueryable<TEntity>>()
                .Setup(m => m.Provider)
                .Returns(new TestDbAsyncQueryProvider<TEntity>(queryable.Provider));

            dbsetMock.As<IQueryable<TEntity>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbsetMock.As<IQueryable<TEntity>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbsetMock.As<IQueryable<TEntity>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());

            return dbsetMock;
        }
    }
}
