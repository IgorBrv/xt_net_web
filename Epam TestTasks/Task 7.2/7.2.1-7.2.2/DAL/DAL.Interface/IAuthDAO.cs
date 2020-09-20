
namespace InterfaceDAL
{
	public interface IAuthDAO
	{
		bool CreateUser(string name, string password);
		bool CheckUser(string name, string password);
	}
}
