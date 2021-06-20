using IBA_Task_2.Views;
using System.Collections;
using System.Collections.Generic;

namespace IBA_Task_1.Abstract
{
    public interface ISaver
    {
        void SaveToFile(List<UserViewModel> users, string path);
    }
}
