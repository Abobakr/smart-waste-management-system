using System.Runtime.Serialization;

namespace SCADADataContracts
{
    [DataContract]
    public class ItemFullness
    {
        int id = 0;
        double lastValue = 0;


        public ItemFullness(int id, double lastValue)
        {
            this.id = id;
            this.lastValue = lastValue;
        }

        [DataMember]
        public double LastValue
        {
            get { return lastValue; }
            set { lastValue = value; }
        }
     
        [DataMember]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
    }
}
