namespace Forum.App.Factories
{
    using Contracts;
    using System;
    using System.Linq;
    using System.Reflection;

    public class CommandFactory : ICommandFactory
    {
        private IServiceProvider serviceProvider;

        public CommandFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public ICommand CreateCommand(string commandName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var commandType = assembly
                .GetTypes()
                .First(t => t.Name == commandName + "Command");

            if (commandType == null)
            {
                throw new InvalidOperationException("Command not found!");
            }

            if (!typeof(ICommand).IsAssignableFrom(commandType))
            {
                throw new InvalidOperationException($"{commandType} is not a command!");
            }

            var ctorParams = commandType.GetConstructors().First().GetParameters();
            var args = new object[ctorParams.Length];

            for (int i = 0; i < ctorParams.Length; i++)
            {
                args[i] = this.serviceProvider.GetService(ctorParams[i].ParameterType);
            }

            ICommand command = (ICommand)Activator
                .CreateInstance(commandType, args);

            return command;
        }
        
    }
}