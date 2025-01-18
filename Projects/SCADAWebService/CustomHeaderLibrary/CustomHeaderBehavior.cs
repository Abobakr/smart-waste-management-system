using System.Collections.Generic;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace CustomHeader
{
    public class CustomHeaderBehavior : IEndpointBehavior
    {
        private readonly IDictionary<string, string> customHttpHeaders;

        public CustomHeaderBehavior(IDictionary<string, string> customHttpHeaders)
        {
            this.customHttpHeaders = customHttpHeaders;
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            var customHeaderMessageInspector = new CustomHeaderMessageInspector(customHttpHeaders);
            clientRuntime.MessageInspectors.Add(customHeaderMessageInspector);
        }

        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
        }

        public void Validate(ServiceEndpoint endpoint)
        {
        }
    }
}
