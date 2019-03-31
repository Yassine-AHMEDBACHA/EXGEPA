using System.Collections.Generic;
using System.IO;
using System.Linq;
using CORESI.Data;
using CORESI.IoC;
using CORESI.Tools.Collections;
using CORESI.WPF;
using CORESI.WPF.Core;
using CORESI.WPF.Model;
using EXGEPA.Core;
using EXGEPA.Label.Core.Model;
using EXGEPA.Model;

namespace EXGEPA.Label.Core
{
    public class ReportProvider
    {
        private static IUIMessage uIMessage;
        private static IParameterProvider parameterProvider;
        static ReportProvider()
        {
            uIMessage = ServiceLocator.GetPriorizedInstance<IUIMessage>();
            ServiceLocator.Resolve(out parameterProvider);
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
                    List<OfficeLabel> dataSource = new List<OfficeLabel>();

                    IDataProvider<Office> officeServices = ServiceLocator.Resolve<IDataProvider<Office>>();
                    IList<Office> listOfOffice = officeServices.SelectAll();
                    listOfOffice.Where(x => x.PrintLabel).ForEach(x =>
                        {
                            OfficeLabel label = new OfficeLabel
                            {
                                CodeBare = x.Key,
                                OfficeCaption = x.Caption,
                                CodeOffice = x.Code,
                                CodeLevel = x.Level.Code,
                                CodeBuilding = x.Level.Building.Code,
                                CodeSite = x.Level.Building.Site.Code,
                                CodeRegion = x.Level.Building.Site.Region.Key
                            };
                            dataSource.Add(label);
                        });
                    if (dataSource.Count > 0)
                    {
                        IParameterProvider parameterProvider = ServiceLocator.Resolve<IParameterProvider>();
                        string companyName = parameterProvider.GetValue<string>("CompanyName");
                        string logo = Path.Combine(parameterProvider.GetValue("PicturesDirectory", @"C:\SQLIMMO\Images"), parameterProvider.GetValue("LogoFileName", "logo.jpg"));
                        Reports.LabelOffice6030 etiquete = new Label.Core.Reports.LabelOffice6030(companyName, logo)
                        {
                            DataSource = dataSource
                        };
                        etiquete.CreateDocument();
                        Page page = CORESI.Report.Controls.ReportViewModel.GetModulePage("Etiquettes Locaux", etiquete);
                        IUIService uIService = ServiceLocator.Resolve<IUIService>();
                        uIService.AddPage(page);
                    }
                    else
                    {
                        uIMessage.Information("Acune Etiquette à imprimer !");
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
                      IParameterProvider parameterProvider = ServiceLocator.Resolve<IParameterProvider>();
                      string companyName = parameterProvider.GetValue<string>("CompanyName");
                      string logo = Path.Combine(parameterProvider.GetValue("PicturesDirectory", @"C:\SQLIMMO\Images"), parameterProvider.GetValue("LogoFileName", "logo.jpg"));
                      Reports.LabelItem5025 etiquete = new Label.Core.Reports.LabelItem5025(companyName, logo)
                      {
                          DataSource = result
                      };
                      etiquete.CreateDocument();
                      Page page = CORESI.Report.Controls.ReportViewModel.GetModulePage("Etiquettes Articles", etiquete);
                      IUIService uIService = ServiceLocator.Resolve<IUIService>();
                      uIService.AddPage(page);
                  }
                  else
                  {
                      uIMessage.Information("Acune Etiquette à imprimer !");
                  }
              });
            return group;
        }
    }
}
