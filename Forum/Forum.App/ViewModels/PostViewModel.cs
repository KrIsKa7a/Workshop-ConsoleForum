namespace Forum.App.ViewModels
{
    using Forum.App.Contracts;
    using System.Collections.Generic;
    using System.Linq;

    public class PostViewModel : ContentViewModel, IPostViewModel
    {
        public PostViewModel(int id, string title, string author, string content, IEnumerable<IReplyViewModel> replies) 
            : base(content)
        {
            this.Id = id;
            this.Title = title;
            this.Author = author;
            this.Replies = replies.ToArray();
        }

        public int Id { get; }

        public string Title { get; }

        public string Author { get; }

        public IReplyViewModel[] Replies { get; }
    }
}
