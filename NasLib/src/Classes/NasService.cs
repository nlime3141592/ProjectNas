using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAS
{
    public abstract class NasService
    {
        public abstract ServiceResult Execute();
    }
}
