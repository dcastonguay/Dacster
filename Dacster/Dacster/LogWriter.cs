using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dacster {
    public class LogWriter {
        private string m_exePath = @"C:\Users\dcastonguay\";
        public LogWriter(string logMessage) {
            LogWrite(logMessage);
        }
        public void LogWrite(string logMessage) {
            try {
                using (StreamWriter w = File.AppendText(m_exePath + "log.txt")) {
                    Log(logMessage, w);
                }
            }
            catch (Exception ex) {
            }
        }

        public void Log(string logMessage, TextWriter txtWriter) {
            try {
                txtWriter.WriteLine("{0} {1}: {2}", DateTime.Now.ToShortTimeString(),
                    DateTime.Now.ToShortDateString(), logMessage);
            }
            catch (Exception ex) {
            }
        }
    }
}
