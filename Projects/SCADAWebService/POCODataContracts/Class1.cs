using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace POCODataContracts
{
    [DataContract]
    public class CompositeType
    {
        bool _boolValue = true;
        string _stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return _boolValue; }
            set { _boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return _stringValue; }
            set { _stringValue = value; }
        }
    }
}
