using System.Runtime.Serialization;

namespace SCADADataContracts
{
    [DataContract]
    public class ItemLocation
    {
        double latitude = 0;
        double longtitude = 0;

        double itemId = 0;
        public ItemLocation(int itemId, double latitude, double longtitude)
        {
            this.itemId = itemId;
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

        [DataMember]
        public double ItemId
        {
            get { return itemId; }
            set { itemId = value; }
        }
    }
}
