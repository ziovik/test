using DbFetcherConsoleApp.Converter;
using DbFetcherConsoleApp.DTO;
using DbFetcherConsoleApp.Model;
using System;
using System.Collections.Generic;
using System.Data.Common;

namespace DbFetcherConsoleApp
{
    class Program
    {
        static void ReadCars()
        {
            /*
            
            this using is not like top using...
            it means every single class which implement IDisposalbe could be closed using "using statemetn"
            otherwise you need to close connection or something else manualy like this..
            you see?
            yes
            open it manualy, and forget about closing if you put creation in using
            so go on

             */

            using (var connection = DbHelper.GetConnection()) // here you create object connection
            {
                connection.Open(); // open connection to start operate with database
                DbCommand testCmd = connection.CreateCommand(); // creation command - like createion query (CRUD operations)
                testCmd.CommandText = "select * from car"; // query test
                var cars = new List<Car>(); // i want to do it now 


                // we need to create reader to read from query result... execute reader return to us dataset
                using (var reader = testCmd.ExecuteReader())
                {
                    // loop by records in table (fucken car)
                    while (reader.Read())
                    {
                        var id = reader.GetInt64(0); // 1st column
                        var name = reader.GetString(1); // 2nd column
                        var year = reader.GetInt32(2); // 3rd column
                        var car = new Car(id, name, year);
                        cars.Add(car);
                    }
                }

            }
        }

        static void ReadPersons()
        {
            using (var connection = DbHelper.GetConnection())
            {
                connection.Open();
                var query = connection.CreateCommand();
                query.CommandText = "select * from Person";
                var persons = new List<Person>();

                using (var reader = query.ExecuteReader())
                {
                    var id = reader.GetInt64(0);
                    var name = reader.GetString(1);
                    var age = reader.GetInt32(2);

                    var person = new Person(id, name, age);
                    persons.Add(person);
                }
            }
        }

        static List<PersonInfo> GetAllPersonsInfo()
        {
            // create empty list which will contain persons
            var persons = new List<Person>();

            using (var connection = DbHelper.GetConnection())
            {
                connection.Open();
                var query = connection.CreateCommand();
                query.CommandText = "select " +
                    "p.id," +
                    "p.name," +
                    "p.age," +
                    "c.id," +
                    "c.name," +
                    "c.year " +
                    "from Person p left join car_n_person cp on cp.person_id = p.id  join car c on c.id = cp.car_id order by p.id";

                using (var reader = query.ExecuteReader())
                {
                    // we read row from DB one by one
                    while (reader.Read())
                    {
                        var personId = reader.GetInt64(0);
                        var personName = reader.GetString(1);
                        var personAge = reader.GetInt32(2);

                        var carId = reader.GetInt64(3);
                        var carName = reader.GetString(4);
                        var carYear = reader.GetInt32(5);

                        Car car = new Car(carId, carName, carYear);

                        // here we are trying to find person with new read personId in persons list
                        Person person = persons.Find(p => p.Id == personId);

                        //if there is no person in collection with personId, we need to create it and add to persons list
                        // first time (loop iteration) the list will be empty with no person at all
                        if (person == null)
                        {
                            person = new Person(personId, personName, personAge);
                            person.Cars.Add(car);
                            persons.Add(person);
                            continue;
                        }

                        //otherwise if person is exists in colleciton, we just need to add a car to his cars
                        person.Cars.Add(car);
                    }
                }
            }

            /* 
             * now we've got persons list with persons inside, so wee need to convert every single person into PersonInfo-class object
             * so we are creating converter and then magic starts.....
            */ 
            var pConverter = new PersonToPersonInfoConverter();

            /*
             * persons variable is List type of... class List has many methods... one of these is ConvertAll
             * // this p is a variable? yes, but you can name it as you likea variable like container to hold a person?
             * yeap!!!
             */
            return persons.ConvertAll(p => pConverter.Convert(p, new PersonInfo()));
        }

        static void DisplayInConsole(List<PersonInfo> persons)
        {
            persons.ForEach(p => Console.WriteLine(p.Info));
            Console.ReadLine();
        }

        static void DisplayOnWeb()
        {
            throw new NotSupportedException("method is not full yet"); // so i need a method to show on web?
        }

        static void Main(string[] args)
        {

            DisplayInConsole(GetAllPersonsInfo());
        }
    }


}
