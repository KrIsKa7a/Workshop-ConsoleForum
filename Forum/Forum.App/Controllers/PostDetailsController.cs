namespace Forum.App.Controllers
{
    using Forum.App.Controllers.Contracts;
    using Forum.App.Services;
    using Forum.App.UserInterface;
    using Forum.App.UserInterface.Contracts;
    using Forum.App.Views;
    using System;

    public class PostDetailsController : IController, IUserRestrictedController
    {
        private enum Command
        {
            Back, AddReply
        }

        public bool LoggedInUser { get; set; }

        public int PostId { get; set; }

        public MenuState ExecuteCommand(int index)
        {
            switch ((Command)index)
            {
                case Command.Back:
                    ForumViewEngine.ResetBuffer();
                    return MenuState.Back;
                case Command.AddReply:
                    return MenuState.AddReplyToPost;
            }

            throw new InvalidOperationException();
        }

        public IView GetView(string userName)
        {
            var pvm = PostService
                .GetPostViewModel(this.PostId);

            return new PostDetailsView(pvm, this.LoggedInUser);
        }

        public void SetPostId(int postId)
        {
            this.PostId = postId;
        }

        public void UserLogIn()
        {
            this.LoggedInUser = true;
        }

        public void UserLogOut()
        {
            this.LoggedInUser = false;
        }
    }
}
