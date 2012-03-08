using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;
using System.ComponentModel.Composition;
using System.Globalization;
using LibCheck.Contracts;
using LibCheck.Core;
using LibCheck.Model;

namespace LibCheck.Plugins.Ursynoteka
{
    [Export(typeof(IBookChecker))]
    [ExportMetadata("Type", "Ursynoteka")]
    public class UrsynotekaChecker : IBookChecker
    {
        private CookieContainer cookies = new CookieContainer();

        public void Login(string userName, string password)
        {
            HttpHelper.HttpPost("http://www.warszawa-ursynow.sowwwa.pl/sowacgi.php?typ=acc",
                "KatID=0&login_attempt=1&id=info&swww_user=" + userName + "&swww_method=2&swww_pass=" + password, cookies);
        }

        public void Logout()
        {
            HttpHelper.HttpGet("http://www.warszawa-ursynow.sowwwa.pl/sowacgi.php?KatID=0&typ=acc&id=logout", cookies);
        }

        public IList<BorrowedItem> GetBorrowedItems()
        {
            var result = new List<BorrowedItem>();

            string html = HttpHelper.HttpGet("http://www.warszawa-ursynow.sowwwa.pl/sowacgi.php?KatID=0&typ=acc&id=wypozyczenia", cookies);

            //[Braci Wagów 1 (Wyp. 127)] Termin zwrotu: <span class="acct-bktOK">29.06.2011</span><br/>Egz. <span class="acct-egz">0200--031281-00</span> Sygnatura: <span class="acct-sign">821-2 FR.</span></div>
            //<div class="acct-opis">                                                                                                                                                                                                                                                                                          Kubuś, czyli uległość / Eugéne Ionesco ; przekł. Jan Błoński, Jan Kott, Adam Tarn ; przedmowa Ewa Andruszko.
            Regex regex = new Regex(@"<td class=\""td-(even|odd)""><div class=\""acct-header\"">\[([\w\s\.0-9]+ \(Wyp. \d{1,5}\))\] Termin zwrotu: <span class=\""acct-bktOK\"">(\d{2}.\d{2}.20\d{2})</span><br/>Egz. <span class=\""acct-egz\"">[\w-]+</span> Sygnatura: <span class=\""acct-sign\"">[\w\s\-\.\:\,]+</span></div>[.\n\r]*<div class=\""acct-opis\"">\r([^<>]+)\r</div>");

            Match m;
            for (m = regex.Match(html); m.Success; m = m.NextMatch())
            {
                BorrowedItem item = new BorrowedItem();

                string [] titleAuthor = m.Groups[4].ToString().Split(new string [] {" / "}, StringSplitOptions.RemoveEmptyEntries);

                item.Author = titleAuthor[1].Substring(0, titleAuthor[1].IndexOf("\r"));
                item.LibraryName = m.Groups[2].ToString();
                item.ReturnDate = DateTime.ParseExact(m.Groups[3].ToString(), "dd.MM.yyyy", CultureInfo.InvariantCulture);
                item.Title = titleAuthor[0];
                result.Add(item);
            }

            return result;
        }
    }
}
