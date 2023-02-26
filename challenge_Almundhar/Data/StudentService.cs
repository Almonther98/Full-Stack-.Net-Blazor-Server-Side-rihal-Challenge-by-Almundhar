
namespace challenge_Almundhar.Data;

    public class StudentService 
    {
        
        private static List<studrntEntry> entries = new List<studrntEntry>();

       

        public async Task<List<studrntEntry>> getAllEnteris()
        {
        entries.Clear();
        var DB = new DB();
        List<Student> DBData =  DB.getAllStudents();
        foreach (var entry in DBData) {
            string className="";
            if (entry.cuontyeId == 1)
            {
                className = "class A";
            }
            else if (entry.cuontyeId == 2)
            {
                className = "class B";
            }
            else
            {
                 className = "class C";
            }

            string countryName = "";
            if (entry.cuontyeId == 1)
            {
                countryName = "Oman";
            }
            else if (entry.cuontyeId == 2)
            {
                countryName = "UAE";
            }
            else
            {
                countryName = "KSA";
            }
            
			

			entries.Add(new studrntEntry { Id= entry.Id, classNsme = className , cuontyeName= countryName ,name= entry.name, dateOfBirth= entry.dateOfBirth });
        }
         
            return entries;
        }



        

   
}

