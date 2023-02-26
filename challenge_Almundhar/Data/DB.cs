
using Microsoft.AspNetCore.Http.HttpResults;
using System.Data.SQLite;


namespace challenge_Almundhar.Data

{
    public class DB
    {


        public  DB()
        {
            // creation of the database 
            string filePath = "database.db";
            if (File.Exists(filePath))
            {
            }
            else
            {
                /// ****** creation of initial DB
                // Set up the database connection
                SQLiteConnection.CreateFile("database.db");
                SQLiteConnection connection = new SQLiteConnection("Data Source=database.db;Version=3;");
                connection.Open();

                // Create the classes table
                string createClassesTableQuery = "CREATE TABLE classes (Id INTEGER PRIMARY KEY AUTOINCREMENT, classesName VARCHAR(50))";
                SQLiteCommand createClassesTableCommand = new SQLiteCommand(createClassesTableQuery, connection);
                createClassesTableCommand.ExecuteNonQuery();

                // Create the country table
                string createCountryTableQuery = "CREATE TABLE country (Id INTEGER PRIMARY KEY AUTOINCREMENT, countryName VARCHAR(50))";
                SQLiteCommand createCountryTableCommand = new SQLiteCommand(createCountryTableQuery, connection);
                createCountryTableCommand.ExecuteNonQuery();

                // Create the Student table
                string createStudentTableQuery = "CREATE TABLE Student (Id INTEGER PRIMARY KEY AUTOINCREMENT," +
                                                 "classesId INTEGER, " +
                                                 "countryId INTEGER, " +
                                                 "name VARCHAR(50)," +
                                                 "dateOfBairth VARCHAR(50)," +
                                                 "FOREIGN KEY(classesId) REFERENCES classes(Id), " +
                                                 "FOREIGN KEY(countryId) REFERENCES country(Id))";


                SQLiteCommand createStudentTableCommand = new SQLiteCommand(createStudentTableQuery, connection);
                createStudentTableCommand.ExecuteNonQuery();


                // insert Classes
                string classAQuery = "INSERT INTO classes (Id,classesName) VALUES ( 1, 'class A'); SELECT last_insert_rowid(); ";
                SQLiteCommand insertClassACommand = new SQLiteCommand(classAQuery, connection);
                insertClassACommand.ExecuteNonQuery();

                string classBQuery = "INSERT INTO classes (Id,classesName) VALUES ( 2, 'class B'); SELECT last_insert_rowid(); ";
                SQLiteCommand insertClassBCommand = new SQLiteCommand(classBQuery, connection);
                insertClassBCommand.ExecuteNonQuery();

                string classCQuery = "INSERT INTO classes (Id,classesName) VALUES ( 3, 'class C'); SELECT last_insert_rowid(); ";
                SQLiteCommand insertClassCCommand = new SQLiteCommand(classCQuery, connection);
                insertClassCCommand.ExecuteNonQuery();

                // insert countries
                string omanQuery = "INSERT INTO country (Id,countryName) VALUES ( 1, 'oman'); SELECT last_insert_rowid(); ";
                SQLiteCommand insertOmanCommand = new SQLiteCommand(omanQuery, connection);
                insertOmanCommand.ExecuteNonQuery();

                string uaeQuery = "INSERT INTO country (Id,countryName) VALUES ( 2, 'UAE'); SELECT last_insert_rowid(); ";
                SQLiteCommand insertUaeCommand = new SQLiteCommand(uaeQuery, connection);
                insertUaeCommand.ExecuteNonQuery();

                string ksaQuery = "INSERT INTO country (Id,countryName) VALUES ( 3, 'KSA'); SELECT last_insert_rowid(); ";
                SQLiteCommand insertKsaCommand = new SQLiteCommand(ksaQuery, connection);
                insertKsaCommand.ExecuteNonQuery();
                // Clean up and close the connection
                connection.Close();


                // insert Students
                insertStudent(1, 1, "Ahmed", new DateTime(1999, 01, 22));

                insertStudent(2, 1, "Ali", new DateTime(1999, 12, 4));



            }

        }

