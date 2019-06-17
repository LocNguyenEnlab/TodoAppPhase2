using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ToDoAppPhase1.BLL;
using System.IO;

namespace ToDoAppPhase2
{
    public partial class Form1 : Form
    {
        public int _pointYTodo = 0;
        public int _pointYDoing = 0;
        public int _pointYDone = 0;
        public List<Task> _listTask;
        public List<TextBox> _listTbTodo;
        public List<TextBox> _listTbDoing;
        public List<TextBox> _listTbDone;
        public int _index = 0;
        public int _optionData; //0: sql server, 1: file sytem

        public BLLSqlTask _bllSql;
        public BLLFileSystemTask _bllFileSystem;

        public Form1()
        {
            InitializeComponent();
            _listTask = new List<Task>();
            _listTbTodo = new List<TextBox>();
            _listTbDoing = new List<TextBox>();
            _listTbDone = new List<TextBox>();
            _bllSql = new BLLSqlTask();
            _bllFileSystem = new BLLFileSystemTask();
        }

        private void ShowData()
        {
            if (_optionData == 0)
            {
                _listTask = _bllSql.GetAllTask();
            } else
            {
                _listTask = _bllFileSystem.GetAllTask();
            }
            foreach(var item in _listTask)
            {
                CreateNewTextBox(item);
            }
        }

        private void CreateNewTextBox(Task t)
        {
            TextBox tb = new TextBox();

            if (t.TypeList == 0)
            {
                tb.Location = new Point(0, _pointYTodo);
                tb.Text = t.Title;
                tb.Name = "tb" + (t.Id);
                tb.ReadOnly = true;
                tb.Size = new Size(325, 20);
                _listTbTodo.Add(tb);
                tb.MouseDown += TextBox_MouseDown;
                tb.Show();
                lvToDo.Controls.Add(tb);
                _pointYTodo += 25;
            }
            else if (t.TypeList == 1)
            {
                tb.Location = new Point(0, _pointYDoing);
                tb.Text = t.Title;
                tb.Name = "tb" + (t.Id);
                tb.ReadOnly = true;
                tb.Size = new Size(325, 20);
                _listTbDoing.Add(tb);
                tb.MouseDown += TextBox_MouseDown;
                tb.Show();
                lvDoing.Controls.Add(tb);
                _pointYDoing += 25;
            }
            else if (t.TypeList == 2)
            {
                tb.Location = new Point(0, _pointYDone);
                tb.Text = t.Title;
                tb.Name = "tb" + (t.Id);
                tb.ReadOnly = true;
                tb.Size = new Size(325, 20);
                _listTbDone.Add(tb);
                tb.MouseDown += TextBox_MouseDown;
                tb.Show();
                lvDone.Controls.Add(tb);
                _pointYDone += 25;
            }
        }

        private bool IsInListTbByName(string nameTb, List<TextBox> list)
        {
            foreach(var item in list)
            {
                if (item.Name == nameTb)
                {
                    return true;
                }
            }
            return false;
        }

        private TextBox FindTbByName(string name, List<TextBox> list)
        {
            foreach (var item in list)
            {
                if (item.Name == name)
                {
                    return item;
                }
            }
            return null;
        }

        private int FindIndexTbInList(List<TextBox> list, TextBox tb)
        {
            for(int i = 0; i < list.Count; i++)
            {
                if (list[i] == tb)
                {
                    return i;
                }
            }
            return -1;
        }

