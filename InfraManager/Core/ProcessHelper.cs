using InfraManager.Core.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace InfraManager.Core
{
    public static class ProcessHelper
    {
        public static void KillProcessAndChildrens(int pid)
        {
            var processSearcher = new ManagementObjectSearcher("Select * From Win32_Process Where ParentProcessID=" + pid);
            var processCollection = processSearcher.Get();
            //
            try
            {
                Process proc = Process.GetProcessById(pid);
                if (!proc.HasExited)
                    proc.Kill();
            }
            catch (Exception exc)
            {
                // Process already exited.
                Logger.Error(exc);
            }
            //
            if (processCollection != null)
            {
                foreach (ManagementObject mo in processCollection)
                {
                    KillProcessAndChildrens(Convert.ToInt32(mo["ProcessID"])); //kill child processes(also kills childrens of childrens etc.)
                }
            }
        }
    }
}
