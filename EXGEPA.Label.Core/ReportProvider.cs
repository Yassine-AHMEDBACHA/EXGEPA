using System.Collections.Generic;
using System.IO;
using System.Linq;
using CORESI.Data;
using CORESI.IoC;
using CORESI.Report.Controls;
using CORESI.Tools.Collections;
using CORESI.WPF;
using CORESI.WPF.Core;
using CORESI.WPF.Model;
using DevExpress.XtraReports.UI;
using EXGEPA.Core;
using EXGEPA.Label.Core.Model;
using EXGEPA.Model;

namespace EXGEPA.Label.Core
{
    public class ReportProvider
    {
        private static IUIMessage uIMessage;
        private static IParameterProvider parameterProvider;
        private static bool useMatrixBarCode;
        private static string shortCompanyName;
        private static string companyName;
        private static string officeLevelSeparator;

        static ReportProvider()
        {
            uIMessage = ServiceLocator.GetPriorizedInstance<IUIMessage>();
            ServiceLocator.Resolve(out parameterProvider);
            useMatrixBarCode = parameterProvider.TryGet("UseMatrixBarCode", false);

            shortCompanyName = parameterProvider.TryGet("ShortCompanyName", string.Empty);
            companyName = parameterProvider.GetValue<string>("CompanyName");
            officeLevelSeparator = parameterProvider.TryGet("OfficeLevelSeparator", "*");

        }

        public static Group GetOfficeLabelDialog()
        {
            Group group = new Group();
            group.Commands.Add(new RibbonButton()
            {
                Caption = "Imprimer",
                LargeGlyph = IconProvider.PrintDialog,
                Action =
                () =>
                {
                    var dataSource = new List<OfficeLabel>();

                    var officeServices = ServiceLocator.Resolve<IDataProvider<Office>>();
                    var listOfOffice = officeServices.SelectAll();
                    listOfOffice.Where(x => x.PrintLabel).ForEach(x =>
                        {
                            var label = new OfficeLabel
                            {
                                CodeBare = x.Key,
                                OfficeCaption = x.Caption,
                                CodeOffice = x.Code,
                                CodeLevel = x.Level.Code,
                                CodeBuilding = x.Level.Building.Code,
                                CodeSite = x.Level.Building.Site.Code,
                                CodeRegion = x.Level.Building.Site.Region.Key,
                                Code = $"{x.Level.Code}{officeLevelSeparator}{x.Code}"
                            };
                            dataSource.Add(label);
                        });
                    if (dataSource.Count > 0)
                    {
                        var parameterProvider = ServiceLocator.Resolve<IParameterProvider>();
                        var logo = Path.Combine(parameterProvider.GetValue("PicturesDirectory", @"C:\SQLIMMO\Images"), parameterProvider.GetValue("LogoFileName", "logo.jpg"));
                        var ettiquete = GetOfficeLabel(dataSource, logo);
                        ettiquete.CreateDocument();
                        var page = ReportViewModel.GetModulePage("Etiquettes Locaux", ettiquete);
                        var uIService = ServiceLocator.Resolve<IUIService>();
                        uIService.AddPage(page);
                    }
                    else
                    {
                        uIMessage.Information("Aucune Etiquette à imprimer !");
                    }
                }
            });

            return group;

        }

        public static Group GetItemLabelDialog()
        {
            Group group = new Group();
            group.AddCommand("Imprimer", IconProvider.BarCode, () =>
              {
                  ILabelItemGenerator labelItemGenerator = LabelGeneratorFactoy.GetGeneraor();
                  List<ItemLabel> result = labelItemGenerator.LoadLabels();
                  if (result.Count > 0)
                  {
                      var parameterProvider = ServiceLocator.Resolve<IParameterProvider>();
                      var logo = Path.Combine(parameterProvider.GetValue("PicturesDirectory", @"C:\SQLIMMO\Images"), parameterProvider.GetValue("LogoFileName", "logo.jpg"));
                      XtraReport etiquete = GetItemLabel(result, companyName, logo);
                      etiquete.CreateDocument();
                      var page = ReportViewModel.GetModulePage("Etiquettes Articles", etiquete);
                      var uIService = ServiceLocator.Resolve<IUIService>();
                      uIService.AddPage(page);
                  }
                  else
                  {
                      uIMessage.Information("Acune Etiquette à imprimer !");
                  }
              });
            return group;
        }

        private static XtraReport GetItemLabel(List<ItemLabel> dataSource, string companyName, string logo)
        {
            if (useMatrixBarCode)
            {
                return new Reports.Label2D(shortCompanyName)
                {
                    DataSource = dataSource
                };
            }

            return new Reports.LabelItem5025(companyName, logo)
            {
                DataSource = dataSource
            };
        }

        private static XtraReport GetOfficeLabel(List<OfficeLabel> dataSource, string logo)
        {
            if (useMatrixBarCode)
            {
                return new Reports.Label2D(shortCompanyName)
                {
                    DataSource = dataSource
                };
            }

            return new Label.Core.Reports.LabelOffice6030(companyName, logo)
            {
                DataSource = dataSource
            };
        }
    }
}
