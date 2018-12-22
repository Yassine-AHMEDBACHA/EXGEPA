
using CORESI.WPF.Model;
using DevExpress.Xpf.Ribbon;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CORESI.WPF.Core.Shell
{
    public class PageCategoryTemplateSelector : DataTemplateSelector
    {
        public DataTemplate DefaultCategoryTemplate { get; set; }
        public DataTemplate CategoryTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, System.Windows.DependencyObject container)
        {
            return ((IList)((RibbonControl)container).CategoriesSource).IndexOf(item) == 0 ? DefaultCategoryTemplate : CategoryTemplate;
        }
    }

    public class CommandTemplateSelector : DataTemplateSelector
    {
        public DataTemplate SmallItemTemplate { get; set; }
        public DataTemplate ItemTemplate { get; set; }
        public DataTemplate CheckedItemTemplate { get; set; }
        public DataTemplate DateEditTemplate { get; set; }
        public DataTemplate LegendTemplate { get; set; }

        public DataTemplate ComboBoxEditTemplate { get; set; }
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item != null)
            {
                if (item is CheckedRibbonButton)
                    return CheckedItemTemplate;
                if (item.GetType().IsGenericType && item.GetType().GetGenericTypeDefinition() == typeof(ComboBoxRibbon<>))
                {
                    return ComboBoxEditTemplate;
                }
                if (item is DateEditRibbon)
                    return DateEditTemplate;
                if (item is LegendItem)
                    return LegendTemplate;
                if (item is RibbonButton)
                {
                    return ((RibbonButton)item).IsSmall ? SmallItemTemplate : ItemTemplate;
                }
                else
                {
                    throw new NotSupportedException($"item type not supported, type = {item.GetType()}");
                }
            }
            else
            {
                return ItemTemplate;
            }
        }
    }
}
