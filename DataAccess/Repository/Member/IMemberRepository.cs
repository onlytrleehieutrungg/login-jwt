

namespace DataAccess.Repository.Member;

public interface IMemberRepository
{
    bool Login(string email, string password);
}