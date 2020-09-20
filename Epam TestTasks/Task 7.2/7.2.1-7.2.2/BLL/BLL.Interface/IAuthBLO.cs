
namespace InterfacesBLL
{
	public interface IAuthBLO
	{
		bool CreateUser(string name, string password);
		bool CheckUser(string name, string password);
	}
}
