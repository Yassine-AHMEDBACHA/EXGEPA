﻿namespace CORESI.WPF
{
    using System.Windows;
    using CORESI.WPF.Model;

    public interface IUIService
    {
        ClientInformation ClientInformation { get; }

        bool AddCategorie(Categorie categorie);
        bool AddGroupToHomePage(Group group);
        bool AddPage(Page page, bool includeCloseButton = true);
        Window CreateShell();
        Group GetGroupPageByCaption(string name);

        Group GetGroupPageByName(string name);

        void IncludeCloseButton(Page page);
        void InitShellInformation(ClientInformation clientInformation);
        bool IsWaintingCursor { get; set; }
        bool RemoveCategorie(Categorie categorie);
        bool RemovePage(Page page);
        void SetApplicationTitle(string applicationTitle);
        void SetTheme(string themeName);
        ClientInformation ShowLoginWindow();
    }
}
