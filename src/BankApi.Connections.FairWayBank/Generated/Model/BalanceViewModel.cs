/* 
 * FairWayBank
 *
 * FairWayBank Accounts + Transactions API
 *
 * OpenAPI spec version: v1
 * Contact: development@bizfitech.com
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace BankApi.Connections.FairWayBank.Generated.Model
{
    /// <summary>
    ///     BalanceViewModel
    /// </summary>
    [DataContract]
    public class BalanceViewModel : IEquatable<BalanceViewModel>, IValidatableObject
    {
        /// <summary>
        ///     Indicates whether the amount is in credit or debit.
        /// </summary>
        /// <value>Indicates whether the amount is in credit or debit.</value>
        public enum TypeEnum
        {
            /// <summary>
            ///     Enum NUMBER_0 for value: 0
            /// </summary>
            NUMBER_0 = 0,

            /// <summary>
            ///     Enum NUMBER_1 for value: 1
            /// </summary>
            NUMBER_1 = 1
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="BalanceViewModel" /> class.
        /// </summary>
        /// <param name="Amount">The amount of the transaction.</param>
        /// <param name="Type">Indicates whether the amount is in credit or debit..</param>
        /// <param name="Overdraft">Overdraft on the balance - when populated the amount total includes the overdraft.</param>
        /// <param name="DateTime">The date of the balance.</param>
        public BalanceViewModel(double? Amount = default(double?), TypeEnum? Type = default(TypeEnum?),
            OverdraftViewModel Overdraft = default(OverdraftViewModel), DateTime? DateTime = default(DateTime?))
        {
            this.Amount = Amount;
            this.Type = Type;
            this.Overdraft = Overdraft;
            this.DateTime = DateTime;
        }

        /// <summary>
        ///     Indicates whether the amount is in credit or debit.
        /// </summary>
        /// <value>Indicates whether the amount is in credit or debit.</value>
        [DataMember(Name = "type", EmitDefaultValue = false)]
        public TypeEnum? Type { get; set; }

        /// <summary>
        ///     The amount of the transaction
        /// </summary>
        /// <value>The amount of the transaction</value>
        [DataMember(Name = "amount", EmitDefaultValue = false)]
        public double? Amount { get; set; }


        /// <summary>
        ///     Overdraft on the balance - when populated the amount total includes the overdraft
        /// </summary>
        /// <value>Overdraft on the balance - when populated the amount total includes the overdraft</value>
        [DataMember(Name = "overdraft", EmitDefaultValue = false)]
        public OverdraftViewModel Overdraft { get; set; }

        /// <summary>
        ///     The date of the balance
        /// </summary>
        /// <value>The date of the balance</value>
        [DataMember(Name = "dateTime", EmitDefaultValue = false)]
        public DateTime? DateTime { get; set; }

        /// <summary>
        ///     Returns true if BalanceViewModel instances are equal
        /// </summary>
        /// <param name="input">Instance of BalanceViewModel to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(BalanceViewModel input)
        {
            if (input == null)
                return false;

            return
                (
                    Amount == input.Amount ||
                    Amount != null &&
                    Amount.Equals(input.Amount)
                ) &&
                (
                    Type == input.Type ||
                    Type != null &&
                    Type.Equals(input.Type)
                ) &&
                (
                    Overdraft == input.Overdraft ||
                    Overdraft != null &&
                    Overdraft.Equals(input.Overdraft)
                ) &&
                (
                    DateTime == input.DateTime ||
                    DateTime != null &&
                    DateTime.Equals(input.DateTime)
                );
        }

        /// <summary>
        ///     To validate all properties of the instance
        /// </summary>
        /// <param name="validationContext">Validation context</param>
        /// <returns>Validation Result</returns>
        IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            yield break;
        }

        /// <summary>
        ///     Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class BalanceViewModel {\n");
            sb.Append("  Amount: ").Append(Amount).Append("\n");
            sb.Append("  Type: ").Append(Type).Append("\n");
            sb.Append("  Overdraft: ").Append(Overdraft).Append("\n");
            sb.Append("  DateTime: ").Append(DateTime).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        ///     Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        /// <summary>
        ///     Returns true if objects are equal
        /// </summary>
        /// <param name="input">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object input)
        {
            return Equals(input as BalanceViewModel);
        }

        /// <summary>
        ///     Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                var hashCode = 41;
                if (Amount != null)
                    hashCode = hashCode * 59 + Amount.GetHashCode();
                if (Type != null)
                    hashCode = hashCode * 59 + Type.GetHashCode();
                if (Overdraft != null)
                    hashCode = hashCode * 59 + Overdraft.GetHashCode();
                if (DateTime != null)
                    hashCode = hashCode * 59 + DateTime.GetHashCode();
                return hashCode;
            }
        }
    }
}