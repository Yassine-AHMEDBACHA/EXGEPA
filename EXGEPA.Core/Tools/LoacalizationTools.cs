using System.Collections.Generic;
using System.Linq;
using EXGEPA.Model;

namespace EXGEPA.Core
{
    public class LoacalizationTools
    {
        public static void BindLocalization(IEnumerable<Office> allOffices, IEnumerable<Level> AllLevel, IEnumerable<Site> allSites, IEnumerable<Building> allbuildings, IEnumerable<Region> allRegions)
        {
            foreach (var region in allRegions)
            {
                region.Sites = new List<Site>();
                BindSites(allOffices, AllLevel, allSites, allbuildings, region);
            }
        }

        private static void BindSites(IEnumerable<Office> allOffices, IEnumerable<Level> AllLevel, IEnumerable<Site> allSites, IEnumerable<Building> allbuildings, Region region)
        {
            foreach (var site in allSites.Where(s => s.Region.Id == region.Id))
            {
                region.Sites.Add(site);
                site.Region = region;
                site.Buildings = new List<Building>();
                BindBuildings(allOffices, AllLevel, allbuildings, site);
            }
        }

        private static void BindBuildings(IEnumerable<Office> allOffices, IEnumerable<Level> AllLevel, IEnumerable<Building> allbuildings, Site site)
        {
            foreach (var building in allbuildings.Where(b => b.Site.Id == site.Id))
            {
                site.Buildings.Add(building);
                building.Site = site;
                building.Levels = new List<Level>();
                BindLevels(allOffices, AllLevel, building);
            }
        }

        private static void BindLevels(IEnumerable<Office> allOffices, IEnumerable<Level> AllLevel, Building building)
        {
            foreach (var level in AllLevel.Where(l => l.Building.Id == building.Id))
            {
                level.Offices = new List<Office>();
                level.Building = building;
                building.Levels.Add(level);
                foreach (var office in allOffices.Where(o => o.Level.Id == level.Id))
                {
                    office.Level = level;
                    level.Offices.Add(office);
                }
            }
        }

        public static string NormelizeOfficeCode(string code)
        {
            return NormelizeCode(code, 4);
        }

        public static string NormelizeCode(string code, int length = 2, char charToUse = '0')
        {
            while (code.Length < length)
            {
                code = charToUse + code;
            }
            return code;
        }
    }
}
