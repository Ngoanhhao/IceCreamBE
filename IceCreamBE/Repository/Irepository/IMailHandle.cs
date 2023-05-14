
namespace IceCreamBE.Repository.Irepository
{
    public interface IMailHandle
    {
        public bool send(string header, string message, string Receiver);
    }
}
