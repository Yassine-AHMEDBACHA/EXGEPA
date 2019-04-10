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

        public Group AddNewGroup(string caption = null, InsertPosition insertPosition = InsertPosition.Right)
        {
            var group = new Group(caption);
            this.AddGroup(group, insertPosition);
            return group;
        }

        public Group AddNewGroup(string caption, IList<RibbonButton> buttons, InsertPosition insertPosition = InsertPosition.Right)
        {
            var group = new Group(caption, buttons);
            this.AddGroup(group, insertPosition);
            return group;
        }

        public void AddGroup(Group group, InsertPosition insertPosition = InsertPosition.Right)
        {
            if (group != null)
            {
                if (insertPosition == InsertPosition.Right)
                {
                    this.Groups.Add(group);
                }
                else
                {
                    this.Groups.Insert(0, group);
                }
            }
        }

        public enum InsertPosition
        {
            Right = 1,
            Left
        }
    }
}
