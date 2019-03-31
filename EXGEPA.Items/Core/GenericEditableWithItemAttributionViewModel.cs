
using CORESI.Data;
using CORESI.WPF.Controls;
using System;

namespace EXGEPA.Items.Core
{
    public class GenericEditableWithItemAttributionViewModel<T> : GenericEditableViewModel<T> where T : KeyRow, ICloneable
    {

        //public override Page GetModulePage<Tview>(string pageTitle,string propertyName = null)
        //{

        //    var page = base.GetModulePage<Tview>(pageTitle);
        //    Group group = new Group();
        //    group.Commands.Add(new RibbonButton()
        //    {
        //        Caption = "Etiquettes",
        //        LargeGlyph = IconProvider.BarCode,
        //        CommandAction = new DelegateCommand(() =>
        //        {
        //            var printLabelProperty = ItemService.GetInstance().DataAccessor.Fields.First(x => x.Name == propertyName).PropertyInfo;
        //            UIItemService.ShowItemAttribution<bool>(printLabelProperty, true, false, pageTitle);
        //        })
        //    });
        //    page.Groups.Add(group);
        //    return page;
        //}

    }
}
