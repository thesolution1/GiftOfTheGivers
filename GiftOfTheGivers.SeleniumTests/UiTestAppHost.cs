using System.Diagnostics;

namespace GiftOfTheGivers.SeleniumTests;

public sealed class UiTestAppHost : IDisposable
{
    private readonly Process _process;
    private readonly string _projectPath;

    public UiTestAppHost()
    {
        _projectPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "GiftOfTheGivers.csproj"));
        _process = StartApp();
        WaitUntilReady();
    }

    public string BaseUrl => "http://localhost:5005";

    public void Dispose()
    {
        try
        {
            if (!_process.HasExited)
            {
                _process.Kill(entireProcessTree: true);
                _process.WaitForExit(5000);
            }
        }
        finally
        {
            _process.Dispose();
        }
    }

    private Process StartApp()
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = $"run --no-build --project \"{_projectPath}\" --launch-profile http",
            WorkingDirectory = Path.GetDirectoryName(_projectPath)!,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
        };

        startInfo.Environment["ASPNETCORE_ENVIRONMENT"] = "Development";

        var process = new Process { StartInfo = startInfo };
        process.Start();
        return process;
    }

    private void WaitUntilReady()
    {
        var start = DateTime.UtcNow;
        while (DateTime.UtcNow - start < TimeSpan.FromSeconds(60))
        {
            if (_process.HasExited)
            {
                var output = _process.StandardOutput.ReadToEnd();
                var error = _process.StandardError.ReadToEnd();
                throw new InvalidOperationException($"The app exited before Selenium tests could start.\nSTDOUT:\n{output}\nSTDERR:\n{error}");
            }

            try
            {
                using var client = new HttpClient { Timeout = TimeSpan.FromSeconds(2) };
                var response = client.GetAsync(BaseUrl).GetAwaiter().GetResult();
                if (response.IsSuccessStatusCode)
                {
                    return;
                }
            }
            catch
            {
                // retry until app is ready
            }

            Thread.Sleep(500);
        }

        throw new TimeoutException("The application did not become ready on http://localhost:5005 within 60 seconds.");
    }
}
