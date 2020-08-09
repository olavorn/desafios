using api.Models.ServiceModel;
using System;

namespace tests
{
    public class FakeAcquirerFactory
    {
        public FakeAcquirerFactory()
        {
        }

        internal IAcquirerApi CreateAcquirer()
        {
            return new AcquirerApi();
        }
    }
}