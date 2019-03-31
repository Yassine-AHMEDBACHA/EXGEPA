using log4net;
using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using System.Windows;

namespace CORESI.WPF
{
    [Export(typeof(IUIMessage))]
    public class UIMessageBase : IUIMessage
    {
        public virtual int Priority
        {
            get { return 0; }
        }

        #region Error Messages
        public void Error(ILog log, Exception exception)
        {
            log.Error(exception);
            Error(exception.Message);
        }

        public void Error(string message)
        {
            ShowMessage(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        #endregion

        #region Warning Messages
        public MessageBoxResult Warning(string message)
        {
            return Warning(message, MessageBoxButton.YesNo);
        }

        public MessageBoxResult Warning(string message, MessageBoxButton button)
        {
            return ShowMessage(message, "Warning", button, MessageBoxImage.Warning);
        }
        #endregion

        #region Information Messages
        public void Information(string message)
        {
            ShowMessage(message, "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        #endregion

        #region Messages
        MessageBoxResult ShowMessage(string message, string title, MessageBoxButton buttons, MessageBoxImage image)
        {
            if (Application.Current != null)
            {
                var result = Application.Current.Dispatcher.Invoke((Func<MessageBoxResult>)(() =>
                {
                    return ShowUiMessageBox(message, title, buttons, image);
                }));
                return (MessageBoxResult)result;
            }
            else
                return ShowUiMessageBox(message, title, buttons, image);
        }
        #endregion

        protected virtual MessageBoxResult ShowUiMessageBox(string message, string title, MessageBoxButton buttons, MessageBoxImage image)
        {
            return MessageBox.Show(message, title, buttons, image);
        }

        public void TryDoAction(ILog log, Action action, Action finallyAction = null)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                Error(log, ex);
            }
            finally
            {
                finallyAction?.Invoke();
            }
        }

        public void TryDoActionAsync(ILog log, Action action, Action finallyAction = null)
        {
            var task = new Task(() =>
            {
                TryDoAction(log, action, finallyAction);
            });
            task.Start();
        }

        public void ConfirmeAndTryDoAction(ILog log, string ConfirmationMessage, Action action, bool IsAsync = false, Action finallyAction = null)
        {
            if (this.Warning(ConfirmationMessage) == MessageBoxResult.Yes)
            {
                if (IsAsync)
                {
                    TryDoActionAsync(log, action, finallyAction);
                }
                else
                {
                    TryDoAction(log, action, finallyAction);
                }
            }
            else
            {
                finallyAction?.Invoke();
            }
        }

        public void TryDoUIActionAsync(ILog log, Action action, Action finallyAction = null)
        {
            try
            {
                if (Application.Current != null)
                {
                    Application.Current.Dispatcher.Invoke(action);
                }
                else
                    action();
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            finally
            {
                if (finallyAction != null)
                {
                    this.TryDoAction(log, finallyAction);
                }
            }
        }

        public virtual void Notify(string message)
        {
            this.Information(message);
        }
    }
}
