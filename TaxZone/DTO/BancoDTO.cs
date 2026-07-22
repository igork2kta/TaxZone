
namespace TaxZone
{
    public class BancoDTO
    {
        public string database;
        public string owner;

        public BancoDTO(string database, string owner)
        {
            this.database = database;
            this.owner = owner;
        }
    }
}
