using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelingToolsAppWithMVVM.Common
{
    /// <summary>
    /// 接口值类型,用于从报文中解析出具体的值
    /// </summary>
    public class InterfaceValue
    {
        private string id;
        private string name;
        private ValueTypes type;
        private int startBit;
        private int length;
        private int scale;
        private bool isBit;
        private int bitOrder;
        private double dividedBy;       
        private int limit;
        private string expectValue;
        private IOTypes ioType;

      
        public InterfaceValue() {
            id = Guid.NewGuid().ToString();
        }

        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public ValueTypes Type
        {
            get { return type; }
            set { type = value; }
        }

        public int StartBit
        {
            get { return startBit; }
            set { startBit = value; }
        }

        public int Length
        {
            get { return length; }
            set { length = value; }
        }

        public int Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        public bool IsBit
        {
            get { return isBit; }
            set { isBit = value; }
        }

        public int BitOrder
        {
            get { return bitOrder; }
            set { bitOrder = value; }
        }


        public double DividedBy
        {
            get { return dividedBy; }
            set { dividedBy = value; }
        }

        public string ExpectValue
        {
            get { return expectValue; }
            set { expectValue = value; }
        }

        public IOTypes IoType
        {
            get { return ioType; }
            set { ioType = value; }
        }

    }

}
