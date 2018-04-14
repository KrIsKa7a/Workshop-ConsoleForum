namespace Forum.App.Factories
{
    using Forum.App.Contracts;
    using System;
    using System.Linq;
    using System.Reflection;

    public class MenuFactory : IMenuFactory
    {
        private IServiceProvider serviceProvider;

        public MenuFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public IMenu CreateMenu(string menuName)
        {
            var assembly = Assembly
                .GetExecutingAssembly();

            var type = assembly
                .GetTypes()
                .First(t => t.Name == menuName);

            if (type == null)
            {
                throw new InvalidOperationException("Menu not found!");
            }

            if (!typeof(IMenu).IsAssignableFrom(type))
            {
                throw new InvalidOperationException($"{type} is not a menu!");
            }

            var ctorParams = type.GetConstructors().First().GetParameters();
            var args = new object[ctorParams.Length];

            for (int i = 0; i < ctorParams.Length; i++)
            {
                args[i] = this.serviceProvider.GetService(ctorParams[i].ParameterType);
            }

            IMenu menu = (IMenu)Activator
                .CreateInstance(type, args);

            return menu;
        }
    }
}
