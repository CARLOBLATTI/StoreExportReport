using System;
using System.Diagnostics;

class StartPythonProcess
{
    public int generateExcellReport()
    {
        ProcessStartInfo startInfo = new ProcessStartInfo();
        startInfo.FileName = Constants.pythonInterpreter;
        startInfo.Arguments = Constants.pythonScript;
        startInfo.RedirectStandardOutput = true;
        startInfo.UseShellExecute = false;

        Process process = new Process();
        process.StartInfo = startInfo;

        process.Start();

        string output = process.StandardOutput.ReadToEnd();

        process.WaitForExit();
        int exitCode = process.ExitCode;

        // Display the exit code
        Console.WriteLine("Python script exited with code: " + exitCode);
        return exitCode;
    }
}