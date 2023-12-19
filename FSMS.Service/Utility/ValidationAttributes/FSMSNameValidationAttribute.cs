using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMS.Service.Utility.ValidationAttributes
{
    public class FSMSNameValidationAttribute : ValidationAttribute
    {
        public FSMSNameValidationAttribute() { }

        public override bool IsValid(object? value)
        {
            string[] NameParts = value.ToString().Split(" ");
            bool ErrorFlag = false;
            for (int i = 0; i < NameParts.Length; i++)
            {
                char firstCharacter = NameParts[i].First();
                char firstCharacterUpper = NameParts[i].ToUpper().First();
                if (firstCharacter.Equals(firstCharacterUpper) == false)
                {
                    ErrorFlag = true;
                }
            }

            if (ErrorFlag)
            {
                return false;
            }
            return true;
        }
    }
}
