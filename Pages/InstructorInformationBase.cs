using Fitness.Data;
using Fitness.Entities;
using Fitness.Pages;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.Xml;
using System.Xml.Linq;

namespace InstructorInformationBase
{
    public class InstructorInformationBase : ComponentBase
    {
        public InstructorInformation InstructorInformation { get; set; }
 
        private const int maxHours = 20;
        private static bool removed = true;
        private static bool saved = false;
        private static bool savedAttempted = true;
        private static bool idGenerator = false;


        public static bool Removed 
        { 
            get { return removed; }
            set { removed = value; } 
        }

        public static bool Saved
        {
            get { return saved; }
            set { saved = value; }
        }

        public static bool SavedAttempted
        {
            get { return savedAttempted; }
            set { savedAttempted = value; }
        }

        public static bool IdGenerator
        {
            get { return idGenerator; }
            set { idGenerator = value; }
        }

        Random random = new Random();

        protected override Task OnInitializedAsync()
        {
            InstructorInformation = new InstructorInformation();

            return base.OnInitializedAsync(); 
        }

        public void GenerateID()
        {
            if (!string.IsNullOrEmpty(InstructorInformation.FirstName) && !string.IsNullOrEmpty(InstructorInformation.LastName) && InstructorInformation.FirstName.Length >= 2 && InstructorInformation.LastName.Length >= 2)
            {
                InstructorInformation.InstructorId = random.Next(1, 999);

                // Aici folosesc baza de date pentru a verifica daca numarul generat exista deja deoarece avem nevoie de un ID unic!!!

                FitnessGym gym = new FitnessGym();

                var exists = gym.InstructorInformations.Any(x => x.InstructorId == InstructorInformation.InstructorId);

                while(exists == true)
                {
                    InstructorInformation.InstructorId = random.Next(1, 999);
                }
            }
            idGenerator = true;
        }

        public static bool CheckInstructorHours()
        {
            FitnessGym data = new FitnessGym();

            if (data.InstructorInformations.Any(x => x.Hours < maxHours))
            {
                return true; // Putem sa adaugam clientul respectiv la acel instructor
            }

            return false;
        }

        public void SaveInstructorInformation()
        {
            if(string.IsNullOrEmpty(InstructorInformation.FirstName) || string.IsNullOrEmpty(InstructorInformation.LastName) || InstructorInformation.InstructorId == null)
            {
                savedAttempted = false;
                return;
            }
            else if (InstructorInformation.FirstName.Length < 2 || InstructorInformation.LastName.Length < 2 || InstructorInformation.InstructorId == null)
            {
                savedAttempted = false;
                return;
            }
            else
            {
                savedAttempted = true;
            }

            using (FitnessDateAcces date = new FitnessDateAcces())
            {
                date.SaveIstructorInformation(InstructorInformation);
            }
            Saved = true;
        }

        public void RemoveInstructor()
        {
            ClientEntity clientData = new ClientEntity();
            FitnessGym instructorDate = new FitnessGym();

            int minHours = 20;
            int mostHours = 0;
            int newInstructorID = 0;
            int clientHours = 0;

            foreach (var instructor in instructorDate.InstructorInformations)
            {
                if (InstructorInformation.InstructorId == instructor.InstructorId)
                {
                    instructorDate.InstructorInformations.Remove(instructor);
                    Removed = true;
                }
                else
                {
                    Removed = false;
                }
            }

            instructorDate.SaveChanges();
            OnInitialized();
            StateHasChanged();

            foreach (var client in clientData.ClientInformations)  
            {
                int fewestHours = instructorDate.InstructorInformations.Select(x => x.Hours).Min();

                foreach (var instructor in instructorDate.InstructorInformations)
                {

                    if (fewestHours != instructor.Hours)
                    {
                        continue;     
                    }
                    else
                    {
                        minHours = instructor.Hours;
                        newInstructorID = (int)instructor.InstructorId;  // Orele si ID-ul instructorului cu cele mai putine ore 
                    }

                    if (client.InstructorID == InstructorInformation.InstructorId)
                    {
                        client.InstructorID = newInstructorID;
                        clientHours += client.Hours;

                        if (instructor.InstructorId == newInstructorID)
                        {
                            instructor.Hours += clientHours;
                        }
                        clientHours = 0;
                    }
                }
                instructorDate.SaveChanges();
            }

            clientData.SaveChanges();
        }

    }
}
