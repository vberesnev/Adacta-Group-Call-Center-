using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace CallCenter.Model
{
    /// <summary>
    /// Основной класс работы колл центра. Перекладывает разговоры с очереди входяхих разговоров на операторов или ставит разговор в ожидание
    /// </summary>
    class CallBroker
    {
        private Firm firm;
        private CallReceiver callReceiver;
        private ConcurrentQueue<Call> waitCalls; //очередь "на ожидании". Очередь их тех, чей звонок приняли, но не смогли обработать из-за загрузки системы
       
        /// <summary>
        /// Конструктор класса работы колл центра
        /// </summary>
        /// <param name="firm">Объект колл центра</param>
        /// <param name="callReceiver">Объект обработчика входящих звонков</param>
        public CallBroker(Firm firm, CallReceiver callReceiver)
        {
            this.firm = firm;
            this.callReceiver = callReceiver;
            waitCalls = new ConcurrentQueue<Call>();
        }

        /// <summary>
        /// Основной метод работы.
        /// Вначале смотрим на очередь ожидающих звонков. Если там кто-то есть, работаем с ним, НО НЕ СНИМАЕМ с очереди, 
        /// потому что возможна ситуация, когда ему опять не ответят.
        /// Затем смотрим на очередь вновь поступивших звонков, берем оттуда первый и сразу в обработку.
        /// </summary>
        public void Work()
        {
            Call newCall;
            Call waitCall;

            if (waitCalls.TryPeek(out waitCall))
                SendWaitingCallToWork(waitCall);
            
            if (callReceiver.Calls.TryDequeue(out newCall))
                SendNewCallToWork(newCall);

        }

        /// <summary>
        /// Метод обработки разговора из очереди ожидания. После исполнения разговор удаляется из очереди ожидания. Если все исполнители заняты - ничего не происходит.
        /// </summary>
        /// <param name="call"></param>
        private void SendWaitingCallToWork(Call call)
        {

            IWorker worker;
            
            if (firm.FreeOperators.TryDequeue(out worker))
            {
                Task task = Task.Run(() => WorkWithCall(call, worker));
                waitCalls.TryDequeue(out call);
            }
            else if (firm.FreeManagers.TryDequeue(out worker))
            {
                Task task = Task.Run(() => WorkWithCall(call, worker));
                waitCalls.TryDequeue(out call);
            }
            else if (firm.FreeDirector.TryDequeue(out worker))
            {
                Task task = Task.Run(() => WorkWithCall(call, worker));
                waitCalls.TryDequeue(out call);
            }
            else
            {
                return;
            }
        }

        /// <summary>
        /// Метод обработки нового разговора. Если все исполнители заняты, то разговор переводится в очередь ожидания
        /// </summary>
        /// <param name="call">Разговор</param>
        private void SendNewCallToWork(Call call)
        {
            
            IWorker worker;
            
            if (firm.FreeOperators.TryDequeue(out worker))
            {
                Task task = Task.Run(() => WorkWithCall(call, worker));
            }
            
            else if (firm.FreeManagers.TryDequeue(out worker))
            {
                Task task = Task.Run(() => WorkWithCall(call, worker));
            }
            
            else if (firm.FreeDirector.TryDequeue(out worker))
            {
                Task task = Task.Run(() => WorkWithCall(call, worker));
            }
            else
            {
                waitCalls.Enqueue(call);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{call.Name} в ожидании. {waitCalls.Count}-й в очереди, ждать примерно {firm.WaitingTime(waitCalls.Count)} секунд");
            }
        }

        /// <summary>
        /// Метод обработки разговора. После окончания выполнения исполнитель возвращается в свою очередь на получение нового разговора
        /// </summary>
        /// <param name="call">Разговор</param>
        /// <param name="worker">Исполнитель</param>
        private void WorkWithCall(Call call, IWorker worker)
        {
            worker.DoWork(call);
            if (worker is Operator)
                firm.FreeOperators.Enqueue(worker);
            if (worker is Manager)
                firm.FreeManagers.Enqueue(worker);
            if (worker is Director)
                firm.FreeManagers.Enqueue(worker);
        }
    }
}
