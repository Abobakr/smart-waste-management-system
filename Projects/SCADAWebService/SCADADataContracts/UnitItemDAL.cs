using System.Runtime.Serialization;

namespace SCADADataContracts
{
    [DataContract]
    public class UnitItemDAL : UnitItemData  // UnitItemDAL ; Data And Location ; Data Access Layer
    {

        bool isOverflow = false;

        [DataMember]
        public bool IsOverflow
        {
            get { return isOverflow; }
            set { isOverflow = value; }
        }



        public UnitItemDAL(int id, int activeModeId, double lastValue, double alertValue , double latitude, double longtitude) 
        {
            this.id = id;
            this.activeModeId = activeModeId;
            this.lastValue = lastValue;
            this.alertValue = alertValue;

            location = new LocationType(latitude, longtitude);           
        }

    }
}
