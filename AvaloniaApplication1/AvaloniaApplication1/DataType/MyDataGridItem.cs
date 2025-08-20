using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaApplication1.DataType
{
    public class MyDataGridItem
    {
        public MyDataGridItem(string column_0, string column_1, string column_2, string column_3, string column_4)
        {
            this.column_0 = column_0;
            this.column_1 = column_1;
            this.column_2 = column_2;
            this.column_3 = column_3;
            this.column_4 = column_4;
        }

        public string column_0 { get; set; }
        public string column_1 { get; set; }
        public string column_2 { get; set; }
        public string column_3 { get; set; }
        public string column_4 { get; set; }

    }
}
