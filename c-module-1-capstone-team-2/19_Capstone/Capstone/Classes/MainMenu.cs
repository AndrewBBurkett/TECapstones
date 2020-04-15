using System;
using System.Collections.Generic;
using System.Text;
using System.IO;


namespace Capstone.Classes
{
    public class MainMenu
    {
        private VendingMachine vm;

        public MainMenu(VendingMachine theVm)
        {
            this.vm = theVm;
            int variable = 1;
            while (variable == 1)
            {
                Console.WriteLine();
                Console.WriteLine("WELCOME TO THE VENDO -MATIC 800");
                Console.WriteLine("*******************************");
                Console.WriteLine("   Vending Machine Main Menu   ");
                Console.WriteLine("*******************************");
                Console.WriteLine("1)Display Vending Machine Items");
                Console.WriteLine("2)Purchase");
                Console.WriteLine("3)Exit");
                Console.Write("Enter your choice (1, 2, 3) here: ");
                string choice = Console.ReadLine();
                Console.WriteLine();

                if (!(choice == "4" || choice == "3" || choice == "2" || choice == "1"))
                {
                    Console.WriteLine("****Please choose again.****");
                }
                else
                {
                    if (choice == "1")
                    {
                        Console.WriteLine("Product Code\tProduct Name\tPrice");
                        foreach (KeyValuePair<string, Slot> item in vm.inventory)

                        {
                            Console.WriteLine($"\t{item.Key} { item.Value.ItemName,20}\t {item.Value.Price}");
                        }
                    }

                    else if (choice == "2")
                    {
                        int menu2 = 1;
                        while (menu2 == 1)
                        {
                            Console.WriteLine();
                            Console.WriteLine("\t**********************************");
                            Console.WriteLine($"\t Purchase Menu\t Balance: {vm.Balance:C}");
                            Console.WriteLine("\t**********************************");
                            Console.WriteLine("\t1) Feed Money");
                            Console.WriteLine("\t2) Select Product");
                            Console.WriteLine("\t3) Finish Transaction");
                            Console.Write("\tEnter your choice (1, 2, 3) here: ");

                            string selection = Console.ReadLine();
                            Console.WriteLine();
                            {
                                if (selection == "1")
                                {
                                    Console.Write("\tHow much are you depositing?\t");

                                    try
                                    {
                                        decimal moneyIn = decimal.Parse(Console.ReadLine());

                                        if (!(moneyIn % 1 == 0))
                                        {
                                            Console.WriteLine("\t****Not valid. Please enter whole dollar amounts****");
                                        }
                                        else
                                        {
                                            vm.FeedMoney(moneyIn);
                                        }
                                        Console.WriteLine($"\tYour new balance is {vm.Balance:C}");
                                    }
                                    catch (Exception)
                                    {
                                        Console.WriteLine("\t****Not valid. Please enter whole numeric dollar amounts****");
                                    }

                                }
                                else if (selection == "2")
                                {
                                    // select product (item.key)
                                    Console.Write("\tWhat is your product?\t ex (A1)\t");
                                    string keyNumber = Console.ReadLine();
                                    keyNumber = keyNumber.ToUpper();
                                    if (!(vm.inventory.ContainsKey(keyNumber)))
                                    {
                                        Console.WriteLine("\t****Invalid entry. Please enter your product. ex. A1****");
                                    }

                                    else
                                    {
                                        if (vm.inventory[keyNumber].Quantity > 0)
                                        {
                                            if (vm.Balance < vm.inventory[keyNumber].Price)
                                            {
                                                Console.WriteLine($"\t****Your Balance is {vm.Balance:C}****{keyNumber} costs {vm.inventory[keyNumber].Price}**** .");
                                                Console.WriteLine("\t****Please add more money****");
                                            }

                                            else
                                            {
                                                vm.WriteToLog($"{vm.inventory[keyNumber].ItemName} {vm.Balance} { vm.Balance - vm.inventory[keyNumber].Price}");
                                                //decriment balance by item.value.price
                                                vm.Balance -= vm.inventory[keyNumber].Price;

                                                //decriment item.value.quanity by 1
                                                vm.inventory[keyNumber].Quantity -= 1;

                                                //some fancy CW 
                                                Console.WriteLine($"\t Enjoy your {vm.inventory[keyNumber].ItemName}");


                                                //chomp chop stuff
                                                if (vm.inventory[keyNumber].ItemCategory == "Chip")
                                                {
                                                    Console.WriteLine("\t\t Crunch Crunch, Yum!");
                                                }
                                                if (vm.inventory[keyNumber].ItemCategory == "Candy")
                                                {
                                                    Console.WriteLine("\t\t Munch Munch, Yum!");
                                                }
                                                if (vm.inventory[keyNumber].ItemCategory == "Drink")
                                                {
                                                    Console.WriteLine("\t\t Glug Glug, Yum!");
                                                }
                                                if (vm.inventory[keyNumber].ItemCategory == "Gum")
                                                {
                                                    Console.WriteLine("\t\t Chew Chew, Yum!");
                                                }
                                                Console.WriteLine($"\t Your new balance is {vm.Balance:C}");
                                            }
                                        }
                                        else
                                        {
                                            Console.WriteLine($"\t ****{vm.inventory[keyNumber].ItemName} is Sold Out!****");
                                            Console.WriteLine("\t ****Please make another selection.****");
                                        }
                                    }
                                }
                                else if (selection == "3")
                                {
                                    Console.WriteLine($"\t Your change is {vm.Balance:C}");
                                    vm.CoinChange();
                                    
                                    
                                    vm.EndTransaction();
                                    vm.Balance = 0;
                                    menu2 = 2;
                                }
                                else
                                {
                                    Console.WriteLine($"\t ****Your selection is invalid, Please select again****");
                                }
                            }
                        }
                    }
                    else if (choice == "3")
                    {
                        Console.WriteLine("\t Thank you for using the best vending machine on the planet!");
                        variable = 2;
                    }
                    else if (choice == "4")
                    {
                        decimal sales = 0;
                        foreach (KeyValuePair<string, Slot> item in vm.inventory)
                        {
                            int quant = 5 - (vm.inventory[item.Key].Quantity);
                            Console.WriteLine($"{vm.inventory[item.Key].ItemName}, {quant}");
                            sales = sales + (quant * (vm.inventory[item.Key].Price));
                        }
                        Console.WriteLine("**********************************************************");
                        Console.WriteLine($"\t Total Sales are: {sales:C}");
                    }
                }
            }


        }
    }
}