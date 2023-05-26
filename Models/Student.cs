using System.ComponentModel.DataAnnotations;

namespace schoolAPI.Models
{
    public class Student
        {  
            public int id { set; get; }  
            [Required]  
            public string? first_name { set; get; }  
            [Required]  
            public string? last_name { set; get; }  
        }  
}