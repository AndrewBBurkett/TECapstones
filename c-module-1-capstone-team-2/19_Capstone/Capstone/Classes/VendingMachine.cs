using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Capstone.Classes
{
    public class VendingMachine
    {
        public decimal Balance { get; set; }
        

        public Dictionary<string, Slot> inventory = new Dictionary<string, Slot>();

        public void FeedMoney(decimal feedMoney)
        {
             Balance += feedMoney;
            WriteToLog($"FEEDMONEY: { feedMoney} { Balance}");
        }

        string file = @"C:\Users\Student\git\c-module-1-capstone-team-2\19_Capstone\Capstone\vendingmachine.csv";

        private void Load(string filePath)
        {
            using (StreamReader sr = new StreamReader(filePath))
            {
                while (!sr.EndOfStream)
                {
                    string input = sr.ReadLine();
                    string[] fields = input.Split("|");
                    string idNumber = fields[0];
                    string itemName = fields[1];
                    decimal price = decimal.Parse(fields[2]);
                    string itemCategory = fields[3];

                    Slot slot = new Slot(idNumber, itemName, price, itemCategory, 5);
                    inventory.Add(idNumber, slot);
                }
            }
        }
        public void EndTransaction()
        {
            WriteToLog($"GIVECHANGE {Balance}, $0.00");
            Balance = 0;
        }
       

        public VendingMachine()
        {
            Load(file);
            Balance = 0;
        }

        public void WriteToLog(string input)
        {
            string timestamp = DateTime.Now.ToString();
            string output = ($"{timestamp}{input}");
            using (StreamWriter sw = new StreamWriter("Log.txt", true))
            {
                sw.WriteLine(output);
            }
        }
        decimal amountInCents=0;
        int quarters;
        int dimes;
        int nickels;
        public void CoinChange()
        {
            
            amountInCents = 0;
            amountInCents = Balance * 100;
            while (amountInCents > 0)

            {
                if (amountInCents >= 25)
                {
                    quarters++;
                    amountInCents =amountInCents - 25;
                }
                else if (amountInCents >= 10)
                {
                    dimes++;
                    amountInCents -=amountInCents;
                }
                else if (amountInCents >= 5)
                {
                    nickels++;
                    amountInCents = amountInCents - 5;
                }


               
            }
            
            Console.WriteLine($"\t You'll receive {quarters} quarters, {dimes} dimes, and {nickels} nickels");
            nickels = 0;
            dimes = 0;
            quarters = 0;
        }
        
    }
}


