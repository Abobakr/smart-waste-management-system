using System.Runtime.Serialization;

namespace SCADADataContracts
{
    [DataContract]
    public class UnitItemLocation : UnitItemData
    {
        double latitude = 0;
        double longtitude = 0;

        public UnitItemLocation(int id, int terminalComponentRoleId, double lastValue, double latitude, double longtitude) //int terminalItemId,
        {
            this.id = id;
            //this.terminalItemId = terminalItemId;
            this.terminalComponentRoleId = terminalComponentRoleId;
            this.lastValue = lastValue;
            this.latitude = latitude;
            this.longtitude = longtitude;
        }

        [DataMember]
        public double Latitude
        {
            get { return latitude; }
            set { latitude = value; }
        }

        [DataMember]
        public double Longtitude
        {
            get { return longtitude; }
            set { longtitude = value; }
        }

    }
}
