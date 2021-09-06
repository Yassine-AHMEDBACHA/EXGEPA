using CORESI.WPF.Model;
using EXGEPA.Model;
using System;
using System.Collections.Generic;

namespace EXGEPA.Core.Interfaces
{
    public class ItemAttributionOptions
    {
        public ItemAttributionOptions()
        {
            this.Groups = new List<Group>();
            this.LeftPanelCaption = "Tous les Articles";
            this.RightPanelCaption = "Articles affectés";
            this.SetConfirmationMessage = this.ResetConfirmationMessage = "Etes vous sûr de vouloir déplacer ces items ?";
        }

        public Predicate<Item> Tester { get; set; }

        public Predicate<Item> RowFilter { get; set; }

        public Action<Item> Setter { get; set; }
        public Action<Item> Resetter { get; set; }
        public string PageCaption { get; set; }
        public Categorie Categorie { get; set; }
        public List<Group> Groups { get; private set; }

        public string SetConfirmationMessage { get; set; }
        public string ResetConfirmationMessage { get; set; }

        public string LeftPanelCaption { get; set; }
        public string RightPanelCaption { get; set; }
    }
}
