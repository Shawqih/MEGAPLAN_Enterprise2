
using System;

namespace MEGAPLAN.Infrastructure
{
    public class ErrorHandler
    {
        public static void Handle(Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.Message);
        }
    }
}
