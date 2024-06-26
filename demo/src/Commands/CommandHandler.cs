﻿using System;
using System.Windows.Input;

namespace Jabra_SDK_Demo.Commands
{
	public class CommandHandler : ICommand
	{
		private Action _action;
		private bool _canExecute;
		public CommandHandler(Action action, bool canExecute)
		{
			_action = action;
			_canExecute = canExecute;
		}

		public bool CanExecute(object parameter)
		{
			return _canExecute;
		}

#pragma warning disable CS0067 // The event 'CommandHandler.CanExecuteChanged' is never used
		public event EventHandler CanExecuteChanged;
#pragma warning restore CS0067 // The event 'CommandHandler.CanExecuteChanged' is never used

		public void Execute(object parameter)
		{
			_action();
		}
	}
	public class CommandHandlerArguments : ICommand
	{
		private Action<object> _action;
		private bool _canExecute;
		public CommandHandlerArguments(Action<object> action, bool canExecute)
		{
			_action = action;
			_canExecute = canExecute;
		}

		public bool CanExecute(object parameter)
		{
			return _canExecute;
		}

#pragma warning disable CS0067 // The event 'CommandHandlerArguments.CanExecuteChanged' is never used
		public event EventHandler CanExecuteChanged;
#pragma warning restore CS0067 // The event 'CommandHandlerArguments.CanExecuteChanged' is never used

		public void Execute(object parameter)
		{
			_action(parameter);
		}
	}
}