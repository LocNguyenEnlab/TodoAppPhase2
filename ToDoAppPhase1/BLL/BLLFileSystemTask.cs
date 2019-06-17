using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ToDoAppPhase1.DAL;
using ToDoAppPhase2;

namespace ToDoAppPhase1.BLL
{
    public class BLLFileSystemTask
    {
        IFileSystemTaskRepository _fileSystem;
        public BLLFileSystemTask()
        {
            _fileSystem = new FileSystemTaskRepository();
        }
        public void AddTask(Task t)
        {
            _fileSystem.AddTask(t);
        }

        public int GetMaxId()
        {
            return _fileSystem.GetMaxId();
        }

        public List<Task> GetAllTask()
        {
            return _fileSystem.GetAllTask();
        }

        public Task GetATask(int id)
        {
            return _fileSystem.GetATask(id);
        }

        public void UpdateTask(Task t)
        {
            _fileSystem.UpdateTask(t);
        }

        public void DeleteTask(int idTask)
        {
            _fileSystem.DeleteTask(idTask);
        }

        public bool IsDuplicateTask(Task t)
        {
            t.Title = " " + t.Title;
            List<Task> list = GetAllTask();
            foreach(var item in list)
            {
                if (item.Compare(t))
                    return true; 
            }
            return false;
        }
    }
}
