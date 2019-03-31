using CORESI.Data;
using CORESI.IoC;
using EXGEPA.Label.Core.Model;
using EXGEPA.Model;
using System.Collections.Generic;
using System.Linq;

namespace EXGEPA.Core
{
    public class LabelGeneratorFactoy
    {
        public static ILabelItemGenerator GetGeneraor()
        {

            IParameterProvider parameterProvider = ServiceLocator.Resolve<IParameterProvider>();
            string itemLabelGenratorType = parameterProvider.GetValue<string>("ItemLabelType", "SimpleCodeWithLocalCodeGenerator");
            // string itemLabelGenratorType = "SimpleCodeWithLocalCodeGenerator";
            ILabelItemGenerator generator = null;

            switch (itemLabelGenratorType)
            {
                case ("SimpleCodeWithRegionCodeLabelGenerator"):
                    generator = new SimpleWithRegionGenerator();
                    break;

                case ("SimpleCodeWithLocalCodeGenerator"):
                    generator = new SimpleCodeWithLocalCodeGenerator();
                    break;
                case ("RegionCode_ItemCode_LocalCode"):
                    generator = new RegionCode_ItemCode_LocalCode();
                    break;
                case ("ItemCode"):
                    generator = new ItemCode();
                    break;
                default:
                    generator = new SimpleGenerator();
                    break;

            }

            return generator;
        }
    }

    public class SimpleGenerator : ILabelItemGenerator
    {

        public List<ItemLabel> LoadLabels()
        {
            IDbFacade dbFacade = ServiceLocator.Resolve<IDbFacade>();
            string Query = "SELECT Items.[key] as [KEY],Offices.[key] as OfficeCode ,isnull([SmallDescription],[Description]) as Description,[References].Caption FROM Items INNER JOIN Offices on Items.Office_Id = Offices.Id INNER JOIN [References] ON Reference_Id = [References].Id WHERE (items.PrintLabel = 1)";
            List<ItemLabel> result = dbFacade.ExecuteReader<ItemLabel>(Query, (r) =>
            {
                ItemLabel itemLabel = new ItemLabel()
                {
                    Caption = r["Description"].ToString(),
                    CodeBare = r["KEY"].ToString(),
                    Code = r["KEY"].ToString(),
                    SuffixCode = r["OfficeCode"].ToString(),
                };

                if (string.IsNullOrEmpty(itemLabel.Caption) || string.IsNullOrWhiteSpace(itemLabel.Caption))
                {
                    itemLabel.Caption = r["Caption"].ToString();
                }

                return itemLabel;
            });
            return result;
        }
    }

    public class SimpleWithRegionGenerator : ILabelItemGenerator
    {
        public List<ItemLabel> LoadLabels()
        {
            IDbFacade dbFacade = ServiceLocator.Resolve<IDbFacade>();
            IDataProvider<Region> regionService = ServiceLocator.Resolve<IDataProvider<Region>>();
            Region region = regionService.SelectAll().FirstOrDefault();
            string Query = "SELECT items.[Key],isnull([SmallDescription],[Description]) as Description, Caption FROM Items INNER JOIN [References] ON Reference_Id = [References].Id WHERE (PrintLabel = 1)";
            List<ItemLabel> result = dbFacade.ExecuteReader<ItemLabel>(Query, (r) =>
            {
                ItemLabel itemLabel = new ItemLabel()
                {
                    Caption = r["Description"].ToString(),
                    CodeBare = r["Key"].ToString(),
                    Code = r["Key"].ToString()
                };

                if (string.IsNullOrEmpty(itemLabel.Caption) || string.IsNullOrWhiteSpace(itemLabel.Caption))
                {
                    itemLabel.Caption = r["Caption"].ToString();
                }

                itemLabel.CodeBare = region.Key + itemLabel.CodeBare;
                return itemLabel;
            });
            return result;
        }
    }

    public class SimpleCodeWithLocalCodeGenerator : ILabelItemGenerator
    {
        public List<ItemLabel> LoadLabels()

        {
            IDbFacade dbFacade = ServiceLocator.Resolve<IDbFacade>();
            string Query = "SELECT Items.[key]  as [Key],Offices.[Key] as office ,isnull([SmallDescription],[Description]) as Description,[References].Caption FROM Items INNER JOIN Offices on Items.Office_Id = Offices.Id INNER JOIN [References] ON Reference_Id = [References].Id WHERE (Items.PrintLabel = 1)";
            List<ItemLabel> result = dbFacade.ExecuteReader<ItemLabel>(Query, (r) =>
            {
                ItemLabel itemLabel = new ItemLabel()
                {
                    Caption = r["Description"].ToString(),
                    CodeBare = r["Key"].ToString(),
                    Code = r["Key"].ToString() + "       " + r["office"].ToString()
                };

                if (string.IsNullOrEmpty(itemLabel.Caption) || string.IsNullOrWhiteSpace(itemLabel.Caption))
                {
                    itemLabel.Caption = r["Caption"].ToString();
                }

                return itemLabel;
            });
            return result;
        }
    }

    public class RegionCode_ItemCode_LocalCode : ILabelItemGenerator
    {
        public List<ItemLabel> LoadLabels()
        {
            IDataProvider<Region> regionService = ServiceLocator.Resolve<IDataProvider<Region>>();
            Region region = regionService.SelectAll().FirstOrDefault();
            IDbFacade dbFacade = ServiceLocator.Resolve<IDbFacade>();
            string Query = "SELECT Items.[key]  as [Key],Offices.[Code] as office ,isnull([SmallDescription],[Description]) as Description FROM Items INNER JOIN Offices on Items.Office_Id = Offices.Id WHERE (Items.PrintLabel = 1)";
            List<ItemLabel> result = dbFacade.ExecuteReader<ItemLabel>(Query, (r) =>
            {
                ItemLabel itemLabel = new ItemLabel()
                {
                    Caption = r["Description"].ToString(),
                    CodeBare = r["Key"].ToString(),
                    Code = r["Key"].ToString(),
                    PrefixCode = region.Key,
                    SuffixCode = r["office"].ToString(),
                };
                return itemLabel;
            });
            return result;
        }
    }

    public class ItemCode : ILabelItemGenerator
    {
        public List<ItemLabel> LoadLabels()
        {
            IDbFacade dbFacade = ServiceLocator.Resolve<IDbFacade>();
            string Query = "SELECT Items.[key]  as [Key],isnull([SmallDescription],[Description]) as Description FROM Items WHERE (Items.PrintLabel = 1)";
            List<ItemLabel> result = dbFacade.ExecuteReader<ItemLabel>(Query, (r) =>
            {
                ItemLabel itemLabel = new ItemLabel()
                {
                    Caption = r["Description"].ToString(),
                    CodeBare = r["Key"].ToString(),
                    Code = r["Key"].ToString(),
                };
                return itemLabel;
            });

            return result;
        }
    }

}
