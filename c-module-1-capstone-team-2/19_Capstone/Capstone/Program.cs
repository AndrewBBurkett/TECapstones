using System;
using Capstone.Classes;
using System.Collections.Generic;
using Capstone;

namespace Capstone
{
    class Program
    {
        private static void Main()
        {
            VendingMachine vm = new VendingMachine();
            MainMenu thisOne = new MainMenu(vm);
        }
     }
}
