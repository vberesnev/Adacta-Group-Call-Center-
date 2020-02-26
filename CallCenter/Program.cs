using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CallCenter.Model;
using System.IO;

namespace CallCenter
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Получение настроек программы . . .");
            Settings settings = Settings.Load(Path.Combine(Environment.CurrentDirectory, "settings.ini"));
            Console.WriteLine($"Сегодня на смене {settings.OperatorsCount} операторов, {settings.ManagersCount} менеджеров и 1 директор");
            Firm firm = new Firm(settings.OperatorsCount, settings.ManagersCount, (settings.CallTimeFinish + settings.CallTimeStart)/2.0);
            CallReceiver callReceiver = new CallReceiver(settings.CallTimeStart, settings.CallTimeFinish, settings.CallRespawnStart, settings.CallRespawmFinish);
            CallBroker callBroker = new CallBroker(firm, callReceiver);

            //отдельная таска для генерирования звонков
            Task callReceiverTask = new Task(() => callReceiver.Work());
            callReceiverTask.Start();

            while (true)
            {
                callBroker.Work();
            }
        }
    }
}
