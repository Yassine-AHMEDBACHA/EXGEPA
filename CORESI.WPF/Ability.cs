using System;

namespace CORESI.WPF
{
    public class Ability<T>
    {
        public readonly Type type;

        public ActionCanDo ActionCanDo { get; set; }

        public Ability()
        {
            this.type = typeof(T);
        }

        public bool CanDoAction(ActionCanDo actionCanDo)
        {
            return this.ActionCanDo.HasFlag(actionCanDo);
        }
    }
    
    public enum ActionCanDo
    {
        Add = 0,

        Delete = 2,

        Edit = 4,

        Read = 8
    }
}
