using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace CallCenter.Model
{
    /// <summary>
    /// Класс обработчик звонков.
    /// </summary>
    class CallReceiver
    {
        public ConcurrentQueue<Call> Calls; //очередь входящих звонков
        private int callStart; //длительность разговора от
        private int callFinish; //длительность разговора до
        private int callRespawnTimeStart; //время респауна разговора от
        private int callRespawnTimeFinish; //время респауна разговора до

        /// <summary>
        /// Конструктор класса обрабочика входящих звонков
        /// </summary>
        /// <param name="cStart">Минимальное время разговора в миллисекундах</param>
        /// <param name="cFinish">Максимальное время разговора в миллисекундах</param>
        /// <param name="cRespStart">Минимальное время на респаун разговора в миллисекундах</param>
        /// <param name="cRespFinish">Максимальное время на респаун разговора в миллисекундах</param>
        public CallReceiver(int cStart, int cFinish, int cRespStart, int cRespFinish)
        {
            Calls = new ConcurrentQueue<Call>();
            callStart = cStart;
            callFinish = cFinish;
            callRespawnTimeStart = cRespStart;
            callRespawnTimeFinish = cRespFinish;
        }

        public void Work()
        {
            int i = 0;
            Random rnd = new Random();
            while (true)
            {
                Calls.Enqueue(new Call($"Звонок {++i}", callStart, callFinish));
                Thread.Sleep(rnd.Next(callRespawnTimeStart, callRespawnTimeFinish));
            }
        }
    }
}
