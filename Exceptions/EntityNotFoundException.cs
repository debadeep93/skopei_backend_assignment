using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkopeiBackendAssignment.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException() { }

        public EntityNotFoundException(string entityType, long id)
            : base(string.Format("No {0} found with the provided id {1}", entityType, id)) { }
    }
}
