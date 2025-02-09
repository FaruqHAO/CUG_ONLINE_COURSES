using CUG_ONLINE_COURSES.Data;
using CUG_ONLINE_COURSES.Models;

namespace CUG_ONLINE_COURSES.Services.RolesServices
{
  
        public interface IRoleService
        {
            Task EnsureRolesExistAsync();
        Task<List<Admin>> GetAllAdmin();
        Task<List<Admin>> GetAllStaff();
        Task<List<UserWithRoles>> GetUsersWithRolesAsync();
            Task<UserWithRoles> GetUsersWithRolesByUserAsync(ApplicationUser user);
            Task<bool> SaveUserRoles(UserWithRoles user);
             Task<bool> AddCourse(Course course);
        Task<List<Course>> GetAllCoursesAsync();
        Task<List<Course>> GetAllFacultyCoursesAsync(string facultyID);

    }

 
}
