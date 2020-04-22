using System;
using System.Linq;
using Linq.PropertyTranslator.Core.Examples.Interfaces;
using Linq.PropertyTranslator.Examples.Interfaces;
using Newtonsoft.Json;
using QueryInterceptor.Core;

namespace Linq.PropertyTranslator.Core.Examples
{
    class Program
    {
        static void Main(string[] args)
        {
            var persons = new IPerson[] { new Student { Name = "s-1", Id = "1234" }, new Teacher { Name = "t-1", Age = 2 } }.AsQueryable();

            var intercepted = persons.InterceptWith(new PropertyVisitor()).ToList();
            Console.WriteLine(JsonConvert.SerializeObject(intercepted, Formatting.Indented));
        }
    }
}