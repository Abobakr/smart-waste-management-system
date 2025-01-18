using System.Runtime.Serialization;

namespace SCADADataContracts
{
    [DataContract]
    public class ItemLocation
    {
        int id = 0;
        double latitude = 0;
        double longtitude = 0;

        public ItemLocation(int id, double latitude, double longtitude)
        {
            this.id = id;
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
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
    }
}
