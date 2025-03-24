using Application.Users;
using Domain.Users;

namespace Application.Mapper;

public static class MapUser
{
    public static UserResponse MapUserResponse(this User user) => new(user.Id, user.FirstName, user.LastName);
}
