using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace SolCom.Clases
{
    public class cLog
    {
        private string m_exePath = string.Empty;
        public cLog(string logMessage)
        {
            LogWrite(logMessage);
        }
        public void LogWrite(string logMessage)
        {
            m_exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            try
            {
                using (StreamWriter w = File.AppendText(m_exePath + "\\" + "log_" + DateTime.Now.ToLongDateString() + ".txt"))
                {
                    Log(logMessage, w);
                }
            }
            catch (Exception ex)
            {
                string sError = ex.Message;
            }
        }

        public void Log(string logMessage, TextWriter txtWriter)
        {
            try
            {
                txtWriter.Write("\r\nLog Entry : ");
                txtWriter.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                    DateTime.Now.ToLongDateString());
                txtWriter.WriteLine("  :");
                txtWriter.WriteLine("  :{0}", logMessage);
                txtWriter.WriteLine("-------------------------------");
            }
            catch (Exception ex)
            {
            }
        }
    }
}
