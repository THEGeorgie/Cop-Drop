using System.Drawing;
using System.Dynamic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Security.Cryptography.X509Certificates;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using System;
using System.Linq;
using System.Reflection;


namespace CopDrop
{
    public class Button : Texture
    {
        int[] buttonAreaPositionX;
        int[] buttonAreaPositionY;
        public string[] onPress;
        private int change;
        private string[] scriptPaths;
        IlinkButtonScripts[] linkButtonScripts;
        public Text text;
        public Button(IntPtr surface, int textureWidth, int textureHeight, int rotation, int x, int y, string[] onPress, string[] scriptPaths) : base(surface, textureWidth, textureHeight, rotation)
        {
            transform.x = x;
            transform.y = y;
            this.onPress = onPress;
            change = 0;
            if (scriptPaths != null)
            {
                this.scriptPaths = scriptPaths;
                linkButtonScripts = new IlinkButtonScripts[this.scriptPaths.Length];
                for (int i = 0; i < this.scriptPaths.Length; i++)
                {
                    linkButtonScripts[i] = CreateScriptInstance(this, this.scriptPaths[i]);
                    linkButtonScripts[i].start();
                }
            }

            // Sets values to array by taking buttons x cord and ads +1 for every pixle of its width same for y and height.
            // This will then calculate the surface of the button plus the cordinate of every pixle 
            buttonAreaPositionX = new int[transform.w];
            buttonAreaPositionY = new int[transform.h];
            for (int i = 0; i < transform.w; i++)
            {
                buttonAreaPositionX[i] = transform.x + i;
            }
            for (int i = 0; i < transform.h; i++)
            {
                buttonAreaPositionY[i] = transform.y + i;
            }
        }
        public Button(IntPtr surface, int textureWidth, int textureHeight, int rotation, Text text, int textX, int textY, int x, int y, string[] onPress, string[] scriptPaths) : base(surface, textureWidth, textureHeight, rotation)
        {
            this.text = text.deepCopy();
            transform.x = x;
            transform.y = y;
            this.text = text;
            if (scriptPaths != null)
            {
                this.scriptPaths = scriptPaths;
                linkButtonScripts = new IlinkButtonScripts[this.scriptPaths.Length];
                for (int i = 0; i < this.scriptPaths.Length; i++)
                {
                    linkButtonScripts[i] = CreateScriptInstance(this, this.scriptPaths[i]);
                    linkButtonScripts[i].start();
                }
            }

            //makes the position of the text relative to the position of the button and not on the whole window
            if (textX <= transform.w)
            {
                this.text.x = transform.x + textX;
            }
            else
            {
                this.text.x = transform.x + transform.w;
            }
            if (textY <= transform.h)
            {
                this.text.y = transform.y + textY;
            }
            else
            {
                this.text.y = transform.y + transform.h;
            }

            text.update();


            this.onPress = onPress;


        }
        public void showText()
        {
            if (text != null)
            {
                text.render();
            }
        }
        public void update()
        {
            if (linkButtonScripts != null)
            {
                for (int i = 0; i < scriptPaths.Length; i++)
                {
                    linkButtonScripts[i].update();
                }

            }
        }
        public void discardText()
        {
            if (text != null)
            {
                text.dealocate();
            }
        }
        public bool isButtonPressed()
        {
            // Sets values to array by taking buttons x cord and ads +1 for every pixle of its width same for y and height.
            // This will then calculate the surface of the button plus the cordinate of every pixle 
            buttonAreaPositionX = new int[this.transform.w];
            buttonAreaPositionY = new int[this.transform.h];
            for (int i = 0; i < this.transform.w; i++)
            {
                buttonAreaPositionX[i] = this.transform.x + i;
            }
            for (int i = 0; i < this.transform.h; i++)
            {
                buttonAreaPositionY[i] = this.transform.y + i;
            }
            for (int i = 0; i < transform.w; i++)
            {
                for (int k = 0; k < transform.h; k++)
                {
                    //checks if the mouse cords are in the area of the button 
                    if (buttonAreaPositionX[i] == GlobalVariable.Instance.mouseX && buttonAreaPositionY[k] == GlobalVariable.Instance.mouseY)
                    {
                        // GlobalVariable.Instance.mouse.changeCursor(mouseCursors.POINTER);
                        // Console.WriteLine("Mouse in area");
                        if (GlobalVariable.Instance.mouseButtonClick == 1)
                        {
                            GlobalVariable.Instance.mouseButtonClick = 0;
                            // GlobalVariable.Instance.mouse.changeCursor(mouseCursors.CURSOR);

                            return true;
                        }

                    }
                    else
                    {
                        // GlobalVariable.Instance.mouse.changeCursor(mouseCursors.CURSOR);
                    }
                }
            }
            return false;
        }
        private IlinkButtonScripts CreateScriptInstance(Button button, string scriptPath)
        {
            try
            {
                // Read the script code from the file
                string scriptCode = System.IO.File.ReadAllText(scriptPath);

                // Compile the script code into an assembly
                var compilation = CSharpCompilation.Create("ButtonScriptAssembly")
                    .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
                    .AddReferences(AppDomain.CurrentDomain.GetAssemblies().Select(a => MetadataReference.CreateFromFile(a.Location)))
                    .AddSyntaxTrees(SyntaxFactory.ParseSyntaxTree(scriptCode));

                using (var ms = new System.IO.MemoryStream())
                {
                    EmitResult result = compilation.Emit(ms);

                    if (!result.Success)
                    {
                        Console.WriteLine("Script compilation failed:");
                        foreach (var diagnostic in result.Diagnostics)
                        {
                            Console.WriteLine(diagnostic);
                        }
                        return null;
                    }

                    // Load the compiled assembly
                    Assembly assembly = Assembly.Load(ms.ToArray());

                    // Find the class that implements IButtonScriptLink
                    var scriptType = assembly.GetTypes()
                        .FirstOrDefault(t => typeof(IlinkButtonScripts).IsAssignableFrom(t) && t.IsClass);

                    // If a type is found, create an instance of it, passing the Button instance
                    if (scriptType != null)
                    {
                        return Activator.CreateInstance(scriptType, button) as IlinkButtonScripts;
                    }
                    else
                    {
                        Console.WriteLine("No class implementing IlinkButtonScripts found in the compiled assembly.");
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fatal crash: {ex.Message}");
                return null;
            }

        }
    }

    public class commandLine
    {
        private string[] accitonList = { "load_map", "exit", "parse_Jdata" };
        private string[] severd;

        private DAccitons[] dAccitons = new DAccitons[3];
        public commandLine()
        {
            dAccitons[0] = LoadMap;
            dAccitons[1] = Exit;
            dAccitons[2] = ParseJData;
        }
        public void cli(string command)
        {
            severd = command.Split(' ');
            if (severd[0] != "null")
            {
                for (int i = 0; i < accitonList.Length; i++)
                {
                    if (severd[0] == accitonList[i])
                    {
                        Console.WriteLine(i);
                        if (i == 1)
                        {
                            dAccitons[i](severd[0]);
                        }
                        else
                        {
                            dAccitons[i](severd[1]);
                        }
                    }
                }
            }

        }
        DAccitons LoadMap = (string mapLocation) =>
        {
            Map map = new Map(mapLocation);
            try
            {
                MapManager.Instance.LoadMap(map);
            }
            catch (System.Exception)
            {
                Console.WriteLine("Map specifed dose not exist");
                throw;
            }

        };
        DAccitons Exit = (string blank) =>
        {
            GlobalVariable.Instance.exit = true;
        };
        DAccitons ParseJData = (string data) =>
        {
            string[] buffer = data.Split(".json");
            JArray jsonFile;

            if (buffer[0].Contains('('))
            {
                string location = buffer[0].Remove(0, 1);
                location = location.Insert(location.Length, ".json");

                //Console.WriteLine(location);
                string jsonContent = File.ReadAllText(location);

                jsonFile = JArray.Parse(jsonContent);

                if (buffer[1].Contains('<') && buffer[1].Contains('>'))
                {
                    if (buffer[1].Contains('(') && buffer[1].Contains(')'))
                    {
                        buffer[1] = buffer[1].Remove(0, 1);
                        string[] tempBuffer = buffer[1].Split(')');
                        tempBuffer[0] = tempBuffer[0].Remove(0, 2);
                        tempBuffer[1] = tempBuffer[1].Remove(tempBuffer[1].Length - 1);

                        // stores the propety desciption of the object to be monipulated
                        string[] propertyDescription = tempBuffer[0].Split(':');
                        //to what property to change value
                        string[] propertyValue = tempBuffer[1].Split(':');

                        foreach (var type in jsonFile)
                        {
                            if ((int)type["type"] != 0)
                            {
                                foreach (var objects in type["objects"])
                                {
                                    if ((string)objects[propertyDescription[0]] == propertyDescription[1])
                                    {
                                        objects[propertyValue[0]] = propertyValue[1];
                                    }
                                }
                            }

                        }
                        string updatedJsonString = jsonFile.ToString();

                        // Write the updated JSON back to the file
                        File.WriteAllText(location, updatedJsonString);
                    }

                }
            }

        };

        private delegate void DAccitons(string input);
        public static commandLine Instance
        {
            get
            {
                return LazyInstance.Value;
            }
        }

        private static readonly Lazy<commandLine> LazyInstance = new Lazy<commandLine>(() => new commandLine());
    }

}
