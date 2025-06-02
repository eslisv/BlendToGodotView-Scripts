using System;
using System.Diagnostics;
using System.IO;

namespace EVHelpers
{
    public static class WindowsCmdHelper
    {
        public static void RunCommandInBackground(string command, string workingDirectory, out string output, out string error)
        {
            using Process process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    RedirectStandardInput = true,
                    Arguments = "/c " + command,
                    CreateNoWindow = true,
                    WorkingDirectory = workingDirectory
                }
            };
            process.Start();
            output = process.StandardOutput.ReadToEnd();
            error = process.StandardError.ReadToEnd();
        }
    }

}