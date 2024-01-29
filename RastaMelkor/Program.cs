using System;
using System.IO;

namespace RastaMelkor
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("[>] Reading assembly as Byte[]");
            byte[] fileBytes = File.ReadAllBytes(@"demoModule.exe");

            Melkor melkor = new Melkor();
            melkor.Init(ref fileBytes);

            try
            {
                melkor.InvokeAndUnload("dothething");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            Console.Write("Done");
            Console.Read();
        }
    }
}
