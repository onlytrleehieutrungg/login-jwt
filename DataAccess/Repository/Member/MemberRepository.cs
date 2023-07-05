using DataAccess.Models;


namespace DataAccess.Repository.Member;

public class MemberRepository : IMemberRepository
{
    private readonly FStoreDBContext _context;

    public MemberRepository(FStoreDBContext context)
    {
        _context = context;
    }


    public bool Login(string email, string password)
    {
        var mem = _context.Members.FirstOrDefault(m => m.Email.Equals(email) && m.Password.Equals(password));
        if (mem != null)
        {
            return true;
        }
        return false;
    }
}