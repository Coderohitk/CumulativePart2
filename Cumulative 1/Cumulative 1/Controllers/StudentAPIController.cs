using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Cumulative_1.Models;
using System;
using MySql.Data.MySqlClient;

namespace Cumulative_1.Controllers
{
    
    [Route("api/Student")]
    [ApiController]
    public class StudentAPIController : ControllerBase
    {
        private readonly SchooldbContext _context;
        public StudentAPIController(SchooldbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route(template: "listStudents")]
        public List<Student> ListStudent()
        {
            List<Student> Students = new List<Student>();
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                MySqlCommand Command = Connection.CreateCommand();
                Command.CommandText = "Select * from students";
                Command.Prepare();
                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    while (ResultSet.Read())
                    {
                        int id = Convert.ToInt32(ResultSet["studentid"]);
                        string FirstName = ResultSet["studentfname"].ToString();
                        string LastName = ResultSet["studentlname"].ToString();
                        string StudentNumber = ResultSet["studentnumber"].ToString();
                        DateTime EnrolDate = Convert.ToDateTime(ResultSet["enroldate"]);
                        Student CurrentStudent = new Student()
                        {
                            StudentId = id,
                            StudentFName = FirstName,
                            StudentLName = LastName,
                            EnrollDate = EnrolDate,
                            StudentNumber = StudentNumber,

                        };

                        Students.Add(CurrentStudent);

                    }
                }

            }
            return Students;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(template: "FindStudent/{id}")]
        public Student FindStudent(int id)
        {

            Student SelectedStudents = new Student();

            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                MySqlCommand Command = Connection.CreateCommand();
                Command.CommandText = "Select * from students WHERE studentid = @id";
                Command.Parameters.AddWithValue("@id", id);

                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    while (ResultSet.Read())
                    { 
                        int StudentId = Convert.ToInt32(ResultSet["studentid"]);
                        string FirstName = ResultSet["studentfname"].ToString();
                        string LastName = ResultSet["studentlname"].ToString();
                        string StudentNumber = ResultSet["studentnumber"].ToString();
                        DateTime EnrolDate = Convert.ToDateTime(ResultSet["enroldate"]);


                        SelectedStudents.StudentId = StudentId;
                        SelectedStudents.StudentFName = FirstName;
                        SelectedStudents.StudentLName = LastName;
                        SelectedStudents.EnrollDate = EnrolDate;
                        SelectedStudents.StudentNumber = StudentNumber;
                                          
                    }
                }
            }


            return SelectedStudents;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="StudentData"></param>
        /// <returns></returns>
        [HttpPost(template:"AddStudent")]
        public int AddStudent([FromBody]Student StudentData)
        {
            using (MySqlConnection Connection = _context.AccessDatabase()) 
            {
                Connection.Open();

                MySqlCommand Command = Connection.CreateCommand();
                Command.CommandText = "INSERT INTO students(studentfname,studentlname,studentnumber,enroldate) VALUES(@studentfname,@studentlname,@studentnumber,@enroldate)";
                Command.Parameters.AddWithValue("@studentfname", StudentData.StudentFName);
                Command.Parameters.AddWithValue("@studentlname", StudentData.StudentLName);
                Command.Parameters.AddWithValue("@studentnumber", StudentData.StudentNumber);
                Command.Parameters.AddWithValue("@enroldate", StudentData.EnrollDate);

                Command.ExecuteNonQuery();
                return Convert.ToInt32(Command.LastInsertedId);

            }
            return 0;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="StudentId"></param>
        /// <returns></returns>
        [HttpDelete(template: "DeleteStudent/{StudentId}")]
        public int DeleteStudent(int StudentId)
        {
            // 'using' will close the connection after the code executes
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                //Establish a new command (query) for our database
                MySqlCommand Command = Connection.CreateCommand();


                Command.CommandText = "delete from students where studentid=@id";
                Command.Parameters.AddWithValue("@id", StudentId);
                return Command.ExecuteNonQuery();

            }
            // if failure
            return 0;
        }

    }
}
