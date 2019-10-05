
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace CORESI.WPF.Model
{
    public class Group : UiNotifier
    {
        public Group()
        {
            Commands = new ObservableCollection<SimpleItem>();
        }

        public Group(string caption, IList<RibbonButton> buttons = null)
            : this()
        {
            this.Caption = caption;
            if (buttons != null)
            {
                foreach (RibbonButton button in buttons)
                {
                    this.Commands.Add(button);
                }
            }
        }

        public string Name { get; set; }

        public RibbonButton AddCommand(string caption, Action actionToDo = null, bool isSmall = false)
        {
            return AddCommand<RibbonButton>(caption, null, actionToDo, isSmall);
        }

        public T AddCommand<T>(T command = null, string caption = null) where T : RibbonButton
        {
            if (command == null)
            {
                command = (T)Activator.CreateInstance(typeof(T), new[] { caption });
            }

            this.Commands.Add(command);
            return command;
        }
        public RibbonButton AddCommand(string caption, ImageSource icon = null, Action actionToDo = null, bool isSmall = false)
        {
            return AddCommand<RibbonButton>(caption, icon, actionToDo, isSmall);
        }

        public T AddCommand<T>(string caption, ImageSource icon = null, Action actionToDo = null, bool isSmall = false) where T : RibbonButton
        {
            T command = this.AddCommand<T>(null, caption);
            command.Caption = caption;
            command.IsSmall = isSmall;
            if (isSmall)
                command.Glyph = icon;
            else
                command.LargeGlyph = icon;
            command.Action = actionToDo;
            return command;
        }

        public RibbonButton AddNewButton(string caption)
        {
            RibbonButton ribbonCommand = new RibbonButton(caption);
            this.Commands.Add(ribbonCommand);
            return ribbonCommand;
        }

        public LegendItem AddLegend(string caption, Color? color = null)
        {
            LegendItem legend = new LegendItem(caption, color);
            this.Commands.Add(legend);

            return legend;
        }

        public string Caption { get; set; }

        public ObservableCollection<SimpleItem> Commands { get; set; }
    }
}
