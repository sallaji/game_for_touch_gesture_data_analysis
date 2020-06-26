using System.IO;
using UnityEngine;
using System.Text;

public class JsonReadWriteController
{
    static string path = Application.persistentDataPath;

    internal static bool FileExists(string locationPath)
    {
        string tempPath = path + "/" + locationPath;
        return File.Exists(tempPath);
    }

    /// <summary>
    /// Reads a file out of the application datapath.
    /// This is a readable/writable folder for all plattforms
    /// </summary>
    /// <returns>The json as external resource.</returns>
    /// <param name="path">Path.</param>
    public static string LoadJsonAsExternalResource(string locationPath)
    {
        var fileTemp = path + "/" + locationPath;
        if (!FileExists(locationPath))
        {
            return null;
        }

        StreamReader streamReader = new StreamReader(fileTemp);
        string response = "";
        while (!streamReader.EndOfStream)
        {
            response += streamReader.ReadLine();
        }
        return response;
    }

    public static void WriteJsonToExternalResource(string locationPath, string content)
    {
        string fileTemp = path + "/" + locationPath;
        Directory.CreateDirectory(path);
        FileStream fs = File.Create(fileTemp);
        byte[] contentBytes = new UTF8Encoding(true).GetBytes(content);
        fs.Write(contentBytes, 0, contentBytes.Length);

    }
}
