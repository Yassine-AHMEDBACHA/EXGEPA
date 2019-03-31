using CORESI.WPF.Core;
using System.Windows.Media;
using CORESI.WPF.Core.Framework;
using CORESI.WPF.Model;


namespace CORESI.Report.Controls
{
    public class ReportViewModel : PageViewModel
    {
        private object _DocumentSource;

        public object DocumentSource
        {
            get => _DocumentSource;
            set
            {
                _DocumentSource = value;
                RaisePropertyChanged("DocumentSource");
            }
        }



        private static Categorie printCategorie = new Categorie()
        {
            Caption = "Impressions",
            Color = Color.FromRgb(212, 175, 55)

        };




        public static Page GetModulePage(string pageTitle, object documentSource = null)
        {
            ReportViewModel viewModel = new ReportViewModel();
            ReportView view = ReportView.GetInstance(viewModel);

            viewModel.DocumentSource = documentSource;

            Page page = new Page(viewModel, view)
            {
                Caption = pageTitle,
                Categorie = printCategorie
            };
            printCategorie.Pages.Add(page);
            Group printGroup = new Group()
            {
                Caption = "Impression",

            };
            page.Groups.Add(printGroup);
            printGroup.Commands.Add(new RibbonButton()
            {
                Caption = "Impression rapide",
                LargeGlyph = IconProvider.Print,
                Action = view.QuickPrint
            });

            printGroup.Commands.Add(new RibbonButton()
            {
                Caption = "Impression",
                LargeGlyph = IconProvider.PrintDialog,
                Action = view.Print
            });
            printGroup.Commands.Add(new RibbonButton()
            {
                Caption = "parametre de la Page",
                LargeGlyph = IconProvider.PrintArea,
                Action = view.ShowPageSetup
            });

            Group reportGroup = new Group()
            {
                Caption = "Rapport"
            };
            reportGroup.Commands.Add(new RibbonButton()
            {
                Caption = "Premiere Page",
                LargeGlyph = IconProvider.First,
                Action = view.MoveToFirstPage
            });
            reportGroup.Commands.Add(new RibbonButton()
            {
                Caption = "Page Précédente",
                LargeGlyph = IconProvider.Prev,
                Action = view.MoveToPreviousPage
            });

            reportGroup.Commands.Add(new RibbonButton()
            {
                Caption = "Page Suivante",
                LargeGlyph = IconProvider.Next,
                Action = view.MoveToNextPage
            });

            reportGroup.Commands.Add(new RibbonButton()
            {
                Caption = "Derniere page",
                LargeGlyph = IconProvider.Last,
                Action = view.MoveToLastPage
            });
            page.Groups.Add(reportGroup);

            Group zoomGroup = new Group()
            {
                Caption = "Zoom"
            }
;
            zoomGroup.Commands.Add(new RibbonButton()
            {
                Caption = "Zoom in",
                LargeGlyph = IconProvider.ZoomIn,
                Action = view.ZoomIn
            });
            zoomGroup.Commands.Add(new RibbonButton()
            {
                Caption = "Toute la Page",
                LargeGlyph = IconProvider.Zoom,
                Action = view.ZoomToWholePage
            });
            zoomGroup.Commands.Add(new RibbonButton()
            {
                Caption = "Zoom out",
                LargeGlyph = IconProvider.ZoomOut,
                Action = view.ZoomOut
            });
            page.Groups.Add(zoomGroup);
            reportGroup.Commands.Add(new RibbonButton()
            {
                Caption = "Watermark",
                LargeGlyph = IconProvider.Watermark
                 ,
                Action = view.ShowWaterMarkDialog
            });
            Group exportGroup = new Group();
            exportGroup.Commands.Add(new RibbonButton()
            {
                Caption = "Export",
                LargeGlyph = IconProvider.Export,
                Action = view.ExportToExcel
            });
            page.Groups.Add(exportGroup);
            return page;
        }
    }
}