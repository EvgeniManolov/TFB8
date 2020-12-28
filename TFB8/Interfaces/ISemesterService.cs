namespace TFB8.Interfaces
{
    using System.Collections.Generic;
    using Models;

    public interface ISemesterService
    {
        IEnumerable<Semester> GetAllSemesters();

        Semester GetSemesterById(int id);

        void CreateSemester(Semester semester);

        void UpdateSemester(int id, Semester semester);

        void DeleteSemester(int id);
    }
}
