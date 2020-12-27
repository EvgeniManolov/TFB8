namespace TFB8.Interfaces
{
    using System.Collections.Generic;
    using Models;

    public interface ISemesterService
    {
        IEnumerable<Semester> GetAllSemesters();

        void CreateSemester(Semester semester);

        void DeleteSemester(int id);
    }
}
