using System;
using System.Linq;
using FluentAssertions;
using InfraManager.BLL.ServiceDesk.Search;
using NUnit.Framework;

namespace IM.Core.BLL.Test
{
    [TestFixture]
    internal class SearchCacheTests
    {
        [Test]
        public void ItShouldCacheAndReturn()
        {
            var key = "key";
            var cache = new ServiceDeskSearchCache(8);
            var entry = Enumerable.Repeat(new FoundObject(), 8)
                .ToArray();
            cache.Cache(key, entry);
            cache.TryTakeNext(key, 4, out var result);
            result.Should().BeEquivalentTo(entry.Take(4));
            cache.TryTakeNext(key, 4, out result);
            result.Should().BeEquivalentTo(entry.Take(new Range(3,7)));
        }

        [Test]
        public void ItShouldCacheEvenIfEntryIsBiggerThanCapacity()
        {
            var key = "key";
            var cache = new ServiceDeskSearchCache(8);
            var entry = Enumerable.Repeat(new FoundObject(), 10)
                .ToArray();
            cache.Cache(key, entry);
            cache.TryGet(key, out var cached).Should().BeTrue();
            cached.Count().Should().Be(10);
        }

        [Test]
        public void ItShouldDeleteIfCapacityExceeded()
        {
            var key1 = "key";
            var key2 = "another_key";
            var cache = new ServiceDeskSearchCache(8);
            var entry = Enumerable.Repeat(new FoundObject(), 8)
                .ToArray();
            cache.Cache(key1,entry);
            cache.TryGet(key1, out _).Should().BeTrue();
            cache.Cache(key2, entry);
            cache.TryGet(key1, out _).Should().BeFalse();
            cache.TryGet(key2, out _).Should().BeTrue();
        }

        [Test]
        public void ItShouldIntersectById()
        {
            var guid1 = Guid.NewGuid();
            var guid2 = Guid.NewGuid();

            var array = new[] { new FoundObject { ID = guid1 }, new FoundObject { ID = guid2 } };
            var array2 = new[] { new FoundObject { ID = guid1 }, new FoundObject { ID = guid2 } };
            array.Intersect(array2).Count().Should().Be(2);
        }
    }
}
