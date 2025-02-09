using System.ComponentModel.DataAnnotations;

namespace CUG_ONLINE_COURSES.Models
{
    public class UserWithRoles
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string RoleName { get; set; }
        public bool Error { get; set; }
        public bool Saved { get; set; }
        public bool Saving { get; set; }
    }

    public class Admin
    {


        public string AdminID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public string Role { get; set; }

        public string Password { get; set; }

        public bool Status { get; set; }

        public DateTime? createdOn { get; set; }

    }
    public class Course
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "CourseCode")]
        public string CourseCode { get; set; } = "";
        [Required]
        [Display(Name = "CourseName")]
        public string CourseName { get; set; } = "";
        [Required]
        [Display(Name = "Faculty")]
        public string Faculty { get; set; } = "";
        [Required]
        [Display(Name = "AssignedLecturer")]
        public string AssignedLecturer { get; set; } = "";

        public string AssignedLecturerName { get; set; } = "";


    }
}
