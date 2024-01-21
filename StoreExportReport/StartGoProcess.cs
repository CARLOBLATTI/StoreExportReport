using System;
using System.Diagnostics;

class StartGoProcess
{
    public int generateJsonReport()
    {
        // Specify the path to the Go script
        string goScriptPath = Constants.goScript;

        // Create a new process to start the Go script
        Process process = new Process();

        // Set the Go executable as the process start info
        process.StartInfo.FileName = Constants.goInterpreter;

        // Pass the Go script path as an argument to the Go executable
        process.StartInfo.Arguments = "run " + goScriptPath;

        // Start the process
        process.Start();

        // Wait for the process to exit
        process.WaitForExit();

        // Get the exit code of the process
        int exitCode = process.ExitCode;

        // Display the exit code
        Console.WriteLine("Go script exited with code: " + exitCode);

        return exitCode;
    }
}