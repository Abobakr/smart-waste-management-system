using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel.Configuration;

namespace CustomHeader
{
    public class CustomHeaderBehaviorExtensionElement : BehaviorExtensionElement
    {
        protected override object CreateBehavior()
        {
            Dictionary<string, string> customHeaders = null;
            if (CustomHeaders != null)
            {
                customHeaders = CustomHeaders.AllKeys.ToDictionary(key => key, key => CustomHeaders[key].Value);
            }
            return new CustomHeaderBehavior(customHeaders);
        }

        public override Type BehaviorType
        {
            get { return typeof(CustomHeaderBehavior); }
        }

        [ConfigurationProperty("headers", IsRequired = true)]
        [ConfigurationCollection(typeof(KeyValueConfigurationCollection))]
        public KeyValueConfigurationCollection CustomHeaders
        {
            get
            {
                return (KeyValueConfigurationCollection)base["headers"];
            }
        }
    }
}
