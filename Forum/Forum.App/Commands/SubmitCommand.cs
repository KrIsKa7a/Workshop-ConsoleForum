namespace Forum.App.Commands
{
    using Forum.App.Contracts;

    public class SubmitCommand : ICommand
    {
        private ISession session;
        private IPostService postService;
        private ICommandFactory commandFactory;

        public SubmitCommand(ISession session, IPostService postService, ICommandFactory commandFactory)
        {
            this.session = session;
            this.postService = postService;
            this.commandFactory = commandFactory;
        }

        public IMenu Execute(params string[] args)
        {
            var userId = this.session.UserId;

            var replyContent = args[0];
            var postId = int.Parse(args[1]);

            this.postService.AddReplyToPost(postId, replyContent, userId);

            this.session.Back();

            var command = this.commandFactory.CreateCommand("ViewPostMenu");
            var commandView = command.Execute(postId.ToString());

            return commandView;
        }
    }
}
