using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySiteUpdater.Models;

namespace MySiteUpdater.Helpers
{
    class SortListHelper
    {
        public static List<EmployerModel> SortEmployerListDescending(List<EmployerModel> listToSort )
        {
            var list = listToSort.OrderByDescending(x => x.EndDate);
            return list.ToList();
        } 
    }
}
