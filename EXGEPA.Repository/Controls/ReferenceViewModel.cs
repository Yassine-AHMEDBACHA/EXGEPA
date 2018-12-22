using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CORESI.Data;
using CORESI.Data.Tools;
using CORESI.IoC;
using CORESI.WPF.Controls;
using CORESI.WPF.Core.Interfaces;
using EXGEPA.Model;

namespace EXGEPA.Repository.Controls
{
    public class ReferenceViewModel : GenericEditableViewModel<Reference>
    {
        private IParameterProvider parameterProvider;

        private IDataProvider<GeneralAccountType> generalAccountTypeService;

        internal Action _SavePicture;

        private ObservableCollection<ReferenceType> _ListOfReferenceType;

        public ReferenceViewModel(IExportable exportableView) : base(exportableView, false)
        {
            this.Caption = "Liste de réfèrences";
            ServiceLocator.Resolve(out this.parameterProvider);
            ServiceLocator.Resolve(out this.generalAccountService);
            ServiceLocator.Resolve(out this.generalAccountTypeService);
            ServiceLocator.Resolve(out this.referenceTypeService);
            this.PicturesDirectory = parameterProvider.GetValue("PicturesDirectory", @"C:\SQLIMMO\Images");
            this.InitData();
        }

        public ObservableCollection<ReferenceType> ListOfReferenceType
        {
            get { return _ListOfReferenceType; }
            set
            {
                _ListOfReferenceType = value;
                RaisePropertyChanged("ListOfReferenceType");
            }
        }

        public ReferenceType ReferenceType
        {
            get { return this.ConcernedRow?.ReferenceType; }
            set
            {
                this.ConcernedRow.ReferenceType = value;
                this.SetCodes(value);
                RaisePropertyChanged("ReferenceType");
            }
        }

        public string Key
        {
            get { return this.ConcernedRow?.Key; }
            set
            {
                this.ConcernedRow.Key = value;
                RaisePropertyChanged("Key");
            }
        }

        private void SetCodes(ReferenceType newItem)
        {
            if (newItem != null && this.OldValues == null)
            {
                var count = this.ListOfRows.Count(x => x.ReferenceType.Id == newItem.Id).ToString(); ;
                while (count.Length < 3)
                {
                    count = "0" + count;
                }
                this.Key = $"{newItem.Key}{count}";
            }
        }

        private ObservableCollection<GeneralAccount> _ChargeAccounts;
        private ObservableCollection<GeneralAccount> _InvestmentAccounts;

        public ObservableCollection<GeneralAccount> ChargeAccounts
        {
            get { return _ChargeAccounts; }
            set
            {
                _ChargeAccounts = value;
                RaisePropertyChanged("ChargeAccounts");
            }
        }

        public ObservableCollection<GeneralAccount> InvestmentAccounts
        {
            get
            {
                return _InvestmentAccounts;
            }
            set
            {
                _InvestmentAccounts = value;
                RaisePropertyChanged("InvestmentAccounts");
            }
        }
        private IDataProvider<GeneralAccount> generalAccountService;
        private IDataProvider<ReferenceType> referenceTypeService;
        public string PicturesDirectory { get; private set; }



        public override void InitData()
        {
            ListOfReferenceType = new ObservableCollection<ReferenceType>();
            StartBackGroundAction(() =>
                {
                    var toto = this.DBservice.SelectAll(true);
                    ListOfRows = new ObservableCollection<Reference>(toto);
                    foreach (var item in this.referenceTypeService.SelectAll())
                    {
                        this.ListOfReferenceType.Add(item);
                    }

                    var generalAccounts = this.generalAccountService.SelectAll();

                    var types = this.generalAccountTypeService.SelectAll();
                    var chargeAccountTypeId = types.Single(x => x.Type == EGeneralAccountType.Charge).Id;
                    var investmentAccountTypeId = types.FirstOrDefault(x => x.Type == EGeneralAccountType.Investment).Id;


                    this.ChargeAccounts = new ObservableCollection<GeneralAccount>(generalAccounts.Where(x => x.GeneralAccountType.Id == chargeAccountTypeId));
                    this.InvestmentAccounts = new ObservableCollection<GeneralAccount>(generalAccounts.Where(x => x.GeneralAccountType.Id == investmentAccountTypeId));

                    Parallel.ForEach(ListOfRows, x =>
                                         {
                                             x.InvestmentAccount = InvestmentAccounts.Single(g => g.Id == x.InvestmentAccount?.Id);
                                             x.ChargeAccount = ChargeAccounts.Single(g => g.Id == x.ChargeAccount.Id);
                                             x.ReferenceType = ListOfReferenceType.Single(rt => rt.Id == x.ReferenceType?.Id);
                                         });
                });
        }

        public string ImagePath
        {
            get
            {
                if (string.IsNullOrEmpty(ConcernedRow?.ImagePath))
                {
                    return null;
                }

                return PicturesDirectory + ConcernedRow.ImagePath;
            }
            set
            {
                string target = null;
                if (string.IsNullOrEmpty(value))
                {
                    if (!string.IsNullOrEmpty(this.ImagePath))
                    {
                        var s = this.ImagePath;
                        _SavePicture = () => { DeleteImage(s); };
                    }
                }
                else
                {
                    target = this.TypeName + this.ConcernedRow.Id.ToString() + Path.GetExtension(value);
                    _SavePicture = () => { CopyPicture(value, target); };
                }
                ConcernedRow.ImagePath = target;
                RaisePropertyChanged("ImagePath");
            }
        }

        private void CopyPicture(string sourcePath, string target)
        {
            if (!Directory.Exists(PicturesDirectory))
                Directory.CreateDirectory(PicturesDirectory);
            File.Copy(sourcePath, PicturesDirectory + target, true);
        }
        private void DeleteImage(string path)
        {
            File.Delete(path);
        }

        public bool IsReadyToGo()
        {
            if (this.ConcernedRow?.ReferenceType == null)
            {
                uIMessage.Error($"Vous devez selectionner une famille");
                return false;
            }

            if (!this.ConcernedRow.Caption.IsValidData())
            {
                uIMessage.Error($"Vous devez saisir une libellé");
                return false;
            }

            if (!this.ConcernedRow.Key.IsValidData() || !this.ConcernedRow.Key.StartsWith(this.ConcernedRow.ReferenceType.Key))
            {
                uIMessage.Error($"Code non valide, le code doit commencer par {this.ConcernedRow.ReferenceType?.Key}");
                return false;
            }
            if (this.ConcernedRow.InvestmentAccount == null)
            {
                uIMessage.Error($"Vous devez selectionner un compte investissment");
                return false;
            }

            return true;
        }

        public override void AddItem(Reference RowToInsert)
        {

            if (!IsReadyToGo())
            {
                return;
            }

            if (_SavePicture != null)
            {
                _SavePicture();
            }

            base.AddItem(RowToInsert);
        }

        public override void UpdateItem()
        {
            if (!IsReadyToGo())
            {
                return;
            }

            if (_SavePicture != null)
            {
                _SavePicture();
            }

            base.UpdateItem();
        }

        public override void AddItem()
        {
            base.AddItem();
            this.ReferenceType = this.ListOfReferenceType?.FirstOrDefault();
        }

        public override void RaisePropertyChangedForEditionPanel()
        {
            RaisePropertyChanged("ImagePath");
            base.RaisePropertyChangedForEditionPanel();
        }
    }
}