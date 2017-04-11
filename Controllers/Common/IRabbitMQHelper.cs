using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace comp_api.common
{
    public interface IRabbitMQHelper
    {
        void connect();
        void sendMessage(string message);
        //void startExchange(string exchangeName);
    }
}
