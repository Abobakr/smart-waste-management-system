using System.Runtime.Serialization;

namespace SCADADataContracts
{
    [DataContract]
    public class UnitItemData
    {
        protected int id = 0;
        protected int activeModeId = 0;
        protected double lastValue = 0;
        protected double alertValue = 0;
        protected double[] subLastValues = null;
        protected int[] subItemIdes = null;
        protected LocationType location;
                
        
        public UnitItemData() 
        { 
        
        }

        [DataMember]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        
        public int ActiveModeId
        {
            get { return activeModeId; }
            set { activeModeId = value; }
        }

        [DataMember]
        public double LastValue
        {
            get { return lastValue; }
            set { lastValue = value; }
        }

       
        [DataMember]
        public double[] SubLastValues
        {
            get { return subLastValues; }
            set { subLastValues = value; }
        }


        [DataMember]
        public int[] SubItemIdes
        {
            get { return subItemIdes; }
            set { subItemIdes = value; }
        }

        [DataMember]
        public LocationType Location
        {
            get { return location; }
            set { location = value; }
        }
        
        public double AlertValue
        {
            get { return alertValue; }
            set { alertValue = value; }
        }
    }
}
