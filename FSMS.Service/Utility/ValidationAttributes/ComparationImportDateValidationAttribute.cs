using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Service.Utility.ValidationAttributes
{
    public class ComparationImportDateValidationAttribute : ValidationAttribute
    {
        public ComparationImportDateValidationAttribute()
        {
        }

        public override bool IsValid(object? value)
        {
            DateTime dateTime;
            if (DateTime.TryParse(value.ToString(), out dateTime) && dateTime.CompareTo(DateTime.Today) <= 0)
            {
                return true;
            }
            return false;
        }
    }
}
