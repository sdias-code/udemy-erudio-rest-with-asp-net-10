using RestWithAspNet10_Scaffold.Infrastructure.Query;
using System.Linq.Expressions;

namespace RestWithAspNet10.Tests.Query
{
    public class SortingExtensionsTests
    {
        private readonly List<TestEntity> _data =
        [
            new TestEntity { Id = 1, Name = "Carlos", Value = 300 },
            new TestEntity { Id = 2, Name = "Ana", Value = 100 },
            new TestEntity { Id = 3, Name = "Bruno", Value = 200 }
        ];

        private readonly Dictionary<string, Expression<Func<TestEntity, object?>>> _sortMap =
            new(StringComparer.OrdinalIgnoreCase)
            {
                ["name"] = x => x.Name,
                ["value"] = x => x.Value,
                ["id"] = x => x.Id
            };

        [Fact]
        public void ApplySorting_Should_Order_By_Value_Ascending()
        {
            var result = _data
                .AsQueryable()
                .ApplySorting("value", "asc", _sortMap)
                .ToList();

            Assert.Equal(100, result[0].Value);
            Assert.Equal(200, result[1].Value);
            Assert.Equal(300, result[2].Value);
        }

        [Fact]
        public void ApplySorting_Should_Order_By_Value_Descending()
        {
            var result = _data
                .AsQueryable()
                .ApplySorting("value", "desc", _sortMap)
                .ToList();

            Assert.Equal(300, result[0].Value);
            Assert.Equal(200, result[1].Value);
            Assert.Equal(100, result[2].Value);
        }

        [Fact]
        public void ApplySorting_Should_Order_By_Name_Ascending()
        {
            var result = _data
                .AsQueryable()
                .ApplySorting("name", "asc", _sortMap)
                .ToList();

            Assert.Equal("Ana", result[0].Name);
            Assert.Equal("Bruno", result[1].Name);
            Assert.Equal("Carlos", result[2].Name);
        }

        [Fact]
        public void ApplySorting_Should_Return_Unchanged_When_Field_Not_Exists()
        {
            var result = _data
                .AsQueryable()
                .ApplySorting("invalid", "asc", _sortMap)
                .ToList();

            Assert.Equal(_data[0].Id, result[0].Id);
            Assert.Equal(_data[1].Id, result[1].Id);
            Assert.Equal(_data[2].Id, result[2].Id);
        }

        private class TestEntity
        {
            public long Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public int Value { get; set; }
        }
    }
}
