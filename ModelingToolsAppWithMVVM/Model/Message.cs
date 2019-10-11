using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using ModelingToolsAppWithMVVM.Common;

namespace ModelingToolsAppWithMVVM.Model
{
    /// <summary>
    /// 消息报文
    /// </summary>
    public class Message:ObservableObject
    {   
        /// <summary>
        /// 内部唯一识别码
        /// </summary>
        private string id;     

        /// <summary>
        /// 报文显示名字
        /// </summary>
        private string name;
       
        /// <summary>
        /// 报文类型
        /// </summary>
        private string type;

        //报文具体含义描述
        private string description;

      
        /// <summary>
        /// 数据含义字典
        /// </summary>
        private Dictionary<string, DataMeaning> dataMeaningMap;

        /// <summary>
        /// 排好序的数据含义字段,从第0位到第n位排好序的
        /// </summary>
        private List<DataMeaning> sortedDataMeaningList;

     
        /// <summary>
        /// 数据报文字节数组
        /// </summary>
        private byte[] data;

        public Message() {
            id = Guid.NewGuid().ToString();
            dataMeaningMap = new Dictionary<string, DataMeaning>();
        }

       
        public Dictionary<string, DataMeaning> DataMeaningMap
        {
            get { return dataMeaningMap; }
            set { dataMeaningMap = value; }
        }


        public void DataMeaningMapToDataMeaningList()
        {
            List<DataMeaning> dataMeaningList = DataMeaningMap.Values.ToList();
            dataMeaningList.Sort((a,b)=>a.StartBit.
                CompareTo(b.StartBit));
            SortedDataMeaningList = dataMeaningList;
        }

        public List<DataMeaning> DataMeaningMapToList()
        {
            List<DataMeaning> dataMeaningList = DataMeaningMap.Values.ToList();
            dataMeaningList.Sort((a, b) => a.StartBit.
                CompareTo(b.StartBit));
            return dataMeaningList;
        }

        public List<DataMeaning> SortedDataMeaningList
        {
            get {
                if (sortedDataMeaningList == null)
                {
                    DataMeaningMapToDataMeaningList();
                }
                return sortedDataMeaningList; 
            }
            set { sortedDataMeaningList = value;
                  RaisePropertyChanged(() => SortedDataMeaningList);
                  ListToMap();
                }
        }

        private void ListToMap()
        {
            Dictionary<string, DataMeaning> tempDataMeaningMap = new Dictionary<string, DataMeaning>();
            for (int i = 0; i < sortedDataMeaningList.Count; i++)
            {
                tempDataMeaningMap.Add(sortedDataMeaningList[i].Id, sortedDataMeaningList[i]);
            }
            DataMeaningMap = tempDataMeaningMap;
        }



        /// <summary>
        /// 从数据字典中获取Byte数组
        /// </summary>
        /// <returns></returns>
        public byte[] getByteDataFromDataMeaningMap()
        {
            DataMeaningMapToDataMeaningList();
            //添加返回数组
            byte[] resultBytes = getByteDataFromSortedList();
            //添加crc校验码
            byte[] crcBytes = CrcUtils.Get_crc16(resultBytes, resultBytes.Length);            
            byte[] resultWithCrcBytes=new byte[resultBytes.Length+crcBytes.Length];
            resultBytes.CopyTo(resultWithCrcBytes,0);
            crcBytes.CopyTo(resultWithCrcBytes, resultBytes.Length);
            return resultBytes;
            
        }

        /// <summary>
        /// 从排序好的列表中获取byte数组
        /// </summary>
        /// <returns></returns>
        private byte[] getByteDataFromSortedList() {

            List<byte> byteList = new List<byte>();
            for (int i = 0; i < SortedDataMeaningList.Count; i++)
            {
                DataMeaning insDataMeaning = SortedDataMeaningList[i];
                string data = insDataMeaning.InitValue;
                byte[] convertedByte = ByteUtils.StringToByteArr(data);
                for (int j = 0; j < convertedByte.Length; j++)
                {
                    byteList.Add(convertedByte[j]);
                }           

            }
            return byteList.ToArray();
        }

        private Dictionary<string, DataMeaning> getDataMapFromByteData(){
            foreach (DataMeaning insDataMeaning in dataMeaningMap.Values)
            {
                byte[] toBeConvertedBytes = getDataMeaningBytes(insDataMeaning.StartBit, insDataMeaning.Length);
                insDataMeaning.BytesToRealValue(toBeConvertedBytes);               
                dataMeaningMap[insDataMeaning.Id] = insDataMeaning;
            }
            return dataMeaningMap;
        }

        private byte[] getDataMeaningBytes(int startIndex, int length)
        {
            List<byte> byteList = new List<byte>();
            for (int i = startIndex; i < startIndex + length; i++)
            {
                byteList.Add(Data[i]);
            }
            return byteList.ToArray();
        }


        /// <summary>
        /// 增加一个DataMeaning字段
        /// </summary>
        /// <param name="dataMeaning"></param>
        public bool Add(DataMeaning dataMeaning){
            if (dataMeaningMap.ContainsKey(dataMeaning.Id))
            {
                return false;
            } else {
                dataMeaningMap.Add(dataMeaning.Id, dataMeaning);
                return true;
            }
        }

        /// <summary>
        /// 删除一个DataMeaning
        /// </summary>
        /// <param name="dataMeaning"></param>
        public bool Delete(DataMeaning dataMeaning) {

            if (dataMeaningMap.ContainsKey(dataMeaning.Id))
            {
                dataMeaningMap.Remove(dataMeaning.Id);
                return true;
            }
            return false;
        }

        public  void Update(DataMeaning dataMeaning)
        {
            if(dataMeaningMap.ContainsKey(dataMeaning.Id)){
                dataMeaningMap.Remove(dataMeaning.Id);
                dataMeaningMap.Add(dataMeaning.Id,dataMeaning);
            }            
        }


        public string Id
        {
            get { return id; }
            set { id = value;
            }
        }


        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        public byte[] Data
        {
            get { return data; }
            set { data = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }


      
    }
}
