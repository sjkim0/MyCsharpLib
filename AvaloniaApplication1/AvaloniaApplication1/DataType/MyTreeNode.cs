using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaApplication1.DataType
{
    public class MyTreeNode
    {
        public ObservableCollection<MyTreeNode>? SubNodes { get; }
    }
}
