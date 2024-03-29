﻿using CORESI.IoC;
using CORESI.WPF;
using System.IO;
using System.Windows;

namespace EXGEPA.Inventory.Core
{
    public abstract class ADeviceFileManager
    {
        protected const string remoteFileName = @"\invent.txt";

        protected const string tempFile = @"temp.txt";

        protected const string backupFile = @"buckup.txt";

        public ADeviceFileManager()
        {
            this.UIMessage = ServiceLocator.GetPriorizedInstance<IUIMessage>();
        }

        public string TargetPath { get; } = @"invent.txt";

        public IUIMessage UIMessage { get; private set; }

        public abstract bool DownloadFile(string destinationPath = tempFile);

        public bool UpdateInventFile()
        {

            bool updateStatus = false;
            if (File.Exists(TargetPath))
            {
                MessageBoxResult userChoice = UIMessage.Warning(@"Attention, il existe sur votre ordinateur un fichier INVENT non traiter\t\n\n\t
pour le mettre à jour appuyez sur Oui\n\n\t
pour le remplacer appuyer sur Non\n\n\t
pour quitter appuyez sur Annuler", MessageBoxButton.YesNoCancel);

                switch (userChoice)
                {
                    case MessageBoxResult.Cancel:
                        break;
                    case MessageBoxResult.Yes:
                        DownloadFile();
                        File.AppendAllText(TargetPath, File.ReadAllText(tempFile));
                        updateStatus = true;
                        break;
                    case MessageBoxResult.No:
                        DownloadFile();
                        File.Replace(tempFile, TargetPath, backupFile);
                        File.Delete(backupFile);
                        updateStatus = true;
                        break;
                }
            }
            else
            {

                updateStatus = DownloadFile(TargetPath);
            }
            return updateStatus;
        }
    }
}
