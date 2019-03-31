using System;
using CORESI.IoC;
using CORESI.WPF.Core.Interfaces;
using CORESI.WPF.Model;
using log4net;

namespace CORESI.WPF.Core
{
    public abstract class AModule : IModule
    {
        protected static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public AModule()
        {
            this.UIService = ServiceLocator.Resolve<IUIService>();
        }

        public abstract int Priority { get; }

        protected IUIService UIService { get; }

        public abstract void AddGroups();

        protected void AddPage<TView, TViewModel>()
            where TViewModel : IPageSetter
            where TView : System.Windows.Controls.UserControl, IExportableGrid
        {
            TView view = Activator.CreateInstance<TView>();
            IPageSetter viewModel = (IPageSetter)Activator.CreateInstance(typeof(TViewModel), new[] { view });
            Page page = new Page(viewModel, view);
            UIService.AddPage(page);
        }

        public void LoadModule()
        {
            this.AddGroups();
            this.InitializeModule();
        }

        public virtual void InitializeModule()
        {
            return;
        }
    }
}
