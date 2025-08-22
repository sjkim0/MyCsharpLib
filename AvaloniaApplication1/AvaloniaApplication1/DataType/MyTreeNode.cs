using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AvaloniaApplication1.DataType
{
    public class MyTreeNode
    {
        /* https://docs.avaloniaui.net/docs/reference/controls/treeview-1 */

        public ObservableCollection<MyTreeNode>? SubNodes { get; }

        public string Title { get; }

        public MyTreeNode(string title)
        {
            Title = title;
        }

        public MyTreeNode(string title, ObservableCollection<MyTreeNode> subNodes)
        {
            Title = title;
            SubNodes = subNodes;
        }
    }
}
