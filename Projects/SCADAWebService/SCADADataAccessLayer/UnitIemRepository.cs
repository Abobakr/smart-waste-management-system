using DomainDataEntities;
using SCADADataContracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SCADADataAccessLayer
{
    public interface IUnitIemRepository : ISCADARepository<UnitItem>
    {
        IList<UnitItemDAL> GetUnitItemsByTerminal(IList<int> terminalId, double southWestLat, double southWestLng, double northEastLat, double northEastLng);
        IList<UnitItemDetails> GetUnitItemDetails(int unitItemId);

        IList<UnitTypeContract> GetUnitTypes(string accountId);
    }

    public class UnitIemRepository : SCADARepository<UnitItem>, IUnitIemRepository
    {
        public IList<UnitItemDAL> GetUnitItemsByTerminal(IList<int> terminalIdes, double southWestLat, double southWestLng, double northEastLat, double northEastLng)
        {
            using (var context = new SCADADbContext())
            {
                IQueryable<int> TCR_Ides = from trc in context.TerminalComponentRoles
                               where terminalIdes.Contains(trc.TerminalId) && trc.IsForDisplay == true // one item per terminal
                               select trc.Id;

                var activities = from activity in context.Activities
                                 from tcrId in TCR_Ides
                                 where tcrId == activity.TerminalComponentRoleId
                                 select new { activity.TerminalItemId, tcrId, activity.LastValue };

                var unitItemValueList = from terminalItem in context.TerminalItems
                                        from activity in activities
                                        where activity.TerminalItemId == terminalItem.Id && terminalItem.UnitItemId != null
                                        select new { terminalItem.UnitItemId, activity.tcrId, activity.LastValue };

                var unitItemList = from unitItem in context.UnitItems
                                   from unitItemValue in unitItemValueList
                                   where unitItem.Id == unitItemValue.UnitItemId && //unitItem.MotherUnitItemId == null
                                   unitItem.Latitude <= northEastLat && unitItem.Latitude >= southWestLat &&
                                       unitItem.Longtitude <= northEastLng && unitItem.Longtitude >= southWestLng
                                   select new { unitItem.Id, unitItemValue.tcrId, unitItemValue.LastValue, unitItem.Latitude, unitItem.Longtitude };

                IList<UnitItemDAL> itemLocationList = new List<UnitItemDAL>();
                foreach (var item in unitItemList)
                {
                    UnitItemDAL unitItem = new UnitItemDAL(item.Id, item.tcrId, item.LastValue, item.Latitude, item.Longtitude);                    
                    itemLocationList.Add(unitItem);
                }
                return itemLocationList;
            }
        }
      
        public IList<UnitItemDetails> GetUnitItemDetails(int unitItemId)
        {
            using (var context = new SCADADbContext())
            {
                IQueryable<int> terminalItemsIdes = from terminalItem in context.TerminalItems
                                                    where terminalItem.UnitItemId == unitItemId
                                                    select terminalItem.Id;

                var activities = from activity in context.Activities
                                 where terminalItemsIdes.Contains(activity.TerminalItemId)
                                 select new { activity.TerminalComponentRoleId, activity.LastValue, activity.Date };

                var terminalComponentRole = from trc in context.TerminalComponentRoles
                                            join activity in activities on trc.Id equals activity.TerminalComponentRoleId
                                            select new { trc.ComponentId, activity.TerminalComponentRoleId, trc.RoleId };

                var components = from component in context.Components
                                 join trc in terminalComponentRole on component.Id equals trc.ComponentId
                                 select new { component.ModelNo, component.ComponentTypeId, trc.TerminalComponentRoleId, trc.RoleId };


                var componentNames = from componentType in context.ComponentTypes
                                     join component in components on componentType.Id equals component.ComponentTypeId
                                     select new { component.ModelNo, componentType.Name, component.TerminalComponentRoleId, component.RoleId };



                var itemDetails = (from activity in activities
                                   join componentName in componentNames
                                   on activity.TerminalComponentRoleId equals componentName.TerminalComponentRoleId
                                   select new { componentName.ModelNo, activity.TerminalComponentRoleId, componentName.Name, activity.LastValue, activity.Date, componentName.RoleId }).Distinct();


                var itemDetailsFinal = from role in context.Roles
                                       join itemDetail in itemDetails on role.Id equals itemDetail.RoleId
                                       select new { itemDetail.Name, itemDetail.ModelNo, roleName = role.Name, itemDetail.LastValue, itemDetail.Date };

                IList<UnitItemDetails> itemDetailsList = new List<UnitItemDetails>();

                foreach (var item in itemDetailsFinal)
                {
                    itemDetailsList.Add(new UnitItemDetails(item.Name, item.ModelNo, item.roleName, item.LastValue, item.Date));
                }

                return itemDetailsList;
            }
        }

        public IList<UnitTypeContract> GetUnitTypes(string accountId)
        {
            using (var context = new SCADADbContext())
            {
                //IQueryable<int> terminalIdes = from accountTerminal in context.AccountTerminals
                //                               where accountId == accountTerminal.AccountId
                //                               select accountTerminal.TerminalId;
                //var terminals = from terminal in context.Terminals
                //                where terminalIdes.Contains(terminal.Id) //&& terminal.IsStandard == false
                //                select new { terminal.Id, terminal.Name };

                var unitTypes = from unitType in context.UnitTypes
                                //where 
                                select new { unitType.Id, unitType.Name,unitType.Capacity };

                IList<UnitTypeContract> unitTypeList = new List<UnitTypeContract>();

                foreach (var item in unitTypes)
                {
                    unitTypeList.Add(new UnitTypeContract(item.Id, item.Name,item.Capacity));
                }
                return unitTypeList;
            }
        }

    }
}
