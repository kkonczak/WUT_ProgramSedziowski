using System.Diagnostics;

namespace ProgramSedziowski.Modules
{
    public static class ProcessManagmentModule
    {
        public static void KillAllProcesses(Process[] arr)
        {
            foreach (var process in arr)
            {
                try
                {
                    if (process != null && !process.HasExited)
                    {
                        try
                        {
                            process.Kill();
                        }
                        catch { }
                    }
                }
                catch { }
            }
        }
    }
}
