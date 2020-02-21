using System;
using System.Windows.Controls;
using CORESI.IoC;
using CORESI.WPF.Core.Interfaces;
using CORESI.WPF.Model;
using log4net;
using Page = CORESI.WPF.Model.Page;

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

        protected void AddPage<TView, TViewModel>(TView view, TViewModel viewModel)
            where TViewModel : IPageSetter
            where TView : UserControl, IExportableGrid
        {
            var page = new Page(viewModel, view);
            UIService.AddPage(page);
        }

        protected void AddPage<TView, TViewModel>()
            where TViewModel : IPageSetter
            where TView : UserControl, IExportableGrid
        {
            var view = Activator.CreateInstance<TView>();
            var viewModel = (IPageSetter)Activator.CreateInstance(typeof(TViewModel), new[] { view });
            this.AddPage(view, viewModel);
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
