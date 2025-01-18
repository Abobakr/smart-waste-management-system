using System;
using System.Runtime.Serialization;

namespace SCADADataContracts
{
    [DataContract]
    public class UnitItemDetails
    {
        string name = "";
        string roleName = "";
        string modelNo = "";
        double lastValue = 0;
        double alertValue = 0;
        bool isOverflow = false;

     

        DateTime date;

        public UnitItemDetails(string name, string modelNo,string roleName, double lastValue,double ?alertValue, DateTime date)
        {
            this.name = name;
            this.modelNo = modelNo;
            this.roleName = roleName;
            this.lastValue = lastValue;
            if (alertValue != null)
                this.alertValue =  (double)alertValue;
            else this.alertValue = double.MaxValue;
            this.date = date;
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
        [DataMember]
        public string ModelNo
        {
            get { return modelNo; }
            set { modelNo = value; }
        }
        [DataMember]
        public string RoleName
        {
            get { return roleName; }
            set { roleName = value; }
        }

        public double AlertValue
        {
            get { return alertValue; }
            set { alertValue = value; }
        }

        [DataMember]
        public bool IsOverflow
        {
            get { return isOverflow; }
            set { isOverflow = value; }
        }

    }
}
