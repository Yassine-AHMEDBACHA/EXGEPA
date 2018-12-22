using System.Collections.Generic;
using CORESI.WPF.Model;

namespace CORESI.WPF
{
    public static class PageExtensions
    {
        public static Group AddNewGroup(this Page page, string caption = null, IList<RibbonButton> buttons = null)
        {
            var group = new Group(caption, buttons);
            page.AddNewGroup(group);
            return group;
        }

        public static void AddNewGroup(this Page page, Group group)
        {
            page.Groups.Add(group);
        }

    

    }
}
