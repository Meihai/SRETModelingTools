using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;


namespace ModelingToolsAppWithMVVM.Common
{
    public class DataMeaning:ObservableObject
    {
        private string id;
        private string name;        
        private int startBit;       
        private int length;       
        private string initValue;        
        private string description;        
        private int scale;      
        private string unit;     
        private bool isBit;      
        private int bitOrder;       
        private double dividedBy;
        private double offset;      
        private bool isCrc;
        private ValueTypes valueType;
        //关联变量ID
        private string relatedValueId;

        public string RelatedValueId
        {
            get { return relatedValueId; }
            set { relatedValueId = value;
                  RaisePropertyChanged(() => RelatedValueId);
                }
        }
        //注入方式
        private InjectedModeTypes injectedMode;

        public InjectedModeTypes InjectedMode
        {
            get { return injectedMode; }
            set { injectedMode = value;
                  RaisePropertyChanged(() => InjectedMode);
            }
        }
        //是否用于固定解析
        private bool isUsedForParser;

        public bool IsUsedForParser
        {
            get { return isUsedForParser; }
            set { isUsedForParser = value;
                 RaisePropertyChanged(() => IsUsedForParser);
            }
        }

     
        public DataMeaning() {
            Id = Guid.NewGuid().ToString();
            Name = "";
            startBit = 0;
            length = 1;
            initValue = "";
            description = "";
            scale = 0;
            unit = "";
            isBit = false;
            bitOrder = 0;
            dividedBy = 1;
            offset = 0;
            isCrc = false;
            valueType = ValueTypes.HexString;
            RelatedValueId = "";
            InjectedMode = InjectedModeTypes.InjectedToEachOther;
            IsUsedForParser = false;
        }


        public string Id
        {
            get { return id; }
            set { 
                id = value;
                RaisePropertyChanged(() => Id);
            }
        }

        public string Name
        {
            get { return name; }
            set { 
                name = value;
                RaisePropertyChanged(() => Name);
            }
        }
        public int StartBit
        {
            get { return startBit; }
            set { startBit = value;
                  RaisePropertyChanged(() => StartBit);
            }
        }
        public int Length
        {
            get { return length; }
            set { length = value;
                 RaisePropertyChanged(() => Length);
            }
        }
        public string InitValue
        {
            get { return initValue; }
            set { initValue = value;
                  RaisePropertyChanged(() => InitValue);
            }
        }
        public string Description
        {
            get { return description; }
            set { 
                description = value;
                RaisePropertyChanged(() => Description);
            }
        }
        //保留小数位数
        public int Scale
        {
            get { return scale; }
            set { scale = value;
                  RaisePropertyChanged(() => Scale);
            }
        }
        //单位
        public string Unit
        {
            get { return unit; }
            set { unit = value;
                  RaisePropertyChanged(() => Unit);
            }
        }
        public bool IsBit
        {
            get { return isBit; }
            set { isBit = value;
                  RaisePropertyChanged(() => IsBit);
            }
        }
        public int BitOrder
        {
            get { return bitOrder; }
            set { bitOrder = value;
                  RaisePropertyChanged(() => BitOrder);
            }
        }

        /// <summary>
        /// 除数 一般先除数 尺度变换
        /// </summary>
        public double DividedBy
        {
            get { return dividedBy; }
            set { dividedBy = value;
                  RaisePropertyChanged(() => DividedBy);
            }
        }

        /// <summary>
        /// 位移变换 从byte 数组到 原始值需要先进性尺度变换，再进行位移变换 y=a*(x+b)
        /// y/a-b=x
        /// y 代表byte数组值 ,x 代表原始值
        /// </summary>
        public double Offset
        {
            get { return offset; }
            set { 
                offset = value;
                RaisePropertyChanged(() => Offset);
            }
        }

        public bool IsCrc
        {
            get { return isCrc; }
            set { isCrc = value;
                 RaisePropertyChanged(() => IsCrc);
            }
        }

        public ValueTypes ValueType
        {
            get { return valueType; }
            set { 
                valueType = value;
                RaisePropertyChanged(() => ValueType);
            }
        }

        /// <summary>
        /// 将设置的值转换为byte数组
        /// </summary>
        /// <returns></returns>
        public byte[] RealValueToBytes() {
            if (valueType == ValueTypes.Bool) {
                return BoolToBytes();
            }
            else if (valueType == ValueTypes.Float)
            {
                return DoubleToBytes();
            }
            else if (valueType == ValueTypes.Int)
            {
                return IntegerToBytes();
            }
            else if (valueType == ValueTypes.String)
            {
                return StringToBytes();
            }
            else if (valueType == ValueTypes.HexString)
            {
                return HexStringToBytes();
            }
            return null;

        }

        /// <summary>
        /// 布尔型转字节数组
        /// </summary>
        /// <returns></returns>
        private byte[] BoolToBytes() {
            bool flag = bool.Parse(initValue);
            byte[] convertedBytes = new byte[length];
            InitBytes(convertedBytes);
            if (flag)
            {
                int byteTh = (int)bitOrder/8;
                int bitOrderTmp = (int)bitOrder % 8;               
                convertedBytes[byteTh] = (byte)(convertedBytes[byteTh] | (0x01 << bitOrderTmp));
            }
            return convertedBytes;
        }

