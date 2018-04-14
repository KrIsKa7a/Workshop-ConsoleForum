namespace Forum.App.Services
{
    using Forum.App.Contracts;
    using Forum.Data;
    using Forum.DataModels;
    using System;
    using System.Linq;

    public class UserService : IUserService
    {
        private ForumData forumData;
        private ISession session;

        public UserService(ForumData forumData, ISession session)
        {
            this.forumData = forumData;
            this.session = session;
        }

        public User GetUserById(int userId)
        {
            var user = this.forumData.Users
                .FirstOrDefault(u => u.Id == userId);

            return user;
        }

        public string GetUserName(int userId)
        {
            var user = this.forumData.Users
                .FirstOrDefault(u => u.Id == userId);

            return user.Username;
        }

        public bool TryLogInUser(string username, string password)
        {
            if (String.IsNullOrWhiteSpace(username) || String.IsNullOrWhiteSpace(password))
            {
                return false;
            }

            var user = this.forumData.Users
                .FirstOrDefault(u => u.Username == username && u.Password == password);

            if (user == null)
            {
                return false;
            }

            this.session.Reset();
            this.session.LogIn(user);

            return true;
        }

        public bool TrySignUpUser(string username, string password)
        {
            bool validUsername = !String.IsNullOrWhiteSpace(username) && username.Length > 3;
            bool validPassword = !String.IsNullOrWhiteSpace(password) && password.Length > 3;

            if (!validUsername || !validPassword)
            {
                throw new ArgumentException("Username and Password must be longer than 3 symbols!");
            }

            bool userAlreadyExists = this.forumData.Users.Any(u => u.Username == username);

            if (userAlreadyExists)
            {
                throw new InvalidOperationException("Username taken!");
            }

            var userId = forumData.Users.LastOrDefault()?.Id + 1 ?? 1;
            var user = new User(userId, username, password);

            this.forumData.Users.Add(user);
            this.forumData.SaveChanges();

            this.TryLogInUser(username, password);

            return true;
        }
    }
}
