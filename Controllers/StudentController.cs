using System.Data;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using schoolAPI.Models;

namespace schoolAPI.Controllers
{
    [Route("[controller]")]
    public class StudentController : Controller
    {
        private readonly ILogger<StudentController> _logger;
        private IConfiguration Configuration;

        public StudentController(ILogger<StudentController> logger, IConfiguration _configuration)
        {
            _logger = logger;
            Configuration = _configuration;
        }

        private string GetConnectionString()
        {
            return this.Configuration.GetConnectionString("MyConn");
        }

        [HttpGet]
        public ActionResult<List<Student>> GetStudents()
        {
            List<Student> students = new List<Student>();
            using (NpgsqlConnection con = new NpgsqlConnection(GetConnectionString()))
            {
                using(NpgsqlCommand cmd = new NpgsqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Clear();

                    cmd.CommandText = "SELECT * FROM student;";

                    con.Open();
                    NpgsqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr.HasRows)
                    {
                        while (rdr.Read())
                        {
                            students.Add(new Student()
                            {
                                id = Convert.ToInt32(rdr[0]),
                                first_name = rdr[1].ToString(),
                                last_name = rdr[2].ToString()
                            });
                        }
                    }
                    else {
                        return NotFound("Empty student table");
                    }
                    con.Close();
                }
            }
            return Ok(students);
        }

        [HttpGet]
        [Route("{id}")]
        public ActionResult<Student> GetStudent(int id)
        {
            Student student = new Student();
            using (NpgsqlConnection con = new NpgsqlConnection(GetConnectionString()))
            {
                using(NpgsqlCommand cmd = new NpgsqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Clear();

                    cmd.CommandText = "SELECT * FROM student WHERE id = @id;";
                    cmd.Parameters.AddWithValue("@id", id);

                    con.Open();
                    NpgsqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr.HasRows)
                    {
                        while (rdr.Read())
                        {
                            student  = new Student()
                            {
                                id = Convert.ToInt32(rdr[0]),
                                first_name = rdr[1].ToString(),
                                last_name = rdr[2].ToString()
                            };
                        } 
                    } else 
                    {
                        return NotFound("Student not found");
                    }
                    con.Close();
                }
            }
            return Ok(student);
        }
    }



}