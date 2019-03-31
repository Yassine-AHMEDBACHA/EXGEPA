
using DevExpress.Images;
using DevExpress.Utils.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CORESI.WPF.Core
{
    public static class IconProvider
    {
        static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static DXImageServicesImp dXImageServicesImp = new DXImageServicesImp();
        static IEnumerable<IDXImageInfo> Images = dXImageServicesImp.GetAllImages().Where(x => x.ImageType == ImageType.Colored);


        public static ImageSource GetImageSource(string source, bool isSmall = false)
        {
            string size = "_32x32.png";
            if (isSmall)
            {
                size = "_16x16.png";
            }
            source += size;
            var result = Images.Where(image => image.Name == source).FirstOrDefault();
            if (result != null)
            {
                Uri uri = new Uri(result.MakeUri());
                return new BitmapImage(uri);
            }
            else
            {
                return null;
            }

        }

        public static readonly ImageSource Close = GetImageSource("Close");
        public static readonly ImageSource AddItem = GetImageSource("AddItem");
        public static readonly ImageSource Save = GetImageSource("Save");
        public static readonly ImageSource Properties = GetImageSource("Properties");
        public static readonly ImageSource PropertiesSmall = GetImageSource("Properties", true);
        public static readonly ImageSource Settings = GetImageSource("IDE");
        public static readonly ImageSource SaveAndClose = GetImageSource("SaveAndClose");
        public static readonly ImageSource SaveAndNew = GetImageSource("SaveAndNew");
        public static readonly ImageSource Edit = GetImageSource("Edit");
        public static readonly ImageSource Cancel = GetImageSource("Cancel");
        public static readonly ImageSource Add = GetImageSource("Add");
        public static readonly ImageSource Delete = GetImageSource("Delete");
        public static readonly ImageSource DeleteSmall = GetImageSource("Delete", true);
        public static readonly ImageSource View = GetImageSource("ChartsShowLegend");
        public static readonly ImageSource Grid = GetImageSource("Grid");

        public static readonly ImageSource Xlsx = GetImageSource("ExportToXLSX");
        public static readonly ImageSource XlsxSmall = GetImageSource("ExportToXLSX", true);

        public static readonly ImageSource PDF = GetImageSource("ExportToPDF");
        public static readonly ImageSource PDFSmall = GetImageSource("ExportToPDF", true);
        public static readonly ImageSource Print = GetImageSource("Print");
        public static readonly ImageSource PrintSmall = GetImageSource("Print", true);
        public static readonly ImageSource Inventory = GetImageSource("ConvertToRange");
        public static readonly ImageSource CloseDetail = GetImageSource("DeleteList2");
        public static readonly ImageSource Localization = GetImageSource("GeoPointMap");
        public static readonly ImageSource AnalyticalAccount = GetImageSource("InsertHeader");
        public static readonly ImageSource AnalyticalAccountSmall = GetImageSource("InsertHeader", true);
        public static readonly ImageSource GeneralAccount = GetImageSource("GroupHeader");
        public static readonly ImageSource GeneralAccountSmall = GetImageSource("GroupHeader", true);
        public static readonly ImageSource Provider = GetImageSource("BOOrderItem");
        public static readonly ImageSource ProviderSmall = GetImageSource("BOOrderItem", true);
        public static readonly ImageSource Reference = GetImageSource("ConvertToParagraphs");
        public static readonly ImageSource ReferenceSmall = GetImageSource("ConvertToParagraphs", true);
        public static readonly ImageSource Invoice = GetImageSource("Financial");
        public static readonly ImageSource BarCode = GetImageSource("Barcode");
        public static readonly ImageSource BarCodeSmall = GetImageSource("Barcode", true);
        public static readonly ImageSource AddNewDataSource = GetImageSource("AddNewDataSource");
        public static readonly ImageSource AddNewDataSourceSmall = GetImageSource("AddNewDataSource", true);
        public static readonly ImageSource Download = GetImageSource("Download");
        public static readonly ImageSource DownloadSmall = GetImageSource("Download", true);
        public static readonly ImageSource Team = GetImageSource("Team");
        public static readonly ImageSource TeamSmall = GetImageSource("Team", true);

        public static readonly ImageSource GroupUser = GetImageSource("GroupUser");
        public static readonly ImageSource GroupUserSmall = GetImageSource("GroupUser", true);

        public static readonly ImageSource AssignTo = GetImageSource("AssignTo");
        public static readonly ImageSource AssignToSmall = GetImageSource("AssignTo", true);

        public static readonly ImageSource Role = GetImageSource("Role");
        public static readonly ImageSource RoleSmall = GetImageSource("Role", true);

        public static readonly ImageSource Currency = GetImageSource("Currency");
        public static readonly ImageSource CurrencySmall = GetImageSource("Currency", true);

        public static readonly ImageSource Merge = GetImageSource("Merge");
        public static readonly ImageSource MergeSmall = GetImageSource("Merge", true);

        public static readonly ImageSource Percentage = GetImageSource("Percentage");
        public static readonly ImageSource PercentageSmall = GetImageSource("Percentage", true);

        public static readonly ImageSource Cession = GetImageSource("LoadMap");
        public static readonly ImageSource CessionSmall = GetImageSource("LoadMap", true);

        public static readonly ImageSource Destruction = GetImageSource("DefineName");
        public static readonly ImageSource DestructionSmall = GetImageSource("DefineName", true);

        public static readonly ImageSource Reforme = GetImageSource("BORules");
        public static readonly ImageSource ReformeSmall = GetImageSource("BORules", true);

        public static readonly ImageSource Disparition = GetImageSource("FindCustomers");
        public static readonly ImageSource DisparitionSmall = GetImageSource("FindCustomers", true);

        public static readonly ImageSource AddNewItem = GetImageSource("BOProduct");
        public static readonly ImageSource AddNewItemSmall = GetImageSource("BOProduct", true);

        public static readonly ImageSource ReceiveOrder = GetImageSource("BOReport2");
        public static readonly ImageSource ReceiveOrderSmall = GetImageSource("BOReport2", true);

        public static readonly ImageSource TransferOrder = GetImageSource("BOReport");
        public static readonly ImageSource TransferOrderSmall = GetImageSource("BOReport", true);

        public static readonly ImageSource InputSheet = GetImageSource("Notes");
        public static readonly ImageSource InputSheetSmall = GetImageSource("Notes", true);

        public static readonly ImageSource Filtre = GetImageSource("MasterFilter");
        public static readonly ImageSource FiltreSmall = GetImageSource("MasterFilter", true);

        public static readonly ImageSource GroupRows = GetImageSource("GroupRows");
        public static readonly ImageSource GroupRowsSamll = GetImageSource("GroupRows", true);

        public static readonly ImageSource FindByID = GetImageSource("FindByID");
        public static readonly ImageSource FindByIDSmall = GetImageSource("FindByID", true);

        public static readonly ImageSource FloatindWidth = GetImageSource("FloatindWidth");
        public static readonly ImageSource FloatindWidthSmall = GetImageSource("FloatindWidth", true);

        public static readonly ImageSource Refresh = GetImageSource("Refresh2");
        public static readonly ImageSource RefreshSmall = GetImageSource("Refresh2", true);

        public static readonly ImageSource GreaterThan = GetImageSource("GreaterThan");
        public static readonly ImageSource GreaterThanSmall = GetImageSource("GreaterThan", true);

        public static readonly ImageSource Summary = GetImageSource("Summary");
        public static readonly ImageSource SummarySmall = GetImageSource("Summary", true);

        public static readonly ImageSource PrintDialog = GetImageSource("PrintDialog");

        public static readonly ImageSource PageSetup = GetImageSource("PageSetup");

        public static readonly ImageSource PrintArea = GetImageSource("PrintArea");

        public static readonly ImageSource ZoomOut = GetImageSource("ZoomOut");

        public static readonly ImageSource ZoomIn = GetImageSource("ZoomIn");

        public static readonly ImageSource Zoom = GetImageSource("Zoom");

        public static readonly ImageSource Watermark = GetImageSource("Watermark");

        public static readonly ImageSource First = GetImageSource("First");

        public static readonly ImageSource Next = GetImageSource("Next");

        public static readonly ImageSource Prev = GetImageSource("Prev");

        public static readonly ImageSource Last = GetImageSource("Last");

        public static readonly ImageSource Export = GetImageSource("Export");

        public static readonly ImageSource DeleteSheet = GetImageSource("DeleteSheet");
        public static readonly ImageSource DeleteSheetSmall = GetImageSource("DeleteSheet", true);

        public static readonly ImageSource Task = GetImageSource("Task");
        public static readonly ImageSource TaskSmall = GetImageSource("Task", true);


        public static readonly ImageSource AddCalculatedField = GetImageSource("AddCalculatedField");
        public static readonly ImageSource AddCalculatedFieldSmall = GetImageSource("AddCalculatedField", true);

        public static readonly ImageSource ShowFormulas = GetImageSource("ShowFormulas");
        public static readonly ImageSource ShowFormulasSmall = GetImageSource("ShowFormulas", true);


        public static readonly ImageSource CalculateNow = GetImageSource("CalculateNow");
        public static readonly ImageSource CalculateNowSmall = GetImageSource("CalculateNow", true);

        public static readonly ImageSource Article = GetImageSource("Article");
        public static readonly ImageSource ArticleSmall = GetImageSource("Article", true);


        public static readonly ImageSource MoreFunctions = GetImageSource("MoreFunctions");
        public static readonly ImageSource MoreFunctionsSmall = GetImageSource("MoreFunctions", true);

        public static readonly ImageSource BOUser = GetImageSource("BOUser");
        public static readonly ImageSource BOUserSmall = GetImageSource("BOUser", true);

        public static readonly ImageSource Walking = GetImageSource("Walking");
        public static readonly ImageSource WalkingSmall = GetImageSource("Walking", true);

        public static readonly ImageSource BOPermission = GetImageSource("BOPermission");
        public static readonly ImageSource BOPermissionSmall = GetImageSource("BOPermission", true);


        public static readonly ImageSource ManageDatasource = GetImageSource("ManageDatasource");
        public static readonly ImageSource ManageDatasourceSmall = GetImageSource("ManageDatasource", true);

        public static readonly ImageSource OtherCharts = GetImageSource("OtherCharts");
        public static readonly ImageSource OtherChartsSmall = GetImageSource("OtherCharts", true);

        public static readonly ImageSource ActionsLeft = GetImageSource("Actions_left");
        public static readonly ImageSource ActionsLeftSmall = GetImageSource("actions_left", true);

        public static readonly ImageSource Redo = GetImageSource("Redo");
        public static readonly ImageSource RedoSmall = GetImageSource("Redo", true);

        public static readonly ImageSource Undo = GetImageSource("Undo");
        public static readonly ImageSource UndoSmall = GetImageSource("Undo", true);

        public static readonly ImageSource Phone = GetImageSource("Phone");
        public static readonly ImageSource PhoneSmall = GetImageSource("Phone", true);

        public static readonly ImageSource Reading = GetImageSource("Reading");
        public static readonly ImageSource ReadingSmall = GetImageSource("Reading", true);



    }

}
