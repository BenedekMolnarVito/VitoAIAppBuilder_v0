There are multiple RAG model implementations (experimental phase) in the 
.\AIAppBuilder_v0\PythonScripts directory!

PYTHON setup:
1. Create a virtual environment with python3.7 where the exe file (assembly) is (e.g. C:\Users\User\source\repos\AIAppBuilder_v0\bin\Debug\net8.0-windows)
N.B. Use VS Code: Select interpreter -> python 3.7! ; +Create virtual environment -> Venv

2. pip install all the modules needed for the script
N.B. for docx module, you need pip install python-docx!

3. Install Python.Runtime.AllPlatforms NuGet Package! (it only supports python 3.7!) 

4. Copy your python script(s) AND the python3.dll + python37.dll (must be python 3.7!) files into the folder where the exe (assembly) is

