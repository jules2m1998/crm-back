using CRM.core.Models;

namespace CRM.core.DataAccess
{
    public interface IDataAccess
    {
        List<Person> GetPeople();
        Person InsertPerson(string firstName, string lastname);
    }
}