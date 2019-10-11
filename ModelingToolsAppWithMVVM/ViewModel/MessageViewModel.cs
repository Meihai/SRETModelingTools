using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Ioc;
using ModelingToolsAppWithMVVM.Common;
using ModelingToolsAppWithMVVM.Model;
using ModelingToolsAppWithMVVM.Test;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Forms;

namespace ModelingToolsAppWithMVVM.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MessageViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MessageViewModel class.
        /// </summary>
        [PreferredConstructorAttribute]
        public MessageViewModel()
        {
            initMessage();
            
        }

        public MessageViewModel(ModelingToolsAppWithMVVM.Model.Message msg)
        {
            Message = msg;
        }

        private void initMessage()
        {
            _message = MessageTestUtil.getSendMessage1();
            //TestDataMeanings = new ObservableCollection<DataMeaning>(Message.SortedDataMeaningList);
        }

        private bool canUserAddRows = false;
        public bool CanUserAddRows
        {
            get { return this.canUserAddRows; }
            set {
                this.canUserAddRows = value;
                RaisePropertyChanged(() => CanUserAddRows);
            }
        }

        private DataMeaning selectedItem;
        public DataMeaning SelectedItem
        {
            get { 
                return selectedItem; 
            }
            set {
                selectedItem = value;
                RaisePropertyChanged(()=>SelectedItem);
            }
        }

        private ModelingToolsAppWithMVVM.Model.Message _message;
        public ModelingToolsAppWithMVVM.Model.Message Message
        {
            get { 
                return _message; 
            }
            set {
                 _message = value;
                 RaisePropertyChanged(() => Message);
            }
        }


        private RelayCommand cmdSave;
        public  RelayCommand CmdSave
        {
            get
            {
                if (cmdSave == null)
                {
                    return new RelayCommand(() => CmdSaveExecute());
                }
                return cmdSave;
            }
            set
            {
                cmdSave = value;
            }
        }

        private void CmdSaveExecute() {
            CanUserAddRows = false;
        }

        private RelayCommand cmdAdd;

        public RelayCommand CmdAdd
        {
            get
            {
                if (cmdAdd == null)
                {
                    return new RelayCommand(() => CmdAddExecute());
                }
                return cmdAdd;
            }
            set
            {
                cmdAdd = value;
            }
        }

        //添加消息字段含义逻辑实现
        private void CmdAddExecute()
        {
            CanUserAddRows = true;

        }


        private RelayCommand cmdDelete;
        public RelayCommand CmdDelete
        {
            get
            {
                if (cmdDelete == null)
                {
                    return new RelayCommand(() => CmdDeleteExecute());
                }
                return cmdDelete;
            }
            set
            {
                cmdDelete = value;
            }
        }

        private void CmdDeleteExecute(){
            if (SelectedItem != null)
            {
                List<DataMeaning> dataMeaningList = Message.SortedDataMeaningList;
                dataMeaningList.Remove(SelectedItem);
                Message.SortedDataMeaningList = dataMeaningList;
                Message.DataMeaningMapToDataMeaningList();                
            } else
            {
                MessageBox.Show("未选中行");
            }
        }

      
      


    }
}