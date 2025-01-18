using SCADABussinessLogicLayer;
using SCADADataContracts;
using SCADAServiceContracts;
using System;
using System.Collections.Generic;
using System.ServiceModel;


// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service" in code, svc and config file together.
[ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
public class SCADAWebService :ISCADAServiceContracts
{
    IBusinessLayer businessLayer = new BusinessLayer();

    public IList<UnitItemDAL> GetUnitItems(UnitItemFilter unitItemFilter)
    {
        return businessLayer.GetUnitItems(unitItemFilter);
    }

    public IList<UnitItemDetails> GetUnitItemDetails(string unitItemId)
    {
        int id = -1;
        int.TryParse(unitItemId, out id);
        return businessLayer.GetUnitItemDetails(id);
    }

    public IList<UnitTypeContract> GetUnitTypes(string accountId)
    {
        return businessLayer.GetUnitTypes(accountId);
    }

    public bool AddNewUnitItem(UnitItemDAL itemLocation)
    {
        businessLayer.AddNewUnitItem(itemLocation);
        return true;
    }
    
    public IList<TerminalType> GetTerminals(string accountId)
    {
        return businessLayer.GetTerminals(accountId);
    }

    public IList<FilterComponent> GetFilterComponents(string accountId)
    {
        return businessLayer.GetFilterComponents(accountId);
    }

    public bool UpdateCenter(LocationType location)
    {
        string accountId = "5ec709f8-4677-434d-b45e-e1600050d505";
        bool result = businessLayer.UpdateCenter(location, accountId);
        return result;
    }

    public LocationType GetUserMapCenter(string accountId)
    {
        return businessLayer.GetUserMapCenter(accountId);
    }
}
