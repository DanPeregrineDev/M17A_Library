using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M17A_Library
{
    internal interface iItem
    {
        // Validate Data
        List<string> Validate();

        // Add Item
        void Add();             //TODO: database

        // Edit Item 
        void Edit();

        // Delete Item
        void Delete();
    }
}
