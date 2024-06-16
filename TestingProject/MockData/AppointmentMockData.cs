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

                    Id = 5,
                    DateTime= new DateTime(2024, 6, 17, 0, 0, 0),
                    Status="new",
                    PatientId=1,
                    CreatedAt= new DateTime(2024, 6, 17, 0, 0, 0),
                    DoctorId=1,
                    RecepId=7
                },
                new Appointment
                {
                    Id = 2,
                    DateTime= new DateTime(2024, 6, 1, 0, 0, 0),
                    Status="new",
                    PatientId=2,
                    CreatedAt= new DateTime(2024, 6, 19, 0, 0, 0),
                    DoctorId=2,
                    RecepId=7

                }, new Appointment
                {
                    Id = 3,
                    DateTime= new DateTime(2024, 6, 27, 0, 0, 0),
                    Status="new",
                    PatientId=3,
                    CreatedAt= new DateTime(2024, 6, 23, 0, 0, 0),
                    DoctorId=3,
                    RecepId=7

                },
                new Appointment
                {
                    Id = 4,
                    DateTime= new DateTime(2024, 6, 11, 0, 0, 0),
                    Status="new",
                    PatientId=4,
                    CreatedAt= new DateTime(2024, 6, 9, 0, 0, 0),
                    DoctorId=1,
                    RecepId=7
                }
            };
        }

        public static List<User> getUsers() {
            return new List<User>
            {
                new User {
                    Id=1,
                    Name="Chathura",
                    Role="Doctor"

                },
                new User
                {
                    Id=24,
                    Name="Yasiry",
                    Role="Doctor"

                },
                new User
                {
                    Id=3,
                    Name="Dhammika",
                    Role="Doctor"
                },
                new User { 
                    Id=4,
                    Name="Hasari",
                    Role="Admin"
                }

            };

           
        }
        public  static List<User> getDoctors()
        {
            return new List<User>
            {
                new User()
                {
                    Id=1,
                    Name="Yasiry",
                    Role="Doctor"


                },
                new User()
                {
                    Id=24,
                    Name="kamal",
                    Role="Doctor"
                },
                 new User()
                {
                    Id=67,
                    Name="samal",
                    Role="Doctor"
                }

            };
        }
        public static List<Doctor> getDoctUserDoctors()
        {
            return new List<Doctor>
            {
                new Doctor()
                {
                    Id=1,
                    UserId=24,


                },
                new Doctor()
                {
                    Id=2,
                    UserId=45
                },
                  new Doctor()
                {
                    Id=6,
                    UserId=67
                }

            };
        }
        public static Appointment AddNewAppointment()
        {
            return new Appointment
            {
                Id = 43,
                DateTime = new DateTime(2024, 6, 17, 0, 0, 0),
                Status = "new",
                PatientId = 78,
                CreatedAt = new DateTime(2024, 6, 17, 0, 0, 0),
                DoctorId = 6,
                RecepId = 7
            };
            
        }

        public static List<Patient> GetPatients()
        {
            return new List<Patient>
            {
                new Patient()
                {
                    Id=78,
                    Email="dfaf@gmail.com",
                    FullName="fdsaf",

                },
                new Patient()
                {
                    Id=21,
                    Email="dfaf@gmail.com",
                    FullName="fdsaf",

                }
            };
        }

        public static Patient GetPatient()
        {
            return new Patient()
            {
                Id = 88,
                Email = "dfaf@gmail.com",
                FullName = "fdsaf",


            };
        }
        public static Appointment getAppointment()
        {
            return new Appointment()
            {
                Id = 55,
                DateTime = new DateTime(2024, 6, 17, 0, 0, 0),
                Status = "new",
                PatientId = 1,
                CreatedAt = new DateTime(2024, 6, 17, 0, 0, 0),
                DoctorId = 1,
                RecepId = 7

            };
        }
        public static List<Appointment> getDoctor1Appointment()
        {
            return new List<Appointment>
            {
                new Appointment
                {
                    Id = 5,
                    DateTime= new DateTime(2024, 6, 17, 0, 0, 0),
                    Status="new",
                    PatientId=1,
                    CreatedAt= new DateTime(2024, 6, 17, 0, 0, 0),
                    DoctorId=1,
                    RecepId=7
                },
                new Appointment
                {
                    Id = 2,
                    DateTime= new DateTime(2024, 6, 1, 0, 0, 0),
                    Status="new",
                    PatientId=2,
                    CreatedAt= new DateTime(2024, 6, 19, 0, 0, 0),
                    DoctorId=1,
                    RecepId=7

                }, new Appointment
                {
                    Id = 3,
                    DateTime= new DateTime(2024, 6, 27, 0, 0, 0),
                    Status="new",
                    PatientId=3,
                    CreatedAt= new DateTime(2024, 6, 23, 0, 0, 0),
                    DoctorId=1,
                    RecepId=7

                },
                new Appointment
                {
                    Id = 4,
                    DateTime= new DateTime(2024, 6, 11, 0, 0, 0),
                    Status="new",
                    PatientId=4,
                    CreatedAt= new DateTime(2024, 6, 9, 0, 0, 0),
                    DoctorId=1,
                    RecepId=7
                }
            };
        }
    }
}
