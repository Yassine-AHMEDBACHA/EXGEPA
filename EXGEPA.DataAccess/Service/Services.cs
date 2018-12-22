using CORESI.Data;
using CORESI.DataAccess.Core;
using EXGEPA.Model;
using System.ComponentModel.Composition;

namespace EXGEPA.DataAccess.Service
{
    [Export(typeof(IDataProvider<Person>)), PartCreationPolicy(CreationPolicy.Shared)]
    public class PersonService : DbService<Person> { }

    [Export(typeof(IDataProvider<Provider>)), PartCreationPolicy(CreationPolicy.Shared)]
    public class ProviderService : DbService<Provider> { }

    [Export(typeof(IDataProvider<ReceiveOrder>)), PartCreationPolicy(CreationPolicy.Shared)]
    public class ReceiveOrderService : DbService<ReceiveOrder> { }

    [Export(typeof(IDataProvider<ItemState>)), PartCreationPolicy(CreationPolicy.Shared)]
    public class ItemStateService : DbService<ItemState> { }

    [Export(typeof(IDataProvider<Tva>)), PartCreationPolicy(CreationPolicy.Shared)]
    public class TvaService : DbService<Tva> { }

    [Export(typeof(IDataProvider<InputSheet>)), PartCreationPolicy(CreationPolicy.Shared)]
    public class InputSheetService : DbService<InputSheet> { }

    [Export(typeof(IDataProvider<AccountingPeriod>)), PartCreationPolicy(CreationPolicy.Shared)]
    public class AccountingPeriodService : DbService<AccountingPeriod> { }

    [Export(typeof(IDataProvider<ReformeCertificate>)), PartCreationPolicy(CreationPolicy.Shared)]
    public class ReformeCertificateService : DbService<ReformeCertificate> { }

    [Export(typeof(IDataProvider<ProposeToReformCertificate>)), PartCreationPolicy(CreationPolicy.Shared)]
    public class ProposeToReformCertificateService : DbService<ProposeToReformCertificate> { }

    [Export(typeof(IDataProvider<OutputCertificate>)), PartCreationPolicy(CreationPolicy.Shared)]
    public class OutputCertificateService : DbService<OutputCertificate> { }

    [Export(typeof(IDataProvider<TransferOrder>)), PartCreationPolicy(CreationPolicy.Shared)]
    public class TransferOrderService : DbService<TransferOrder> { }

    [Export(typeof(IDataProvider<AnalyticalAccount>)), PartCreationPolicy(CreationPolicy.Shared)]
    public class AnalyticalAccountService : DbService<AnalyticalAccount> { }

    [Export(typeof(IDataProvider<Currency>)), PartCreationPolicy(CreationPolicy.Shared)]
    public class CurrencyService : DbService<Currency> { }

    [Export(typeof(IDataProvider<Region>)), PartCreationPolicy(CreationPolicy.Shared)]
    public class RegionService : DbService<Region> { }

    [Export(typeof(IDataProvider<Site>)), PartCreationPolicy(CreationPolicy.Shared)]
    public class SiteService : DbService<Site> { }

    [Export(typeof(IDataProvider<Building>)), PartCreationPolicy(CreationPolicy.Shared)]
    public class BuildingService : DbService<Building> { }

    [Export(typeof(IDataProvider<Level>)), PartCreationPolicy(CreationPolicy.Shared)]
    public class LevelService : DbService<Level> { }

    [Export(typeof(IDataProvider<GeneralAccountType>)), PartCreationPolicy(CreationPolicy.Shared)]
    public class GeneralAccountTypeService : DbService<GeneralAccountType> { }

    [Export(typeof(IDataProvider<ReferenceType>)), PartCreationPolicy(CreationPolicy.Shared)]
    public class ReferenceTypeService : DbService<ReferenceType> { }

    [Export(typeof(IDataProvider<Project>)), PartCreationPolicy(CreationPolicy.Shared)]
    public class ProjectService : DbService<Project> { }

    [Export(typeof(IDataProvider<Assignment>)), PartCreationPolicy(CreationPolicy.Shared)]
    public class AssignmentService : DbService<Assignment> { }


    [Export(typeof(IDataProvider<AnalyticalAccountType>)), PartCreationPolicy(CreationPolicy.Shared)]
    public class AnalyticalAccountTypeService : DbService<AnalyticalAccountType> { }

        
    [Export(typeof(IDataProvider<OrderDocumentType>)), PartCreationPolicy(CreationPolicy.Shared)]
    public class OrderDocumentTypeService : DbService<OrderDocumentType> { }
}
