namespace Forum.App.Contracts
{
    public interface IPostViewModel
    {
        int Id { get; }

		string Title { get; }

		string Author { get; }

		string[] Content { get; }

		IReplyViewModel[] Replies { get; }
    }
}
