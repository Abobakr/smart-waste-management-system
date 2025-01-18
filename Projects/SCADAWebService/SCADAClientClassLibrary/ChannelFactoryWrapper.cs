using System;
using System.ServiceModel;

namespace SCADAWebServiceClientLibrary
{
    public class ChannelFactoryWrapper<T> : IDisposable where T : class
    {

        ChannelFactory<T> channelFactory = null;
        private T channel = null;

        private readonly object lockObject = new object();
        private bool disposed;

        public ChannelFactoryWrapper(string configName)
        {
            channelFactory = new ChannelFactory<T>(configName);
            disposed = false;
        }

        public T Channel
        {
            get
            {
                if (disposed)
                {
                    throw new ObjectDisposedException("Resource ServiceWrapper<" + typeof(T) + "> has been disposed");
                }

                lock (lockObject)
                {
                    if (channel == null)
                    {
                        channel = channelFactory.CreateChannel();
                        ((IClientChannel)channel).Open();
                    }
                }
                return channel;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        public void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    lock (lockObject)
                    {
                        if (channel != null)
                        {
                            ((IClientChannel)channel).Close();
                        }
                        if (channelFactory != null)
                        {
                            channelFactory.Close();
                        }
                    }

                    channel = null;
                    channelFactory = null;
                    disposed = true;
                }
            }
        }
    }
}