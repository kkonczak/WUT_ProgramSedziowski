using System.Diagnostics;

namespace ProgramSedziowski.Modules
{
    public static class ProcessManagmentModule
    {
        public static void KillAllProcesses(Process[] arr)
        {
            foreach (var process in arr)
            {
                if (process != null && !process.HasExited)
                {
                    process.Kill();
                }
            }
        }
    }
}
