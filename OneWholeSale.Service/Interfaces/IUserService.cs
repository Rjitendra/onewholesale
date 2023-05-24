namespace OneWholeSale.Service.Interfaces
{
    using OneWholeSale.Model.Dto;
    using OneWholeSale.Service.Utility;
    public interface IUserService
    {
        public int GetUserId();

        int UpdateUser(ApplicationUserDto dto);

        int CreateUser(ApplicationUserDto dto);

        int CreateMultipleUser(IList<ApplicationUserDto> dto);

        Task<Result<IEnumerable<ApplicationUserRoleDto>>> Roles();

        bool DeleteApplicationUsers(List<int> ids);
        bool DeleteApplicationUser(int id);
    }
}
