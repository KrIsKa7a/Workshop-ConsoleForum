namespace Forum.App.Commands
{
    using Forum.App.Contracts;

    public class ViewCategoryMenuCommand : ICommand
    {
        private IMenuFactory menuFactory;

        public ViewCategoryMenuCommand(IMenuFactory menuFactory)
        {
            this.menuFactory = menuFactory;
        }

        public IMenu Execute(params string[] args)
        {
            var categoryId = int.Parse(args[0]);

            var commandName = this.GetType().Name;
            var menuName = commandName.Substring(0, commandName.Length - "Command".Length);

            IIdHoldingMenu menu = (IIdHoldingMenu)this.menuFactory.CreateMenu(menuName);
            menu.SetId(categoryId);

            return menu;
        }
    }
}
