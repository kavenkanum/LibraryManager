using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryMVC.Domain.Models
{
    public class ExtensionMethod
    {
        public void ActivationStatus(bool activationStatus)
        {
            if (activationStatus == true)
            {
                Console.WriteLine("Aktywny");
            }
                Console.WriteLine("Nieaktywny");
        }
        
    }
}
