using System;
using System.Runtime.Serialization;

namespace SCADADataContracts
{
    [DataContract]
    public class ItemDetails
    {
        int terminalComponentRoleId = 0;
        string name = "";
        double lastValue = 0;
        DateTime date;

        public ItemDetails(int terminalComponentRoleId, string name, double lastValue, DateTime date)
        {
            this.terminalComponentRoleId = terminalComponentRoleId;
            this.name = name;
            this.lastValue = lastValue;
            this.date = date;
        }

        [DataMember]
        public int TerminalComponentRoleId
        {
            get { return terminalComponentRoleId; }
            set { terminalComponentRoleId = value; }
        }

        [DataMember]
        public double LastValue
        {
            get { return lastValue; }
            set { lastValue = value; }
        }

        [DataMember]
        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }

        [DataMember]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
    }
}
