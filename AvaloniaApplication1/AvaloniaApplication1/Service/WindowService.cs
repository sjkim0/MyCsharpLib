using Avalonia.Controls;
using AvaloniaApplication1.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaApplication1.Service
{
    public class WindowService : IWindowService
    {
        public void close<T>() where T : Window
        {
            throw new NotImplementedException();
        }

        public void show<T>() where T : Window
        {
            throw new NotImplementedException();
        }
    }
}
