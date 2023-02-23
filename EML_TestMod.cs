using HarmonyLib;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

[assembly: IgnoresAccessChecksTo("Cosmoteer")]

namespace System.Runtime.CompilerServices
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public class IgnoresAccessChecksToAttribute : Attribute
    {
        public IgnoresAccessChecksToAttribute(string assemblyName)
        {
            AssemblyName = assemblyName;
        }

        public string AssemblyName { get; }
    }
}

namespace EML_TestMod
{
    public class Main
    {
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int MessageBox(int hWnd, string text, string caption, uint type);

        private static Harmony? harmony;

        [UnmanagedCallersOnly]
        public static void InitializePatches()
        {
            harmony = new Harmony("com.emltestmod.patch");

            harmony.PatchAll(typeof(Main).Assembly);
			
			MsgBox("This is an EML Test Mod", "Its working");
        }

        public static void MsgBox(string text, string caption)
        {
            MessageBox(0, text, caption, 0);
        }
    }
}