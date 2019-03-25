// <copyright file="ViewModel.cs" company="PlaceholderCompany">
// Copyright (c) CORESI. All rights reserved.
// </copyright>

namespace CORESI.WPF.Core.Framework
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using CORESI.IoC;
    using CORESI.WPF.Model;

    public class PageViewModel : UiNotifier, IPageSetter
    {
        protected static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected readonly IUIMessage uIMessage;
        protected readonly IUIService uIService;

        public PageViewModel()
        {
            ServiceLocator.GetDefault(out this.uIMessage);
            ServiceLocator.Resolve(out this.uIService);
            this.Groups = new ObservableCollection<Group>();
        }

        public string Caption { get; set; }

        public ObservableCollection<Group> Groups { get; set; }

        public Group AddNewGroup(string caption = null, IList<RibbonButton> buttons = null)
        {
            var group = new Group(caption, buttons);
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

        public Action ClosePage { get; set; }

        public bool IsSelected { get; set; }

        public Categorie Categorie { get; set; }
    }
}
