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

        // 만들어진 속성이 바뀌지 않으므로 get만 선언

        public ObservableCollection<MyTreeNode>? SubNodes { get; }  // 자식 노드

        public string Title { get; }
        public object payload { get; }  // 노드가 가진 데이터 인스턴스
        public string nodeKind { get; } // node description 현재는 string이나 이후 enum으로 처리해 알아보기 쉽게할 수 있겠다.
        public MyTreeNode Parent { get; }  // 부모 노드

        public bool isLeaf
        {
            get
            {
                if(SubNodes == null || SubNodes.Count == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public MyTreeNode(string title)
        {
            Title = title;
            SubNodes = new ObservableCollection<MyTreeNode>();
        }

        public MyTreeNode(string title, ObservableCollection<MyTreeNode> subNodes)
        {
            Title = title;
            SubNodes = subNodes;
        }
    }
}
