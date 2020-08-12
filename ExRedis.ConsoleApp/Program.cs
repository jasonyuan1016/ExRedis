using ExRedis.Exchange.Service;
using StackExchange.Redis;
using System;
using System.Threading;

namespace ExRedis.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //StringDemo();
            //HashDemo();
            SetDemo();
            Console.ReadKey();
        }

        static void StringDemo()
        {
            //即key/value  
            //和Memcached类似
            using (ExString service = new ExString())
            {
                service.KeyFulsh();
                service.StringSet("sk1", "aaa");
                Console.WriteLine(service.StringGet("sk1"));
                Console.WriteLine(service.StringGetSet("sk1", "abc"));
                Console.WriteLine(service.StringGet("sk1"));
                service.StringAppend("sk1", "defg");
                Console.WriteLine(service.StringGet("sk1"));
                service.StringSet("sk1", "aaa", new TimeSpan(0, 0, 0, 5));
                Console.WriteLine(service.StringGet("sk1"));
                Thread.Sleep(5000);
                Console.WriteLine("5秒过期：" + service.StringGet("sk1"));
            }
        }

        static void HashDemo()
        {
            //hashtable：解决序列化的问题 节约内存空间，一个hash保存多个key-value
            Employee emp_1 = new Employee()
            {
                Id = 1,
                Name = "emp1",
                Description = "eeeeeeee11111111"
            };

            using (ExHash service = new ExHash())
            {
                service.KeyFulsh();
                service.HashSet<Employee>("employees", emp_1.Id.ToString(), emp_1);
                Console.WriteLine("HashExists Id:1-" + service.HashExists("employees", "1"));
                Console.WriteLine("HashExists Id:2-" + service.HashExists("employees", "2"));
                var _emp = service.HashGet<Employee>("employees", "1");
                _emp.Description = "xxxxx";
                service.HashSet<Employee>("employees", _emp.Id.ToString(), _emp);
                Console.WriteLine(service.HashGet<Employee>("employees", "1").Description);
            }
        }
        
        static void SetDemo()
        {
            //Set:key-List<value>  场景：去重、共同好友、共同关注
            using (ExSet service = new ExSet())
            {
                service.KeyFulsh();
                // ex去重，统计用户ip记录
                {
                    service.SetAdd<string>("user1", "36.235.120.63");
                    service.SetAdd<string>("user1", "36.235.120.63");
                    service.SetAdd<string>("user1", "36.225.120.63");
                    service.SetAdd<string>("user1", "36.235.120.63");
                    service.SetAdd<string>("user1", "36.235.111.63");
                    service.SetAdd<string>("user1", "36.235.120.63");
                    service.SetAdd<string>("user1", "33.235.120.63");
                    service.SetAdd<string>("user1", "36.235.120.66");
                    service.SetAdd<string>("user1", "36.235.120.66");
                }
                Console.WriteLine("IP Count:" + service.SetLength("user1"));
                service.SetAdd<string>("A", "1");
                service.SetAdd<string>("A", "2");
                service.SetAdd<string>("A", "3");
                service.SetAdd<string>("A", "4");
                service.SetAdd<string>("A", "5");

                service.SetAdd<string>("B", "3");
                service.SetAdd<string>("B", "4");
                service.SetAdd<string>("B", "5");
                service.SetAdd<string>("B", "6");
                service.SetAdd<string>("B", "7");
                Console.WriteLine("");
                Console.WriteLine("[并集]：");
                var AB_Union = service.SetCombine(SetOperation.Union, "A", "B");
                foreach (string str in AB_Union)
                {
                    Console.Write(str + " ");
                }
                Console.WriteLine("");
                Console.WriteLine("[交集] ：");
                var AB_Intersect = service.SetCombine(SetOperation.Intersect, "A", "B");
                foreach (string str in AB_Intersect)
                {
                    Console.Write(str + " ");
                }
                Console.WriteLine("");
                Console.WriteLine("AB[差集]：");
                var AB_Difference = service.SetCombine(SetOperation.Difference, "A", "B");
                foreach (string str in AB_Difference)
                {
                    Console.Write(str + " ");
                }
                Console.WriteLine("");
                Console.WriteLine("BA[差集]：");
                var BA_Difference = service.SetCombine(SetOperation.Difference, "B", "A");
                foreach (string str in BA_Difference)
                {
                    Console.Write(str + " ");
                }
            }
        }

        static void SortSetDemo()
        {
            using (ExSortedSet service = new ExSortedSet())
            {
                //去重   而且自带排序  
                service.KeyFulsh();//清理全部数据
                
            }
        }
    }
}
