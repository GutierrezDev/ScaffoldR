﻿using System;
using System.Diagnostics;
using ScaffoldR.Core.Transactions;
using SimpleInjector;

namespace ScaffoldR.Infrastructure.Transactions
{
    internal sealed class CommandLifetimeScopeDecorator<TCommand> : IHandleCommand<TCommand> where TCommand : ICommand
    {
        private readonly Container _container;
        private readonly Func<IHandleCommand<TCommand>> _handlerFactory;

        public CommandLifetimeScopeDecorator(Container container, Func<IHandleCommand<TCommand>> handlerFactory)
        {
            _container = container;
            _handlerFactory = handlerFactory;
        }

        [DebuggerStepThrough]
        public void Handle(TCommand command)
        {
            if (_container.GetCurrentLifetimeScope() != null)
            {
                _handlerFactory().Handle(command);
            }
            else
            {
                using (_container.BeginLifetimeScope())
                {
                    _handlerFactory().Handle(command);
                }
            }
        }
    }
}
