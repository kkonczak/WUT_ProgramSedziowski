using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramSedziowski
{
    public static class StreamReaderExtension
    {
        public async static Task<string> ReadLineWithTimeout(this StreamReader reader, int timeout)
        {
            var task = Task.Factory.StartNew(reader.ReadLine);
            var completedTask = await Task.WhenAny(task, Task.Delay(timeout));
            var result = object.ReferenceEquals(task, completedTask) ? task.Result : string.Empty;
            return result;
        }
    }
}
