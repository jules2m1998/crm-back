using CRM.core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.core.DataAccess;

public class DataAccess : IDataAccess
{
    private List<Person> people = new();

    public DataAccess()
    {
        people.Add(new Person { Id = 1, FirstName = "Test", LastName = "Test" });
        people.Add(new Person { Id = 2, FirstName = "Test 2", LastName = "Test 2" });
    }

    public List<Person> GetPeople()
    {
        return people;
    }

    public Person InsertPerson(string firstName, string lastname)
    {
        var user = new Person { Id = people.Count + 1, FirstName = firstName, LastName = lastname };
        people.Add(user);

        return user;
    }
}
