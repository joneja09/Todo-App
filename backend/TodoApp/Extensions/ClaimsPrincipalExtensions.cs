using System;
using System.Security.Claims;

namespace TodoApp.Extensions;

/// <summary>
/// Extension methods for <see cref="ClaimsPrincipal"/> to simplify user ID extraction.
/// </summary>
public static class ClaimsPrincipalExtensions
{
    /// <summary>
    /// Gets the user ID from the <see cref="ClaimsPrincipal"/>.
    /// </summary>
    /// <param name="user">The claims principal representing the user.</param>
    /// <returns>The user ID as an integer.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the user ID claim is missing or invalid.</exception>
    public static int GetUserId(this ClaimsPrincipal user)
    {
        var idValue = user.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(idValue) || !int.TryParse(idValue, out var userId))
            throw new InvalidOperationException("User ID claim is missing or invalid.");
        return userId;
    }
}
