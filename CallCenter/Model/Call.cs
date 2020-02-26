using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace CallCenter.Model
{
    /// <summary>
    /// Класс разговора
    /// Запускаю stopwatch, чтобы отследить, сколько времени прошло с момента создания до момента принятия разговора оператором
    /// </summary>
    class Call
    {
        public string Name { get; private set; }
        public int Time { get; private set; }

        public Stopwatch Watch;
        public Call(string name, int callTimeStart, int callTimeFinish)
        {
            Watch = new Stopwatch();
            Watch.Start();
            Random rnd = new Random();
            Name = name;
            Time = rnd.Next(callTimeStart, callTimeFinish);
        }
    }
}
