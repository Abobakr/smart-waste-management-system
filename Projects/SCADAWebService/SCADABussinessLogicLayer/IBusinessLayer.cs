using SCADADataContracts;
using System;
using System.Collections.Generic;

namespace SCADABussinessLogicLayer
{
    public interface IBusinessLayer
    {
        IList<UnitItemDAL> GetUnitItems(UnitItemFilter unitItemFilter);
        IList<UnitItemDetails> GetUnitItemDetails(int unitItemId);
        void AddNewUnitItem(UnitItemDAL itemLocation);
        IList<TerminalType> GetTerminals(string accountId);
        IList<UnitTypeContract> GetUnitTypes(string accountId);

        bool UpdateCenter(LocationType location, string accountId);

        LocationType GetUserMapCenter(string accountId);

        IList<FilterComponent> GetFilterComponents(string accountId);
    }
}
