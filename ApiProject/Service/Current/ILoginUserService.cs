namespace ApiProject.Service.Current
{
    public interface ILoginUserService
    {
        int UserId { get; }
        int SchoolId { get; }
        int SessionId { get; }
        int ParentId { get; }
        int StudentId { get; }
      //  int StartSessionYear { get; }
        string UserName { get; }
    }
}
