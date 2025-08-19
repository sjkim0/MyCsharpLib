using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaApplication1.Service.Interface
{
    public interface IMyMessageBoxService
    {
        void showErrorBox(string caption, string message);
        public Task<bool> showYesNoBox(string caption, string message);
    }
}
