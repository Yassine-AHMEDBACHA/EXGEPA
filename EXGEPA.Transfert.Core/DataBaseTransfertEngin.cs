using CORESI.Data;
using CORESI.DataAccess;
using CORESI.IoC;
using EXGEPA.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using EXGEPA.Depreciations.Core;
using CORESI.Data.Tools;

namespace EXGEPA.Transfert.Core
{
    public class DataBaseTransfertEngin : IDataBaseTransfertEngin
    {
        static string ConnectionString = DBConnectionFactory.GetConnectionStrings("SourceDataBase");
        static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        IDbFacade DbFacade { get; set; }

        public DataBaseTransfertEngin()
        {
            DbFacade = ServiceLocator.Resolve<IDbFacade>();
            PrepareSourceDataBase();
        }

        private static void PrepareSourceDataBase()
        {
            List<string> fixs = new List<string>();
            System.Console.WriteLine(ConnectionString);
            fixs.Add("update equip set codecar = code where codecar is null ");
            fixs.Add("update equip set DAT_ACQI = '1900' where DAT_ACQI is null ");
            fixs.Add("update equip set DATE_SER = [DAT_ACQI] where DATE_SER is null ");
            //fixs.Add("update personnel set Date_naissance = '19001112' where Date_naissance is null	");
            //fixs.Add("select distinct COD_UNI,'          ' as CPT_ANAL into temploc from equip where COD_UNI not in (select code from bureau)");
            //fixs.Add("update temploc set temploc.CPT_ANAL = equip.CPT_ANAL from equip where equip.COD_UNI = temploc.COD_UNI");
            //fixs.Add("insert into bureau ([cod_site],[cod_bat],[cod_niv],[cod_bur],[Code],[Cod_reg],[Cpt_a] )select  SUBSTRING(COD_UNI,4,2),SUBSTRING(COD_UNI,6,2),SUBSTRING(COD_UNI,8,2),SUBSTRING(COD_UNI,10,4), COD_UNI,SUBSTRING(COD_UNI,1,3),CPT_ANAL from temploc");
            //fixs.Add("drop table  temploc");
            //fixs.Add("insert into fic0n([cod_site],[cod_bat],[cod_niv] ) select[cod_site],[cod_bat],[cod_niv]  from bureau where CONCAT([cod_site], [cod_bat], [cod_niv]) not in (select CONCAT([cod_site], [cod_bat], [cod_niv]) from fic0n)");
            //fixs.Add("insert into fic0b ([cod_site],[cod_bat]) select [cod_site],[cod_bat] from fic0n where CONCAT([cod_site],[cod_bat]) not in (select CONCAT([cod_site],[cod_bat]) from fic0b)");
            //fixs.Add("insert into fic0s (cod_site) select cod_site from fic0b where cod_site not in (select cod_site from fic0s)");
            //fixs.Add("delete from  bureau where code not in (select distinct cod_uni from equip ) and cod_site = '' and cod_bat = '' and cod_niv = ''");
            //fixs.Add("update bureau set Cpt_a = equip.CPT_ANAL from equip where equip.COD_UNI = bureau.Code");
            //fixs.Add("update bureau set Cpt_a = Cpt_a + '000S' where Cpt_a not in (select compte_a from fic04) and  Cpt_a + '000S' in (select compte_a from fic04)");
            //fixs.Add("update equip set Cpt_anal = Cpt_anal + '000S' where Cpt_anal not in (select compte_a from fic04) and  Cpt_anal + '000S' in (select compte_a from fic04)");
            //fixs.Add("update equip  set Etat_art = 'Bon' where Etat_art is null or Etat_art ='' or Etat_art  like 'Bon état%' or Etat_art  like 'Bon etat%' ");
            //fixs.Add("update bureau set Cpt_a =(select top 1 compte_a from fic04) where Cpt_a not in (select compte_a from fic04)");

            //fixs.Add("update equip set Date_limite = [DAT_ACQI] where Date_limite is null or Date_limite > '2100'");
            //fixs.Add("update equip set DAT_CIR = DATE_SER where DAT_CIR is null or DAT_CIR > '2100'");
            // fixs.Add("update equip set cod_uni = 'MIZ0104014187' where cod_uni not in (select code from bureau)");
            //fixs.Add("delete Fic0n where  len(cod_niv) = 1 and len(cod_bat) = 1 ");
            //fixs.Add("delete Fic0n where  cod_site is null or cod_niv is null ");
            //fixs.Add("update bureau set cod_niv = '0' +cod_bat where len(cod_niv) = 1 ");
            //fixs.Add("update bureau set cod_bat = '0' +cod_bat where len(cod_bat) = 1 ");
            //fixs.Add("update bureau set Code  = Cod_reg + cod_site +cod_bat +cod_niv+cod_bur where len(code) < 13");
            fixs.ForEach(fix =>
                {
                    logger.Debug("runnig : " + fix);
                    int a = ExcuteNonQuery(fix);
                    logger.Info(a.ToString() + " Row affected");
                });
        }

