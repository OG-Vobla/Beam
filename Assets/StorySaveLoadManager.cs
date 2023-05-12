using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Ink.Runtime;

public class StorySaveLoadManager { 

     // Set a path to save and restore Story.
    static string savePath = Application.persistentDataPath + "/currentStory";

    // Set a default path (in case the save file does not exist).
    static string defaultPath = Application.persistentDataPath + "/default";

    // Convert a story into a JSON string.
    static public void Serialize(string s, string storyName)
    {
        // Either create or overwrite an existing story file.
        File.WriteAllText(savePath + $"/{storyName}.json", s);
    }

    // Create a story based on saved JSON.
    static public string Deserialize(string storyName)
    {
        // Create a story to return.

        // Create internal JSON string.
        string JSONContents;

        // Does the file exist?
        if (File.Exists(savePath + $"/{storyName}.json"))
        {
            // Read the entire file
            JSONContents = File.ReadAllText(savePath + $"/{storyName}.json");
            // Create a new Story based on JSON
        }
        else
        {
            // File does not exist.
            // Load the default
            JSONContents = File.ReadAllText(defaultPath + $"/{storyName}.json");
            // Create Story based on default
        }

        // Return either default or restored story
        return JSONContents;

    }
}
