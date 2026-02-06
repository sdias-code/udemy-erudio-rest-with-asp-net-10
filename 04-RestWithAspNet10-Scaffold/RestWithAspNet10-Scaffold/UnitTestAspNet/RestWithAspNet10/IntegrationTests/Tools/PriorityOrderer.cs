using Xunit.Abstractions;
using Xunit.Sdk;

namespace RestWithAspNet10.IntegrationTests.Tools
{
    public class PriorityOrderer : ITestCaseOrderer
    {
        public IEnumerable<TTestCase> OrderTestCases<TTestCase>
            (IEnumerable<TTestCase> testCases) where TTestCase : ITestCase
        {
            var sortedMethods = testCases.OrderBy(testCase =>
            {
                var priorityAttribute = testCase.TestMethod.Method
                    .GetCustomAttributes(typeof(TestPriorityAttribute))
                    .FirstOrDefault();
                return priorityAttribute != null
                    ? priorityAttribute.GetNamedArgument<int>("Priority")
                    : int.MaxValue;
            });

            return sortedMethods;
        }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class TestPriorityAttribute : Attribute
    {
        public int Priority { get; }

        public TestPriorityAttribute(int priority)
            => Priority = priority;
       


    }
}
