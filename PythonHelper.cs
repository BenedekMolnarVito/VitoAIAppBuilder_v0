using Python.Runtime;

namespace AIAppBuilder_v0
{
    public static class PythonHelper
    {
        public static void SetPythonEnvironment()
        {
            var pathToVirtualEnv = @"C:\Users\User\source\repos\AIAppBuilder_v0\bin\Debug\net8.0-windows\.venv";

            string path = Environment.GetEnvironmentVariable("PATH")!.TrimEnd(Path.PathSeparator);
            path = string.IsNullOrEmpty(path) ? pathToVirtualEnv : path + Path.PathSeparator + pathToVirtualEnv;
            Environment.SetEnvironmentVariable("PATH", path, EnvironmentVariableTarget.Process);
            Environment.SetEnvironmentVariable("PYTHONHOME", pathToVirtualEnv, EnvironmentVariableTarget.Process);
            Environment.SetEnvironmentVariable("PYTHONPATH",
                $"{pathToVirtualEnv}/Lib/site-packages{Path.PathSeparator}" +
                $"{pathToVirtualEnv}/Lib{Path.PathSeparator}", EnvironmentVariableTarget.Process);

            PythonEngine.PythonPath = PythonEngine.PythonPath + Path.PathSeparator +
                                      Environment.GetEnvironmentVariable("PYTHONPATH", EnvironmentVariableTarget.Process);
            PythonEngine.PythonHome = pathToVirtualEnv;
        }
    }
}
