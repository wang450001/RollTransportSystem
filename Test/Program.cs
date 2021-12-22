using Spiderweb.Device.Print;
using Spiderweb.Device.Weighter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            #region test1
            //Student stu = new Student(18);
            //Console.WriteLine(stu.Name);

            ////创建一个任务
            //Task<int> task = new Task<int>( () =>
            //{
            //    int sum = 0;
            //    Console.WriteLine("使用Task执行异步操作.");//------2
            //    for (int i = 0; i < 100; i++)
            //    {
            //        //Thread.Sleep(100);
            //        //await Task.Delay(100);
            //        sum += i;
            //    }
            //    return sum;
            //});
            ////启动任务,并安排到当前任务队列线程中执行任务(System.Threading.Tasks.TaskScheduler)
            //task.Start();
            //Console.WriteLine("主线程执行其他处理"); //----1
            ////任务完成时执行处理。
            //Task cwt = task.ContinueWith(t =>
            //{
            //    //Thread.Sleep(5000);
            //    Console.WriteLine("任务完成后的执行结果：{0}", t.Result.ToString());//------4
            //});
            //task.Wait();
            //Console.WriteLine("task执行结束");//-----3
            //cwt.Wait();
            //Console.WriteLine("task回调执行结束");//-------5

            //string[] messages = new string[] { "wang", "wen", "chang" };
            //string msg = $"{{\"message\":[{string.Join(",", messages) }]}}";
            //Console.WriteLine(msg);
            #endregion

            #region test2
            //CSerialWeighter weighter = CSerialWeighter.CreateInstance("Spiderweb.Device.Weighter.ToledoWeighter,Spiderweb.Device", "COM2");
            //weighter.ReceiveData += Weighter_ReceiveData;
            //weighter.SendMessage += Weighter_SendMessage;
            //weighter.ConnectChanged += Weighter_ConnectChanged;
            //weighter.Connect();

            //Console.ReadKey();
            //weighter.Disconnect();
            //weighter.Dispose();
            //weighter = null;
            #endregion

            #region test3
            CSerialInkjetPrinter inkjet = CSerialInkjetPrinter.CreateInstance("Spiderweb.Device.Print.KarmaInkjetPrinter,Spiderweb.Device", "COM2");
            inkjet.ReceiveData += Inkjet_ReceiveData;
            inkjet.SendMessage += Inkjet_SendMessage;
            inkjet.ConnectChanged += Inkjet_ConnectChanged;
            inkjet.Connect();
            inkjet.Write(new string[] { "1900/K 120 2143Kg", "XBAC2D0098" });

            Console.ReadKey();
            inkjet.Disconnect();
            inkjet.Dispose();
            inkjet = null;
            #endregion


        }

        private static void Inkjet_ConnectChanged(object sender, bool connected)
        {
            Console.WriteLine($"喷码机连接状态变化：{connected}");
        }

        private static void Inkjet_SendMessage(object sender, string msg)
        {
            Console.WriteLine($"接收到喷码机消息：{msg}");
        }

        private static void Inkjet_ReceiveData(object sender, object data)
        {
            Console.WriteLine($"接收到喷码机反馈数据：{data}");
        }

        private static void Weighter_ConnectChanged(object sender, bool connected)
        {
            Console.WriteLine($"仪表连接状态变化：{connected}");
        }

        private static void Weighter_SendMessage(object sender, string msg)
        {
            Console.WriteLine($"接收到仪表消息：{msg}");
        }

        private static void Weighter_ReceiveData(object sender, object data)
        {
            Console.WriteLine($"接收到仪表重量数据：{data}");
        }

        class Person
        {
            public int Age { get; set; }

            public string Name { get; set; }

            public string Address { get; set; }


            public Person()
            {

            }

            public Person(int age)
            {
                this.Age = age;
            }
        }

        class Student : Person
        {
            public Student()
            {
                this.Name = "spiderweb";
            }

            public Student(int age) : this()
            {
                this.Age = age;
            }     
        }

        public void TastMethod1(string name)
        {
            Thread.Sleep(6000);
            Console.WriteLine("name:" + name);
        }
        public void TastMethod2(string name)
        {
            Thread.Sleep(3000);
            Console.WriteLine("name:" + name);
        }
    }
}
