using Fitness.Data;
using Fitness.Entities;
using Microsoft.AspNetCore.Components;
using System.Security.Cryptography.X509Certificates;


namespace ClientInformationBase
{
    public class ClientInformationBase : ComponentBase
    {
        public ClientInformation ClientInformation { get; set; }
        public InstructorInformation InstructorInformation { get; set; }

        private static bool saved = false;

        public static bool Saved
        {
            get { return saved; }
            set { saved = value; }
        }

        protected override Task OnInitializedAsync()
        {
            ClientInformation = new ClientInformation();

            return base.OnInitializedAsync();
        }

        public void SaveClient()
        {
            using (FitnessDateAcces data = new FitnessDateAcces())
            {
                data.SaveClientInformation(ClientInformation);
                data.SaveInstructorHours();
            }
            OnInitializedAsync();
            StateHasChanged();
        }

        public void SavePotentialClient()
        {
            using (FitnessDateAcces data = new FitnessDateAcces())
            {
                data.SaveClientWithoutInstructor(ClientInformation);
            }
            saved = true;
            OnInitializedAsync();
            StateHasChanged();
        }
        
        public void MoveClient(ClientInformation client)
        {
            FitnessDateAcces data = new FitnessDateAcces();

            using (FitnessGym dataInstructor = new FitnessGym())
            {

                foreach (var instructor in dataInstructor.InstructorInformations)
                {
                    if (instructor.Hours < 20 && instructor.Hours + client.Hours <= 20)
                    {
                        client.InstructorID = (int)instructor.InstructorId;
                        instructor.Hours += client.Hours;
                        break;
                    }
                }
                dataInstructor.SaveChanges();
                data.SavedMovedClient(client);
            }
        }

    }
}
