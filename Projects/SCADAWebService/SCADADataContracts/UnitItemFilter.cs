using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SCADADataContracts
{
    [DataContract]
    public class UnitItemFilter
    {
        string id = "";
        int componentId = -1;
        int roleId = -1;
        double minValue = 0;
        double maxValue = 0;
        DateTime dateTime;
        bool isSubFilter = false;
        LocationType southWest;
        LocationType northEast;

        IList<int> terminalIdes;
        DateTime limitDate;
        bool isFirstRequest = false;
        
        

        public DateTime LimitDate
        {
            get { return limitDate; }
            set { limitDate = value; }
        }
        public IList<int> TerminalIdes
        {
            get { return terminalIdes; }
            set { terminalIdes = value; }
        }
        public bool IsFirstRequest
        {
            get { return isFirstRequest; }
            set { isFirstRequest = value; }
        }

        [DataMember]
        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        [DataMember]
        public int ComponentId
        {
            get { return componentId; }
            set { componentId = value; }
        }
        
        [DataMember]
        public int RoleId
        {
            get { return roleId; }
            set { roleId = value; }
        }

        [DataMember]
        public double MinValue
        {
            get { return minValue; }
            set { minValue = value; }
        }
        
        [DataMember]
        public double MaxValue
        {
            get { return maxValue; }
            set { maxValue = value; }
        }
        
        [DataMember]
        public DateTime DateTime
        {
            get { return dateTime; }
            set { dateTime = value; }
        }
        
        [DataMember]
        public bool IsSubFilter
        {
            get { return isSubFilter; }
            set { isSubFilter = value; }
        }
        
        [DataMember]
        public LocationType SouthWest
        {
            get { return southWest; }
            set { southWest = value; }
        }

        [DataMember]
        public LocationType NorthEast
        {
            get { return northEast; }
            set { northEast = value; }
        }


        public UnitItemFilter()
        {

        }

        public UnitItemFilter(string id, int componentId, int roleId, double minValue, double maxValue, DateTime dateTime, bool isSubFilter, LocationType southWest, LocationType northEast)
        {
            // TODO: Complete member initialization
            this.id = id;
            this.componentId = componentId;
            this.roleId = roleId;
            this.minValue = minValue;
            this.maxValue = maxValue;
            this.dateTime = dateTime;
            this.isSubFilter = isSubFilter;
            this.southWest = southWest;
            this.northEast = northEast;
            
        }

    }
}
