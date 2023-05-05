using System.Xml.Serialization;

namespace Fitness.Entities
{
    public class DataAcces
    {
        FitnessGym db = new FitnessGym();
        ClientEntity data = new ClientEntity();
        
        public List<InstructorInformation> GetInstructors()
        {
            return db.InstructorInformations.ToList();
        }

        public List<ClientInformation> GetClients()
        {
            return data.ClientInformations.ToList();
        }

        public void SaveDataClient()
        {
            data.SaveChanges();
        }

    }
}
