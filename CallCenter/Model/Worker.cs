using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CallCenter.Model
{
    /// <summary>
    /// Абстрактный класс сотрудника колл-центра.
    /// Можно было обойтись одним интерфейсем IWorker и классом Worker, а уровни сотрудников делить с помощью Enum,
    /// но решил пойти через наследование чтобы была возможность расширяться
    /// </summary>
    abstract class Worker: IWorker 
    {
        public readonly string Name;

        protected Worker(string name)
        {
            Name = name;
        }

        public void DoWork(Call call)
        {
            call.Watch.Stop();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"{call.Name} - {this.Name} (ждал {call.Watch.Elapsed.Seconds} секунд)");
            Thread.Sleep(call.Time);
        }
    }
}
