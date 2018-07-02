using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices; // 用 DllImport 需用此 命名空间  
using System.Reflection; // 使用 Assembly 类需用此 命名空间  
using System.Reflection.Emit; // 使用 ILGenerator 需用此 命名空间   

namespace pcb_demo_cs
{

    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public struct PredictRes
    {

        // int
        public int size;
        // result
        public IntPtr result;
    }

    class Program
    {
        [DllImport("D:\\fc_example\\x64\\Debug\\testlib.dll", SetLastError = true, EntryPoint = "init", CallingConvention = CallingConvention.Cdecl)]
        static extern int init([MarshalAs(UnmanagedType.LPStr)] string pythonpath, [MarshalAs(UnmanagedType.LPStr)] string modelpath, [MarshalAs(UnmanagedType.LPStr)] string weightpath);
        [DllImport("D:\\fc_example\\x64\\Debug\\testlib.dll", SetLastError = true, EntryPoint = "uninit", CallingConvention = CallingConvention.Cdecl)]
        static extern int uninit();
        [DllImport("D:\\fc_example\\x64\\Debug\\testlib.dll", SetLastError = true, EntryPoint = "predict", CallingConvention = CallingConvention.Cdecl)]
        static extern void predict([MarshalAs(UnmanagedType.LPStr)] string filePath, ref PredictRes pr);

        static void Main(string[] args)
        {
            PredictRes prst = new PredictRes();


            string pythonfile = "D:/fc_example/python";
            string modelfile = "D:/fc_example/models";
            string weightfile = "D:/fc_example/weights";
            string predictfile = "D:/fc_example/test.jpg";

            Console.WriteLine("pythonfile is:" + pythonfile + "\n");
            Console.WriteLine("modelfile is:" + modelfile + "\n");
            Console.WriteLine("weightfile is:" + weightfile + "\n");
            Console.WriteLine("predictfile is:" + predictfile + "\n");

            int initx = init(pythonfile, modelfile, weightfile);
            Console.WriteLine("invoke init function return is:" + initx + "\n");
            predict(predictfile, ref prst);
            Console.WriteLine("invoke predict function return size is:" + prst.size + "\n");

            if (prst.size == 0)
            {
                return;
            }

            int[] ist = new int[prst.size];
            Marshal.Copy(prst.result, ist, 0, prst.size);

            for (int i = 0; i < prst.size; i++)
            {
                Console.WriteLine("return array item[" + i + "] is:" + ist[i] + ".");
            }
            Console.ReadLine();
        }
    }
}
