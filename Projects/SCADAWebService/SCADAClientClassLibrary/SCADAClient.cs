using SCADADataContracts;
using SCADAServiceContracts;
using SCADAWebServiceClientLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCADAWebServiceClientLibrary
{
    public class SCADAClient
    {
        private const string ENDPOINT_CONFIGURATION_NAME = "RestFulEndpointConfig";
        private static ChannelFactoryWrapper<ISCADAServiceContracts> channelFactoryWrapper = new ChannelFactoryWrapper<ISCADAServiceContracts>(ENDPOINT_CONFIGURATION_NAME);
        private static ISCADAServiceContracts channelInterface = channelFactoryWrapper.Channel;

        public static IList<UnitItemDAL> GetUnitItems(UnitItemFilter unitItemFilter)
        {
            return channelInterface.GetUnitItems(unitItemFilter);
        }

        public static IList<UnitItemDetails> GetUnitItemDetails(int unitItemId)
        {
            return channelInterface.GetUnitItemDetails(unitItemId.ToString());
        }

        public static IList<TerminalType> GetTerminals(string accountId)
        {
            return channelInterface.GetTerminals(accountId);
        }

        public static IList<UnitTypeContract> GetUnitTypes(string accountId)
        {
            return channelInterface.GetUnitTypes(accountId);
        }

        public static IList<FilterComponent> GetFilterComponents(string accountId)
        {
            return channelInterface.GetFilterComponents(accountId);
        }
        public static LocationType GetUserMapCenter(string accountId)
        {
            return channelInterface.GetUserMapCenter(accountId);
        } 
    }
}
