namespace Forum.App.ViewModels
{
    using Forum.App.Contracts;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class ReplyViewModel : ContentViewModel, IReplyViewModel
    {
        public ReplyViewModel(string author, string content) 
            : base(content)
        {
            this.Author = author;
        }

        public string Author { get; }
    }
}
