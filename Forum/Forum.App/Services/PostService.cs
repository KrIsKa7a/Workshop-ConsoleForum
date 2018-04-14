namespace Forum.App.Services
{
    using Forum.App.Contracts;
    using Forum.App.ViewModels;
    using Forum.Data;
    using Forum.DataModels;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class PostService : IPostService
    {
        private ForumData forumData;
        private IUserService userService;

        public PostService(ForumData forumData, IUserService userService)
        {
            this.forumData = forumData;
            this.userService = userService;
        }

        public int AddPost(int userId, string postTitle, string postCategory, string postContent)
        {
            bool emptyTitle = String.IsNullOrWhiteSpace(postTitle);
            bool emptyCategory = String.IsNullOrWhiteSpace(postCategory);
            bool emptyContent = String.IsNullOrWhiteSpace(postContent);

            if (emptyTitle || emptyCategory || emptyContent)
            {
                throw new ArgumentException("All fields must be filled!");
            }

            Category category = this.EnsureCategory(postCategory);

            var postId = this.forumData.Posts.Any() ? this.forumData.Posts.Last().Id + 1 : 1;

            var author = this.userService.GetUserById(userId);

            var post = new Post(postId, postTitle, postContent ,category.Id, userId, new List<int>());

            this.forumData.Posts.Add(post);
            author.Posts.Add(post.Id);
            category.Posts.Add(post.Id);
            this.forumData.SaveChanges();

            return postId;
        }

        private Category EnsureCategory(string postCategory)
        {
            bool categoryExists = this.forumData.Categories
                .Any(c => c.Name == postCategory);

            Category category = null;

            if (categoryExists)
            {
                category = this.forumData.Categories.First(c => c.Name == postCategory);
            }
            else
            {
                var categoryId = this.forumData.Categories.LastOrDefault()?.Id + 1 ?? 1;
                category = new Category(categoryId, postCategory, new List<int>());
                this.forumData.Categories.Add(category);
            }

            return category;
        }

        public void AddReplyToPost(int postId, string replyContents, int userId)
        {
            bool emptyContent = String.IsNullOrWhiteSpace(replyContents);

            if (emptyContent)
            {
                throw new ArgumentException("All fields must be filled!");
            }

            Post post = this.EnsurePost(postId);

            var replyId = this.forumData.Replies.Any() ? 
                this.forumData.Posts.Last().Id + 1 : 1;

            var author = this.userService.GetUserById(userId);

            var reply = new Reply(replyId, replyContents, userId, postId);

            this.forumData.Replies.Add(reply);
            post.Replies.Add(reply.Id);
            this.forumData.SaveChanges();
        }

        private Post EnsurePost(int postId)
        {
            bool postExists = this.forumData.Posts
                .Any(p => p.Id == postId);

            Post post = null;

            if (postExists)
            {
                post = this.forumData.Posts.First(p => p.Id == postId);
            }
            else
            {
                throw new ArgumentException("Inexisting post!");
            }

            return post;
        }

        public IEnumerable<ICategoryInfoViewModel> GetAllCategories()
        {
            IEnumerable<ICategoryInfoViewModel> categories = this.forumData
                .Categories
                .Select(c => new CategoryInfoViewModel(c.Id, c.Name, c.Posts.Count));

            return categories;
        }

        public string GetCategoryName(int categoryId)
        {
            var categoryName = this.forumData
                .Categories
                .FirstOrDefault(c => c.Id == categoryId)?
                .Name;

            if (categoryName == null)
            {
                throw new ArgumentException($"Category with id {categoryId} not found!");
            }

            return categoryName;
        }

        public IEnumerable<IPostInfoViewModel> GetCategoryPostsInfo(int categoryId)
        {
            IEnumerable<IPostInfoViewModel> posts = this.forumData
                .Posts
                .Where(p => p.CategoryId == categoryId)
                .Select(p => new PostInfoViewModel(p.Id, p.Title, p.Replies.Count));

            return posts;
        }

        public IPostViewModel GetPostViewModel(int postId)
        {
            var post = this.forumData.Posts
                .FirstOrDefault(p => p.Id == postId);
            var postViewModel = new PostViewModel(postId, post.Title, this.userService.GetUserName(post.AuthorId), post.Content, this.GetPostReplies(postId));

            return postViewModel;
        }

        private IEnumerable<IReplyViewModel> GetPostReplies(int postId)
        {
            var replies = this.forumData.Replies
                .Where(r => r.PostId == postId)
                .Select(r => new ReplyViewModel(this.userService.GetUserName(r.AuthorId), r.Content));

            return replies;
        }
    }
}
