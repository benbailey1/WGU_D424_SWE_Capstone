using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using StudentTermTracker.Models;

namespace StudentTermTracker.Services
{
    public static class DatabaseService
    {
        private static SQLiteAsyncConnection? _dbConn;

        static async Task Init()
        {
            if (_dbConn != null)
                return;

            var databasePath = Path.Combine(FileSystem.AppDataDirectory, "Terms.db");
            _dbConn = new SQLiteAsyncConnection(databasePath);

            await _dbConn.CreateTableAsync<Term>();
            await _dbConn.CreateTableAsync<Course>();
            await _dbConn.CreateTableAsync<Assessment>();

        }


        #region Term Methods

        public static async Task<int> CreateTermAsync(Term term)
        {
            await Init();

            await _dbConn.InsertAsync(term);

            return term.Id;
        }

        public static async Task DeleteTerm(int id)
        {
            try
            {
                await Init();

                await _dbConn.DeleteAsync<Term>(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Delete term error: {ex.Message}");
                throw;
            }

        }

        public static async Task<IEnumerable<Term>> GetAllTerms()
        {
            await Init();

            return await _dbConn.Table<Term>().ToListAsync();
        }

        public static async Task<Term> GetTermByIdAsync(int termId)
        {
            await Init();

            return await _dbConn.Table<Term>().Where(t => t.Id == termId).FirstOrDefaultAsync();
        }

        public static async Task UpdateTerm(Term term)
        {
            await Init();

            var foundTerm = await _dbConn.Table<Term>().Where(t => t.Id == term.Id)
                .FirstOrDefaultAsync();

            if (foundTerm != null)
            {
                foundTerm.Name = term.Name;
                foundTerm.StartDate = term.StartDate;
                foundTerm.EndDate = term.EndDate;

                await _dbConn.UpdateAsync(foundTerm);
            }
        }

        #endregion

        #region CourseMethods

        public static async Task<IEnumerable<Course>> GetAllCoursesAsync()
        {
            await Init();

            return await _dbConn.Table<Course>().ToListAsync();
        }

        public static async Task<IEnumerable<Course>> GetCoursesByTermIdAsync(int selectedTermId)
        {
            await Init();

            return await _dbConn.Table<Course>().Where(c => c.TermId == selectedTermId).ToListAsync();
        }

        public static async Task<Course> GetCourseByIdAsync(int courseId)
        {
            await Init();

            return await _dbConn.Table<Course>().Where(c => c.Id == courseId).FirstOrDefaultAsync();
        }

        public static async Task<int> AddCourseAsync(Course course)
        {
            await Init();

            await _dbConn.InsertAsync(course);
            return course.Id;
        }

        public static async Task UpdateCourseAsync(Course course)
        {
            try
            {
                await Init();

                var courseToUpdate = await _dbConn.Table<Course>()
                                    .Where(c => c.Id == course.Id)
                                    .FirstOrDefaultAsync();

                if (courseToUpdate != null)
                {
                    courseToUpdate.Name = course.Name;
                    courseToUpdate.StartDate = course.StartDate;
                    courseToUpdate.EndDate = course.EndDate;
                    courseToUpdate.InstructorName = course.InstructorName;
                    courseToUpdate.InstructorPhone = course.InstructorPhone;
                    courseToUpdate.InstructorEmail = course.InstructorEmail;
                    courseToUpdate.StartNotification = course.StartNotification;
                    courseToUpdate.EndNotification = course.EndNotification;
                    courseToUpdate.Status = course.Status;
                    courseToUpdate.Notes = course.Notes;

                    await _dbConn.UpdateAsync(courseToUpdate);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }

        }

        public static async Task DeleteCourseAsync(int Id)
        {
            await Init();

            await _dbConn.DeleteAsync<Course>(Id);
        }

        #endregion

        #region Assessment Methods

        public static async Task<IEnumerable<Assessment>> GetAllAssessmentsAsync()
        {
            await Init();

            return await _dbConn.Table<Assessment>().ToListAsync();
        }

        public static async Task<IEnumerable<Assessment>> GetAssessmentsForCourseAsync(int courseId)
        {
            return await _dbConn.Table<Assessment>().Where(a => a.CourseId == courseId).ToListAsync();
        }

        public static async Task AddAssessmentAsync(Assessment assessment)
        {
            await Init();

            await _dbConn.InsertAsync(assessment);

        }

        public static async Task UpdateAssessmentAsync(Assessment assessment)
        {
            await Init();

            var assessmentToUpdate = await _dbConn.Table<Assessment>()
                                     .Where(a => a.Id == assessment.Id)
                                     .FirstOrDefaultAsync();

            if (assessmentToUpdate != null)
            {
                await _dbConn.UpdateAsync(assessment);
            }
        }

        public static async Task DeleteAssessmentAsync(int Id)
        {
            await Init();

            await _dbConn.DeleteAsync<Assessment>(Id);
        }


        #endregion

        #region DemoData

        public static async Task LoadSampleData()
        {
            await Init();

            Term term = new Term
            {
                Name = "Sample Term 1",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(6)
            };
            await _dbConn.InsertAsync(term);

            Course course = new Course
            {
                TermId = term.Id,
                Name = "Under Water Basket Weaving",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(6),
                InstructorName = "Anika Patel",
                InstructorPhone = "555-123-4567",
                InstructorEmail = "anika.patel@strimeuniversity.edu",
                Status = Course.StatusType.InProgress
            };

            await _dbConn.InsertAsync(course);

            Assessment oA = new Assessment
            {
                CourseId = course.Id,
                Name = "Under Water Basket Weaving OA",
                StartDate = DateTime.Now.AddDays(15),
                EndDate = DateTime.Now.AddDays(30),
                Type = Assessment.AssessmentType.Objective,
                StartNotification = true,
                EndNotification = true
            };

            await _dbConn.InsertAsync(oA);

            Assessment pA = new Assessment
            {
                CourseId = course.Id,
                Name = "Under Water Basket Weaving pA",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(14),
                Type = Assessment.AssessmentType.Performance,
                StartNotification = true,
                EndNotification = true
            };

            await _dbConn.InsertAsync(pA);

        }

        #endregion
    }
}
