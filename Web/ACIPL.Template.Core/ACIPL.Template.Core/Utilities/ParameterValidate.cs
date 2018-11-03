using System;
using System.Linq;

namespace ACIPL.Template.Core.Utilities
{
    public static class ParameterValidate
    {
        /// <summary>
        /// Check Number
        /// </summary>
        /// <param name="id"></param>
        /// <param name="splitValue"></param>
        /// <returns></returns>
        public static bool IsInputNumber(string id, char splitValue)
        {
            var multipleVariable = id.Contains(splitValue);
            if (multipleVariable)
            {
                var inputString = id.Split(Convert.ToChar(splitValue));
                var id1 = inputString[0].ToString();
                var id2 = inputString[1].ToString();
                int n1;
                bool isNumeric1 = int.TryParse(id1, out n1);

                int n2;
                bool isNumeric2 = int.TryParse(id2, out n2);

                if (isNumeric1 == true && isNumeric2 == true)
                    return true;
                else
                    return false;
            }
            else
            {
                int n;
                bool isNumeric = int.TryParse(id, out n);
                return isNumeric;
            }
        }


        /// <summary>
        /// Check Date is in Valid Format or not
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static bool IsDate(string date)
        {
            DateTime dDate;
            if (DateTime.TryParse(date, out dDate))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
