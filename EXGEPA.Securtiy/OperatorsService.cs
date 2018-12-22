using CORESI.Data;
using CORESI.DataAccess.Core;
using EXGEPA.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;

namespace EXGEPA.Securtiy
{
    [Export(typeof(IDataProvider<Operator>))]
    public class OperatorsService : DbService<Operator>
    {    }
}
