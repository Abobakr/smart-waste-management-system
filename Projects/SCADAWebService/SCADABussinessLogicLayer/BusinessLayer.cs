using DomainDataEntities;
using SCADADataAccessLayer;
using SCADADataContracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SCADABussinessLogicLayer
{
    public class BusinessLayer : IBusinessLayer
    {
        private readonly IGlobalTypeRepository globalTypeRepository;        

        public BusinessLayer()
        {
            globalTypeRepository = new GlobalTypeRepository();
        }

        public IList<UnitItemDAL> GetUnitItems(UnitItemFilter unitItemFilter)
        {
            //********************************************* Filter parameters Logic *********************************************//

            int Id = -1;
            bool isTerminalId = int.TryParse(unitItemFilter.Id, out Id);


            if (isTerminalId)
            {
                unitItemFilter.TerminalIdes = new List<int>();
                unitItemFilter.TerminalIdes.Add(Id);
            }
            else // we have account Id 
            {
                string accountId = unitItemFilter.Id;
                unitItemFilter.TerminalIdes = globalTypeRepository.GetTerminals(accountId).Select(ter => ter.Id).ToList();
            }

            unitItemFilter.IsFirstRequest = (unitItemFilter.TerminalIdes.Count > 1) ? true : false;

            if (unitItemFilter.DateTime.Equals(DateTime.Now))
            {
                TimeSpan oneMinute = new TimeSpan(0, 1, 0);
                unitItemFilter.LimitDate = DateTime.Now.Subtract(oneMinute);
            }
            else
            {
                unitItemFilter.LimitDate = unitItemFilter.DateTime;
            }

            IList<UnitItemData> unitItemList = globalTypeRepository.GetUnitItemsByTerminal(unitItemFilter);

            //********************************************* Average or Formula Logic *********************************************//

            IList<UnitItemDAL> unitItemListFinal = new List<UnitItemDAL>();

            var positions = (from ui in unitItemList
                             select new { ui.Location.Latitude, Longtitude = ui.Location.Longitude }).ToList();

            for (int i = 0; i < unitItemList.Count; i++)
            {
                UnitItemData unitItem = unitItemList[i];
                bool isOverFlow = false;
                int count = positions.Count(pos => unitItem.Location.Latitude == pos.Latitude && unitItem.Location.Longitude == pos.Longtitude);
                if (count > 1)
                {
                    IList<UnitItemData> commonPosSubList = unitItemList.Where(ui => ui.Location.Latitude == unitItem.Location.Latitude && ui.Location.Longitude == unitItem.Location.Longitude).ToList();

                    IList<int> activeModeIdes = (from ui in commonPosSubList
                                          select ui.ActiveModeId).ToList();

                    for (int j = 0; j < commonPosSubList.Count; j++)
                    {
                        unitItem = commonPosSubList[j];
                        int activeModeCount = activeModeIdes.Count(tcrId => unitItem.ActiveModeId == tcrId);

                        if (activeModeCount > 1)
                        {
                            IList<UnitItemData> commonActiveModeSubList = commonPosSubList.Where(ui => ui.ActiveModeId == unitItem.ActiveModeId).ToList();

                            IList<int> unitItemIdes = (from ui in commonPosSubList
                                                       select ui.Id).ToList();


                            for (int l = 0; l < commonActiveModeSubList.Count; l++)
                            {
                                unitItem = commonPosSubList[l];

                                int unitItemCount = unitItemIdes.Count(uiId => unitItem.Id == uiId);

                                if (unitItemCount > 1)
                                {
                                    IList<UnitItemData> commonUnitItemsSubList = commonActiveModeSubList.Where(ui => ui.Id == unitItem.Id).ToList();
                                    for (int k = 0; k < commonUnitItemsSubList.Count; k++)
                                    {
                                        UnitItemData deletedItem = commonUnitItemsSubList[k];
                                        commonActiveModeSubList.Remove(deletedItem);
                                        commonPosSubList.Remove(deletedItem);
                                        unitItemList.Remove(deletedItem);
                                    }
                                    unitItem.LastValue = commonUnitItemsSubList.Average(ui => ui.LastValue);
                                    commonActiveModeSubList.Add(unitItem);
                                }
                            }

                            // after making sure that all unit items now are aggregated // multiple units time
                            unitItem = commonActiveModeSubList[0];
                            activeModeCount = commonActiveModeSubList.Count;
                            if (activeModeCount > 1)
                            {
                                unitItem.SubLastValues = new double[activeModeCount];
                                unitItem.SubItemIdes = new int[activeModeCount];
                                for (int k = 0; k < activeModeCount; k++)
                                {
                                    UnitItemData deletedItem = commonActiveModeSubList[k];
                                    unitItem.SubLastValues[k] = deletedItem.LastValue;
                                    unitItem.SubItemIdes[k] = deletedItem.Id;

                                    commonPosSubList.Remove(deletedItem);
                                    unitItemList.Remove(deletedItem);

                                    // new if any is over flow
                                    if (deletedItem.LastValue >= deletedItem.AlertValue)
                                    {
                                        isOverFlow = true;
                                    }
                                }

                                unitItem.LastValue = commonActiveModeSubList.Average(ui => ui.LastValue);
                                UnitItemDAL unitItemDAL = (UnitItemDAL)unitItem;
                                unitItemDAL.IsOverflow = isOverFlow;
                                unitItemListFinal.Add(unitItemDAL);
                            }
                            else
                            {
                                unitItemListFinal.Add((UnitItemDAL)unitItem);
                            }

                        }
                        else
                        {
                            unitItemListFinal.Add((UnitItemDAL)unitItem);
                        }
                    }
                }
                else
                {
                    unitItemListFinal.Add((UnitItemDAL)unitItem);
                }
            }

            for(int i = 0 ; i<unitItemListFinal.Count;i++)
            {
                UnitItemDAL item = unitItemListFinal[i];

                if (!unitItemFilter.IsSubFilter && (item.LastValue < unitItemFilter.MinValue || item.LastValue > unitItemFilter.MaxValue))
                {
                    unitItemListFinal.RemoveAt(i);
                    i--;
                    continue;
                }

                if (item.LastValue >= item.AlertValue)
                {
                    item.IsOverflow = true;
                }
            }
            return unitItemListFinal;
        }

       
        public IList<UnitItemDetails> GetUnitItemDetails(int unitItemId)
        {
            IList<UnitItemDetails> unitItemDetails = globalTypeRepository.GetUnitItemDetails(unitItemId);

            foreach (UnitItemDetails item in unitItemDetails)
            {
                if (item.LastValue >= item.AlertValue)
                {
                    item.IsOverflow = true;
                }
            }

            return unitItemDetails;
        }

        public IList<UnitTypeContract> GetUnitTypes(string accountId)
        {
            return globalTypeRepository.GetUnitTypes(accountId);
        }

        public void AddNewUnitItem(UnitItemDAL itemLocation)
        {
            ISCADARepository<UnitItem> mySCADARepository = new SCADARepository<UnitItem>();
            mySCADARepository.Add(new UnitItem { Latitude = itemLocation.Location.Latitude, Longtitude = itemLocation.Location.Longitude, UnitTypetId = 1, ObjectState = EntityObjectState.Added });
        }

        public IList<TerminalType> GetTerminals(string accountId)
        {
            return globalTypeRepository.GetTerminals(accountId);
        }

        public IList<FilterComponent> GetFilterComponents(string accountId)
        {
            return globalTypeRepository.GetFilterComponents(accountId);
        }

        public bool UpdateCenter(LocationType location, string accountId)
        {
             bool result = globalTypeRepository.UpdateCenter(location, accountId);
             return result;
        }

        public LocationType GetUserMapCenter(string accountId)
        {
            return globalTypeRepository.GetUserMapCenter(accountId);
        }
        
    }
}
