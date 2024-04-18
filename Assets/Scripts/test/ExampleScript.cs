using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleScript : MonoBehaviour
{
    Rect windowRect = new Rect(20, 20, 120, 50);

    void OnGUI()
    {
        // Register the window. Notice the 3rd parameter
        if (GUILayout.Button("aaa"))
        {
            windowRect = GUILayout.Window(0, windowRect, DoMyWindow, "My Window");
        }
        
    }

    // Make the contents of the window
    void DoMyWindow(int windowID)
    {
        // This button is too large to fit the window
        // Normally, the window would have been expanded to fit the button, but due to
        // the GUILayout.Width call above the window will only ever be 100 pixels wide
        if (GUILayout.Button("Please click me a lot"))
        {
            print("Got a click");
        }
    }
}