        public static List<T> LoadData<T>(string Command, Func<SqlDataReader, T> mapper)
        {
            List<T> list = new List<T>();
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand
                {
                    CommandText = Command,
                    CommandTimeout = 60 * 1000 * 5,
                    Connection = sqlConnection
                };
                sqlConnection.Open();

                using (SqlDataReader sqlDataReader = sqlCommand.ExecuteReader())
                {
                    while (sqlDataReader.Read())
                    {

                        list.Add(mapper(sqlDataReader));
                    }
                }
            }
            return list;
        }

        public static int ExcuteNonQuery(string Command)
        {
            int count = 0;
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                SqlCommand sqlCommand = new SqlCommand
                {
                    CommandText = Command,
                    Connection = sqlConnection
                };
                sqlConnection.Open();
                count = sqlCommand.ExecuteNonQuery();
            }
            return count;
        }
        public List<InventoryRow> loadInventory()
        {
            return new List<InventoryRow>();
        }
        public List<Invoice> LoadInvoices()
        {
            string query = "select NUM_FACT ,min(DAT_ACQI) as DAT_ACQI, sum(montant) as montant,max(COD_FOURN) as COD_FOURN,max(NUM_FE) as NUM_FE,min(COMPTE_G) as COMPTE_G from equip where REF_CPTA like 'Facture%' and  NUM_FACT is not null and NUM_FACT not like '' group by NUM_FACT";
            return LoadData<Invoice>(query, (sqlDataReader) =>
            {
                Invoice invoice = new Invoice()
                {
                    Key = sqlDataReader["NUM_FACT"].ToString().TrimEnd(' ').TrimStart(' '),
                };
                invoice.Amount = decimal.Parse(sqlDataReader["montant"].ToString());
                invoice.Provider = new Provider() { Key = sqlDataReader["COD_FOURN"].ToString().TrimEnd(' ').TrimStart(' ') };
                invoice.InputSheet = new InputSheet() { Key = sqlDataReader["NUM_FE"].ToString().TrimEnd(' ').TrimStart(' ') };
                invoice.Date = Convert.ToDateTime(sqlDataReader["DAT_ACQI"]);

                invoice.Type = "Facture";
                invoice.Way = "Achat";
                return invoice;
            });
        }

        public List<TransferOrder> loadTransferOrder()
        {
            string query = "select NUM_FACT ,isnull(min(DAT_ACQI),'19000101') as DAT_ACQI,MAX(FOURN) as FOURN from equip where (REF_CPTA not like 'Facture%' or REF_CPTA is null) and  NUM_FACT is not null and NUM_FACT not like '' group by NUM_FACT";
            return LoadData<TransferOrder>(query, (sqlDataReader) =>
            {
                TransferOrder transferOrder = new TransferOrder()
                {
                    Key = sqlDataReader["NUM_FACT"].ToString().TrimEnd(' ').TrimStart(' '),
                };

                transferOrder.Date = Convert.ToDateTime(sqlDataReader["DAT_ACQI"]);
                transferOrder.Tag = sqlDataReader["FOURN"].ToString().TrimEnd(' ').TrimStart(' ');
                return transferOrder;
            });
        }

        public List<ReceiveOrder> LoadReceiveOrder()
        {
            string query = "select  NUM_BE ,MAX(DAT_ACQI) as DAT_ACQI from equip where NUM_BE is not null and NUM_BE not like '' group by NUM_BE";
            return LoadData<ReceiveOrder>(query, (sqlDataReader) =>
            {
                return new ReceiveOrder()
                {
                    Key = sqlDataReader["NUM_BE"].ToString().TrimEnd(' ').TrimStart(' '),
                    Date = Convert.ToDateTime(sqlDataReader["DAT_ACQI"])
                };
            });
        }

        public List<InputSheet> LoadInputSheet()
        {
            string query = "select NUM_FE, min(DAT_ACQI) as DATE from equip   where NUM_FE is not null and NUM_FE not like '' group by NUM_FE";
            return LoadData<InputSheet>(query, (sqlDataReader) =>
            {
                return new InputSheet()
                {
                    Key = sqlDataReader["NUM_FE"].ToString().TrimEnd(' ').TrimStart(' '),
                    Date = Convert.ToDateTime(sqlDataReader["DATE"])
                };
            });
        }

        public List<Provider> LoadProviders()
        {
            string query = "SELECT isnull([MATRI],row_number() over (order by code)) as code,[RAISON],isnull([ADRES],' ') as adres , isnull([NUM_IF],'') as [NUM_IF]      , isnull([NUM_RC],'') as [NUM_RC]     , isnull(TELE1,'') as TEL,isnull([FAX],'') as FAX   FROM [dbo].[Fic0f]";
            return LoadData<Provider>(query, (sqlDataReader) =>
{
    Provider p = new Provider
    {
        Key = sqlDataReader["code"].ToString().TrimEnd(' ').TrimStart(' '),
        Caption = sqlDataReader["RAISON"].ToString().TrimEnd(' ').TrimStart(' ')
    };
    string n = sqlDataReader["ADRES"].ToString();
    string[] names = n.TrimEnd(' ').Split(' ');
    p.City = names.LastOrDefault();
    p.Address = n.Remove(n.IndexOf(p.City), p.City.Length).TrimEnd(' ').TrimStart(' ');
    p.Country = "Algeria";
    p.FaxNumber = sqlDataReader["FAX"].ToString().TrimEnd(' ').TrimStart(' ');
    p.PhoneNumber = sqlDataReader["TEL"].ToString().TrimEnd(' ').TrimStart(' ');
    p.TradeRegisterId = sqlDataReader["NUM_RC"].ToString().TrimEnd(' ').TrimStart(' ');
    p.TaxId = sqlDataReader["NUM_IF"].ToString().TrimEnd(' ').TrimStart(' ');
    ;
    return p;
});
        }

        public List<Reference> LoadReferences()
        {
            string query = "SELECT  [Cod_famille],[N_ref],[N_design],[Compte_inv],[Compte_charge] FROM [dbo].[fic05]";
            return LoadData<Reference>(query, (sqlDataReader) =>
            {
                return new Reference()
                {
                    Key = sqlDataReader["N_ref"].ToString().TrimEnd(' ').TrimStart(' '),
                    Caption = sqlDataReader["N_design"].ToString().TrimEnd(' ').TrimStart(' '),

                    ReferenceType = new ReferenceType { Key = sqlDataReader["Cod_famille"].ToString().TrimEnd(' ').TrimStart(' '), },
                    InvestmentAccount = new GeneralAccount() { Key = sqlDataReader["Compte_inv"].ToString().TrimEnd(' ').TrimStart(' ') },
                    ChargeAccount = new GeneralAccount() { Key = sqlDataReader["Compte_charge"].ToString().TrimEnd(' ').TrimStart(' ') },
                };
            });
        }

        public List<Person> LoadPeron()
        {

            string query = "SELECT  [Matricule] ,convert(date,[Date_naissance],103) as [Date_naissance] ,[nom_prenom],[fonction]  FROM [dbo].[personnel]";
            return LoadData<Person>(query, (sqlDataReader) =>
            {
                Person p = new Person
                {
                    BirthDate = Convert.ToDateTime(sqlDataReader["Date_naissance"])
                };
                string n = sqlDataReader["nom_prenom"].ToString();

                string[] names = n.TrimEnd(' ').Split(' ');
                p.FirstName = names.LastOrDefault();
                p.Name = n.Remove(n.IndexOf(p.FirstName), p.FirstName.Length).TrimEnd(' ').TrimStart(' ');
                p.Function = sqlDataReader["fonction"].ToString().TrimEnd(' ').TrimStart(' ');
                p.BirthPlace = "Algerie";
                p.Key = sqlDataReader["Matricule"].ToString().TrimEnd(' ').TrimStart(' ');
                return p;
            });
        }

        public List<GeneralAccount> LoadGeneralAccount()
        {
            string query = "SELECT [Compte_inv],[Taux],[Lib_compte_inv],[Compte_amo],[Lib_compte_amo],[Compte_dot],[Lib_compte_dot],[Type_compte]FROM [fic03]";
            return LoadData(query, (sqlDataReader) =>
            {
                decimal.TryParse(sqlDataReader["Taux"].ToString(), out decimal rate);
                GeneralAccount account = BuildGeneralAccountFromDataReader(sqlDataReader, "Compte_inv", "Lib_compte_inv", EGeneralAccountType.Charge);
                if (sqlDataReader["Compte_amo"].ToString().IsValidData())
                {
                    account.Rate = rate;
                    account.GeneralAccountType.Type = EGeneralAccountType.Investment;
                    account.Children = BuildGeneralAccountFromDataReader(sqlDataReader, "Compte_amo", "Lib_compte_amo", EGeneralAccountType.Depreciation);
                    account.Children.Children = BuildGeneralAccountFromDataReader(sqlDataReader, "Compte_dot", "Lib_compte_dot", EGeneralAccountType.Endowment);
                }
                return account;
            });


        }

        private static GeneralAccount BuildGeneralAccountFromDataReader(SqlDataReader sqlDataReader, string keyField, string captionField, EGeneralAccountType type)
        {
            return new GeneralAccount()
            {
                Caption = sqlDataReader[captionField].ToString().TrimEnd(' ').TrimStart(' '),
                Key = sqlDataReader[keyField].ToString().TrimEnd(' ').TrimStart(' '),
                GeneralAccountType = new GeneralAccountType { Type = type }
            };
        }

        public List<Site> LoadSites()
        {
            string query = "SELECT  [cod_site],[Lib_site],[Cod_reg] FROM Fic0s ";
            return LoadData<Site>(query, (sqlDataReader) =>
            {
                Site instance = new Site()
                {
                    Caption = sqlDataReader["Lib_site"].ToString().TrimEnd(' ').TrimStart(' '),
                    Key = sqlDataReader["cod_site"].ToString().TrimEnd(' ').TrimStart(' ').ToUpper(),
                    Region = new Region()
                    {
                        Key = sqlDataReader["Cod_reg"].ToString().TrimEnd(' ').TrimStart(' ').ToUpper(),
                    }
                };
                instance.Code = instance.Key;
                return instance;
            });
        }

        public List<Building> LoadBuildings()
        {
            string query = "SELECT  [cod_site]      ,[cod_bat]      ,[LIB_BAT]  FROM [Fic0b]";
            return LoadData<Building>(query, (sqlDataReader) =>
            {
                Building instance = new Building()
                {

                    Caption = sqlDataReader["LIB_BAT"].ToString().TrimEnd(' ').TrimStart(' '),
                    Code = sqlDataReader["cod_bat"].ToString().TrimEnd(' ').TrimStart(' ').ToUpper(),
                    Site = new Site()
                    {
                        Code = sqlDataReader["cod_site"].ToString().TrimEnd(' ').TrimStart(' ').ToUpper(),
                        Region = new Region()
                        {
                            Key = ""
                        }
                    }
                };
                return instance;
            });
        }

        public List<Level> LoadLevels()
        {
            string query = "SELECT  isnull([cod_bat],'#') as [cod_bat], isnull([cod_site],'#') as [cod_site],isnull([cod_niv],'#') as [cod_niv] FROM [Fic0n]";
            return LoadData<Level>(query, (sqlDataReader) =>
            {
                Level instance = new Level()
                {
                    Caption = sqlDataReader["cod_niv"].ToString().TrimEnd(' ').TrimStart(' '),
                    Code = sqlDataReader["cod_niv"].ToString().TrimEnd(' ').TrimStart(' ').ToUpper(),
                    Building = new Building()
                    {
                        Code = sqlDataReader["cod_bat"].ToString().TrimEnd(' ').TrimStart(' ').ToUpper(),
                        Site = new Site()
                        {
                            Code = sqlDataReader["cod_site"].ToString().TrimEnd(' ').TrimStart(' ').ToUpper(),
                            Region = new Region()
                            {
                                Key = ""
                            }
                        }
                    }
                };

                return instance;
            });


        }

        public List<Region> LoadRegion()
        {
            string query = "SELECT TOP 1 [reg]   FROM [dbo].[parametres]";
            return LoadData(query, (sqlDataReader) =>
                {
                    return new Region()
                    {
                        Caption = sqlDataReader["reg"].ToString().TrimEnd(' ').TrimStart(' '),
                        Key = sqlDataReader["reg"].ToString().TrimEnd(' ').TrimStart(' ').ToUpper(),
                    };
                });
        }

        public string GetCompanyName()
        {
            string query = "SELECT TOP 1 [en_tete1]   FROM [dbo].[parametres]";
            return LoadData(query, (sqlDataReader) =>

                   sqlDataReader["en_tete1"].ToString().TrimEnd(' ').TrimStart(' ')
            ).First();
        }

        public List<Office> LoadOffices()
        {
            string query = "SELECT [Code],[Local],[Cpt_a]  FROM [bureau]";
            return LoadData<Office>(query, (sqlDataReader) =>
            {

                string codeUni = sqlDataReader["Code"].ToString().TrimEnd(' ').TrimStart(' ').ToUpper();
                if (string.IsNullOrEmpty(codeUni) || codeUni.Length != 13)
                    throw new Exception("Invalide localization code");

                return new Office()
                {
                    AnalyticalAccount = new AnalyticalAccount()
                    {
                        Key = sqlDataReader["Cpt_a"].ToString().TrimEnd(' ').TrimStart(' ').ToUpper()
                    },
                    Caption = sqlDataReader["Local"].ToString().TrimEnd(' ').TrimStart(' '),
                    Code = codeUni.Substring(9, 4),
                    Level = new Level()
                    {
                        Code = codeUni.Substring(7, 2).ToUpper(),
                        Building = new Building()
                        {
                            Code = codeUni.Substring(5, 2).ToUpper(),
                            Site = new Site()
                            {
                                Code = codeUni.Substring(3, 2).ToUpper(),
                                Region = new Region()
                                {
                                    Key = codeUni.Substring(0, 3).ToUpper()
                                }
                            }
                        }
                    },
                    Key = codeUni,
                };
            });
        }

        public List<AnalyticalAccountType> LoadAnalyticalAccountTypes()
        {
            return new List<AnalyticalAccountType>()
            {
                new AnalyticalAccountType { Key  ="Internal", Caption = "Interne" },
                new AnalyticalAccountType { Key  ="External", Caption = "Externe" }
            };
        }

        public List<OrderDocumentType> LoadOrderDocumentTypes()
        {
            return new List<OrderDocumentType>()
            {
                new OrderDocumentType { Key  ="Contract", Caption = "Contrat" },
                new OrderDocumentType { Key  ="OrderCommand", Caption = "Bon de commande" }
            };
        }

        public List<AnalyticalAccount> LoadAnalyticalAccount()
        {
            string query = "SELECT [Compte_A]      ,[Libelle_A]      ,[Cod_reg], 'Internal' as Type      FROM [fic04]";
            return LoadData<AnalyticalAccount>(query, (r) =>
            {
                return new AnalyticalAccount()
                {
                    AnalyticalAccountType = new AnalyticalAccountType { Key = r["Type"].ToString().TrimEnd(' ').TrimStart(' ') },
                    Caption = r["Libelle_A"].ToString().TrimEnd(' ').TrimStart(' '),
                    Key = r["Compte_A"].ToString().TrimEnd(' ').TrimStart(' '),
                };
            });
        }



        public List<ItemState> LoadStates()
        {
            string query = "select distinct Etat_art from equip";
            string[] states = new[] { "Neuf", "Bon", "Vétuste" };
            List<ItemState> result = LoadData<ItemState>(query, (sqlDataReader) =>
            {
                ItemState itemState = new ItemState()
                {
                    Key = sqlDataReader["Etat_art"].ToString()
                };
                return itemState;
            }).Where(x => x.Key.IsValidData()).ToList();
            if (result.Count == 0)
            {
                return states.Select(x => new ItemState { Key = x.ToUpperInvariant() }).ToList();
            }

            return result;
        }

        public List<Item> LoadItem()
        {
            string query = "SELECT TAUX_AMO, isnull(N_ELEM,1) as N_ELEM, DESIG,Etat_art, isnull(pv_cession,'') as pv_cession,isnull(pv_disp,'') as pv_disp,isnull(pv_distruction,'') as pv_distruction , isnull(pv_reforme,'') as pv_reforme,[CODECAR],isnull([N_ELEM],0) as N_ELEM,[QUANT],[OBS_ARTICLE],[DATE_SER],[Date_limite] ,[DAT_PIECE],[REF_CPTA],[NUM_FACT],[DAT_FACT],[VAL_REEVALUEE],[AMOR_CUMULE] ,[VALEUR_NETTE],[NUM_FE],[NUM_BE],[COD_UNI],[NUM_INV],isnull([sous_desig],'') as sous_desig,[MARQUE],[NUM_SERIE],[GENRE],isnull([DAT_ACQI],'19000101' ) as [DAT_ACQI],[COD_FOURN],isnull([MONTANT],0) as MONTANT,[COMPTE_G],convert (date,[DAT_CIR],103) as [DAT_CIR],[CPT_ANAL],[PIECE_B],[NUM_BS],[Etat_art],[EMA],[Chemin],isnull([amo_ant_fiscal],0) as [amo_ant_fiscal] FROM [equip]";
            return LoadData<Item>(query, (sqlDataReader) =>
            {
                Item item = new Item
                {
                    Key = $"DEB{sqlDataReader["CODECAR"].ToString().ToUpperInvariant()}",
                    //item.Key = $"{sqlDataReader["CODECAR"].ToString().ToUpperInvariant()}";
                    Description = sqlDataReader["DESIG"].ToString().ToUpperInvariant(),
                    SmallDescription = sqlDataReader["sous_desig"].ToString().ToUpperInvariant()
                };
                if (!item.SmallDescription.IsValidData())
                {
                    item.SmallDescription = item.Description;
                }

                item.PreviousDepreciation = decimal.Parse(sqlDataReader["amo_ant_fiscal"].ToString().ToUpperInvariant());
                item.Amount = decimal.Parse(sqlDataReader["MONTANT"].ToString().ToUpperInvariant());
                item.Office = new Office() { Key = sqlDataReader["COD_UNI"].ToString().ToUpperInvariant() };
                item.Invoice = new Invoice() { Key = sqlDataReader["NUM_FACT"].ToString().ToUpperInvariant() };
                item.TransferOrder = new TransferOrder() { Key = sqlDataReader["NUM_FACT"].ToString().ToUpperInvariant() };
                item.Model = sqlDataReader["GENRE"].ToString().ToUpperInvariant();
                item.Brand = sqlDataReader["MARQUE"].ToString().ToUpperInvariant();
                item.SerialNumber = sqlDataReader["NUM_SERIE"].ToString().ToUpperInvariant();
                item.Reference = new Reference() { Key = sqlDataReader["NUM_INV"].ToString().ToUpperInvariant() };
                item.AnalyticalAccount = new AnalyticalAccount() { Key = sqlDataReader["CPT_ANAL"].ToString().ToUpperInvariant() };
                item.InputSheet = new InputSheet() { Key = sqlDataReader["NUM_FE"].ToString().ToUpperInvariant() };
                item.ReceiveOrder = new ReceiveOrder() { Key = sqlDataReader["NUM_BE"].ToString().ToUpperInvariant() };

                //item.ChargeAccount = new GeneralAccount() { Account = sqlDataReader["COMPTE_G"].ToString() };

                //item.DepreciationAccount = new GeneralAccount() { Account = sqlDataReader["COMPTE_G"].ToString() };

                item.GeneralAccount = new GeneralAccount() { Key = sqlDataReader["COMPTE_G"].ToString().ToUpperInvariant() };

                string dateAcquiString = sqlDataReader["DAT_ACQI"].ToString().ToUpperInvariant();
                item.AquisitionDate = DateTime.Parse(dateAcquiString).Date;
                item.ElementCount = int.Parse(sqlDataReader["N_ELEM"].ToString().ToUpperInvariant());
                string datcir = sqlDataReader["DAT_CIR"].ToString().ToUpperInvariant();
                if (string.IsNullOrEmpty(datcir))
                    datcir = dateAcquiString;

                item.OfficeAssignmentStartDate = DateTime.Parse(datcir).Date;
                item.OldCode = sqlDataReader["EMA"].ToString().ToUpperInvariant();
                string datelimiteString = sqlDataReader["Date_limite"].ToString().ToUpperInvariant();
                if (string.IsNullOrEmpty(datelimiteString))
                    datelimiteString = dateAcquiString;

                string tauxString = sqlDataReader["TAUX_AMO"].ToString().ToUpperInvariant();
                decimal fiscalRate = decimal.Parse(tauxString);
                item.FiscalRate = fiscalRate;
                item.LimiteDate = DateTime.Parse(datelimiteString).Date;
                if (fiscalRate > 0 && datelimiteString == dateAcquiString)
                {
                    item.LimiteDate = DepriciationHelper.GetLimiteDate(fiscalRate, item.AquisitionDate);
                }
                item.StartDepreciationDate = StartDepreciationDate.AqusitionDate;
                item.Comment = sqlDataReader["OBS_ARTICLE"].ToString().ToUpperInvariant();
                string dateServiceString = sqlDataReader["DATE_SER"].ToString().ToUpperInvariant();
                if (string.IsNullOrEmpty(dateServiceString))
                    dateServiceString = dateAcquiString;

                item.StartServiceDate = DateTime.Parse(dateServiceString).Date;
                item.Provider = new Provider() { Key = sqlDataReader["COD_FOURN"].ToString().ToUpperInvariant() };
                item.ElementCount = int.Parse(sqlDataReader["N_ELEM"].ToString().ToUpperInvariant());
                item.ReformeCertificate = new ReformeCertificate() { Key = sqlDataReader["pv_reforme"].ToString().ToUpperInvariant() };
                List<string> Certificates = new List<string>();
                Certificates.Add(sqlDataReader["pv_cession"].ToString().ToUpperInvariant());
                Certificates.Add(sqlDataReader["pv_disp"].ToString().ToUpperInvariant());
                Certificates.Add(sqlDataReader["pv_distruction"].ToString().ToUpperInvariant());
                item.OutputCertificate = new OutputCertificate() { Key = Certificates.Max() };
                item.ItemState = new ItemState() { Key = sqlDataReader["Etat_art"].ToString().ToUpperInvariant() };
                if (!item.ItemState.Key.IsValidData())
                {
                    item.ItemState = null;
                }
                //if (item.AquisitionDate.Year > 2100)
                //    throw new Exception("On aquisition date, item code = " + item.Code);
                return item;
            });

        }

        public List<Assignment> LoadAssignament()
        {
            string query = "select Code,codecar,date_affectation,date_retour from bon_affectation where date_retour is null";
            return LoadData(query, (sqlDataReader) =>
            {
                Assignment assignment = new Assignment
                {
                    Key = sqlDataReader["Code"].ToString(),
                    Item = new Item { Key = sqlDataReader["codecar"].ToString() },
                    StartDate = DateTime.Parse(sqlDataReader["date_affectation"].ToString()),
                    Person = new Person() { Key = sqlDataReader["id"].ToString() }
                };
                return assignment;
            });
        }

        public List<ReformeCertificate> LoadReformsCertificate()
        {
            string query = "select pv_reforme,date_reforme from equip  where pv_reforme is not null and pv_reforme not like '-' group by pv_reforme,date_reforme";
            return LoadData(query, (sqlDataReader) =>
                {
                    return new ReformeCertificate()
                    {
                        Key = sqlDataReader["pv_reforme"].ToString(),
                        Date = DateTime.Parse(sqlDataReader["date_reforme"].ToString())
                    };
                });
        }

        public List<OutputCertificate> LoadOutputCertificate()
        {
            string query = "select pv,date,3 as 'type' from pv_disparition union select pv,date,1 as 'type' from pv_cession union select pv,date,2 as 'type' from pv_distruction";
            return LoadData(query, (sqlDataReader) =>
            {
                return new OutputCertificate()
                {
                    Key = sqlDataReader["pv"].ToString(),
                    Date = DateTime.Parse(sqlDataReader["date"].ToString()),
                    OutputType = (OutputType)int.Parse(sqlDataReader["type"].ToString())
                };
            });
        }

        public List<AccountingPeriod> LoadAccountingPeriods()
        {
            Func<string, string, AccountingPeriod> builder = (year, cloture) =>
            {
                bool approuved = bool.Parse(cloture);
                DateTime startdate = DateTime.Parse(@"01/01/" + year);
                DateTime endtdate = DateTime.Parse(@"31/12/" + year);
                return new AccountingPeriod()
                {
                    Approved = approuved,
                    Key = "Exercice " + year,
                    StartDate = startdate,
                    EndDate = endtdate
                };
            };
            string query = "SELECT [exercice]      ,[cloturer]     FROM [dbo].[exercice]";
            List<AccountingPeriod> exercies = LoadData(query, (sqlDataReader) =>
             {
                 string year = sqlDataReader["exercice"].ToString();
                 string approuved = sqlDataReader["cloturer"].ToString();
                 return builder(year, approuved);
             });

            if (exercies != null)
            {
                return exercies;
            }
            else
            {
                return new List<AccountingPeriod>() { builder(DateTime.Now.Year.ToString(), "false") };
            }

        }

        public List<ReferenceType> LoadReferenceType()
        {
            string query = "SELECT [Cod_famille],[Lib_famille]  FROM [dbo].[Familles]";
            return LoadData(query, (sqlDataReader) =>
            {
                return new ReferenceType()
                {
                    Key = sqlDataReader["Cod_famille"].ToString(),
                    Caption = sqlDataReader["Lib_famille"].ToString()
                };
            });
        }

        public List<GeneralAccountType> ListOfGeneralAccountType()
        {
            List<GeneralAccountType> result = Enum.GetValues(typeof(EGeneralAccountType))
                .OfType<EGeneralAccountType>()
                .Select(x => new GeneralAccountType { Type = x, Key = x.ToString() })
                .ToList();

            return result;
        }
    }
}