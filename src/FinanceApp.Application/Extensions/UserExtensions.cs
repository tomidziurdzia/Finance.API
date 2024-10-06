namespace FinanceApp.Application.Extensions;

public static class UserExtensions
{
    public static IEnumerable<UserDto> ToUserDtoList(this IEnumerable<User> users)
    {
        return users.Select(user => new UserDto(
            Id: new Guid(user.Id),
            Name: user.Name!,
            Lastname: user.Lastname!,
            Email: user.Email!
        ));
    }

    public static UserDto ToUserDto(this User user)
    {
        return DtoFromUser(user);
    }

    private static UserDto DtoFromUser(User user)
    {
        return new UserDto(
            Id: new Guid(user.Id),
            Name: user.Name!,
            Lastname: user.Lastname!,
            Email: user.Email!
        );
    }
}