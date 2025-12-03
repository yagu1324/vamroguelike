using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace vamroguelike
{
    static class Program
    {
        /// <summary>
        /// 해당 애플리케이션의 주 진입점입니다.
        /// </summary>
        [STAThread]
        //[DllImport("kernel32.dll")] //콘솔창 띄우기 용
        //public static extern bool AllocConsole();//콘솔창 띄우기 용
        static void Main()
        {
            //AllocConsole();//콘솔창 띄우기 용
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new start());
        }
    }
}
