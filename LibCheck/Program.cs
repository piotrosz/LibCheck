using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LibCheck.Contracts;
using LibCheck.Model;

namespace LibCheck
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args == null || args.Length != 3)
            {
                Console.WriteLine("LibCheck.exe user password library");
                return;
            }

            string userName = args[0];
            string password = args[1];
            string type = args[2];

            var checkersFactory = new CheckersFactory();
            var checkers = checkersFactory.GetCheckers(type);

            foreach (var checker in checkers)
            {
                checker.Login(userName, password);
                ShowBorrowedItems(checker.GetBorrowedItems());
                checker.Logout();
            }
        }

        static void ShowBorrowedItems(IList<BorrowedItem> items)
        {
            foreach (var item in items)
            {
                Console.WriteLine("Title: {0}", item.Title);
                Console.WriteLine("Author: {0}", item.Author);

                ConsoleColor color = ConsoleColor.DarkGreen;
                int days = (DateTime.Today - item.ReturnDate.Date).Days;
                if (days < 0)
                    color = ConsoleColor.DarkRed;
                else if (days < 7)
                    color = ConsoleColor.DarkYellow;

                Console.ForegroundColor = color;
                Console.WriteLine("Return date: {0:D}", item.ReturnDate);
                Console.ResetColor();
                Console.WriteLine("Library: {0}", item.LibraryName);
            }
        }
    }
}