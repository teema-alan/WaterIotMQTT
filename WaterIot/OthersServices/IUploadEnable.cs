using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaterIot.OthersServices
{
    public interface IUploadEnable
    {
        bool Send(string topic, string message);
    }
}