        /////////// insert functions are below 

        public  void insertStudent(int classesId, int countryId, string name, DateTime dateOfBirth)
        {
           
                using (var connection = new SQLiteConnection("Data Source=database.db;Version=3;"))
            {

                connection.Open();

                var query = "INSERT INTO Student ( classesId, countryId, name, dateOfBairth) VALUES (@classesId, @countryId, @name, @dateOfBirth)";

                using var command = new SQLiteCommand(query, connection);


                command.Parameters.AddWithValue("classesId", classesId);
                command.Parameters.AddWithValue("countryId", countryId);
                command.Parameters.AddWithValue("name", name);
                command.Parameters.AddWithValue("dateOfBirth", dateOfBirth);
                command.ExecuteNonQueryAsync();
                connection.Close();
            }
        }


        /////////// get functions are below >>>>>>>>>>>>>>>>>> 
        public  List<Student> getAllStudents()
        {
            SQLiteConnection connection = new SQLiteConnection("Data Source=database.db;Version=3; PRAGMA journal_mode=WAL;");
            connection.Open();

            string getStudentsQuery = "SELECT * FROM Student";
            SQLiteCommand getStudentsCommand = new SQLiteCommand(getStudentsQuery, connection);

            var result = new List<Student>();

            using var reader =  getStudentsCommand.ExecuteReader();
            while ( reader.Read())
            {
                var item = new Student
                {
                    Id = reader.GetInt32(0),
                    classId = reader.GetInt32(1),
                    cuontyeId = reader.GetInt32(2),
                    name = reader.GetString(3),
                    dateOfBirth = DateTime.Parse(reader.GetString(4)),
                };
                result.Add(item);
            }
            connection.Close();
            return result;
        }





        public async Task<List<Class>> getAllClass()
        {
            SQLiteConnection connection = new SQLiteConnection("Data Source=database.db;Version=3;PRAGMA journal_mode=WAL;");
            connection.Open();

            string getStudentsQuery = "SELECT * FROM classes";
            SQLiteCommand getStudentsCommand = new SQLiteCommand(getStudentsQuery, connection);

            var result = new List<Class>();

            using var reader = await getStudentsCommand.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var item = new Class
                {
                    Id = reader.GetInt32(0),
                    className = reader.GetString(1),
                };
                result.Add(item);
            }
            connection.Close();
            return result;
        }




        /////// update functions are below >>>>>>>>>>>>>>>



        public async void updateStudent(int id, int classesId, int countryId, string name, DateTime dateOfBirth)
        {
            using (var connection = new SQLiteConnection("Data Source=database.db;Version=3;PRAGMA journal_mode=WAL;"))
            {
                connection.Open();

                var query = "UPDATE Student SET classesId = @classesId, countryId = @countryId, name = @name, dateOfBairth = @dateOfBairth WHERE Id = @id";

                using var command = new SQLiteCommand(query, connection);

                command.Parameters.AddWithValue("id", id);
                command.Parameters.AddWithValue("classesId", classesId);
                command.Parameters.AddWithValue("countryId", countryId);
                command.Parameters.AddWithValue("name", name);
                command.Parameters.AddWithValue("dateOfBairth", dateOfBirth);
                
                await command.ExecuteNonQueryAsync();
                connection.Close();
            }
        }



        ///////delete functions are below


        public void DeleteStudent(int studentId)
        {
            using (var connection = new SQLiteConnection("Data Source=database.db"))
            {
                connection.Open();

                using (var command = new SQLiteCommand(connection))
                {
                    command.CommandText = "DELETE FROM Student WHERE Id = @studentId";
                    command.Parameters.AddWithValue("@studentId", studentId);

                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }
     

    }
    }
