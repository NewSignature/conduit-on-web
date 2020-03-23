﻿using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Microsoft.EntityFrameworkCore;
using Conduit.Features.Articles;
using Conduit.IntegrationTests.Features.Comments;
using Conduit.IntegrationTests.Features.Users;

namespace Conduit.IntegrationTests.Features.Articles
{
    public class DeleteTests : SliceFixture
    {
        [Fact]
        public async Task Expect_Delete_Article()
        {
            var createCmd = new Create.Command()
            {
                Article = new Create.ArticleData()
                {
                    Title = "Test article dsergiu77",
                    Description = "Description of the test article",
                    Body = "Body of the test article",
                }
            };

            var article = await ArticleHelpers.CreateArticle(this, createCmd);
            var slug = article.Slug;

            var deleteCmd = new Delete.Command(slug);

            var dbContext = GetDbContext();

            var articleDeleteHandler = new Delete.QueryHandler(dbContext);
            await articleDeleteHandler.Handle(deleteCmd, new System.Threading.CancellationToken());

            var dbArticle = await ExecuteDbContextAsync(db => db.Articles.Where(d => d.Slug == deleteCmd.Slug).SingleOrDefaultAsync());

            Assert.Null(dbArticle);
        }

        [Fact]
        public async Task Expect_Delete_Article_With_Tags()
        {
            var createCmd = new Create.Command()
            {
                Article = new Create.ArticleData()
                {
                    Title = "Test article dsergiu77",
                    Description = "Description of the test article",
                    Body = "Body of the test article",
                    TagList = new string[] { "tag1", "tag2" }
                }
            };

            var article = await ArticleHelpers.CreateArticle(this, createCmd);
            var dbArticleWithTags = await ExecuteDbContextAsync(
                db => db.Articles.Include(a => a.ArticleTags)
                .Where(d => d.Slug == article.Slug).SingleOrDefaultAsync()
            );

            var deleteCmd = new Delete.Command(article.Slug);

            var dbContext = GetDbContext();

            var articleDeleteHandler = new Delete.QueryHandler(dbContext);
            await articleDeleteHandler.Handle(deleteCmd, new System.Threading.CancellationToken());

            var dbArticle = await ExecuteDbContextAsync(db => db.Articles.Where(d => d.Slug == deleteCmd.Slug).SingleOrDefaultAsync());
            Assert.Null(dbArticle);
        }

        [Fact]
        public async Task Expect_Delete_Article_With_Comments()
        {
            var createArticleCmd = new Create.Command()
            {
                Article = new Create.ArticleData()
                {
                    Title = "Test article dsergiu77",
                    Description = "Description of the test article",
                    Body = "Body of the test article",
                }
            };

            var article = await ArticleHelpers.CreateArticle(this, createArticleCmd);
            var dbArticle = await ExecuteDbContextAsync(
                db => db.Articles.Include(a => a.ArticleTags)
                .Where(d => d.Slug == article.Slug).SingleOrDefaultAsync()
            );

            var articleId = dbArticle.ArticleId;
            var slug = dbArticle.Slug;

            // create article comment
            var createCommentCmd = new Conduit.Features.Comments.Create.Command()
            {
                Comment = new Conduit.Features.Comments.Create.CommentData()
                {
                    Body = "article comment"
                },
                Slug = slug
            };

            var comment = await CommentHelpers.CreateComment(this, createCommentCmd, UserHelpers.DefaultUserName);

            // delete article with comment
            var deleteCmd = new Delete.Command(slug);

            var dbContext = GetDbContext();

            var articleDeleteHandler = new Delete.QueryHandler(dbContext);
            await articleDeleteHandler.Handle(deleteCmd, new System.Threading.CancellationToken());

            var deleted = await ExecuteDbContextAsync(db => db.Articles.Where(d => d.Slug == deleteCmd.Slug).SingleOrDefaultAsync());
            Assert.Null(deleted);
        }
    }
}
