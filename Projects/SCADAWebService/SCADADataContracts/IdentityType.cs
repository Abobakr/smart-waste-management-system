using System.Runtime.Serialization;

namespace SCADADataContracts
{
    // Use a data contract as illustrated in the sample below to add composite types to service operations.

    [DataContract]
    public class IdentityType
    {
        int id = -1;
        string name = "";

        public IdentityType(int id, string name)
        {
            this.id = id;
            this.name = name;
        }

        [DataMember]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        
        [DataMember]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
    }

}
