using System;
using System.Collections.Generic;
using LibCheck.Model;

namespace LibCheck.Contracts
{
    public interface IBookChecker
    {
        void Login(string userName, string password);
        void Logout();
        IList<BorrowedItem> GetBorrowedItems();
    }
}
