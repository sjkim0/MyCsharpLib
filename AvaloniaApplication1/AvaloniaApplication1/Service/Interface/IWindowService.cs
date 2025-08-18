using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaApplication1.Service.Interface
{
    public interface IWindowService
    {
        // getrequiredservice<t>에 리턴하기위해 T를 규명한다.
        void show<T>() where T : Window;
        void close<T>() where T : Window;
        void Register<T, VM>() where T : Window;
    }
}
