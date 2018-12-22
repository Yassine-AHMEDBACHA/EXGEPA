using System;
using CORESI.IoC;
using CORESI.WPF.Core.Interfaces;
using CORESI.WPF.Model;

namespace CORESI.WPF.Core
{
    public abstract class AModule : IModule
    {
        protected static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public abstract int Priority
        {
            get;
        }

        protected readonly IUIService uIService;

        public AModule()
        {
            ServiceLocator.Resolve(out this.uIService);
        }

        public abstract void AddGroups();

        protected void AddPage<TView, TViewModel>()
            where TViewModel : IPageSetter
            where TView : System.Windows.Controls.UserControl, IExportable
        {
            var view = Activator.CreateInstance<TView>();
            var viewModel = (IPageSetter)Activator.CreateInstance(typeof(TViewModel), new[] { view });
            var page = new Page(viewModel, view);
            uIService.AddPage(page);
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
