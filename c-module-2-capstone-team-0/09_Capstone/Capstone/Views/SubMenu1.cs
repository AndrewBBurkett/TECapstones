using Capstone.DAL;
using Capstone.Models;
using System;
using System.Collections.Generic;

namespace Capstone.Views
{
    /// <summary>
    /// The top-level menu in our Market Application
    /// </summary>
    public class SubMenu1 : CLIMenu
    {
        // Store any private variables, including DAOs here....
        private ISiteDAO siteDAO;
        private IReservationDAO reservationDAO;
        private IParkDAO parkDAO;
        private ICampgroundDAO campgroundDAO;

        /// <summary>
        /// Constructor adds items to the top-level menu
        /// </summary>
        public SubMenu1(ISiteDAO siteDAO, IReservationDAO reservationDAO, IParkDAO parkDAO, ICampgroundDAO campgroundDAO/** DAOs may be passed in... ***/) :
            base("Sub-Menu 1")
        {
            this.siteDAO = siteDAO;
            this.reservationDAO = reservationDAO;
            this.parkDAO = parkDAO;
            this.campgroundDAO = campgroundDAO;
            // Store any values or DAOs passed in....
        }
       
        protected override void SetMenuOptions()
        {
            this.menuOptions.Add("1", "Check for Camp Availablity and Make a reservation");
            //this.menuOptions.Add("2", "Make a Reservation");
            this.menuOptions.Add("B", "Back to Main Menu");
            this.quitKey = "B";
        }

        /// <summary>
        /// The override of ExecuteSelection handles whatever selection was made by the user.
        /// This is where any business logic is executed.
        /// </summary>
        /// <param name="choice">"Key" of the user's menu selection</param>
        /// <returns></returns>
        public int reqCampId;
        public DateTime reqArrival;
        public DateTime reqDeparture;
        protected override bool ExecuteSelection(string choice)
        {
            switch (choice)
            {
                case "1": // Do whatever option 1 is
                    //prompt for campid
                    ListAllParks();
                    int parkWant = GetInteger("Which Park are you interested in?\n\t(0 to return to the Menu):\t");
                    if (parkWant == 0)
                    {
                        return true;
                    }
                    else
                    {
                        ListAllCampgrounds(parkWant);
                    }
                    reqCampId = GetInteger("What is the Campground Id you would like to reserve?\n\t(0 to return to the Menu):\t");
                    if (reqCampId == 0)
                    {
                        return true;
                    }
                    else
                    {
                        //prompt for startDate
                        try
                        {
                            Console.WriteLine();
                            Console.WriteLine("What is the date of your Arrival?");
                            Console.Write("Enter your Arrival Date here (MM/DD/YYYY) ----->\t");
                            string strA = Console.ReadLine();
                            reqArrival = Convert.ToDateTime(strA);
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Please enter a valid date.");
                            Pause("");
                            return true;
                        }
                        //prompt for endDate
                        try
                        {
                            Console.WriteLine();
                            Console.WriteLine("What is the date of your Departure?");
                            Console.Write("Enter the Departure Date here (MM/DD/YYYY) ------>\t");
                            string strD = Console.ReadLine();
                            reqDeparture = Convert.ToDateTime(strD);
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Please enter a valid date.");
                            Pause("");
                            return true;
                        }
                        //return whether or not avalible
                        if (reqArrival >= reqDeparture)
                        {
                            Console.WriteLine();
                            Console.WriteLine("Please enter a valid Avival/Departure Date Combo");
                            //Pause("");
                        }
                        else
                        {
                            Console.WriteLine();
                            Console.WriteLine("Here are the Available Sites");
                            GetAllSites(reqCampId, reqArrival, reqDeparture);
                            string resAnswer = GetString("Would you like to make a Reservation? y/n\t");
                            if (resAnswer.ToLower() == "y")
                            {
                                MakeReservation(reqArrival, reqDeparture);
                            }
                            else
                            {
                                return false;
                            }
                        }
                            Pause("");
                            return true;
                        
                    }
                    
                //case "2": // Do whatever option 2 is
                //    //MakeReservation();
                    
                //    Pause("");
                //    return false;
            }
            return true;
        }
        private void MakeReservation(DateTime resArrival, DateTime resDeparture)
        {
            //prompt for name
            string resName = GetString("What is your Last Name?\t");
            //promt for siteID
            int resId = GetInteger("What is the Site ID you would like to reserve?\t");
            //prompt for startDate
            //Console.WriteLine("What is the date of your Arrival?");
            //Console.Write("Enter your Arrival Date (MM/DD/YYYY) here ----->\t");
            //string resStrA = Console.ReadLine();
            //DateTime resArrival = Convert.ToDateTime(resStrA);
            ////prompt for endDate
            //Console.WriteLine("What is the date of your Departure?");
            //Console.Write("Enter the Departure Date here (MM/DD/YYYY) ------>\t");
            //string resStrD = Console.ReadLine();
            //DateTime resDeparture = Convert.ToDateTime(resStrD);
            Reservation res = new Reservation()
            {
                SiteId = resId,
                Name = resName,
                FromDate = resArrival,
                ToDate = resDeparture
            };
            int rId = reservationDAO.MakeReservation(res);

            Console.WriteLine($"Your Confomation Number is: {rId}. \n\t Enjoy your stay!");
        }
        private void GetAllSites(int reqCampId, DateTime reqArrival, DateTime reqDeparture)
        {
            IList<Site> sites = siteDAO.TopFiveSites(reqCampId, reqArrival, reqDeparture);

            if (sites.Count > 0)
            {
                foreach (Site site in sites)
                {
                    int totalDays = (reqDeparture - reqArrival).Days;
                    decimal totalPrice = totalDays * site.DailyFee;
                    Console.WriteLine($"Site # {site.SiteId} in Camp # {site.CampgroundId} with a max occupancy of {site.MaxOccupancy} is Availible.\n\t{totalDays} day(s) at this site would cost {totalPrice:C}.\n\tAccessible: {site.Accessible}  RV's up to: {site.MaxRvLength} Length.  Utility Hook Up's: {site.Utilities}");  
                }
            }
            else
            {
                Console.WriteLine("Not availible");
            }

        }
        private void ListAllCampgrounds(int parkWant)
        {

            IList<Campground> campgrounds = campgroundDAO.GetCampground(parkWant);

            if (campgrounds.Count > 0)
            {
                foreach (Campground campground in campgrounds)
                {
                    Console.WriteLine($"\tCamp ID\tCamp Name");
                    Console.WriteLine($"\t{campground.CampgroundId}\t{campground.Name}");
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("******No Results, Sorry for your luck.******");
            }
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
        protected override void BeforeDisplayMenu()
        {
            PrintHeader();
        }

        protected override void AfterDisplayMenu()
        {
            base.AfterDisplayMenu();
            SetColor(ConsoleColor.Cyan);
            Console.WriteLine("Check to see if there is avaliblity for your reservation.");
            ResetColor();
        }

        private void PrintHeader()
        {
            SetColor(ConsoleColor.Magenta);
            Console.WriteLine(Figgle.FiggleFonts.Standard.Render("Camp Site Reservations"));
            ResetColor();
        }

    }
}
