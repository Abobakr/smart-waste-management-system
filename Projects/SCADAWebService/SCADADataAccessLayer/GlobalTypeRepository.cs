using DomainDataEntities;
using SCADADataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace SCADADataAccessLayer
{
    public interface IGlobalTypeRepository
    {
        IList<UnitItemData> GetUnitItemsByTerminal(UnitItemFilter unitItemFilter);
        IList<UnitItemDetails> GetUnitItemDetails(int unitItemId);

        IList<UnitTypeContract> GetUnitTypes(string accountId);


        IList<TerminalType> GetTerminals(string accountId);


        bool UpdateCenter(LocationType location, string accountId);

        LocationType GetUserMapCenter(string accountId);


        IList<FilterComponent> GetFilterComponents(string accountId);
    }

    public class GlobalTypeRepository : IGlobalTypeRepository
    {
        public IList<UnitItemData> GetUnitItemsByTerminal(UnitItemFilter unitItemFilter)
        {
            IList<int> terminalIdes = unitItemFilter.TerminalIdes;
            DateTime limitDate = unitItemFilter.LimitDate;
            bool isFirstRequest = unitItemFilter.IsFirstRequest;
            //string terminalId = unitItemFilter.TerminalId;
            int componentId = unitItemFilter.ComponentId;
            int roleId = unitItemFilter.RoleId;
            double minValue = unitItemFilter.MinValue;
            double maxValue = unitItemFilter.MaxValue;
            //DateTime dateTime = unitItemFilter.DateTime;
            bool isSubFilter = unitItemFilter.IsSubFilter;
            double southWestLat = unitItemFilter.SouthWest.Latitude;
            double southWestLng = unitItemFilter.SouthWest.Longitude;
            double northEastLat = unitItemFilter.NorthEast.Latitude;
            double northEastLng = unitItemFilter.NorthEast.Longitude; 

            using (var context = new SCADADbContext())
            {

                var terminalComponents = from tc in context.TerminalComponents
                                         from terId in terminalIdes
                                         where terId == tc.TerminalId && tc.ActiveModeId != null && (componentId == -1 || componentId == tc.ComponentId)
                                         select new { tc.Id, tc.ActiveModeId };

                var filteredTerminalComponents = from activeMode in context.ActiveModes
                                                 from tc in terminalComponents
                                                 where tc.ActiveModeId == activeMode.Id && activeMode.isForDisplay == true && (roleId == -1 || roleId == activeMode.RoleId)
                                                 select new { tc.Id, tc.ActiveModeId, activeMode.AlertValue };


                var activities = from activity in context.Activities
                                 from tc in filteredTerminalComponents
                                 where tc.Id == activity.TerminalComponentId && (isFirstRequest || activity.Date >= limitDate) && (!isSubFilter || (activity.LastValue >= minValue && activity.LastValue <= maxValue))
                                 select new { activity.TerminalItemId, tc.ActiveModeId, activity.LastValue, tc.AlertValue };

                var unitItemValueList = from terminalItem in context.TerminalItems
                                        from activity in activities
                                        where activity.TerminalItemId == terminalItem.Id && terminalItem.UnitItemId != null
                                        select new { terminalItem.UnitItemId, activity.ActiveModeId, activity.LastValue, activity.AlertValue };

                var unitItemList = from unitItem in context.UnitItems
                                   from unitItemValue in unitItemValueList
                                   where unitItem.Id == unitItemValue.UnitItemId && //unitItem.MotherUnitItemId == null
                                   unitItem.Latitude <= northEastLat && unitItem.Latitude >= southWestLat &&
                                       unitItem.Longtitude <= northEastLng && unitItem.Longtitude >= southWestLng
                                   select new { unitItem.Id, unitItemValue.ActiveModeId, unitItemValue.LastValue, unitItemValue.AlertValue, unitItem.Latitude, unitItem.Longtitude };

                IList<UnitItemData> itemLocationList = new List<UnitItemData>();
                foreach (var item in unitItemList)
                {
                    UnitItemDAL unitItem = new UnitItemDAL(item.Id, (int)item.ActiveModeId, item.LastValue, (double)item.AlertValue, item.Latitude, item.Longtitude);
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
                                 select new { activity.TerminalComponentId, activity.LastValue, activity.Date };

                var terminalComponent = from tc in context.TerminalComponents
                                        join activity in activities on tc.Id equals activity.TerminalComponentId
                                        select new { tc.ComponentId, tc.ActiveModeId, activity.LastValue, activity.Date };


                var components = from component in context.Components
                                 join tc in terminalComponent on component.Id equals tc.ComponentId
                                 select new { component.ComponentTypeId, tc.ActiveModeId, component.ModelNo, tc.LastValue, tc.Date };


                var componentNames = from componentType in context.ComponentTypes
                                     join component in components on componentType.Id equals component.ComponentTypeId
                                     select new { component.ActiveModeId, componentType.Name, component.ModelNo, component.LastValue, component.Date };

                var itemDetails = from activeMode in context.ActiveModes
                                  join componentName in componentNames on activeMode.Id equals componentName.ActiveModeId
                                  select new { activeMode.RoleId, componentName.Name, componentName.ModelNo, componentName.LastValue, activeMode.AlertValue, componentName.Date };

                var itemDetailsFinal = from role in context.Roles
                                       join itemDetail in itemDetails on role.Id equals itemDetail.RoleId
                                       select new { itemDetail.Name, itemDetail.ModelNo, roleName = role.Name, itemDetail.LastValue, itemDetail.AlertValue, itemDetail.Date };

                IList<UnitItemDetails> itemDetailsList = new List<UnitItemDetails>();

                foreach (var item in itemDetailsFinal)
                {
                    itemDetailsList.Add(new UnitItemDetails(item.Name, item.ModelNo, item.roleName, item.LastValue, item.AlertValue, item.Date));
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
                                select new { unitType.Id, unitType.Name, unitType.Capacity };

                IList<UnitTypeContract> unitTypeList = new List<UnitTypeContract>();

                foreach (var item in unitTypes)
                {
                    unitTypeList.Add(new UnitTypeContract(item.Id, item.Name, item.Capacity));
                }
                return unitTypeList;
            }
        }

        public IList<FilterComponent> GetFilterComponents(string accountId)
        {
            using (var context = new SCADADbContext())
            {
                //*****//
                //**1**//
                //*****//
                IQueryable<int> terminalIdes = from accountTerminal in context.AccountTerminals
                                               where accountId == accountTerminal.AccountId
                                               select accountTerminal.TerminalId;
                var terminals = from terminal in context.Terminals
                                where terminalIdes.Contains(terminal.Id) //&& terminal.IsStandard == false
                                select new { terminal.Id, terminal.Name };

                IList<FilterComponent> filterComponents = new List<FilterComponent>();

                foreach (var terminal in terminals)
                {
                    FilterComponent newItem = new FilterComponent(terminal.Id, terminal.Name);
                    filterComponents.Add(newItem);

                    //*****//
                    //**2**//
                    //*****//
                    IQueryable<int> componentIdes = from tc in context.TerminalComponents
                                                    where tc.TerminalId == terminal.Id && tc.ActiveModeId != null
                                                    select tc.ComponentId;
                    var components = from component in context.Components
                                     where componentIdes.Contains(component.Id)
                                     select new { component.Id, component.ModelNo };

                    //IQueryable<int> componentTypeIdes = from component in context.Components
                    //                                    where componentIdes.Contains(component.Id)
                    //                                    select component.ComponentTypeId;
                    //var componentTypes = from componentType in context.ComponentTypes
                    //                     where componentTypeIdes.Contains(componentType.Id) //&& terminal.IsStandard == false
                    //                     select new { componentType.Id, componentType.Name };

                    newItem.SubFilterComponents = new List<FilterComponent>();

                    foreach (var component in components)
                    {
                        FilterComponent newSubItem = new FilterComponent(component.Id, component.ModelNo);
                        newItem.SubFilterComponents.Add(newSubItem);

                        //*****//
                        //**3**//
                        //*****//
                        IQueryable<int?> activeModeIdes = from tc in context.TerminalComponents
                                                          where tc.TerminalId == terminal.Id && tc.ActiveModeId != null && tc.ComponentId == component.Id
                                                          select tc.ActiveModeId;
                        IQueryable<int> roleIdes = from activeMode in context.ActiveModes
                                                   where activeModeIdes.Contains(activeMode.Id)
                                                   select activeMode.RoleId;
                        var roles = from role in context.Roles
                                    where roleIdes.Contains(role.Id) //&& terminal.IsStandard == false
                                    select new { role.Id, role.Name };

                        newSubItem.SubFilterComponents = new List<FilterComponent>();
                        foreach (var role in roles)
                        {
                            newSubItem.SubFilterComponents.Add(new FilterComponent(role.Id, role.Name));
                        }
                    }

                }
                return filterComponents;
            }


        }

        public IList<TerminalType> GetTerminals(string accountId)
        {
            using (var context = new SCADADbContext())
            {
                IQueryable<int> terminalIdes = from accountTerminal in context.AccountTerminals
                                               where accountId == accountTerminal.AccountId
                                               select accountTerminal.TerminalId;
                var terminals = from terminal in context.Terminals
                                where terminalIdes.Contains(terminal.Id) //&& terminal.IsStandard == false
                                select new { terminal.Id, terminal.Name };

                IList<TerminalType> terminalTypes = new List<TerminalType>();

                foreach (var item in terminals)
                {
                    terminalTypes.Add(new TerminalType(item.Id, item.Name));
                }
                return terminalTypes;
            }
        }

        public bool UpdateCenter(LocationType location, string accountId)
        {
            using (var context = new SCADADbContext())
            {
                IQueryable<Account> queryableAccount = from acc in context.Accounts
                                                       where acc.Id == accountId
                                                       select acc;
                if (queryableAccount == null)
                {
                    return false;
                }
                else
                {
                    Account account = (Account)queryableAccount;
                    account.LastSavedLatitude = location.Latitude;
                    account.LastSavedLongitude = location.Longitude;

                    context.Entry(account).State = EntityState.Modified;
                    context.SaveChanges();

                    return true;
                }
            }
        }

        public LocationType GetUserMapCenter(string accountId)
        {
            using (var context = new SCADADbContext())
            {
                Account account = context.Accounts.FirstOrDefault(acc => acc.Id == accountId);

                if (account == null)
                {
                    return null;
                }
                else
                {
                    return new LocationType(account.LastSavedLatitude, account.LastSavedLongitude);
                }
            }
        }

    }
}
