using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySiteUpdater.Models
{
    class PersonalLetterModel
    {
        public Guid ID { get; set; }
        public Guid UserID { get; set; }
        public string Title { get; set; }
        public string LetterText { get; set; }
    }
}
