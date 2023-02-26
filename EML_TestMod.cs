using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Halfling.Input;
using Halfling.Application;
using Halfling;
using Cosmoteer.Game;
using Halfling.Gui;
using Halfling.Geometry;
using static Cosmoteer.Input.Inputs;

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

/* Basic Cosmoteer C# Mod
 * Opens a window when you start a game and have finished loading
 * You can toggle window on/off with N Key
 */

namespace EML_TestMod
{
    public class Main
    {
        //Game and SimRoot contain all the information about the current game and simulation
        public static Cosmoteer.Game.GameRoot? gameRoot;
        public static Cosmoteer.Simulation.SimRoot? simRoot;
        public static Keyboard? keyboard;
        public static WeaponsToolbox? weaponsToolBox;

        public static bool loaded;

        [UnmanagedCallersOnly]
        public static void InitializePatches()
        {
            //This function gets called by the C++ Mod Loader and runs on the same thread as it
            //Only use this for initialization

            //Get Keyboard Object (Need this for key checking)
            keyboard = Halfling.App.Keyboard;

            //Subscribe to event which then gets called from the game thread
            Halfling.App.Director.FrameEnded += Worker;
        }

        public static void Worker(object? sender, EventArgs e)
        {
            //Will be called each Frame

            //Gets current game state
            IAppState? currentState = App.Director.States.OfType<IAppState>().FirstOrDefault();

            if(currentState != null)
            {
                if (currentState.GetType() == typeof(GameRoot))
                {
                    //We are ingame

                    if (!loaded)
                    {
                        loaded = true;

                        gameRoot = (GameRoot)currentState;
                        simRoot = gameRoot.Sim;

                        //Create Window

                        WeaponsToolbox weaponsToolBox = new WeaponsToolbox(gameRoot);
                        weaponsToolBox.SelfActive = false;
                        weaponsToolBox.Rect = new Rect(10f, 70f, 274f, 500f);
                        weaponsToolBox.ResizeController.MinSize = new Vector2(274f, 274f);
                        gameRoot.Gui.FloatingWindows.AddChild(weaponsToolBox);

                        Main.weaponsToolBox = weaponsToolBox;
                        weaponsToolBox.SelfActive = true; //Show Window
                    }
                }
            }

            if(loaded)
            {
                //Check if N Key was pressed
                bool result = keyboard.HotkeyPressed(ViKey.N, true);

                if(result)
                {
                    //Key was pressed

                    //Show/Hide Window
                    Main.weaponsToolBox.SelfActive = !Main.weaponsToolBox.SelfActive;
                }
            }
        }
    }

    //A very basic Window class
    public class WeaponsToolbox : WindowBox
    {
        public WeaponsToolbox(GameRoot game)
        {
            base.TitleText = "Test Window";
            base.BoundsProvider = game.Gui.RootWidget;
            base.Children.LayoutAlgorithm = LayoutAlgorithms.StretchTopToBottom;
            base.Children.BorderPadding = new Borders(10f);
            base.Children.WidgetPadding = new Vector2(10f, 10f);
        }
    }
}