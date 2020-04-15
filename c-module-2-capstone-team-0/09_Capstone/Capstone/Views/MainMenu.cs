using Capstone.DAL;
using Capstone.Models;
using System;
using System.Collections.Generic;

namespace Capstone.Views
{
    /// <summary>
    /// The top-level menu in our Market Application
    /// </summary>
    public class MainMenu : CLIMenu
    {
        // DAOs - Interfaces to our data objects can be stored here...
        //protected ICityDAO cityDAO;
        //protected ICountryDAO countryDAO;
        private IParkDAO parkDAO;
        private ICampgroundDAO campgroundDAO;
        private IReservationDAO reservationDAO;
        private ISiteDAO siteDAO;

        /// <summary>
        /// Constructor adds items to the top-level menu. YOu will likely have parameters for one or more DAO's here...
        /// </summary>
        public int parkWant;
        public MainMenu(IParkDAO parkDAO, ICampgroundDAO campgroundDAO, IReservationDAO reservationDAO, ISiteDAO siteDAO) : base("Main Menu")
        {
            this.parkDAO = parkDAO;
            this.campgroundDAO = campgroundDAO;
            this.reservationDAO = reservationDAO;
            this.siteDAO = siteDAO;
        }

        protected override void SetMenuOptions()
        {
            this.menuOptions.Add("1", "List Available Parks");
            this.menuOptions.Add("2", "List the Camps in a Park");
            this.menuOptions.Add("3", "Reservations Menu");
            this.menuOptions.Add("Q", "Quit program");
        }

        /// <summary>
        /// The override of ExecuteSelection handles whatever selection was made by the user.
        /// This is where any business logic is executed.
        /// </summary>
        /// <param name="choice">"Key" of the user's menu selection</param>
        /// <returns></returns>
        protected override bool ExecuteSelection(string choice)
        {
            switch (choice)
            {
                case "1": // Do whatever option 1 is
                    // list parks and stuff
                    GetAllParks();
                    Pause("Press enter to continue");
                    return true;    // Keep running the main menu
                case "2": // Do whatever option 2 is
                    //promt user for parkID
                    ListAllParks();
                    int parkWant = GetInteger("What is the Park ID of the Park you would like info on?\n\t(0 to return to the Main Menu):\t");
                    Console.WriteLine();
                    //list camp info for ParkID
                    if (parkWant == 0)
                    {
                        return true;
                    }
                    else
                    {
                        GetAllCampgrounds(parkWant);
                        Pause("");
                        return true;    // Keep running the main menu
                    }
                case "3": // Create and show the sub-menu
                    SubMenu1 sm = new SubMenu1(siteDAO, reservationDAO, parkDAO, campgroundDAO);
                    sm.Run();
                    return true;    // Keep running the main menu
            }
            return true;
        }

        protected override void BeforeDisplayMenu()
        {
            PrintHeader();
        }


        private void PrintHeader()
        {
            SetColor(ConsoleColor.Yellow);
            Console.WriteLine(Figgle.FiggleFonts.Standard.Render("National Park Program"));
            ResetColor();
        }
        private void ListAllParks()
        {
            IList<Park> parks = parkDAO.GetParks();

            if (parks.Count > 0)
            {
                foreach (Park park in parks)
                {
                    Console.WriteLine($"Park ID\tPark Name");
                    Console.WriteLine($"{park.ParkId}\t{park.Name} National Park");
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("*****No Results*****");
            }
        }
        private void GetAllParks()
        {
            IList<Park> parks = parkDAO.GetParks();
            
            if (parks.Count > 0)
            {
                foreach (Park park in parks)
                {
                    string eDate = park.EstablishDate.ToString("d");
                    Console.WriteLine($"ParkID:  {park.ParkId}\t{park.Name} National Park\n\tLocation:  {park.Location}\n\tEstablished on:  {eDate}\n\tArea:  {park.Area} sq km\n\tAnnual Visitors:  {park.Visitors}\nDescription:  {park.Description}");
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("*****No Results*****");
            }
        }
        private void GetAllCampgrounds(int parkWant)
        {
            Dictionary<int, string> month = new Dictionary<int, string>()
            {
                {1, "January" },
                {2, "February" },
                {3, "March" },
                {4, "April" },
                {5, "May" },
                {6, "June" },
                {7, "July" },
                {8, "August" },
                {9, "September" },
                {10, "October" },
                {11, "November" },
                {12, "December" }
            }; 
          
            IList<Campground> campgrounds = campgroundDAO.GetCampground(parkWant);

            if (campgrounds.Count > 0)
            {
                foreach (Campground campground in campgrounds)
                {
                    Console.WriteLine($"CampID:  {campground.CampgroundId}\tCamp Name:  {campground.Name}\n\tOpen from {month[campground.OpenFrom]} to {month[campground.OpenTo]}\tDaily Fee:  {campground.DailyFee:C}");
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("******No Results, Sorry for your luck.******");
            }
        }
    }
}
