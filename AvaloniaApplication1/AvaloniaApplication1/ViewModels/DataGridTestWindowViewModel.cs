using Avalonia.Remote.Protocol;
using AvaloniaApplication1.DataType;
using AvaloniaApplication1.Service.Interface;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaApplication1.ViewModels
{
    public partial class DataGridTestWindowViewModel : ViewModelBase
    {
        private readonly IMyMessageBoxService? _myMessageboxService;

        [ObservableProperty]
        ObservableCollection<MyDataGridItem> my_item;

        public DataGridTestWindowViewModel()  // for design
        {
        }

        public DataGridTestWindowViewModel(IMyMessageBoxService myMessageboxService)  // for design
        {
            _myMessageboxService = myMessageboxService;
            // messenger 등록
            WeakReferenceMessenger.Default.Register<MyMessengerType, string>(this, typeof(DataGridTestWindowViewModel).ToString(), ReceiveMessage);

            My_item = new ObservableCollection<MyDataGridItem>();

            foreach (int data in Enumerable.Range(0, 10))
            {
                My_item.Add(new MyDataGridItem($"column{data}", $"column{data}", $"column{data}", $"column{data}", $"column{data}"));
            }
        }

        private void ReceiveMessage(object recipient, MyMessengerType message)
        {
            throw new NotImplementedException();
        }
    }
}
