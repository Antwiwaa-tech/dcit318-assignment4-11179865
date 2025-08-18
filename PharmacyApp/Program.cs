using System;
using System.Windows.Forms;

namespace PharmacyApp
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Start with the Main Form
            Application.Run(new MainForm());
        }
    }
}
