using System.Runtime.Serialization;

namespace SCADADataContracts
{
    // Use a data contract as illustrated in the sample below to add composite types to service operations.

    [DataContract]
    public class LocationType
    {
        double latitude = 0;
        double longitude = 0;

        public LocationType(double latitude, double longtitude)
        {
            this.latitude = latitude;
            this.longitude = longtitude;
        }
       
        [DataMember]
        public double Latitude
        {
            get { return latitude; }
            set { latitude = value; }
        }

        [DataMember]
        public double Longitude
        {
            get { return longitude; }
            set { longitude = value; }
        }
    }

}
