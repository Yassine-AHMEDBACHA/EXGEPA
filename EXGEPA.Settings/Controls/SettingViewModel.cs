using CORESI.Data;
using CORESI.IoC;
using CORESI.WPF;
using CORESI.WPF.Core;
using CORESI.WPF.Core.Framework;
using CORESI.WPF.Model;
using EXGEPA.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace EXGEPA.Settings.Controls
{
    public class SettingViewModel : PageViewModel
    {

        private const string CompanyNameKey = "CompanyName";

        private const string DepartmentNameKey = "DepartmentName";

        private const string DirectionNameKey = "DirectionName";

        private const string ThemeKey = "Theme";

        private const string LogoFileNameKey = "LogoFileName";

        private const string PicturesDirectoryKey = "PicturesDirectory";

        static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IDataProvider<AccountingPeriod> accountingPeriodService;

        private readonly IParameterProvider parameterProvider;

        private readonly IUIService uIService;

        public string PicturesDirectory { get; set; }

        public SettingViewModel()
        {
            this.Caption = "Parameters";
            ServiceLocator.Resolve(out this.uIService);
            ServiceLocator.Resolve(out this.accountingPeriodService);
            ServiceLocator.Resolve(out this.parameterProvider);
            ListOfAccountingPeriod = new ObservableCollection<AccountingPeriod>(accountingPeriodService.SelectAll());
            Group group = new Group();
            group.Commands.Add(new RibbonButton()
            {
                Caption = "Enregistrer",
                LargeGlyph = IconProvider.SaveAndClose,
                Action = Save
            });
            this.Groups.Add(group);
            this.CompanyName = this.parameterProvider.GetAndSetIfMissing(CompanyNameKey, "");
            this.DepartmentName = this.parameterProvider.GetAndSetIfMissing(DepartmentNameKey, "");
            this.DirectionName = this.parameterProvider.GetAndSetIfMissing(DirectionNameKey, "");
            this.AvaibleTheme = GetAvailableThemes();
            this.Theme = this.parameterProvider.GetValue<string>(ThemeKey);
            this.LogoFileName = this.parameterProvider.GetValue<string>(LogoFileNameKey);
            this.PicturesDirectory = this.parameterProvider.GetAndSetIfMissing(PicturesDirectoryKey, @"C:\SQLIMMO\Images");
        }

        private static List<string> GetAvailableThemes()
        {
            return typeof(DevExpress.Xpf.Core.Theme)
                .GetFields()
                .Where(p => p.FieldType == typeof(string) && p.Name.EndsWith("Name") && !p.Name.Contains("Full"))
                .Select(x => x.Name.Replace("Name", ""))
                .ToList();
        }

        private void Save()
        {
            this.UIMessage.TryDoActionAsync(logger, () =>
                {
                    this.parameterProvider.TrySetOrAdd(PicturesDirectoryKey, this.PicturesDirectory);
                    this.parameterProvider.TrySetOrAdd(CompanyNameKey, CompanyName);
                    this.parameterProvider.TrySetOrAdd(DepartmentNameKey, DepartmentName);
                    this.parameterProvider.TrySetOrAdd(DirectionNameKey, DirectionName);
                    if (_SavePicture != null)
                    {
                        this._SavePicture();
                    }
                    this.parameterProvider.TrySetOrAdd(LogoFileNameKey, this.LogoFileName);
                    this.parameterProvider.TrySetOrAdd(ThemeKey, this.Theme);
                });
        }

        private ObservableCollection<AccountingPeriod> _ListOfAccountingPeriod;
        public ObservableCollection<AccountingPeriod> ListOfAccountingPeriod
        {
            get => _ListOfAccountingPeriod;
            set
            {
                _ListOfAccountingPeriod = value;
                RaisePropertyChanged("ListOfAccountingPeriod");
            }
        }

        private string _CompanyName;
        public string CompanyName
        {
            get => _CompanyName;
            set
            {
                _CompanyName = value;
                RaisePropertyChanged("CompanyName");
            }
        }


        private string _DirectionName;
        public string DirectionName
        {
            get => _DirectionName;
            set
            {
                _DirectionName = value;
                RaisePropertyChanged("DirectionName");
            }
        }

        private string _DepartmentName;
        public string DepartmentName
        {
            get => _DepartmentName;
            set
            {
                _DepartmentName = value;
                RaisePropertyChanged("DepartmentName");
            }
        }

        private string _LogoFileName;
        public string LogoFileName
        {
            get => _LogoFileName;
            set
            {
                _LogoFileName = value;
                RaisePropertyChanged("LogoFileName");
            }
        }

        private string _Theme;
        public string Theme
        {
            get => _Theme;
            set
            {
                _Theme = value;
                uIService.SetTheme(value);
                RaisePropertyChanged("Theme");
            }
        }

        private List<string> _AvaibleTheme;
        public List<string> AvaibleTheme
        {
            get => _AvaibleTheme;
            set
            {
                _AvaibleTheme = value;
                RaisePropertyChanged("AvaibleTheme");
            }
        }

        private void CopyPicture(string sourcePath, string targetPath)
        {
            if (!Directory.Exists(PicturesDirectory))
                Directory.CreateDirectory(PicturesDirectory);
            if (targetPath != sourcePath)
            {
                File.Copy(sourcePath, targetPath, true);
            }
        }
        private void DeleteImage(string path)
        {
            File.Delete(path);
        }

        internal Action _SavePicture;

        public string ImagePath
        {
            get => Path.Combine(PicturesDirectory, LogoFileName);

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    string s = this.ImagePath;
                    _SavePicture = () =>
                    {
                        if (this.UIMessage.Warning("Etes vous sur de vouloir supprimer le logo ?") == System.Windows.MessageBoxResult.Yes)
                        {
                            DeleteImage(s);
                        }
                    };
                }
                else
                {
                    _SavePicture = () => { CopyPicture(value, this.ImagePath); };
                }
                RaisePropertyChanged("ImagePath");
            }
        }

    }
}