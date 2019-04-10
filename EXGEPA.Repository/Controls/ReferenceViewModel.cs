// <copyright file="ReferenceViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EXGEPA.Repository.Controls
{
    using System;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using CORESI.Data;
    using CORESI.Data.Tools;
    using CORESI.IoC;
    using CORESI.Tools.StringTools;
    using CORESI.WPF.Controls;
    using CORESI.WPF.Core.Interfaces;
    using EXGEPA.Model;

    public class ReferenceViewModel : GenericEditableViewModel<Reference>
    {
        private readonly IDataProvider<GeneralAccount> generalAccountService;

        private readonly IDataProvider<ReferenceType> referenceTypeService;

        private readonly IDataProvider<GeneralAccountType> generalAccountTypeService;

        private readonly int referenceTypeKeyLength;

        private Action savePicture;

        private ObservableCollection<ReferenceType> listOfReferenceType;

        private ObservableCollection<GeneralAccount> chargeAccounts;

        private ObservableCollection<GeneralAccount> investmentAccounts;

        public ReferenceViewModel(IExportableGrid exportableView)
            : base(exportableView, false)
        {
            this.Caption = "Liste de réfèrences";
            ServiceLocator.Resolve(out this.generalAccountService);
            ServiceLocator.Resolve(out this.generalAccountTypeService);
            ServiceLocator.Resolve(out this.referenceTypeService);
            this.PicturesDirectory = this.ParameterProvider.TryGet("PicturesDirectory", @"C:\SQLIMMO\Images");
            this.referenceTypeKeyLength = this.ParameterProvider.GetValue(typeof(ReferenceType).Name + "KeyLength", 2);
            this.InitData();
        }

        public ObservableCollection<GeneralAccount> ChargeAccounts
        {
            get => this.chargeAccounts;
            set
            {
                this.chargeAccounts = value;
                this.RaisePropertyChanged("ChargeAccounts");
            }
        }

        public ObservableCollection<GeneralAccount> InvestmentAccounts
        {
            get => this.investmentAccounts;
            set
            {
                this.investmentAccounts = value;
                this.RaisePropertyChanged("InvestmentAccounts");
            }
        }

        public string PicturesDirectory { get; private set; }

        public ObservableCollection<ReferenceType> ListOfReferenceType
        {
            get => this.listOfReferenceType;
            set
            {
                this.listOfReferenceType = value;
                this.RaisePropertyChanged("ListOfReferenceType");
            }
        }

        public ReferenceType ReferenceType
        {
            get => this.ConcernedRow?.ReferenceType;
            set
            {
                this.ConcernedRow.ReferenceType = value;
                this.SetCodes(value);
                this.RaisePropertyChanged("ReferenceType");
            }
        }

        public string Key
        {
            get => this.ConcernedRow?.Key;
            set
            {
                this.ConcernedRow.Key = value;
                this.RaisePropertyChanged("Key");
            }
        }

        public string ImagePath
        {
            get
            {
                if (string.IsNullOrEmpty(this.ConcernedRow?.ImagePath))
                {
                    return null;
                }

                return this.PicturesDirectory + this.ConcernedRow.ImagePath;
            }

            set
            {
                string target = null;
                if (string.IsNullOrEmpty(value))
                {
                    if (!string.IsNullOrEmpty(this.ImagePath))
                    {
                        string s = this.ImagePath;
                        this.savePicture = () => { this.DeleteImage(s); };
                    }
                }
                else
                {
                    target = this.TypeName + this.ConcernedRow.Id.ToString() + Path.GetExtension(value);
                    this.savePicture = () => { this.CopyPicture(value, target); };
                }

                this.ConcernedRow.ImagePath = target;
                this.RaisePropertyChanged("ImagePath");
            }
        }

        public override void InitData()
        {
            this.ListOfReferenceType = new ObservableCollection<ReferenceType>();
            this.StartBackGroundAction(() =>
                {
                    System.Collections.Generic.IList<Reference> toto = this.DBservice.SelectAll(true);
                    this.ListOfRows = new ObservableCollection<Reference>(toto);
                    foreach (ReferenceType item in this.referenceTypeService.SelectAll())
                    {
                        this.ListOfReferenceType.Add(item);
                    }

                    System.Collections.Generic.IList<GeneralAccount> generalAccounts = this.generalAccountService.SelectAll();

                    System.Collections.Generic.IList<GeneralAccountType> types = this.generalAccountTypeService.SelectAll();
                    int chargeAccountTypeId = types.Single(x => x.Type == EGeneralAccountType.Charge).Id;
                    int investmentAccountTypeId = types.FirstOrDefault(x => x.Type == EGeneralAccountType.Investment).Id;

                    this.ChargeAccounts = new ObservableCollection<GeneralAccount>(generalAccounts.Where(x => x.GeneralAccountType.Id == chargeAccountTypeId));
                    this.InvestmentAccounts = new ObservableCollection<GeneralAccount>(generalAccounts.Where(x => x.GeneralAccountType.Id == investmentAccountTypeId));

                    Parallel.ForEach(this.ListOfRows, x =>
                                         {
                                             x.InvestmentAccount = this.InvestmentAccounts.SingleOrDefault(g => g.Id == x.InvestmentAccount?.Id);
                                             x.ChargeAccount = this.ChargeAccounts.SingleOrDefault(g => g.Id == x.ChargeAccount?.Id);
                                             x.ReferenceType = this.ListOfReferenceType.SingleOrDefault(rt => rt.Id == x.ReferenceType?.Id);
                                         });
                });
        }

        public bool IsReadyToGo()
        {
            if (this.ConcernedRow?.ReferenceType == null)
            {
                this.UIMessage.Error($"Vous devez selectionner une famille");
                return false;
            }

            if (!this.ConcernedRow.Caption.IsValidData())
            {
                this.UIMessage.Error($"Vous devez saisir une libellé");
                return false;
            }

            if (!this.ConcernedRow.Key.IsValidData() || !this.ConcernedRow.Key.StartsWith(this.ConcernedRow.ReferenceType.Key))
            {
                this.UIMessage.Error($"Code non valide, le code doit commencer par {this.ConcernedRow.ReferenceType?.Key}");
                return false;
            }

            if (this.ConcernedRow.InvestmentAccount == null)
            {
                this.UIMessage.Error($"Vous devez selectionner un compte investissment");
                return false;
            }

            return true;
        }

        public override void AddItem(Reference RowToInsert)
        {
            if (!this.IsReadyToGo())
            {
                return;
            }

            this.savePicture?.Invoke();

            base.AddItem(RowToInsert);
        }

        public override void UpdateItem()
        {
            if (!this.IsReadyToGo())
            {
                return;
            }

            this.savePicture?.Invoke();

            base.UpdateItem();
        }

        public override void AddItem()
        {
            base.AddItem();
            this.ReferenceType = this.ListOfReferenceType?.FirstOrDefault();
        }

        public override void RaisePropertyChangedForEditionPanel()
        {
            this.RaisePropertyChanged("ImagePath");
            base.RaisePropertyChangedForEditionPanel();
        }

        private void SetCodes(ReferenceType newItem)
        {
            if (newItem != null && this.OldValues == null)
            {
                var count = this.ListOfRows.Count(x => x.ReferenceType.Id == newItem.Id) + 1;
                var length = this.KeyLength - this.referenceTypeKeyLength;
                this.Key = $"{newItem.Key}{count.ToAlignedString(length, "0")}";
            }
        }

        private void CopyPicture(string sourcePath, string target)
        {
            if (!Directory.Exists(this.PicturesDirectory))
            {
                Directory.CreateDirectory(this.PicturesDirectory);
            }

            File.Copy(sourcePath, this.PicturesDirectory + target, true);
        }

        private void DeleteImage(string path)
        {
            File.Delete(path);
        }
    }
}