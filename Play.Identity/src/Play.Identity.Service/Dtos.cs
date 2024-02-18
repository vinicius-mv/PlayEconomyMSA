using System;
using System.ComponentModel.DataAnnotations;

namespace Play.Identity.Service;

public class Dtos
{
    public record UserDto(
        Guid Id,
        string Username,
        string Email,
        decimal Gil,
        DateTimeOffset CreatedDate
    );

    public record UpdateUserDto(
        [Required][EmailAddress] String Email,
        [Range(0, 1_000_000)] decimal Gil
    );
}