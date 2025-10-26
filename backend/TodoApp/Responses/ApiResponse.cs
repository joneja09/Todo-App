namespace TodoApp.Responses;

/// <summary>
/// Represents a standardized response for API operations, encapsulating the success status,  optional data, and an
/// optional message.
/// </summary>
/// <remarks>This class provides a consistent structure for API responses, making it easier to handle  success and
/// error cases uniformly. Use the <see cref="Ok(T)"/> method to create a successful  response with data, or the <see
/// cref="Error(string)"/> method to create an error response with a message.</remarks>
/// <typeparam name="T">The type of the data payload included in the response. Use a nullable type if the data may be absent.</typeparam>
public class ApiResponse<T>
{
    public bool Success { get; set; }
    public T? Data { get; set; }
    public string? Message { get; set; }

    public static ApiResponse<T> Ok(T data) => new() { Success = true, Data = data };
    public static ApiResponse<T> Error(string message) => new() { Success = false, Message = message };
}
