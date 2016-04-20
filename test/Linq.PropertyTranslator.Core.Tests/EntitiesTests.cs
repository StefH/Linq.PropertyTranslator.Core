using Linq.PropertyTranslator.Core.Tests.Entities;
using QueryInterceptor.Core;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Linq.PropertyTranslator.Core.Tests
{
    public class EntitiesTests
    {
        [Fact]
        public void PropertyVisitorWithQueryInterceptor()
        {
            var employees = new List<Employee>();
            employees.Add(new Employee { FirstName = "first", LastName = "last" });

            var interceptedQuery = employees.AsQueryable().InterceptWith(new PropertyVisitor());
            Assert.NotNull(interceptedQuery);

            var first = interceptedQuery.FirstOrDefault();
            Assert.NotNull(first);

            Assert.Equal("first last", first.FullName);
        }
    }
}