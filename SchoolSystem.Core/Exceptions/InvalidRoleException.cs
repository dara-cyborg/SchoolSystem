namespace SchoolSystem.Core.Exceptions;

/// <summary>
/// Thrown when one or more requested role IDs do not exist in the database.
/// </summary>
public class InvalidRoleException : Exception {
    public InvalidRoleException(string message) : base(message) { }
}
