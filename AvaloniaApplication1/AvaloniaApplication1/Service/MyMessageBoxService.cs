using Avalonia.Controls;
using AvaloniaApplication1.Service.Interface;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaApplication1.Service
{
    public class MyMessageBoxService : IMyMessageBoxService
    {
        public async Task<bool> showYesNoBox(string caption, string message)
        {
            var box = MessageBoxManager.GetMessageBoxStandard(caption,
                                                              message,
                                                              ButtonEnum.YesNo);

            ButtonResult button_result = await box.ShowAsync();
            if (button_result != ButtonResult.Yes)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async void showErrorBox(string caption, string message)
        {
            var box = MessageBoxManager.GetMessageBoxStandard(caption,
                                                              message,
                                                              ButtonEnum.Ok,
                                                              Icon.None,
                                                              WindowStartupLocation.CenterScreen);

            ButtonResult button_result = await box.ShowAsync();
        }
    }
}