        /// <summary>
        /// 整数型数组转byte数组
        /// </summary>
        /// <returns></returns>
        private byte[] IntegerToBytes() {
            int data = int.Parse(initValue);
            //y=a*(x+b)
            data =(int)((data+offset)*dividedBy);
            byte[] convertedBytes = new byte[length];
            InitBytes(convertedBytes);
            //小端格式地址存放方式,高位在前，低位在后
            for (int i = convertedBytes.Length-1; i >= 0; i--)
            {
                convertedBytes[i] = (byte)(data & 0xff);
                data = data >> 8;
            }
            return convertedBytes;            
        }

        /// <summary>
        /// 浮点数数组转byte数组
        /// </summary>
        /// <returns></returns>
        private byte[] DoubleToBytes() {
            double data = double.Parse(initValue);
            //y=a*(x+b)
            data = (data + offset) * dividedBy;//这应该为一个整数
            byte[] convertedBytes = new byte[length];
            InitBytes(convertedBytes);
            long changedData = (long)data;
            //小端格式地址存放方式,高位在前，低位在后
            for (int i = convertedBytes.Length - 1; i >= 0; i--)
            {
                convertedBytes[i] = (byte)(changedData & 0xff);
                changedData = changedData >> 8;
            }
            return convertedBytes;            
        }

        /// <summary>
        /// 字符串转byte数组
        /// </summary>
        /// <returns></returns>
        private byte[] StringToBytes()
        {
            byte[] convertedBytes = new byte[length];
            InitBytes(convertedBytes);
            
            //小端格式地址存放方式,高位在前，低位在后
            for (int i = 0; i < initValue.Length;i++ )
            {
                convertedBytes[i] = (byte)(initValue[i]);
               
            }
            return convertedBytes;            
        }

        private byte[] HexStringToBytes()
        {
            return ByteUtils.hexStringToBytes(initValue);
        }

        private void InitBytes(byte[] byteArr)
        {
            for (int i = 0; i < byteArr.Length; i++)
            {
                byteArr[i] = 0x00;
            }
        }

        
        /// <summary>
        /// 将给定的数组转换为指定类型的数值
        /// </summary>
        public void BytesToRealValue(byte[] toBeConvertedBytes) {
            if (valueType == ValueTypes.Bool)
            {
                BytesToBool(toBeConvertedBytes);               
            }
            else if (valueType == ValueTypes.Float)
            {
                BytesToDouble(toBeConvertedBytes);
            }
            else if (valueType == ValueTypes.Int)
            {
                BytesToInt(toBeConvertedBytes);
            }
            else if (valueType == ValueTypes.String)
            {
                BytesToString(toBeConvertedBytes);
            }
            else if (valueType == ValueTypes.HexString)
            {
                BytesToHexString(toBeConvertedBytes);
            }
        }

        /// <summary>
        /// 将传递进来的字节解析为对应的bool类型变量
        /// </summary>
        /// <param name="toBeConvertedBytes"></param>
        private void BytesToBool(byte[] toBeConvertedBytes)
        {
            if (BytesLengthValidCheck(toBeConvertedBytes)) {
                int byteTh = (int)bitOrder / 8;
                int bitOrderTmp = (int)bitOrder % 8;
                int flag = (toBeConvertedBytes[byteTh]>>bitOrderTmp)& 0x01;
                if (flag != 0)
                {
                    initValue = "true";
                }
                initValue = "false";
            }
        }

        /// <summary>
        /// 将传递进来的字节解析为对应的int类型变量
        /// </summary>
        /// <param name="toBeConvertedBytes"></param>
        private void BytesToInt(byte[] toBeConvertedBytes)
        {
            if (BytesLengthValidCheck(toBeConvertedBytes))
            {
                // y/a-b=x
                int data = 0;
                for (int i = 0; i < toBeConvertedBytes.Length; i++)
                {
                    data = data & toBeConvertedBytes[i];
                    if (i != toBeConvertedBytes.Length - 1)
                    {
                        data = data << 8;
                    }
                }
                int xx =(int) (data * 1.0 / dividedBy - offset);
                initValue = xx.ToString();
            }
        }

        private void BytesToDouble(byte[] toBeConvertedBytes)
        {
            if (BytesLengthValidCheck(toBeConvertedBytes))
            {
                long data = 0;
                for (int i = 0; i < toBeConvertedBytes.Length; i++)
                {
                    data = data & toBeConvertedBytes[i];
                    if (i != toBeConvertedBytes.Length - 1)
                    {
                        data = data << 8;
                    }
                }
                double xx = (data * 1.0 / dividedBy - offset);
                initValue = xx.ToString();
            }
        }

        private void BytesToString(byte[] toBeConvertedBytes)
        {
            if (BytesLengthValidCheck(toBeConvertedBytes))
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < toBeConvertedBytes.Length; i++)
                {
                    char bb= (char)toBeConvertedBytes[i];
                    sb.Append(bb);
                }
              
                initValue = sb.ToString();
            }
        }

        private void BytesToHexString(byte[] toBeConvertedBytes)
        {
            initValue = ByteUtils.ToHexString(toBeConvertedBytes);
        }

        private bool BytesLengthValidCheck(byte[] parserBytes) {
            if (parserBytes.Length != length)
            {
                return false;
            }
            return true;
        }

        private int getWordHigh(byte b) {
            return (b & 0xff) * 256;
        }

        private int getWordLow(byte b)
        {
            return b & 0xff;
        }

    }
}
