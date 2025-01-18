using System.Runtime.Serialization;

namespace SCADADataContracts
{
    // Use a data contract as illustrated in the sample below to add composite types to service operations.

    [DataContract]
    public class UnitTypeContract : IdentityType
    {
        
        //double capacity = 0;

        public UnitTypeContract(int id, string name,double capacity):base(id,name)
        {
            //this.capacity = capacity;
        }

        //[DataMember]
        //public double Capacity
        //{
        //    get { return capacity; }
        //    set { capacity = value; }
        //}
    }

}
