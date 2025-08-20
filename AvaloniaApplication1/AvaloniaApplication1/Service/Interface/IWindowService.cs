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
        void show<VM>() where VM : class;
        void close<VM>() where VM : class;
        void Register<WINDOW_T, VM>() where WINDOW_T : Window;
    }
}