        private int FindIndexTaskByTitle(List<Task> list, string title)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Title == title)
                {
                    return i;
                }
            }
            return -1;
        }        

        private void RemoveTextBoxFromListView(TextBox tb, ListView lv, List<TextBox> list, ref int pointY)
        {
            int indexTb = FindIndexTbInList(list, tb);

            if (indexTb == list.Count - 1)
            {
                pointY -= 25;
            } else
            {
                for (int i = indexTb + 1; i < list.Count; i++)
                {
                    list[i].Location = new Point(0, list[i].Location.Y - 25);                    
                }
                pointY -= 25;
            }

            lv.Controls.Remove(tb);
            list.Remove(tb);            
        }

        private void EditTask(Task t)
        {
            FormAddTask f = new FormAddTask(t);
            f.Text = "Edit task";
            f.pd = new FormAddTask.PassData(PassData);
            f.show = new FormAddTask.ShowForm1(Show);
            f.Show();
            this.Hide();
        }
        
        private void DeleteTask(int idTask)
        {
            if (IsInListTbByName("tb" + idTask, _listTbTodo))
            {
                TextBox tb = FindTbByName("tb" + idTask, _listTbTodo);
                RemoveTextBoxFromListView(tb, lvToDo, _listTbTodo, ref _pointYTodo);
                if (_optionData == 0)
                {
                    _bllSql.DeleteTask(idTask);
                }
                else
                {
                    _bllFileSystem.DeleteTask(idTask);
                }
            }
            else if (IsInListTbByName("tb" + idTask, _listTbDoing)) {
                TextBox tb = FindTbByName("tb" + idTask, _listTbDoing);
                RemoveTextBoxFromListView(tb, lvDoing, _listTbDoing, ref _pointYDoing);
                if (_optionData == 0)
                {
                    _bllSql.DeleteTask(idTask);
                }
                else
                {
                    _bllFileSystem.DeleteTask(idTask);
                }
            } 
            else if (IsInListTbByName("tb" + idTask, _listTbDone)) {
                TextBox tb = FindTbByName("tb" + idTask, _listTbDone);
                RemoveTextBoxFromListView(tb, lvDone, _listTbDone, ref _pointYDone);
                if (_optionData == 0)
                {
                    _bllSql.DeleteTask(idTask);
                }
                else
                {
                    _bllFileSystem.DeleteTask(idTask);
                }
            }
        }

        private void ShowTaskDetail(Task t)
        {
            MessageBoxManager.Yes = "Edit";
            MessageBoxManager.No = "Delete";
            MessageBoxManager.Cancel = "Close";
            MessageBoxManager.Register();

            DialogResult dialogResult = MessageBox.Show(string.Format("Title: {0}\nDescription: {1}\nTime create: {2}", t.Title, t.Description, Convert.ToDateTime(t.TimeCreate)),
                "Task Detail", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
            MessageBoxManager.Unregister();
            if (dialogResult == DialogResult.Yes)
            {
                EditTask(t);
            }
            else if (dialogResult == DialogResult.No)
            {                
                DialogResult resultConfirm = MessageBox.Show("Do you want to delete this task?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (resultConfirm == DialogResult.Yes)
                {
                    DeleteTask(t.Id);
                }
            }
        }

        private void BtnAddTask_Click(object sender, EventArgs e)
        {
            FormAddTask form2 = new FormAddTask();
            form2.pd = new FormAddTask.PassData(PassData);
            form2.show = new FormAddTask.ShowForm1(Show);
            form2.Show();
            this.Hide();
        }

        public void PassData(Task t)
        {
            if (t.Id == -1) //add new task
            {
                if (_optionData == 0)
                {
                    if (!_bllSql.IsDuplicateTask(t))
                    {
                        this.Show();
                        _bllSql.AddTask(t);
                        int id = _bllSql.GetMaxId();
                        t = _bllSql.GetATask(id);
                        CreateNewTextBox(t);
                    }
                    else
                    {
                        MessageBox.Show("This task is already exists", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Show();
                    }
                }
                else
                {
                    if (!_bllFileSystem.IsDuplicateTask(t))
                    {
                        this.Show();
                        t.Id = _bllFileSystem.GetMaxId();
                        _bllFileSystem.AddTask(t);
                        CreateNewTextBox(t);
                    }
                    else
                    {
                        MessageBox.Show("This task is already exists", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Show();
                    }
                }
            } 
            else //update an exists task
            {
                if (_optionData == 0)
                {
                    _bllSql.UpdateTask(t);
                } 
                else
                {
                    _bllFileSystem.UpdateTask(t);
                }
                UpdateTextBox(t);
                this.Show();
            }
        }

        public void ShowForm1()
        {
            this.Show();
        }

        public void UpdateTextBox(Task t)
        {
            TextBox tb = new TextBox();

            if (FindTbByName("tb" + t.Id, _listTbTodo) != null)
            {
                tb = FindTbByName("tb" + t.Id, _listTbTodo);
                _listTbTodo[FindIndexTbInList(_listTbTodo, tb)].Text = t.Title;
            }
            else if (FindTbByName("tb" + t.Id, _listTbDoing) != null)
            {
                tb = FindTbByName("tb" + t.Id, _listTbDoing);
                _listTbDoing[FindIndexTbInList(_listTbDoing, tb)].Text = t.Title;
            }
            else if (FindTbByName("tb" + t.Id, _listTbDone) != null)
            {
                tb = FindTbByName("tb" + t.Id, _listTbDone);
                _listTbDone[FindIndexTbInList(_listTbDone, tb)].Text = t.Title;
            }
        }

        private void LvDoing_DragEnter(object sender, DragEventArgs e)
        {
            int idTb = ((Task)e.Data.GetData(e.Data.GetFormats()[0])).Id;

            if (!IsInListTbByName("tb" + idTb, _listTbDoing))
            {
                e.Effect = DragDropEffects.Move;
            }
        }

        private void LvDone_DragEnter(object sender, DragEventArgs e)
        {
            int idTb = ((Task)e.Data.GetData(e.Data.GetFormats()[0])).Id;

            if (!IsInListTbByName("tb" + idTb, _listTbDone))
            {
                e.Effect = DragDropEffects.Move;
            }
        }

        private void LvTodo_DragEnter(object sender, DragEventArgs e)
        {
            int idTb = ((Task)e.Data.GetData(e.Data.GetFormats()[0])).Id;
            
            if(!IsInListTbByName("tb" + idTb, _listTbTodo))
            {
                e.Effect = DragDropEffects.Move;
            }
        }

        private void LvDoing_DragDrop(object sender, DragEventArgs e)
        {
            ListView lv2 = sender as ListView;
            TextBox tb = new TextBox();
            Task t = new Task();
            t = (Task)e.Data.GetData(e.Data.GetFormats()[0]);
            t.TypeList = 1;
            if (_optionData == 0)
            {
                _bllSql.UpdateTask(t);
            } 
            else
            {
                _bllFileSystem.UpdateTask(t);
            }
            CreateNewTextBox(t);  
            
            if (IsInListTbByName("tb" + t.Id, _listTbTodo))
            {
                var item = FindTbByName("tb" + t.Id, _listTbTodo);
                RemoveTextBoxFromListView(item, lvToDo, _listTbTodo, ref _pointYTodo);
            }

            if (IsInListTbByName("tb" + t.Id, _listTbDone))
            {
                var item = FindTbByName("tb" + t.Id, _listTbDone);
                RemoveTextBoxFromListView(item, lvDone, _listTbDone, ref _pointYDone);
            }
        }

        private void LvDone_DragDrop(object sender, DragEventArgs e)
        {
            ListView lv2 = sender as ListView;
            TextBox tb = new TextBox();
            Task t = new Task();
            t = (Task)e.Data.GetData(e.Data.GetFormats()[0]);
            t.TypeList = 2;
            if (_optionData == 0)
            {
                _bllSql.UpdateTask(t);
            }
            else
            {
                _bllFileSystem.UpdateTask(t);
            }
            CreateNewTextBox(t);

            if (IsInListTbByName("tb" + t.Id, _listTbTodo))
            {
                var item = FindTbByName("tb" + t.Id, _listTbTodo);
                RemoveTextBoxFromListView(item, lvToDo, _listTbTodo, ref _pointYTodo);
            }

            if (IsInListTbByName("tb" + t.Id, _listTbDoing))
            {
                var item = FindTbByName("tb" + t.Id, _listTbDoing);
                RemoveTextBoxFromListView(item, lvDoing, _listTbDoing, ref _pointYDoing);
            }
        }

        private void LvTodo_DragDrop(object sender, DragEventArgs e)
        {
            ListView lv2 = sender as ListView;
            TextBox tb = new TextBox();
            Task t = new Task();
            t = (Task)e.Data.GetData(e.Data.GetFormats()[0]);
            t.TypeList = 0;
            if (_optionData == 0)
            {
                _bllSql.UpdateTask(t);
            }
            else
            {
                _bllFileSystem.UpdateTask(t);
            }
            CreateNewTextBox(t);

            if (IsInListTbByName("tb" + t.Id, _listTbDone))
            {
                var item = FindTbByName("tb" + t.Id, _listTbDone);
                RemoveTextBoxFromListView(item, lvDone, _listTbDone, ref _pointYDone);
            }

            if (IsInListTbByName("tb" + t.Id, _listTbDoing))
            {
                var item = FindTbByName("tb" + t.Id, _listTbDoing);
                RemoveTextBoxFromListView(item, lvDoing, _listTbDoing, ref _pointYDoing);
            }
        }

        private void TextBox_MouseDown(object sender, MouseEventArgs e)
        {
            TextBox tb = sender as TextBox;
            string nameTb = tb.Name;
            int idTb = Convert.ToInt32(nameTb.Replace("tb", ""));
            string title = tb.Text;
            Task t = new Task();

            if (_optionData == 0)
            {
                t = _bllSql.GetATask(idTb);
            } 
            else
            {
                t = _bllFileSystem.GetATask(idTb);
            }

            if (e.Button == MouseButtons.Left && e.Clicks == 1)
            {
                tb.DoDragDrop(t, DragDropEffects.Move);
            }
            else
            {
                ShowTaskDetail(t);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            MessageBoxManager.Yes = "Sql server";
            MessageBoxManager.No = "File system";
            MessageBoxManager.Register();
            DialogResult result = MessageBox.Show("Do you want to use sql server or file system?", "Inform", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            MessageBoxManager.Unregister();
            if (result == DialogResult.Yes)
            {
                _optionData = 0;
                ShowData();
            } 
            else if (result == DialogResult.No)
            {
                _optionData = 1;
                ShowData();                
            } 
        }
    }
}
