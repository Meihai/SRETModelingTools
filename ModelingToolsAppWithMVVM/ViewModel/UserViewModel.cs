using GalaSoft.MvvmLight;
using System.ComponentModel;
using ModelingToolsAppWithMVVM.Model;
using System.Collections.ObjectModel;
using ModelingToolsAppWithMVVM.Common;
using System;
using System.Windows.Input;
namespace ModelingToolsAppWithMVVM.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class UserViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the UserViewModel class.
        /// </summary>
        public UserViewModel()
        {
            //AddCommand = new DelegateCommands();
            //AddCommand.ExecuteCommand = new Action<object>(addStudent);

            //UpdateCommand = new DelegateCommands();
            //UpdateCommand.ExecuteCommand = new Action<object>(updateStudent);//修改方法

            //DeleteCommand = new DelegateCommands();
            //DeleteCommand.ExecuteCommand = new Action<object>(deleteStudent);//修改方法
            mylist.Add(new User() { ID = 1, Name = "张三", Age = 20, Sex = "女", Remarks = "无" });
            mylist.Add(new User() { ID = 2, Name = "李四", Age = 21, Sex = "女", Remarks = "无" });
            mylist.Add(new User() { ID = 3, Name = "王五", Age = 22, Sex = "女", Remarks = "无" });
            mylist.Add(new User() { ID = 4, Name = "赵六", Age = 24, Sex = "女", Remarks = "无" });
            //Binding();
        }

        //数据源
        ObservableCollection<User> _mylist = new ObservableCollection<User>();

        ObservableCollection<User> _showlist = new ObservableCollection<User>();

        public ObservableCollection<User> ShowList
        {

            get { return _showlist; }
            set
            {
                _showlist = value;
                RaisePropertyChanged("Showlist");
            }
        }

        public ObservableCollection<User> mylist
        {

            get { return _mylist; }
            set
            {
                _mylist = value;
                RaisePropertyChanged("mylist");
            }
        }

        public DelegateCommands AddCommand { get; set; }
        public DelegateCommands UpdateCommand { get; set; }
        public DelegateCommands DeleteCommand { get; set; }


        //#region 方法
        //private void Binding()
        //{
        //    ShowList.Clear();
        //    mylist.ToList().ForEach(p => ShowList.Add(p));
        //}
        //public void addStudent(object parameter)
        //{
        //    int id = mylist[mylist.Count - 1].ID;
        //    mylist.Add(new User() { ID = id + 1, Name = Name, Age = Age, Sex = Sex, Remarks = Remarks });
        //    Binding();
        //}
        //public void updateStudent(object parameter)
        //{
        //    if (ID == 0)
        //    {
        //        MessageBox.Show("请选择修改项");
        //        return;
        //    }
        //    foreach (var item in mylist)
        //    {
        //        if (item.ID == user.ID)
        //        {
        //            item.ID = ID;
        //            item.Name = Name;
        //            item.Sex = Sex;
        //            item.Remarks = Remarks;
        //            item.Age = Age;
        //            break;
        //        }
        //    }
        //    Binding();
        //}
        //public void deleteStudent(object parameter)
        //{
        //    if (ID == 0)
        //    {
        //        MessageBox.Show("请选择删除项");
        //        return;
        //    }
        //    User user1 = mylist.Single(p => p.ID == ID);
        //    mylist.Remove(user1);
        //    Binding();
        //}
        //#endregion
        
    }

    public class DelegateCommands : ICommand
    {
        public Action<object> ExecuteCommand = null;

        public Func<object, bool> CanExecuteCommand = null;

        //当命令可执行状态发生改变时，应当被激发
        public event EventHandler CanExecuteChanged;

        //用于判断命令是否可以执行
        public bool CanExecute(object parameter)
        {
            if (CanExecuteCommand != null)
            {
                return this.CanExecuteCommand(parameter);
            }
            else
            {
                return true;
            }
        }
        //命令执行
        public void Execute(object parameter)
        {
            if (this.ExecuteCommand != null) this.ExecuteCommand(parameter);
        }

    }
}