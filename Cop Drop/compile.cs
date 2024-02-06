using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using Microsoft.CSharp;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;

namespace CopDrop
{
    public class ScriptCompiler
    {
        private string code;
        private string dllName;
        private string scriptClassName;
        private string dllPath;
        public string DllPath
        {
            get { return dllPath; }
        }
        public string ScriptClassName
        {
            get { return scriptClassName; }
        }
        public ScriptCompiler(string CSpath)
        {
            if (File.Exists(CSpath))
            {
                code = File.ReadAllText(CSpath);
                dllName = Path.GetFileName(CSpath).Remove(Path.GetFileName(CSpath).Length - 3);
            }
            scriptClassName = GetClassName();
            DLLcompiler();

        }
        static string CompileCode(string code)
        {
            string directoryPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(directoryPath);

            string filePath = Path.Combine(directoryPath, "CustomBehavior.cs");
            File.WriteAllText(filePath, code);

            ExecuteCommand("dotnet", $"new classlib -n CustomBehavior -o {directoryPath}");
            ExecuteCommand("dotnet", $"build {filePath}");

            return Path.Combine(directoryPath, "bin", "Debug", "netstandard2.0", "CustomBehavior.dll");
        }
        // compiles .cs files to dll files
        public void DLLcompiler()
        {
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(code);

            string assemblyName = Path.GetRandomFileName();
            MetadataReference[] references = new MetadataReference[]
            {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(Console).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(IlinkButtonScripts).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(DefaultBehavior).Assembly.Location)
            };

            CSharpCompilation compilation = CSharpCompilation.Create(
                assemblyName,
                syntaxTrees: new[] { syntaxTree },
                references: references,
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            using (MemoryStream ms = new MemoryStream())
            {
                EmitResult result = compilation.Emit(ms);

                if (!result.Success)
                {
                    foreach (Diagnostic diagnostic in result.Diagnostics)
                    {
                        Console.WriteLine($"{diagnostic.Id}: {diagnostic.GetMessage()}");
                    }
                    throw new InvalidOperationException("Failed to compile code.");
                }

                string filePath = Path.Combine(Path.GetTempPath(), $"{assemblyName}.dll");
                File.WriteAllBytes(filePath, ms.ToArray());
                dllPath = filePath;
            }
        }

            private string GetClassName()
            {
                Console.WriteLine(code);
                // Split code by lines
                string[] lines = code.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                // Find the line containing "public class" and extract class name
                string classNameLine = lines.FirstOrDefault(line => line.Trim().StartsWith("public class"));
                if (classNameLine != null)
                {
                    int startIndex = classNameLine.IndexOf("class") + 6;
                    int endIndex = classNameLine.IndexOf(":", startIndex);
                    if (endIndex == -1)
                    {
                        endIndex = classNameLine.IndexOf("{", startIndex);
                    }
                    string className = classNameLine.Substring(startIndex, endIndex - startIndex).Trim();
                    return className;
                }

                throw new InvalidOperationException("Failed to determine class name.");
            }

            static void ExecuteCommand(string command, string arguments)
            {
                Process process = new Process();
                process.StartInfo.FileName = command;
                process.StartInfo.Arguments = arguments;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.Start();

                process.WaitForExit();
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();

                if (!string.IsNullOrEmpty(output))
                {
                    Console.WriteLine(output);
                }

                if (!string.IsNullOrEmpty(error))
                {
                    Console.WriteLine(error);
                }
            }

        }

    }

