using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ToDoAppPhase2;

namespace ToDoAppPhase1.DAL
{
    public class FileSystemTaskRepository : IFileSystemTaskRepository
    {
        string _path;
        public FileSystemTaskRepository()
        {
            _path = @"C:\FileSystemTodoApp\TodoAppPhase2.txt";
            if (!File.Exists(_path))
            {
                var file = File.Create(_path);
                file.Close();
                File.WriteAllText(_path, "MaxID: 0\n");
            }
        }
        public void AddTask(Task t)
        {
            string s = string.Format("Id: {0}, Title: {1}, Description: {2}, TimeCreate: {3}, TypeList: {4}",
                t.Id, t.Title, t.Description, t.TimeCreate.ToString(), t.TypeList);
            string[] s1 = File.ReadAllLines(_path);
            int id = GetMaxId() + 1;
            StreamWriter sww = File.CreateText(_path);
            sww.Flush();
            sww.Close();
            
            using (StreamWriter sw = File.AppendText(_path))
            {
                sw.WriteLine(s);
                for (int i = 0; i < s1.Count()-1; i++)
                {
                    sw.WriteLine(s1[i]);
                }
                sw.WriteLine("MaxID: {0}", id);
            }
        }

        public int GetMaxId()
        {
            string[] s = File.ReadAllLines(_path);
            string s1 = s[s.Count()-1];
            string[] s2 = s1.Split(':');
            int maxID = Convert.ToInt32(s2[1]);
            return maxID;
        }

        public List<Task> GetAllTask()
        {
            List<Task> list = new List<Task>();
            string[] s2 = File.ReadAllLines(_path);
            for (int i = 0; i < s2.Count()-1; i++)
            {
                Task t = new Task();
                string[] s3 = s2[i].Split(':', ',');
                t.Id = Convert.ToInt32(s3[1]);
                t.Title = s3[3];
                t.Description = s3[5];
                string time = s3[7] + ":" + s3[8] + ":" + s3[9];
                t.TypeList = Convert.ToInt32(s3[11]);
                t.TimeCreate = Convert.ToDateTime(time);
                list.Add(t);
            }
            return list;
        }

        public Task GetATask(int id)
        {
            List<Task> list = GetAllTask();
            foreach(var item in list)
            {
                if (item.Id == id)
                {
                    return item;
                }
            }
            return null;
        }

        public void UpdateTask(Task t)
        {
            string[] s = File.ReadAllLines(_path);
            StreamWriter sww = File.CreateText(_path);
            sww.Flush();
            sww.Close();
            foreach(var item in s)
            {
                int id = Convert.ToInt32(item.Split(':', ',')[1]);
                Task t1 = GetATask(id);
                if (t.Id == id)
                {
                    string s1 = string.Format("Id: {0}, Title: {1}, Description: {2}, TimeCreate: {3}, TypeList: {4}",
                                                t.Id, t.Title, t.Description, t.TimeCreate.ToString(), t.TypeList);
                    using (StreamWriter sw = File.AppendText(_path))
                    {
                        sw.WriteLine(s1);
                    }
                }
                else
                {
                    using (StreamWriter sw = File.AppendText(_path))
                    {
                        sw.WriteLine(item);
                    }
                }
            }
        }

        public void DeleteTask(int idTask)
        {
            string[] s = File.ReadAllLines(_path);
            StreamWriter sww = File.CreateText(_path);
            sww.Flush();
            sww.Close();
            foreach (var item in s)
            {
                int id = Convert.ToInt32(item.Split(':', ',')[1]);
                Task t1 = GetATask(id);
                if (idTask != id)
                {
                    using (StreamWriter sw = File.AppendText(_path))
                    {
                        sw.WriteLine(item);
                    }
                }
                //if (idTask == id) ignore;
            }
        }
    }
}
