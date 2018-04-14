namespace Forum.App.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ContentViewModel
    {
        private const int LINE_LENGHT = 37;

        public ContentViewModel(string content)
        {
            this.Content = this.GetLines(content);
        }

        public string[] Content { get; }

        private string[] GetLines(string content)
        {
            var contentChars = content.ToCharArray();

            ICollection<string> lines = new List<string>();

            for (int i = 0; i < content.Length; i += LINE_LENGHT)
            {
                var row = contentChars.Skip(i).Take(LINE_LENGHT).ToArray();
                string rowString = String.Join("", row);
                lines.Add(rowString);
            }

            return lines.ToArray();
        }
    }
}
