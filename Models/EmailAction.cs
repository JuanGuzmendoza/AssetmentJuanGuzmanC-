namespace Hospital.Models
{
    /// <summary>
    /// Represents the possible actions related to email notifications within the hospital system.
    /// </summary>
    public enum EmailAction
    {
        /// <summary>
        /// Indicates an email for confirming an appointment or other action.
        /// </summary>
        Confirmation,

        /// <summary>
        /// Indicates an email for canceling an appointment or other action.
        /// </summary>
        Cancellation
    }
}
