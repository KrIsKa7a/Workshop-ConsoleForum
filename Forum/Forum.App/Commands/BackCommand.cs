namespace Forum.App.Commands
{
    using Forum.App.Contracts;

    public class BackCommand : ICommand
    {
        private ISession session;

        public BackCommand(ISession session)
        {
            this.session = session;
        }

        public IMenu Execute(params string[] args)
        {
            var menu = this.session.Back();

            return menu;
        }
    }
}
