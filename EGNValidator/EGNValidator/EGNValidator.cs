using System;
using System.Linq;
using System.Text;

namespace EGNValidator
{
    public static class EGNValidator
    {
        // See http://www.grao.bg/esgraon.html

        /// <summary>
        /// Parse ENG number and checks if it is valid according http://www.grao.bg/esgraon.html
        /// </summary>
        /// <param name="egn">String containing egn number</param>
        /// <returns>
        /// Output code:
        ///     -2  Checksum is invalid. EGN is invalid.
        ///     -1  Entered EGN does not contains 10 digits.
        ///      0  ENG is valid.
        /// 1 .. 9  i-th character is not an integer.
        /// </returns>
        public static int Validate(string egn)
        {
            int validationCode = 0;
            char[] chars = egn.ToCharArray();
            if (chars.Length != 10)
            {
                validationCode = -1;
            }
            else
            {
                int[] d = new int[10];
                for (int i = 0; i < 10; i++)
                {
                    bool check = int.TryParse(chars[i].ToString(), out d[i]);
                    if (!check)
                    {
                        validationCode = i + 1;
                        d[i] = 0;
                    }
                }
                int checksum = (2*d[0] + 4*d[1] + 8*d[2] + 5*d[3] + 10*d[4] + 9*d[5] + 7*d[6] + 3*d[7] + 6*d[8]) % 11;
                checksum = checksum == 10 ? 0 : checksum;
                if (checksum == d[9])
                {
                    validationCode = 0;
                }
                else
                {
                    validationCode = -2;
                }
            }
            return validationCode;
        }
    }
}
