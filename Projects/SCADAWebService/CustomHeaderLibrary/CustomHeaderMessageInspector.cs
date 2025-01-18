using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace CustomHeader
{
    public class CustomHeaderMessageInspector : IClientMessageInspector
    {
        private readonly IDictionary<string, string> customHttpHeaders;

        public CustomHeaderMessageInspector(IDictionary<string, string> customHttpHeaders)
        {
            if (customHttpHeaders == null) throw new ArgumentNullException("customHttpHeaders");
            this.customHttpHeaders = customHttpHeaders;
        }

        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            // Making sure we have a HttpRequestMessageProperty
            HttpRequestMessageProperty httpRequestMessageProperty;
            if (request.Properties.ContainsKey(HttpRequestMessageProperty.Name))
            {
                httpRequestMessageProperty = request.Properties[HttpRequestMessageProperty.Name] as HttpRequestMessageProperty;
                if (httpRequestMessageProperty == null)
                {
                    httpRequestMessageProperty = new HttpRequestMessageProperty();
                    request.Properties.Add(HttpRequestMessageProperty.Name, httpRequestMessageProperty);
                }
            }
            else
            {
                httpRequestMessageProperty = new HttpRequestMessageProperty();
                request.Properties.Add(HttpRequestMessageProperty.Name, httpRequestMessageProperty);
            }

            // Add custom headers to the WCF request
            foreach (var header in customHttpHeaders)
            {
                httpRequestMessageProperty.Headers.Add(header.Key, header.Value);
            }

            return null;
        }

        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
        }
    }
}
