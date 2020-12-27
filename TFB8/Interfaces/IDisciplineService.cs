namespace TFB8.Interfaces
{
    using System.Collections.Generic;
    using TFB8.Models;

    public interface IDisciplineService
    {
        IEnumerable<Discipline> GetAllDisciplines();

        Discipline GetDisciplineById(int id);

        void CreateDiscipline(Discipline discipline);

        void UpdateDiscipline(int id, Discipline discipline);

        void DeleteDiscipline(int id);
    }
}
