using SCADADataContracts;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace SCADAServiceContracts
{
    [ServiceContract]
    public interface ISCADAServiceContracts
    {
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetUnitItems")]
        [OperationContract]
        IList<UnitItemDAL> GetUnitItems(UnitItemFilter unitItemFilter);
        
        [WebGet(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "UnitItemDetials/{unitItemId}")]
        [OperationContract]
        IList<UnitItemDetails> GetUnitItemDetails(string unitItemId);


        [WebGet(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "UnitTypes/{accountId}")]
        [OperationContract]
        IList<UnitTypeContract> GetUnitTypes(string accountId);
        
        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "AddUnitItem")]
        [OperationContract]
        bool AddNewUnitItem(UnitItemDAL itemLocation);


        [WebGet(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "Terminals/{accountId}")]
        [OperationContract]
        IList<TerminalType> GetTerminals(string accountId);


        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "SetMapCenter")]
        [OperationContract]
        bool UpdateCenter(LocationType location);

        [WebGet(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetMapCenter/{accountId}")]
        [OperationContract]
        LocationType GetUserMapCenter(string accountId);

        [WebGet(RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, UriTemplate = "FilterComponents/{accountId}")]
        [OperationContract]
        IList<FilterComponent> GetFilterComponents(string accountId);
    }
}