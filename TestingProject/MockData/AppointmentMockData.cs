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

        public static List<Appointment> getONlyDoctor1TuesDayAppointment()
        {

            return new List<Appointment>
            {
                new Appointment
                {

                    Id = 5,
                    DateTime= new DateTime(2024, 6, 18, 0, 0, 0),
                    Status="new",
                    PatientId=1,
                    CreatedAt= new DateTime(2024, 6, 17, 0, 0, 0),
                    DoctorId=1,
                    RecepId=7
                },
                new Appointment
                {
                    Id = 8,
                    DateTime= new DateTime(2024, 6, 18, 0, 0, 0),
                    Status="new",
                    PatientId=2,
                    CreatedAt= new DateTime(2024, 6, 11, 0, 0, 0),
                    DoctorId=1,
                    RecepId=7

                }, new Appointment
                {
                    Id = 3,
                    DateTime= new DateTime(2024, 6, 18, 0, 0, 0),
                    Status="new",
                    PatientId=3,
                    CreatedAt= new DateTime(2024, 6, 2, 0, 0, 0),
                    DoctorId=1,
                    RecepId=7

                }
                
            };


        }

        public static List<Unable_Date> GetUnableDates()
        {
            return new List<Unable_Date>
            {

                new Unable_Date
                {
                    Id=1,
                    doctorId=1,
                    Date=new DateTime(2024, 6, 18, 0, 0, 0),
                },
                new Unable_Date
                {
                    Id=2,
                    doctorId=1,
                    Date=new DateTime(2024, 6, 23, 0, 0, 0),
                }
            };
        }
        public static Unable_Date AddNewUnableDate()
        {
            return new Unable_Date
            {
                Id = 3,
                doctorId = 1,
                Date = new DateTime(2024, 6, 28, 0, 0, 0),
            };
        }

        public static List<Unable_Date> GetUnableDatesDoc1()
        {
            return new List<Unable_Date>
            {

                new Unable_Date
                {
                    Id=1,
                    doctorId=1,
                    Date=new DateTime(2024, 6, 18, 0, 0, 0),
                    StartTime=new DateTime(2024,6,18,0,0,0),
                    EndTime=new DateTime(2024,6,18,23,59,0)

                },
                new Unable_Date
                {
                    Id=2,
                    doctorId=1,
                    Date=new DateTime(2024, 6, 23, 0, 0, 0),
                    StartTime=new DateTime(2024,6,23,0,0,0),
                    EndTime=new DateTime(2024,6,23,23,59,0)
                }
            };
        }

        public static List<Notification> GetNotifications() {

            return new List<Notification> {
                new Notification {
                    Id=1,
                    From="7",
                    Seen=false
                },
                new Notification {
                    Id=2,
                    From="8",
                    Seen=false
                }
        };
        }

        public static Notification AddNewNotification()
        {
            return new Notification
            {
                Id = 3,
                From = "11"
            };
        }
        public static List<Notification> GetNotificationsTo3()
        {

            return new List<Notification> {
                new Notification {
                    Id=1,
                    From="7",
                    To="3",
                    Seen=false,
                    SendAt=DateTime.Today
                },
                new Notification {
                    Id=2,
                    From="8",
                    To="3",
                    Seen=false,
                    SendAt=DateTime.Today
                }
        };
        }

        public static List<Unable_Date> GetUnableTimeSlotsDoc1Date18()
        {
            return new List<Unable_Date>
            {

                new Unable_Date
                {
                    Id=1,
                    doctorId=1,
                    Date=new DateTime(2024, 6, 18, 0, 0, 0),
                    StartTime=new DateTime(2024,6,18,08,0,0),
                    EndTime=new DateTime(2024,6,18,09,00,0)

                },
                new Unable_Date
                {
                    Id=2,
                    doctorId=1,
                    Date=new DateTime(2024, 6, 18, 0, 0, 0),
                    StartTime=new DateTime(2024,6,18,01,0,0),
                    EndTime=new DateTime(2024,6,18,04,0,0)
                }
            };
        }

        public static Appointment getAppointmentToBeUpdated()
        {
            return new Appointment
            {

                Id = 234,
                DateTime = new DateTime(2024, 6, 17, 0, 0, 0),
                Status = "new",
                PatientId = 33,
                CreatedAt = new DateTime(2024, 6, 17, 0, 0, 0),
                DoctorId = 1,
                RecepId = 7

            };
        }
        public static Appointment getnewUpdatedAppointment()
        {
            return new Appointment
            {

                Id = 234,
                DateTime = new DateTime(2024, 6, 19, 0, 0, 0),
                Status = "Cancelled",
                PatientId = 33,
                CreatedAt = new DateTime(2024, 6, 17, 0, 0, 0),
                DoctorId = 1,
                RecepId = 7

            };
        }
        public static Patient getUpdatedAppointmentPatient() {
            return new Patient
            {
                Id = 33,
                Email = "dfaf@gmail.com",
                FullName = "fdsaf",
            };
        }

        public static Doctor getUpDatedAppointmentDoctor()
        {
            return new Doctor
            {

                Id = 1,
                UserId = 24,

            };
        }

        public static User getUpdateAppointmentDoctorUser() {
            return new User
            {
                Id = 24,
                Name = "Yasiry",
                Role = "Doctor"

            };


        }

        public static List<Appointment> getAppointments_DoctorToBeCancelled() {
            return new List<Appointment> {
                 new Appointment
                {

                    Id = 5,
                    DateTime= new DateTime(2024, 6, 17, 05, 0, 0),
                    Status="new",
                    PatientId=1,
                    CreatedAt= new DateTime(2024, 6, 17, 0, 0, 0),
                    DoctorId=1,
                    RecepId=7
                },
                new Appointment
                {
                    Id = 2,
                    DateTime= new DateTime(2024, 6, 17, 08, 0, 0),
                    Status="new",
                    PatientId=2,
                    CreatedAt= new DateTime(2024, 6, 19, 0, 0, 0),
                    DoctorId=1,
                    RecepId=7

                }, new Appointment
                {
                    Id = 3,
                    DateTime= new DateTime(2024, 6, 17, 04, 10, 0),
                    Status="new",
                    PatientId=3,
                    CreatedAt= new DateTime(2024, 6, 6, 0, 0, 0),
                    DoctorId=1,
                    RecepId=7

                }



            };
        }

        public static List<Patient> getAppointmentCancelledPatients()
        {
            {
                return new List<Patient> {
                     new Patient()
                {
                    Id=1,
                    Email="kamal@mailnator.com",
                    Name="kamal",

                },
                new Patient()
                {
                    Id=2,
                    Email="saman@mailinator.com",
                    Name="Saman",

                },
                 new Patient()
                {
                    Id=3,
                    Email="anil@mailinator.com",
                    Name="Anil",

                }

                };
            }
        }

        public static List<Appointment> getCancelledAppointmentsByDoctor()
        {
            return new List<Appointment> {
                 new Appointment
                {

                    Id = 5,
                    DateTime= new DateTime(2024, 6, 17, 05, 0, 0),
                    Status="cancelled",
                    PatientId=1,
                    CreatedAt= new DateTime(2024, 6, 17, 0, 0, 0),
                    DoctorId=1,
                    RecepId=7
                },
                new Appointment
                {
                    Id = 2,
                    DateTime= new DateTime(2024, 6, 17, 08, 0, 0),
                    Status="cancelled",
                    PatientId=2,
                    CreatedAt= new DateTime(2024, 6, 19, 0, 0, 0),
                    DoctorId=1,
                    RecepId=7

                }, new Appointment
                {
                    Id = 3,
                    DateTime= new DateTime(2024, 6, 17, 04, 10, 0),
                    Status="cancelled",
                    PatientId=3,
                    CreatedAt= new DateTime(2024, 6, 6, 0, 0, 0),
                    DoctorId=1,
                    RecepId=7

                }



            };
        }
        public static List<Appointment> getDeletedAppointments()
        {
            return new List<Appointment> {
                 new Appointment
                {

                    Id = 5,
                    DateTime= new DateTime(2024, 6, 17, 05, 0, 0),
                    Status="new",
                    PatientId=1,
                    CreatedAt= new DateTime(2024, 6, 17, 0, 0, 0),
                    DoctorId=1,
                    RecepId=7
                },
                new Appointment
                {
                    Id = 2,
                    DateTime= new DateTime(2024, 6, 17, 08, 0, 0),
                    Status="new",
                    PatientId=2,
                    CreatedAt= new DateTime(2024, 6, 19, 0, 0, 0),
                    DoctorId=1,
                    RecepId=7

                }, new Appointment
                {
                    Id = 3,
                    DateTime= new DateTime(2024, 6, 17, 04, 10, 0),
                    Status="new",
                    PatientId=3,
                    CreatedAt= new DateTime(2024, 6, 6, 0, 0, 0),
                    DoctorId=1,
                    RecepId=7

                }



            };
        }

      

    }
}
