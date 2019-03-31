// <copyright file="ViewModel.cs" company="PlaceholderCompany">
// Copyright (c) CORESI. All rights reserved.
// </copyright>

namespace CORESI.WPF.Core.Framework
{
    using CORESI.IoC;
    using CORESI.WPF.Model;
    using log4net;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Reflection;

    public class PageViewModel : UiNotifier, IPageSetter
    {
        private readonly IUIService uIService;

        public PageViewModel()
        {
            this.UIMessage = ServiceLocator.GetDefault<IUIMessage>();
            ServiceLocator.Resolve(out this.uIService);
            Groups = new ObservableCollection<Group>();
        }

        protected IUIMessage UIMessage { get; }

        protected ILog Logger { get; } = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public string Caption { get; set; }

        public Action ClosePage { get; set; }

        public bool IsSelected { get; set; }

        public Categorie Categorie { get; set; }

        public ObservableCollection<Group> Groups { get; set; }

        protected IUIService UIService => uIService;

        public Group AddNewGroup(string caption = null, IList<RibbonButton> buttons = null)
        {
            Group group = new Group(caption, buttons);
            this.AddGroup(group);
            return group;
        }

        public void AddGroup(Group group)
        {
            if (group != null)
            {
                this.Groups.Add(group);
            }
        }
    }
}
