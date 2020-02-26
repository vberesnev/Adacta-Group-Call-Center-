using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace CallCenter.Model
{
    /// <summary>
    /// Класс фирмы - коллцентра
    /// </summary>
    public class Firm
    {
        private IWorker[] operators;
        internal ConcurrentQueue<IWorker> FreeOperators;
        private IWorker[] managers;
        internal ConcurrentQueue<IWorker> FreeManagers;
        internal IWorker Director;
        internal ConcurrentQueue<IWorker> FreeDirector;

        private double AvgCallTime; //среднее время разговора (нужно для вычисления примерного времени ожидания)

        /// <summary>
        /// Конструктор класса фирмы колл-центра
        /// </summary>
        /// <param name="operatorsCount">Количество операторов</param>
        /// <param name="managersCount">Количество менеджеров</param>
        /// <param name="avgCallTime">Среднее время разговора в милисекундах</param>
        public Firm(int operatorsCount, int managersCount, double avgCallTime)
        {
            FreeOperators = new ConcurrentQueue<IWorker>();
            FreeManagers = new ConcurrentQueue<IWorker>();
            FreeDirector = new ConcurrentQueue<IWorker>();

            AvgCallTime = avgCallTime/1000; //делю на 1000, потому что передаю в миллисекундах

            operators = new IWorker[operatorsCount];
            for (int i = 0; i < operators.Length; i++)
            {
                operators[i] = new Operator($"Оператор {i + 1}");
                FreeOperators.Enqueue(operators[i]);
            }

            managers = new IWorker[managersCount];
            for (int i = 0; i < managers.Length; i++)
            {
                managers[i] = new Manager($"Менеджер {i + 1}");
                FreeManagers.Enqueue(managers[i]);
            }

            Director = new Director();
            FreeDirector.Enqueue(Director);
        }
        /// <summary>
        /// Метод возвращает ПРИМЕРНОЕ количество секунд ожидания до ответа оператора 
        /// </summary>
        /// <param name="numberInQueue">Место в очереди</param>
        /// <returns></returns>
        public double WaitingTime(int numberInQueue)
        {
            return  Math.Round(((numberInQueue-1) * AvgCallTime) / (operators.Length + managers.Length + 1));
        }
    }
}
