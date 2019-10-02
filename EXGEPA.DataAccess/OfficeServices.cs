using CORESI.Data;
using CORESI.DataAccess.Core;
using CORESI.IoC;
using EXGEPA.Core;
using EXGEPA.Model;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace EXGEPA.DataAccess
{
    [Export(typeof(IDataProvider<Office>)), PartCreationPolicy(CreationPolicy.Shared)]
    public class OfficeService : DbService<Office>
    {
        public IDataProvider<Level> LevelService { get; set; }
        public IDataProvider<Building> BuildingService { get; set; }
        public IDataProvider<Site> SiteService { get; set; }
        public IDataProvider<Region> RegionService { get; set; }
        public OfficeService()
        {
            this.LevelService = ServiceLocator.Resolve<IDataProvider<Level>>();
            this.BuildingService = ServiceLocator.Resolve<IDataProvider<Building>>();
            this.SiteService = ServiceLocator.Resolve<IDataProvider<Site>>();
            this.RegionService = ServiceLocator.Resolve<IDataProvider<Region>>();
        }



        public override IList<Office> SelectAll()
        {
            List<Office> offices = base.SelectAll().ToList();
            List<Level> levels = this.LevelService.SelectAll().ToList();
            List<Site> sites = SiteService.SelectAll().ToList();
            List<Building> buildings = BuildingService.SelectAll().ToList();
            List<Region> regions = RegionService.SelectAll().ToList();
            LocalizationTools.BindLocalization(offices, levels, sites, buildings, regions);
            return offices;
        }



    }
}
