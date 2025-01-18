using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SCADADataContracts
{
    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public class FilterComponent : IdentityType
    {
        IList<FilterComponent> subFilterComponents;

        [DataMember]
        public IList<FilterComponent> SubFilterComponents
        {
            get { return subFilterComponents; }
            set { subFilterComponents = value; }
        }

        public FilterComponent(int id, string name)
            : base(id, name)
        {

        }
    }
}
