using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace OlympicSwimming
{
    class Program
    {
        static void Main(string[] args)
        {
            //Connection to  the database.
            string connectionString = "Server=127.0.0.1;Database=olympicswimming;User ID=root;Password=root;";
            //Creating an instance of the Competition class and passing the connection string to it.
            Competition competition = new Competition(connectionString);
            //Creating an instance of the Menu class and passing the Competition class to it.
            Menu menu = new Menu(competition);
            //Starting the program by calling the Start method in the Menu class.
            menu.Start();
        }
    }
}

