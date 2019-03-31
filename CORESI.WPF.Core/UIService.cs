using CORESI.IoC;
using CORESI.WPF.Core.Shell;
using CORESI.WPF.Model;
using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows;

namespace CORESI.WPF.Core
{
    [Export(typeof(IUIService)), PartCreationPolicy(CreationPolicy.Shared)]
    public class UIService : IUIService
    {
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        internal ShellViewModel ShellViewModel { get; set; }

        [Obsolete]
        public UIService()
        {
            logger.Debug("Start Composing UIService...");
            ApplicationInitializer.Initialize();
            this.ShellViewModel = ServiceLocator.Resolve<ShellViewModel>();
            logger.Info("UIService is ready for use ");
        }

        public bool AddCategorie(Categorie categorie)
        {
            Categorie target = ShellViewModel.Categories.Where(x => x == categorie).FirstOrDefault();
            if (target == null)
            {
                ShellViewModel.Categories.Add(categorie);
            }
            return true;
        }

        public bool RemoveCategorie(Categorie categorie)
        {
            ShellViewModel.Categories.Remove(categorie);
            return true;
        }

        public bool AddGroupToHomePage(Group group)
        {
            _ = ShellViewModel.HomePage.Groups.LastOrDefault();
            ShellViewModel.HomePage.Groups.Insert(0, group);
            return true;
        }

        public Group GetGroupPageByCaption(string Caption)
        {
            Group result = ShellViewModel.HomePage.Groups.FirstOrDefault(x => x.Caption == Caption);
            return result;
        }

        /// <summary>
        /// Not thread Safe
        /// </summary>
        /// <param name="page"></param>
        /// <param name="includeCloseButton"></param>
        /// <returns></returns>
        public bool AddPage(Page page, bool includeCloseButton = true)
        {
            logger.Info("Adding Page to Shell Ribbon : " + page.Caption);

            if (includeCloseButton)
            {
                IncludeCloseButton(page);
            }

            if (ShellViewModel.CurrentPage != null)
            {
                ShellViewModel.CurrentPage.IsSelected = false;
            }

            page.IsSelected = true;

            if (page.Categorie != null)
            {
                AddCategorie(page.Categorie);
                if (!page.Categorie.Pages.Contains(page))
                {
                    page.Categorie.Pages.Add(page);
                }
            }
            else
            {
                ShellViewModel.DefaultCategory.Pages.Add(page);
            }

            page.Close = () => this.RemovePage(page);
            return true;
        }




        public bool RemovePage(Page page)
        {
            logger.Info("Removing Page from Shell Ribbon: " + page.Caption);
            if (ShellViewModel.CurrentPage == page)
            {
                ShellViewModel.CurrentPage = ShellViewModel.HomePage;
                ShellViewModel.UserControl = null;
                GC.Collect(4, GCCollectionMode.Forced);
            }

            foreach (Categorie item in ShellViewModel.Categories.Where(c => c.Pages.Contains(page)).ToList())
            {
                item.Pages.Remove(page);
                if (item.Pages.Count < 1)
                {
                    RemoveCategorie(item);
                }
            }

            return true;

        }

        public void IncludeCloseButton(Page page)
        {
            Group general = page.AddNewGroup();
            general.AddCommand("Fermer", CORESI.WPF.Core.IconProvider.CloseDetail, () => this.RemovePage(page));
        }

        public void SetApplicationTitle(string applicationTitle)
        {
            ShellViewModel.WindowTitle = applicationTitle;
        }

        public void InitShellInformation(ClientInformation clientInformation)
        {
            ShellViewModel.ClientInformation = clientInformation;
        }

        private int IsWaintingCursorCount;
        static object locker = new object();

        public bool IsWaintingCursor
        {
            get => ShellViewModel.IsWaintingCursor;
            set
            {
                lock (locker)
                {
                    if (value)
                        IsWaintingCursorCount++;
                    else
                        IsWaintingCursorCount--;
                    ShellViewModel.IsWaintingCursor = (IsWaintingCursorCount != 0);
                }
            }
        }

        public Window CreateShell()
        {
            return new Shell.Shell();
        }


        public ClientInformation ShowLoginWindow()
        {
            return Login.LoginViewModel.ShowLoginWindow();
        }

        public void SetTheme(string themeName)
        {
            ApplicationInitializer.SetTheme(themeName);
        }
    }
}
