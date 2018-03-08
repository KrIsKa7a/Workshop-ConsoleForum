using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Forum.Models;

namespace Forum.Data
{
    public class DataMapper
    {
        private const string DATA_PATH = @"../data/";
        private const string CONFIG_PATH = @"config.ini";
        private static string DEFAULT_CONFIG = GetDefaultConfigString();


        private static readonly Dictionary<string, string> config;

        static DataMapper()
        {
            Directory.CreateDirectory(DATA_PATH);
            config = LoadConfig(DATA_PATH + CONFIG_PATH);
        }
        private static string GetDefaultConfigString()
        {
            return "users=users.csv" + Environment.NewLine + "categories=categories.csv" + Environment.NewLine + 
                "posts=posts.csv" + Environment.NewLine 
                + "replies=replies.csv";
        }

        private static void EnsureConfigFile(string configPath)
        {
            if (!File.Exists(configPath))
            {
                File.WriteAllText(configPath, DEFAULT_CONFIG);
            }
        }

        private static void EnsureFile(string path)
        {
            if (!File.Exists(path))
            {
                File.Create(path).Close();
            }
        }

        private static Dictionary<string, string> LoadConfig(string configPath)
        {
            EnsureConfigFile(configPath);

            var contents = ReadLines(configPath);

            var config = contents
                .Select(l => l.Split('='))
                .ToDictionary(t => t[0], t => DATA_PATH + t[1]);

            return config;
        }

        private static string[] ReadLines(string configPath)
        {
            EnsureFile(configPath);

            var lines = File.ReadAllLines(configPath);

            return lines;
        }

        private static void WriteLines(string path, ICollection<string> lines)
        {
            File.WriteAllLines(path, lines);
        }

        public static List<Category> LoadCategories()
        {
            var categories = new List<Category>();

            var dataLines = ReadLines(config["categories"]);

            foreach (var dataLine in dataLines)
            {
                var dataCategoriesArgs = dataLine
                        .Split(";", StringSplitOptions.RemoveEmptyEntries);

                var id = int.Parse(dataCategoriesArgs[0]);
                var name = dataCategoriesArgs[1];

                Category category = null;

                try
                {
                    var postIds = dataCategoriesArgs[2]
                                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                                .Select(int.Parse)
                                .ToArray();

                    category = new Category(id, name, postIds);
                }
                catch (Exception)
                {
                    category = new Category(id, name, new List<int>());
                }

                categories.Add(category);
            }

            return categories;
        }

        public static void SaveCategories(List<Category> categories)
        {
            var lines = new List<string>();

            foreach (var category in categories)
            {
                const string categoryFormat = @"{0};{1};{2}";

                var id = category.Id;
                var name = category.Name;
                var postIds = category.Posts;

                var line = String.Format(categoryFormat, 
                    id, 
                    name, 
                    String.Join(",", postIds));

                lines.Add(line);
            }

            WriteLines(config["categories"], lines);
        }

        public static List<User> LoadUsers()
        {
            var users = new List<User>();

            var dataLines = ReadLines(config["users"]);

            foreach (var line in dataLines)
            {
                var dataUsersArgs = line
                        .Split(";", StringSplitOptions.RemoveEmptyEntries);

                var id = int.Parse(dataUsersArgs[0]);
                var userName = dataUsersArgs[1];
                var password = dataUsersArgs[2];

                User user = null;

                try
                {
                    var postIds = dataUsersArgs[3]
                                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                                .Select(int.Parse)
                                .ToArray();
                    user = new User(id, userName, password, postIds);
                }
                catch (Exception)
                {
                    user = new User(id, userName, password, new List<int>());
                }

                users.Add(user);
            }

            return users;
        }

        public static void SaveUsers(List<User> users)
        {
            var lines = new List<string>();

            foreach (var user in users)
            {
                const string userFormat = @"{0};{1};{2};{3}";

                var id = user.Id;
                var username = user.Username;
                var password = user.Password;
                var postIds = user.Posts;

                var line = String.Format(userFormat,
                    id,
                    username,
                    password,
                    String.Join(",", postIds));

                lines.Add(line);
            }

            WriteLines(config["users"], lines);
        }

        public static List<Post> LoadPosts()
        {
            var posts = new List<Post>();

            var dataLines = ReadLines(config["posts"]);

            foreach (var line in dataLines)
            {
                var dataPostArgs = line
                        .Split(";", StringSplitOptions.RemoveEmptyEntries);

                var id = int.Parse(dataPostArgs[0]);
                var title = dataPostArgs[1];
                var content = dataPostArgs[2];
                var categoryId = int.Parse(dataPostArgs[3]);
                var authorId = int.Parse(dataPostArgs[4]);

                Post post = null;

                try
                {
                    var replyIds = dataPostArgs[5]
                                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                                .Select(int.Parse)
                                .ToArray();

                    post = new Post(id, title, content, categoryId, authorId, replyIds);
                }
                catch (Exception)
                {
                    post = new Post(id, title, content, categoryId, authorId, new List<int>());
                }

                posts.Add(post);
            }

            return posts;
        }

        public static void SavePosts(List<Post> posts)
        {
            var lines = new List<string>();

            foreach (var post in posts)
            {
                const string postFormat = @"{0};{1};{2};{3};{4};{5}";

                var id = post.Id;
                var title = post.Title;
                var content = post.Content;
                var categoryId = post.CategoryId;
                var authorId = post.AuthorId;
                var replyIds = post.ReplyIds;

                var line = String.Format(postFormat,
                    id,
                    title,
                    content,
                    categoryId,
                    authorId,
                    String.Join(",", replyIds));

                lines.Add(line);
            }

            WriteLines(config["posts"], lines);
        }

        public static List<Reply> LoadReplies()
        {
            var replies = new List<Reply>();

            var dataLines = ReadLines(config["replies"]);

            foreach (var line in dataLines)
            {
                var dataReplyArgs = line
                    .Split(';', StringSplitOptions.RemoveEmptyEntries);

                var id = int.Parse(dataReplyArgs[0]);
                var content = dataReplyArgs[1];
                var authorId = int.Parse(dataReplyArgs[2]);
                var postId = int.Parse(dataReplyArgs[3]);

                var reply = new Reply(id, content, authorId, postId);

                replies.Add(reply);
            }

            return replies;
        }

        public static void SaveReplies(List<Reply> replies)
        {
            var lines = new List<string>();

            foreach (var reply in replies)
            {
                const string replyFormat = @"{0};{1};{2};{3}";

                var id = reply.Id;
                var content = reply.Content;
                var authorId = reply.AuthorId;
                var postId = reply.PostId;

                var line = String.Format(replyFormat,
                    id,
                    content,
                    authorId,
                    postId);

                lines.Add(line);
            }

            WriteLines(config["replies"], lines);
        }
    }
}
