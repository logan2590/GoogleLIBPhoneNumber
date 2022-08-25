using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoogleLIBPhoneNumber.Models
{
    public class ValidatePhoneNumberModel
    { 
        public string OriginalNumber { get; set; }
        public string FormattedNumber { get; set; }
        public bool IsMobile { get; set; }
        public bool IsValidNumber { get; set; }
        public bool  IsValidNumberForRegion { get; set; }
        public string Region { get; set; }
    }
}
