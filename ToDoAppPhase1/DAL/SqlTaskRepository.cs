using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using ToDoAppPhase2;

namespace ToDoAppPhase1.DAL
{
    public class SqlTaskRepository : ISqlTaskRepository
    {
        string connectionString;
        SqlConnection cnn;
        public SqlTaskRepository()
        {
            //connectionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=TodoAppPhase2;Integrated Security=True";
            connectionString = ConfigurationManager.ConnectionStrings[1].ConnectionString;
            cnn = new SqlConnection(connectionString);

        }

        public void AddTask(Task t)
        {            
            cnn.Open();
            string sql = string.Format("insert into Task (Title, Description, TypeList, TimeCreate) " +
                "values (N'{0}', N'{1}', {2}, '{3}')", t.Title, t.Description, t.TypeList, t.TimeCreate);
            SqlCommand command = new SqlCommand(sql, cnn);
            command.ExecuteNonQuery();
            command.Dispose();
            cnn.Close();
        }

        public void UpdateTask(Task t)
        {
            cnn.Open();
            string sql = string.Format("update Task set Title = N'{0}', Description = N'{1}', TypeList = {2} where Id = {3}",
                t.Title, t.Description, t.TypeList, t.Id);
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            cnn.Close();
        }

        public int GetMaxId()
        {
            int maxId = 0;
            cnn.Open();
            string sql = "select Max(Id) from Task";
            SqlCommand cmd = new SqlCommand(sql, cnn);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                try
                {
                    maxId = Convert.ToInt32(reader[0]);
                }
                catch
                {
                    maxId = -1;
                }
            }
            reader.Close();
            cmd.Clone();
            cnn.Close();
            return maxId;
        }

        public void DeleteTaskById(int idTask)
        {
            cnn.Open();
            string sql = string.Format("delete Task where Id = {0}", idTask);
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            cnn.Close();
        }

        public Task GetTaskById(int id)
        {
            Task t = new Task();
            cnn.Open();
            string sql = string.Format("select * from Task where Id = {0}", id);
            SqlCommand cmd = new SqlCommand(sql, cnn);
            SqlDataReader reader = cmd.ExecuteReader();
            while(reader.Read())
            {
                t.Id = Convert.ToInt32(reader["Id"]);
                t.Title = reader["Title"].ToString();
                t.Description = reader["Description"].ToString();
                t.TimeCreate = Convert.ToDateTime(reader["TimeCreate"]);
            }
            reader.Close();
            cmd.Dispose();
            cnn.Close();
            return t;
        }

        public List<Task> GetAllTask()
        {
            List<Task> list = new List<Task>();
            Task t;
            cnn.Open();
            string sql = string.Format("select * from Task");
            SqlCommand cmd = new SqlCommand(sql, cnn);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                t =  new Task();
                t.Id = Convert.ToInt32(reader["Id"]);
                t.Title = reader["Title"].ToString();
                t.Description = reader["Description"].ToString();
                t.TimeCreate = Convert.ToDateTime(reader["TimeCreate"]);
                t.TypeList = Convert.ToInt32(reader["TypeList"]);
                list.Add(t);
            }
            reader.Close();
            cmd.Dispose();
            cnn.Close();
            return list;
        }
    }
}
