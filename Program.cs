using System;
using System.Threading;

namespace H2_ProducerConsumerExercise5
{
    class Program
    {
        public static string[] basket = new string[] { "one", "two", "three", "four", "five" };//Creating the Array/Buffer
        public static volatile int counter = 0;//Just a counter to keep track of the indexnumber in the array
        public static Random random = new Random(); // public static Random random = new Random();//initialising a random
        public static object _lock = new object();//The lock object

        static void Main(string[] args)
        {
            Thread producer = new Thread(Produce);//initializing the threads
            Thread consumer = new Thread(Consume);
            producer.Start();//Starts the threads
            Thread.Sleep(100);//Just a little break to get things syncronized from start
            consumer.Start();
        }

        static void Produce()//Method to produce to the buffer
        {
            while (true)//A loop to ensure that the thread keeps running
            {
                Monitor.Enter(_lock);
                int sleepProduce = random.Next(100, 300); //(just played around with some random timing for the sleep to visualize what is happening)

                try//Do the following code if possible
                {
                    if (counter == 5)//if counter = 5 then enter the wait state
                    {
                        Console.WriteLine($"Producer is now waiting for consumer {counter}");

                        Monitor.Wait(_lock);
                    }

                    if (counter < basket.Length)//If counter is less than the buffers length, then write the content of the array with index number equal to the counter and add one to the counter
                    {
                        Console.WriteLine($"Producer produced: {basket[counter]}");
                        counter++;//adding one to the counter
                        Monitor.Pulse(_lock);//Sending signal to other thread
                        Monitor.Exit(_lock);//Release the lock
                        Thread.Sleep(sleepProduce); //(played around with some random timing for the sleep to visualize what is happening)
                    };
                }
                finally
                {
                }
            }
        }




        static void Consume()//Method to consume from the buffer
        {
            while (true)//A loop to ensure that the thread keeps running
            {
                Monitor.Enter(_lock);
                int sleepConsume = random.Next(100, 300); //(played around with some random timing for the sleep to visualize what is happening)

                try//Do the following code if possible
                {
                    if (counter == 0)//if counter = -1 then enter the wait state
                    {
                        Monitor.Wait(_lock);
                        Console.WriteLine($"Consumer is now waiting for producer {counter}");
                    }

                    if (counter > 0)//If counter is higher than the buffers length, then write the content of the array with index number equal to the counter-1 (to avoid out of bounce) and add one to the counter
                    {
                        Console.WriteLine($"Consumer consumed: {basket[counter - 1]}");
                        counter--;//reducing counter with one
                        Monitor.Pulse(_lock);//Sending signal to other thread
                        Monitor.Exit(_lock);//Release the lock
                        Thread.Sleep(sleepConsume); //(just played around with some random timing for the sleep to visualize what is happening)
                    }
                }
                finally
                {
                }
            }
        }
    }
}


