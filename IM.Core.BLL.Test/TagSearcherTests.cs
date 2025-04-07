using System;
using FluentAssertions;
using InfraManager;
using InfraManager.DAL;
using InfraManager.DAL.KnowledgeBase;
using InfraManager.DAL.ServiceDesk;
using NUnit.Framework;

namespace IM.Core.BLL.Test
{
    [TestFixture]
    internal class TagSearcherTests
    {
        [Test]
        public void ItShouldFindTags()
        {
            var ctx = new TestDbContext();
            var expectedTag = new KBTag
            {
                Id = Guid.NewGuid(),
                Name = "Very long tag name"
            };
            ctx.AddRange(expectedTag,
                new KBTag
                {
                    Id = Guid.NewGuid(),
                    Name = "Abc"
                });
            ctx.SaveChanges();

            var query = new TagSearchQuery(ctx.Set<KBTag>());
            var searchResult = query.Query(new SearchCriteria { Text = "on" })
                .Should()
                .ContainSingle().Subject;

            searchResult.ClassID.Should().Be(ObjectClass.KBArticleTag);
            searchResult.ID.Should().Be(expectedTag.Id);
            searchResult.FullName.Should().Be(expectedTag.Name);

            query.Query(new SearchCriteria { Text = "Not Existing Tag" })
                .Should()
                .BeEmpty();
        }
    }
}