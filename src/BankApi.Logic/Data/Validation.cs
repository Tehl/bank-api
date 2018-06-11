using System.Text.RegularExpressions;

namespace BankApi.Logic.Data
{
    /// <summary>
    ///     Contains validation utility methods for incoming data
    /// </summary>
    public static class Validation
    {
        private static readonly Regex AccountNumberPattern = new Regex(@"^[\d]{8}$", RegexOptions.Compiled);

        /// <summary>
        ///     Validates a bank account number
        /// </summary>
        /// <remarks>
        ///     * Must not be null
        ///     * Must be 8 digits long
        ///     * Must only contain numbers
        ///     * Must not start with 0
        /// </remarks>
        /// <param name="accountNumber">Account number to be validated</param>
        /// <returns>True if the account number is valid; otherwise, false</returns>
        public static bool AccountNumberIsValid(string accountNumber)
        {
            return accountNumber != null
                   && AccountNumberPattern.IsMatch(accountNumber)
                   && accountNumber[0] != '0';
        }
    }
}