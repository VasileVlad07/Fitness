using Fitness.Entities;
using System;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;
using ClientInformationBase;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Syncfusion.Blazor.Schedule.Internal;
using Microsoft.Extensions.Logging;

namespace Fitness.Data
{
    public class FitnessDateAcces : IDisposable
    {

        int instructorHours;
        int clientID;
        private static bool clientSaved = false;
        private static bool hoursPassed = false;
        private static bool displayMessage = false;

        public void Dispose()
        {
        }

        public static bool ClientSaved
        {
            get { return clientSaved; }
            set { clientSaved = value; }
        }

        public static bool HoursPassed
        {
            get { return hoursPassed; }
            set { hoursPassed = value; }
        }

        public static bool DisplayMessage
        {
            get { return displayMessage; }
            set { displayMessage = value; }
        }


        public void SaveIstructorInformation(InstructorInformation InstructorInformation)
        {
            using (FitnessGym gym = new FitnessGym())
            {
                gym.InstructorInformations.Add(InstructorInformation);
                
                gym.SaveChanges();  
            }
        }

        public void SaveClientInformation(ClientInformation ClientInformation)
        {
            FitnessGym instructorDate = new FitnessGym();

            using (ClientEntity client = new ClientEntity())
            {
                clientID = ClientInformation.InstructorID;
                instructorHours = ClientInformation.Hours;
                client.ClientInformations.Add(ClientInformation);

                foreach (var instructor in instructorDate.InstructorInformations)
                {
                    if (instructor.InstructorId == clientID && instructorHours + instructor.Hours > 20)
                        return;
                }

                if(CanSaveClient(ClientInformation) == true && ClientInformation.PhoneNumber.Length == 10 && ClientInformation.Email.Contains('@')) // Here we can check how to validate the 
                {                                                                                                                                   // email address in more detail 
                    displayMessage = false;
                    client.SaveChanges();
                }
                else
                {
                    displayMessage = true;
                }
                
            }
        }

        public void SaveClientWithoutInstructor(ClientInformation ClientInformation)
        {
            using (ClientEntity client = new ClientEntity())
            {
                client.ClientInformations.Add(ClientInformation);
                client.SaveChanges();
            }
        }

        public void SavedMovedClient(ClientInformation ClientInformation)
        {
            using (ClientEntity client = new ClientEntity())
            {
                client.ClientInformations.Update(ClientInformation);
                client.SaveChanges();
            }
        }

        public void SaveInstructorHours()
        {
            using (FitnessGym gym = new FitnessGym())
            {
                foreach (var instructor in gym.InstructorInformations)
                {
                    if(instructor.InstructorId == clientID && instructorHours + instructor.Hours <= 20)
                    {
                        instructor.Hours += instructorHours;
                    }
                    else if(instructor.InstructorId == clientID && instructorHours + instructor.Hours > 20)
                    {
                        hoursPassed = true;
                        return;
                    }
                } 
                gym.SaveChanges();
            }
            clientSaved = true;
        }

        private static bool CanSaveClient(ClientInformation ClientInformation)
        {
            
            if (String.IsNullOrEmpty(ClientInformation.FirstName) || String.IsNullOrEmpty(ClientInformation.LastName) || ClientInformation.Hours == 0 || ClientInformation.Email == null || ClientInformation.PhoneNumber == null)
            {
                return false;
            }

            return true;
        }
 
    }
}
