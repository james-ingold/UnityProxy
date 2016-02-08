using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace UnityProxy.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            string proxyAddress = @"http://windows:1367/";

            ServicePointManager.DefaultConnectionLimit = 20;
            ServicePointManager.MaxServicePointIdleTime = 10000;
            //send packets as soon as you get them
            ServicePointManager.UseNagleAlgorithm = false;
            //send both header and body together
            ServicePointManager.Expect100Continue = false;

            Proxy.Start(proxyAddress);
            System.Threading.Thread.Sleep(100000);
        }
    }
}
