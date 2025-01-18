using System.Runtime.Serialization;

namespace SCADADataContracts
{
    [DataContract]
    public class UnitItemFullness
    {
        int id = 0;
        double lastValue = 0;
        double[] subLastValues = null;

        public UnitItemFullness(int id, double lastValue)
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
        [DataMember]
        public double[] SubLastValues
        {
            get { return subLastValues; }
            set { subLastValues = value; }
        }

    }
}
