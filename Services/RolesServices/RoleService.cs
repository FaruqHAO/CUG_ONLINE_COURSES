using CUG_ONLINE_COURSES.Data;
using CUG_ONLINE_COURSES.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Radzen.Blazor.Rendering;

namespace CUG_ONLINE_COURSES.Services.RolesServices
{
    public class RoleService : IRoleService
    {

        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;  // Add this line for DbContext


        public RoleService(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _context = context;
        }

        public async Task EnsureRolesExistAsync()
        {
            foreach (var role in AuthenticationInitalData.AppRoles)
            {
                if (!await _roleManager.RoleExistsAsync(role.Name))
                {
                    await _roleManager.CreateAsync(role);
                }
            }
        }
        public async Task<bool> SaveUserRoles(UserWithRoles user)
        {
            var userToUpdate = await _userManager.FindByIdAsync(user.UserId);
            if (userToUpdate != null)
            {
                var userWithOldRoles = await GetUsersWithRolesByUserAsync(userToUpdate);
                if (!string.IsNullOrEmpty(userWithOldRoles.RoleName))
                {
                    await _userManager.RemoveFromRoleAsync(userToUpdate, userWithOldRoles.RoleName);
                }

                if (!string.IsNullOrEmpty(user.RoleName))
                {
                    await _userManager.AddToRoleAsync(userToUpdate, user.RoleName);
                }
                return true;
            }
            return false;
        }
        public async Task<List<UserWithRoles>> GetUsersWithRolesAsync()
        {
            var users = _userManager.Users.ToList();
            var userWithRolesList = new List<UserWithRoles>();

            foreach (var user in users)
            {
                var userWithRoles = new UserWithRoles
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                };
                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Count > 0)
                {
                    userWithRoles.RoleName = roles.FirstOrDefault();
                }
                userWithRolesList.Add(userWithRoles);
            }

            return userWithRolesList;
        }
        public async Task<UserWithRoles> GetUsersWithRolesByUserAsync(ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);

            return new UserWithRoles
            {
                UserId = user.Id,
                UserName = user.UserName,
                RoleName = roles.FirstOrDefault()
            };
        }
        public async Task<List<Admin>> GetAllAdmin()
        {
            try
            {
                var Admin = _userManager.Users.ToList();
                var AdminList = new List<Admin>();

                foreach (var user in Admin)
                {
                    var admin = new Admin
                    {
                        AdminID = user.Id,
                        Email = user.UserName,
                        Name = user.FullName,
                        Phone = user.PhoneNumber,
                        Password = user.PasswordHash,
                        createdOn = user.createdOn,
                        Status = user.status,
                    };
                    var roles = await _userManager.GetRolesAsync(user);
                    if (roles.Count > 0)
                    {
                        admin.Role = roles.FirstOrDefault();
                    }
                    AdminList.Add(admin);
                }
                return AdminList;
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        public async Task<List<Admin>> GetAllStaff()
        {
            try
            {
                var users = _userManager.Users.ToList();
                var userWithRolesList = new List<Admin>();

                foreach (var user in users)
                {
                    var staff = new Admin
                    {
                        AdminID = user.Id,
                        Email = user.UserName,
                        Name = user.FullName,
                    };
                    var roles = await _userManager.GetRolesAsync(user);

                    if (roles.Count > 0)
                    {
                        staff.Role = roles.FirstOrDefault();
                    }
                    // Check if the user is a "Student" and skip adding them to the list
                    if (!roles.Contains("Student"))  // Assuming "Student" is the role you want to exclude
                    {
                        userWithRolesList.Add(staff);  // Add to list only if not a student
                    }

                }
                //return null;
                return userWithRolesList;
            }
            catch (Exception ex)
            {
                return new List<Admin>();
            }
        }

        public async Task<bool> AddCourse(Course course)
        {
            try
            {
                _context.Courses.Add(course);
                var response=await _context.SaveChangesAsync();
                if (response==1)
                {
                    return true;
                }
                else
                {
                    return false;
                }

               
            }
            catch
            { return false; }
        }
        public async Task<List<Course>> GetAllCoursesAsync()
        {    
            try
            {
                // return await _context.Courses.ToListAsync(); // Return all courses from the database
                var coursesWithLecturers = await _context.Courses
        .Join(
            _userManager.Users, // Assuming Admins is the table holding the lecturer info
            course => course.AssignedLecturer, // The foreign key in the course table (e.g., AdminID)
            lecturer => lecturer.Id, // The primary key in the Admins table
            (course, lecturer) => new Course
            {
                Id = course.Id,
                CourseCode = course.CourseCode,
                CourseName = course.CourseName,
                Faculty = course.Faculty,
                AssignedLecturer = lecturer.Id,
                AssignedLecturerName = lecturer.FullName,
                
                // Lecturer Name from Admins table
            })
        .ToListAsync();

                return coursesWithLecturers;

            }
            catch
            {
                return new List<Course>();
            }
        }


        public async Task<List<Course>> GetAllFacultyCoursesAsync( string facultyID)
        {
            try
            {
                var coursesWithLecturers = await _context.Courses
    .Join(
        _userManager.Users, // Assuming Users table holds lecturer info
        course => course.AssignedLecturer, // Foreign key in Courses table
        lecturer => lecturer.Id, // Primary key in Users table
        (course, lecturer) => new Course
        {
            Id = course.Id,
            CourseCode = course.CourseCode,
            CourseName = course.CourseName,
            Faculty = course.Faculty,
            AssignedLecturer = lecturer.Id,
            AssignedLecturerName = lecturer.FullName, // Lecturer Name from Users table
        })
    .Where(course => course.Faculty.Trim() == facultyID.Trim()) // Replace "YourFacultyName" with your filter value
    .ToListAsync();

               return coursesWithLecturers;
            }
            catch
            {
                return new List<Course>();
            }
        }
    }
}
