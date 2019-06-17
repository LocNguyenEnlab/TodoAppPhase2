using System.Collections.Generic;
using ToDoAppPhase2;

namespace ToDoAppPhase1.DAL
{
    public interface IFileSystemTaskRepository
    {
        void AddTask(Task t);
        int GetMaxId();
        List<Task> GetAllTask();
        Task GetATask(int id);
        void UpdateTask(Task t);
        void DeleteTask(int id);
    }
}
