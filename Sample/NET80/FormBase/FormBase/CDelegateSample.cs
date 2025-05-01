using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormBase
{
    public class CDelegateSample
    {
    }

    public class DelegateTemplate1
    {
        public delegate void ReceiveDataCallback(int clientNumber, byte[] message, int messageSize); // CodeHelper delegate ReceiveDataCallback(): 1. 宣告 delegate, 函數簽名.
        private ReceiveDataCallback? _receive = null; // CodeHelper delegate ReceiveDataCallback(): 2. 建立內部 delegate 欄位.
        public ReceiveDataCallback? OnReceiveData // CodeHelper delegate ReceiveDataCallback(): 3. 建立外部 delegate 屬性.
        {
            get
            {
                return _receive;
            }

            set
            {
                _receive = value;
            }
        }

        public event ReceiveDataCallback? OnReceive2; // CodeHelper delegate ReceiveDataCallback(): 2.a. 改用 event 會比較簡單. CodeHelper Event.

        public void Rasie_OnReceiveData()
        {
            int clientNumber = 9;
            byte[] message = new byte[9];
            int messageSize = 9;
            OnReceiveData?.Invoke(clientNumber, message, messageSize); // CodeHelper delegate ReceiveDataCallback(): 6. Raise delegate.
        }

        public void Rasie_OnReceive2()
        {
            int clientNumber = 9;
            byte[] message = new byte[9];
            int messageSize = 9;
            OnReceive2?.Invoke(clientNumber, message, messageSize); // CodeHelper delegate ReceiveDataCallback(): 6.a Raise event.
        }


    }
    public class DelegateUsage1
    {
        DelegateTemplate1? svr = null;
        public void Start()
        {
            svr = new DelegateTemplate1();
            svr.OnReceiveData += new DelegateTemplate1.ReceiveDataCallback(OnDataReceived); // CodeHelper delegate ReceiveDataCallback(): 4. 銜接屬性到實作函數.
            svr.OnReceive2 += Svr_OnReceive2; // CodeHelper delegate ReceiveDataCallback(): 4.a 銜接 Event 到實作函數, 函數會自動建立.  CodeHelper Event.
        }

        private void Svr_OnReceive2(int clientNumber, byte[] message, int messageSize) // CodeHelper delegate ReceiveDataCallback(): 5.a 實作函數內容, 函數會自動建立.  CodeHelper Event.
        {
            throw new NotImplementedException();
        }

        private void OnDataReceived(int clientNumber, byte[] message, int messageSize) // CodeHelper delegate ReceiveDataCallback(): 5. 實作函數內容, 函數簽名必須一致.
        {
            throw new NotImplementedException();
        }

    }

}
