using System;
using System.Collections.Generic;
using System.Text;

namespace Zx
{
    public class ProcessOutput 
    {
        public string StdOut { get; }
        public string StdError { get; }
        public int ExitCode { get; }

        public ProcessOutput(string stdOut, string stdError, int exitCode = 0)
        {
            StdOut = stdOut;
            StdError = stdError;
            ExitCode = exitCode;
        }

        public override string ToString()
        {
            return StdOut;
        }
    }
}
