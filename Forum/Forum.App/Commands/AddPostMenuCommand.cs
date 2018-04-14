namespace Forum.App.Commands
{
    using Forum.App.Contracts;
    using System;

    public class AddPostMenuCommand : ICommand
    {
        private IMenuFactory menuFactory;

        public AddPostMenuCommand(IMenuFactory menuFactory)
        {
            this.menuFactory = menuFactory;
        }

        public IMenu Execute(params string[] args)
        {
            var commandName = this.GetType().Name;
            var menuName = commandName.Substring(0, commandName.Length - "Command".Length);

            var menu = this.menuFactory.CreateMenu(menuName);

            return menu;
        }
    }
}
