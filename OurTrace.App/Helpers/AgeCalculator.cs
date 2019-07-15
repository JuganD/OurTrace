using System;
using System.Collections.Generic;
using System.Text;

namespace OurTrace.Services.Helpers
{
    public static class AgeCalculator
    {
        public static int GetYears(DateTime Bday)
        {
            return GetYears(Bday, DateTime.Today);
        }
        public static int GetYears(DateTime Bday, DateTime Cday)
        {
            if ((Cday.Year - Bday.Year) > 0 ||
                (((Cday.Year - Bday.Year) == 0) && ((Bday.Month < Cday.Month) ||
                  ((Bday.Month == Cday.Month) && (Bday.Day <= Cday.Day)))))
            {

                if (Cday.Month > Bday.Month)
                {
                    return Cday.Year - Bday.Year;
                }
                else if (Cday.Month == Bday.Month)
                {
                    if (Cday.Day >= Bday.Day)
                    {
                        return Cday.Year - Bday.Year;
                    }
                    else
                    {
                        return (Cday.Year - 1) - Bday.Year;
                    }
                }
                else
                {
                    return (Cday.Year - 1) - Bday.Year;
                }
            }
            return -1;
        }
    }
}
