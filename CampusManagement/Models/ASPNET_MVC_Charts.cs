using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace CampusManagement.Models
{
    public class ASPNET_MVC_Charts
    {

        //DataContract for Serializing Data - required to serve in JSON format
        [DataContract]
        public class DataPoint1
        {
            public DataPoint1(string label, double y)
            {
                this.Label = label;
                this.Y = y;
            }

            //Explicitly setting the name to be used while serializing to JSON.
            [DataMember(Name = "label")]
            public string Label = "";

            //Explicitly setting the name to be used while serializing to JSON.
            [DataMember(Name = "y")]
            public Nullable<double> Y = null;
        }
    }
}  