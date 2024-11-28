using Cumulative_1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace Cumulative_1.Controllers
{
    [Route("api/Course")]
    [ApiController]
    public class CourseAPIController : ControllerBase
    {
        private readonly SchooldbContext _context;
        public CourseAPIController(SchooldbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Retrieves a list of all courses from the database.
        /// </summary>
        /// <returns>
        /// A list of all courses, including their course ID, teacher ID, course code, course name, start date, and finish date.
        /// </returns>
        /// <remarks>
        /// This method connects to the database and retrieves all courses from the `courses` table.
        /// It returns a collection of course objects that include the course details such as ID, teacher, and course dates.
        /// </remarks>
        /// <example>
        /// GET api/Course/listCourse ->[{"courseId":1,"coursecode":"http5101","teacherid":1,"startdate":"2018-09-04T00:00:00","finishdate":"2018-12-14T00:00:00","coursename":"Web Application Development"},...]
        /// </example>
        [HttpGet]
        [Route(template: "listCourse")]
        public List<Course> ListCourse()
        {
            List<Course> Courses = new List<Course>();
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                MySqlCommand Command = Connection.CreateCommand();
                Command.CommandText = "Select * from courses";
                Command.Prepare();
                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    while (ResultSet.Read())
                    {
                        int Couseid = Convert.ToInt32(ResultSet["courseid"]);
                        int teacherId = Convert.ToInt32(ResultSet["teacherid"]);
                        string CourseCode = ResultSet["coursecode"].ToString();
                        string CourseName = ResultSet["coursename"].ToString();
                        DateTime StartDate = Convert.ToDateTime(ResultSet["startdate"]);
                        DateTime FinishDate = Convert.ToDateTime(ResultSet["finishdate"]);

                        Course CurrentCourse = new Course()
                        {
                            courseId = Couseid,
                            teacherid = teacherId,
                            coursecode = CourseCode,
                            coursename = CourseName,
                            startdate = StartDate,
                            finishdate = FinishDate

                        };

                        Courses.Add(CurrentCourse);

                    }
                }

            }
            return Courses;
        }
        /// <summary>
        /// Retrieves details of a specific course by its course ID.
        /// </summary>
        /// <param name="id">The ID of the course to retrieve.</param>
        /// <returns>
        /// The details of the specified course, including its course ID, teacher ID, course code, course name, start date, and finish date.
        /// </returns>
        /// <remarks>
        /// This method connects to the database and retrieves the details of a specific course from the `courses` table
        /// based on the provided course ID. If the course is found, the details will be returned.
        /// </remarks>
        /// <example>
        /// GET api/Course/FindCourse/1 -> {"courseId":1,"coursecode":"http5101","teacherid":1,"startdate":"2018-09-04T00:00:00","finishdate":"2018-12-14T00:00:00","coursename":"Web Application Development"}
        /// </example>
        [HttpGet]
            [Route(template: "FindCourse/{id}")]
            public Course FindCourse(int id)
            {

                Course SelectedCourse = new Course();

            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                MySqlCommand Command = Connection.CreateCommand();

                Command.CommandText = "Select * from courses WHERE courseid = @id";
                Command.Parameters.AddWithValue("@id", id);

                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    while (ResultSet.Read())
                    {
                        int Couseid = Convert.ToInt32(ResultSet["courseid"]);
                        int teacherId = Convert.ToInt32(ResultSet["teacherid"]);
                        string CourseCode = ResultSet["coursecode"].ToString();
                        string CourseName = ResultSet["coursename"].ToString();
                        DateTime StartDate = Convert.ToDateTime(ResultSet["startdate"]);
                        DateTime FinishDate = Convert.ToDateTime(ResultSet["finishdate"]);

                        SelectedCourse.courseId = Couseid;
                        SelectedCourse.teacherid = teacherId;
                        SelectedCourse.coursecode = CourseCode;
                        SelectedCourse.coursename = CourseName;
                        SelectedCourse.startdate = StartDate;
                        SelectedCourse.finishdate = FinishDate;
                    }
                }
            }
                return SelectedCourse;
            }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="CourseData"></param>
        /// <returns></returns>
        [HttpPost(template: "AddCourse")]
        public int AddCourse([FromBody] Course CourseData)
        {
            // 'using' will close the connection after the code executes
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                //Establish a new command (query) for our database
                MySqlCommand Command = Connection.CreateCommand();

                // CURRENT_DATE() for the author join date in this context
                // Other contexts the join date may be an input criteria!
                Command.CommandText = "INSERT INTO courses(coursecode,teacherid,startdate,finishdate,coursename) VALUES(@coursecode,@teacherid,@startdate,@finishdate,@coursename)";
                Command.Parameters.AddWithValue("@coursecode", CourseData.coursecode);
                Command.Parameters.AddWithValue("@teacherid", CourseData.teacherid);
                Command.Parameters.AddWithValue("@startdate", CourseData.startdate);
                Command.Parameters.AddWithValue("@finishdate", CourseData.finishdate);
                Command.Parameters.AddWithValue("@coursename", CourseData.coursename);

                Command.ExecuteNonQuery();

                return Convert.ToInt32(Command.LastInsertedId);

            }
            // if failure
            return 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        [HttpDelete(template: "DeleteCourse/{courseId}")]
        public int DeleteCourse(int courseId)
        {
            // 'using' will close the connection after the code executes
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                //Establish a new command (query) for our database
                MySqlCommand Command = Connection.CreateCommand();


                Command.CommandText = "delete from courses where courseid=@id";
                Command.Parameters.AddWithValue("@id", courseId);
                return Command.ExecuteNonQuery();

            }
            // if failure
            return 0;
        }

    }


    }

