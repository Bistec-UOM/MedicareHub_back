using Microsoft.AspNetCore.Http.HttpResults;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingProject.MockData
{
    public class AppointmentMockData
    {
        public static List<Appointment> getAppointments()
        {
            return new List<Appointment>
            {
                new Appointment
                {
                    Id = 1,
                    DateTime= DateTime.Now,
                    Status="new",
                    PatientId=1,
                    CreatedAt= DateTime.Now,
                    DoctorId=1,
                    RecepId=7
                },
                new Appointment
                {
                    Id = 2,
                    DateTime= DateTime.Now,
                    Status="new",
                    PatientId=2,
                    CreatedAt= DateTime.Now,
                    DoctorId=2,
                    RecepId=7

                }, new Appointment
                {
                    Id = 3,
                    DateTime= DateTime.Now,
                    Status="new",
                    PatientId=3,
                    CreatedAt= DateTime.Now,
                    DoctorId=3,
                    RecepId=7

                },
                new Appointment
                {
                    Id = 4,
                    DateTime= DateTime.Now,
                    Status="new",
                    PatientId=4,
                    CreatedAt= DateTime.Now,
                    DoctorId=4,
                    RecepId=7
                }
            };
        }
    }
}
