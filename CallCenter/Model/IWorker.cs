using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallCenter.Model
{
    interface IWorker
    {
        void DoWork(Call call);
    }
}
