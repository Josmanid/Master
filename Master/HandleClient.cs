using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Master
{
    internal class HandleClient
    {
        //number that keeps increasing each time a client connects.
        private static int chunkCounter = 0;
        public static void Handleclient(TcpClient socket) {
            // read and write to the slave
            NetworkStream stream = socket.GetStream();
            StreamReader streamReader = new StreamReader(stream);
            StreamWriter streamWriter = new StreamWriter(stream);
            streamWriter.AutoFlush = true;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            // splitting dictionary into 4 chunks
            string[] alllines = File.ReadAllLines("webster-dictionary.txt");
            // how many lines per chunk
            int chunkSize = alllines.Length / 4;
            //Make 4 chunks
            //TODO: make loop
            string[] chunk1 = alllines.Take(chunkSize).ToArray();
            string[] chunk2 = alllines.Skip(chunkSize).Take(chunkSize).ToArray();
            string[] chunk3 = alllines.Skip(chunkSize * 2).Take(chunkSize).ToArray();
            string[] chunk4 = alllines.Skip(chunkSize * 3).Take(chunkSize).ToArray();
            //array of arrays:
            string[][] chunks = { chunk1, chunk2, chunk3, chunk4 };
           
            //When a client connects, pick a chunk
            int index = chunkCounter % chunks.Length;
            chunkCounter++;
            string[] chunkToSend = chunks[index];

            //// send the chunks to the client( starting with one chunk)
            ////Since chunk1 is a string[], you need to loop through it and send each line one by one.
            //for (int i = 0; i < chunk1.Length; i++)
            //{
            //    //(looping through each string in the array):
            //    streamWriter.WriteLine(chunk1[i]);
            //}
            // now send the chunk chosed by the client instead of just one chunk
            foreach (string chunk in chunkToSend)
            {
                streamWriter.WriteLine(chunk);
            }
            streamWriter.WriteLine("The chunks is empty");
            // Later receive the result directly as a strings 
            string result;
            while ((result = streamReader.ReadLine()) != null)
            {   
                if (result == "DONE") break;
                Console.WriteLine(result + " Was given by slave");
            }
            stopwatch.Stop();

            Console.WriteLine($"Client {index} processed chunk in {stopwatch.ElapsedMilliseconds} ms");



            socket.Close();
        }
    }
}
