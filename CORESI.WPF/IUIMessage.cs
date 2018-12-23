using CORESI.IoC;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace CORESI.WPF
{
    public interface IUIMessage : IPriority
    {
        void ConfirmeAndTryDoAction(ILog log, string ConfirmationMessage, Action action, bool IsAsync = false, Action finallyAction = null);
        void Error(string message);
        void Error(ILog log, Exception exception);
        void Information(string message);
        void TryDoAction(ILog log, Action action, Action finallyAction = null);
        void TryDoActionAsync(ILog log, Action action, Action finallyAction = null);

        void TryDoUIActionAsync(ILog log, Action action, Action finallyAction = null);

        MessageBoxResult Warning(string message);
        MessageBoxResult Warning(string message, MessageBoxButton button);

        void Notify(string message);
    }
}
