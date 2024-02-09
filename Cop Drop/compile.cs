using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace CopDrop
{
    public class ScriptCompiler
    {
        private string code;
        private string codeScriptLink;
        private string dllName;
        private string scriptClassName;
        private string dllPath;
        //The cs file with the interface
        private string linkScriptPath;
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
            linkScriptPath = "D:/Projects/Projects/Cop-Drop/Cop Drop/linkScripts.cs";
            if (File.Exists(CSpath) && File.Exists(linkScriptPath))
            {

                code = File.ReadAllText(CSpath);
                codeScriptLink = File.ReadAllText(linkScriptPath);
                dllName = Path.GetFileName(CSpath).Remove(Path.GetFileName(CSpath).Length - 3);
                dllPath = "scripts/dlls/" + dllName + ".dll";
                if (File.Exists(dllPath))
                {
                    Console.WriteLine("dll file alredy exists");
                }
                else
                {
                    DLLcompiler();
                }
                scriptClassName = GetClassName();

            }
            else
            {
                Console.WriteLine(CSpath);
                Console.WriteLine("Paths are incorect");
            }

        }
        // compiles .cs files to dll files
        public void DLLcompiler()
        {
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(code);
            SyntaxTree syntaxTreeInterfaceLinkScript = CSharpSyntaxTree.ParseText(codeScriptLink);


            // Define compilation options
            CSharpCompilationOptions compilationOptions = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);

            // Define the compilation

            CSharpCompilation compilation = CSharpCompilation.Create(
                Path.GetFileNameWithoutExtension(dllPath),
                syntaxTrees: new[] { syntaxTree },
                references: GetMetadataReferences(),
                options: compilationOptions);

            // Perform the compilation
            using (var ms = new MemoryStream())
            {
                var result = compilation.Emit(ms);

                if (!result.Success)
                {
                    Console.WriteLine("Compilation failed:");
                    foreach (var diagnostic in result.Diagnostics)
                    {
                        Console.WriteLine(diagnostic);
                    }
                }
                else
                {
                    // Write the compiled bytes to the output DLL file
                    File.WriteAllBytes(dllPath, ms.ToArray());
                    Console.WriteLine($"Compilation successful. Output DLL: {dllPath}");
                }
            }
        }
        MetadataReference[] GetMetadataReferences()
        {
            var references = new List<MetadataReference>();

            // Add references to required assemblies
            references.Add(MetadataReference.CreateFromFile(typeof(object).Assembly.Location));
            references.Add(MetadataReference.CreateFromFile(typeof(Console).Assembly.Location));
            references.Add(MetadataReference.CreateFromFile("Cop Drop.dll"));
            return references.ToArray();
        }

        private string GetClassName()
        {
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
                Console.WriteLine(className);
                return className;
            }

            throw new InvalidOperationException("Failed to determine class name.");
        }

    }

}

