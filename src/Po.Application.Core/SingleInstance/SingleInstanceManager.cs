using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Po.Application.Core.SingleInstance
{
    public class SingleInstanceManager
    {
        public static ISingleInstanceService? Acquire()
        {
            var service = SingleInstanceServiceFactory.Create();

            if (!service.TryAcquireOwnership())
            {
                service.ActivateExistingInstance();
                service.Dispose();
                return null;
            }

            return service;
        }
    }
}
