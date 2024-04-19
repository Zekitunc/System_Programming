using System.Threading.Channels;

internal class Program
{
    private static void Main(string[] args)
    {   
        #region 1. soru
        
        Console.WriteLine("ANA İŞLEM SLEEP VE JOİN KULLANARAK");
        Thread th = new Thread(Work);
        th.Start();
        th.Join();
        Console.WriteLine("bitti");
        //yukarıdaki join işlemini iki farklı şekilde join kullanmadan yapınız
        
        
        Thread th2 = new Thread(WorkManual);
        Console.WriteLine("MANUEL RESET EVENT 1. YÖNTEM");
        th2.Start();
        manual.WaitOne();
        Console.WriteLine("bitti 2"); 

        Console.WriteLine("BARRİER İLE 2. YÖNTEM");
        Thread th3 = new Thread(WorkBarrier);
        th3.Start();
        br.SignalAndWait();
        Console.WriteLine("bitti3");


        Thread th4 = new Thread(WorkOto);
        Console.WriteLine("auto  RESET EVENT 4. YÖNTEM");
        th4.Start();
        otomatik.WaitOne();
        Console.WriteLine("bitti 5"); 
        #endregion
        


        //2. SORU A ŞIKKI
        Console.WriteLine("for");
        for (int i = 0; i < 10; i++)
        {
            new Thread(() => { Console.WriteLine(i); semfor.Release(); }).Start();
            semfor.WaitOne();
        } //çıktı 22 44 66 89 gibi çıkıyor hatalı çıktı nasıl düzeltiriz
        

        //B ŞIKKI
        
        Task<string> task = Task<string>.Factory.StartNew(() => printData("str"));
        Console.WriteLine(task.Result);

        Task<string> task2 = Task<string>.Factory.StartNew(printData,"zeki");
        Console.WriteLine(task2.Result); 

        //C ŞIKKI
        
        ThreadPool.SetMinThreads(1,1);
        ThreadPool.SetMaxThreads(5,5);
        Task.Run(() =>printint(12)).Wait();
        

        
        for(int i = 0; i< 10; i++)
        {
            Thread th12 = new Thread(interlockedA); //veya lockedA 1. ve 2. yöntem
            th12.Name = "th" + i;
            th12.Start();
            th12.Join();

        }
        Console.WriteLine(a);
        
        Parallel.For(0, 10, (i) =>{ a++; }) ; //3. yöntem
        Console.WriteLine(a);

        
        for (int i = 0;i<10;i++)
        {
            Thread th13 = new Thread(() => { a++; sem.Release(); });
            th13.Start();
            
            sem.WaitOne();
        }
        Console.WriteLine(a); 

        //4. SORU


        for (int i = 0; i < 5; i++)
        {
            Thread th14 = new Thread(yaz);
            th14.Name = i.ToString();
            th14.Start();
            th14.Join();
        }


       Func<int,int,string> f = (a,b) => { return "a:" + a + "b" + b; };
        


    }

    public delegate int islem(int x, int y);
    public static int topla (int x, int y) { return x + y; }
    public static void yaz()
    {
        Console.WriteLine(Thread.CurrentThread.Name
            + " " + Convert.ToInt32(Thread.CurrentThread.Name) + " "+
            Convert.ToInt32(Thread.CurrentThread.Name));

    }

    static int a = 0;

    public static Semaphore sem = new Semaphore(0, 5);
    public static Semaphore semfor = new Semaphore(0, 1);

    public static object obj = "";
    public static void lockedA()
    {
        lock(obj)
        { a++; Console.WriteLine(Thread.CurrentThread.Name); }

    }

    public static void interlockedA()
    {
        Interlocked.Increment(ref a);
    }




    public static void printint(int data)
    { Console.WriteLine(data); }
    public static string printData(object data)
    {
        return (string) data;
    }
    public static void Work()
    {
        Console.WriteLine("Merhaba"); Thread.Sleep(2000) ;
    }
    public static void WorkManual()
    {
        Console.WriteLine("Merhaba"); Thread.Sleep(2000); manual.Set(); manual.Reset();
    }
    public static void WorkOto()
    {
        Console.WriteLine("Merhaba"); Thread.Sleep(2000); otomatik.Set();
    }
    public static void WorkBarrier()
    {
        Console.WriteLine("Merhaba"); Thread.Sleep(2000); br.SignalAndWait(); ;
    }
    static ManualResetEvent manual = new ManualResetEvent(false);
    static AutoResetEvent otomatik= new AutoResetEvent(false);
    static Barrier br = new Barrier(2);
}