using System.Collections.ObjectModel;
using System.Diagnostics;

namespace DocConverter.Infrastructure.Utilities
{
    public readonly struct ProcessResult
    {
        public Process Process { get; }
        public int ExitCode { get; }
        public ReadOnlyCollection<string> OutputLines { get; }
        public string Output => string.Join(Environment.NewLine, OutputLines);
        public ReadOnlyCollection<string> ErrorLines { get; }

        public ProcessResult(Process process, int exitCode, ReadOnlyCollection<string> outputLines)
        {
            Process = process;
            ExitCode = exitCode;
            OutputLines = outputLines;
        }
    }

    public readonly struct ProcessInfo
    {
        public Process Process { get; }
        public ProcessStartInfo StartInfo { get; }
        public Task<ProcessResult> Result { get; }

        public int Id => Process.Id;

        public ProcessInfo(Process process, ProcessStartInfo startInfo, Task<ProcessResult> result)
        {
            Process = process;
            StartInfo = startInfo;
            Result = result;
        }
    }

    public static class ProcessRunner
    {
        public static Task<ProcessResult> RunProcessAsync(string executable,
            string arguments,
            string? workingDirectory = null,
            Dictionary<string, string>? environmentVariables = null,
            CancellationToken cancellationToken = default) => CreateProcess(executable, arguments, lowPriority: false, workingDirectory, captureOutput: true, displayWindow: false, environmentVariables, onProcessStartHandler: null, cancellationToken).Result;

        public static ProcessInfo CreateProcess(
            string executable,
            string arguments,
            bool lowPriority = false,
            string? workingDirectory = null,
            bool captureOutput = true,
            bool displayWindow = false,
            Dictionary<string, string>? environmentVariables = null,
            Action<Process>? onProcessStartHandler = null,
            CancellationToken cancellationToken = default) =>
            CreateProcess(
                CreateProcessStartInfo(executable, arguments, workingDirectory, captureOutput, displayWindow, environmentVariables),
                lowPriority: lowPriority,
                onProcessStartHandler: onProcessStartHandler,
                cancellationToken: cancellationToken);

        public static ProcessInfo CreateProcess(
            ProcessStartInfo processStartInfo,
            bool lowPriority = false,
            Action<Process>? onProcessStartHandler = null,
            CancellationToken cancellationToken = default)
        {
            var outputLines = new List<string>();
            var process = new Process();
            var tcs = new TaskCompletionSource<ProcessResult>();

            process.EnableRaisingEvents = true;
            process.StartInfo = processStartInfo;

            process.OutputDataReceived += (s, e) =>
            {
                if (e.Data != null)
                {
                    outputLines.Add(e.Data);
                }
            };


            process.Exited += (s, e) =>
            {
                Task.Run(() =>
                {
                    process.WaitForExit();
                    var result = new ProcessResult(
                        process,
                        process.ExitCode,
                        new ReadOnlyCollection<string>(outputLines));
                    tcs.TrySetResult(result);
                });
            };

            _ = cancellationToken.Register(() =>
            {
                if (tcs.TrySetCanceled())
                {
                    if (!process.HasExited)
                    {
                        try
                        {
                            process.Kill();
                        }
                        catch (InvalidOperationException)
                        {
                        }
                    }
                }
            });

            process.Start();
            onProcessStartHandler?.Invoke(process);

            if (lowPriority)
            {
                process.PriorityClass = ProcessPriorityClass.BelowNormal;
            }

            if (processStartInfo.RedirectStandardOutput)
            {
                process.BeginOutputReadLine();
            }

            if (processStartInfo.RedirectStandardError)
            {
                process.BeginErrorReadLine();
            }

            return new ProcessInfo(process, processStartInfo, tcs.Task);
        }

        public static ProcessStartInfo CreateProcessStartInfo(
            string executable,
            string arguments,
            string? workingDirectory = null,
            bool captureOutput = false,
            bool displayWindow = true,
            Dictionary<string, string>? environmentVariables = null)
        {
            var processStartInfo = new ProcessStartInfo(executable, arguments);

            if (!string.IsNullOrEmpty(workingDirectory))
            {
                processStartInfo.WorkingDirectory = workingDirectory;
            }

            if (environmentVariables != null)
            {
                foreach (var pair in environmentVariables)
                {
                    processStartInfo.EnvironmentVariables[pair.Key] = pair.Value;
                }
            }

            if (captureOutput)
            {
                processStartInfo.UseShellExecute = false;
                processStartInfo.RedirectStandardOutput = true;
                processStartInfo.RedirectStandardError = true;
            }
            else
            {
                processStartInfo.CreateNoWindow = !displayWindow;
                processStartInfo.UseShellExecute = displayWindow;
            }

            return processStartInfo;
        }
    }
}
