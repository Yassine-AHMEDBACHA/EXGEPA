
using EXGEPA.Label.Core.Model;
using System.Collections.Generic;
namespace EXGEPA.Core
{
    public interface ILabelItemGenerator
    {
        List<ItemLabel> LoadLabels();
    }
}
