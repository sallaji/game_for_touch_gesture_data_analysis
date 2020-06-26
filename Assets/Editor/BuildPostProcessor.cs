// filename BuildPostProcessor.cs
// put it in a folder Assets/Editor/
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;

public class BuildPostProcessor
{

    [PostProcessBuild]
    public static void ChangeXcodePlist(BuildTarget buildTarget, string path)
    {

        if (buildTarget == BuildTarget.iOS)
        {

            string plistPath = path + "/Info.plist";
            PlistDocument plist = new PlistDocument();
            plist.ReadFromFile(plistPath);

            PlistElementDict rootDict = plist.root;
            
            // example of changing a value:
            //rootDict.SetString("CFBundleVersion", "6.6.6");

            // example of adding a boolean key...
            rootDict.SetBoolean("UIFileSharingEnabled", true);

            File.WriteAllText(plistPath, plist.WriteToString());
        }
    }
}