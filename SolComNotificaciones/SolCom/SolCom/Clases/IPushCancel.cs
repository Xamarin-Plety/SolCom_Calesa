using System;
using System.Collections.Generic;
using System.Text;

namespace SolCom.Clases
{
    public interface IPushCancel
    {
        void CancelPush(int id, int iCount);
    }
}
