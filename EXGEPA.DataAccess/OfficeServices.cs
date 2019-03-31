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
            var offices = base.SelectAll().ToList();
            var levels = this.LevelService.SelectAll().ToList();
            var sites = SiteService.SelectAll().ToList();
            var buildings = BuildingService.SelectAll().ToList();
            var regions = RegionService.SelectAll().ToList();
            LoacalizationTools.BindLocalization(offices, levels, sites, buildings, regions);
            return offices;
        }



    }
}
