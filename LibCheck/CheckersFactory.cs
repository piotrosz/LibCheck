using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using LibCheck.Contracts;

namespace LibCheck
{
    public class CheckersFactory
    {
        [ImportMany]
        private Lazy<IBookChecker, IDictionary<string, object> /* Metadata */>[] Checkers { get; set; }

        public CheckersFactory()
        {
            string pluginsDir = @".\plugins";

            Directory.CreateDirectory(pluginsDir);

            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new DirectoryCatalog(pluginsDir));
            var container = new CompositionContainer(catalog);
            container.ComposeParts(this);
        }

        public List<IBookChecker> GetCheckers(string checkerType)
        {
            return this.Checkers
                .Where(c => c.Metadata["Type"].ToString() == checkerType)
                .Select(c => c.Value).ToList();
        }
    }
}
