namespace Forum.App.Commands
{
    using Forum.App.Contracts;
    using Forum.App.Menus;

    public class AddReplyCommand : ICommand
    {
        private IMenuFactory menuFactory;

        public AddReplyCommand(IMenuFactory menuFactory)
        {
            this.menuFactory = menuFactory;
        }

        public IMenu Execute(params string[] args)
        {
            var postId = int.Parse(args[0]);

            var commandName = this.GetType().Name;
            var menuName = commandName.Substring(0, commandName.Length - "Command".Length);
            var menu = (AddReplyMenu)this.menuFactory.CreateMenu(menuName + "Menu");
            menu.SetId(postId);
            menu.InitializeTextArea();

            return menu;
        }
    }
}
