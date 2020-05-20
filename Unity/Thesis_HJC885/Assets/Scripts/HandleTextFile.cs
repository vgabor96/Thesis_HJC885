using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System.Linq;

public class HandleTextFile : MonoBehaviour
{
    [MenuItem("Tools/Write file")]
    public static void WriteString()
    {
        string path = "Assets/Resources/test.txt";

        //Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine("Test");
        writer.Close();

        //Re-import the file to update the reference in the editor
        AssetDatabase.ImportAsset(path);
        TextAsset asset = (TextAsset)Resources.Load("test");

        //Print the text from the file
        Debug.Log(asset.text);
    }

    [MenuItem("Tools/Read file")]
    public static void ReadString()
    {
        string path = "Assets/Resources/test.txt";

        //Read the text from directly from the test.txt file
        StreamReader reader = new StreamReader(path);
        Debug.Log(reader.ReadToEnd());
        reader.Close();
    }

    [MenuItem("Tools/Write file")]
    public static void WriteSolutions(Dictionary<Vector3, List<Vector3>> solution)
    {
        string path = "Assets/Resources/test.txt";

        //Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(path, true);
        //writer.WriteLine("Test");
        string temp = string.Empty;
        for (int i = 0; i < solution.Keys.Count; i++)
        {
            temp = string.Empty;
            foreach (Vector3 item in solution.ElementAt(i).Value)
            {
                temp += "\t"+item ;
            }
            writer.WriteLine(solution.ElementAt(i).Key + temp);
        }
        writer.Close();

        //Re-import the file to update the reference in the editor
        AssetDatabase.ImportAsset(path);
        TextAsset asset = (TextAsset)Resources.Load("test");

        //Print the text from the file
        Debug.Log(asset.text);
    }

    [MenuItem("Tools/Write file")]
    public static void WriteSolution(Vector3 bulletdest, List<Vector3> movement)
    {
        string path = "Assets/Resources/test.txt";

        //Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(path, true);
        //writer.WriteLine("Test");
        string temp = string.Empty;
        foreach (Vector3 item in movement)
        {
            temp += "\t"+item;
        }
        writer.WriteLine(bulletdest+temp);
        writer.Close();

        //Re-import the file to update the reference in the editor
        AssetDatabase.ImportAsset(path);
        TextAsset asset = (TextAsset)Resources.Load("test");

        //Print the text from the file
       // Debug.Log(asset.text);
    }

    [MenuItem("Tools/Read file")]
    public static Dictionary<Vector3,List<Vector3>> ReadSolutions()
    {
        Dictionary<Vector3, List<Vector3>> solutions = new Dictionary<Vector3, List<Vector3>>();
        string path = "Assets/Resources/test.txt";


        //Read the text from directly from the test.txt file
        StreamReader reader = new StreamReader(path);
      
        while (!reader.EndOfStream)
        {
           
            string[] splittedvectors = reader.ReadLine().Split('\t');
            if (splittedvectors.Length>2)
            {

        
            Vector3 key = StringToVector3(splittedvectors[0]);
            List<Vector3> movement = new List<Vector3>();
            for (int i = 1; i < splittedvectors.Length; i++)
            {
                movement.Add(StringToVector3(splittedvectors[i]));
            }
            solutions.Add(key, movement);
            }
            else
            {
                break;
            }
        }

        Debug.Log(reader.ReadToEnd());
        reader.Close();

        return solutions;
    }

    private static Vector3 StringToVector3(string sVector)
    {
        // Remove the parentheses
        if (sVector.StartsWith("(") && sVector.EndsWith(")"))
        {
            sVector = sVector.Substring(1, sVector.Length - 2);
        }

        // split the items
        string[] sArray = sVector.Split(',');

        for (int i = 0; i < sArray.Length; i++)
        {
            sArray[i] = sArray[i].Replace('.', ',');
        }
        
          
        
        // store as a Vector3
        Vector3 result = new Vector3(
            float.Parse(sArray[0]),
            float.Parse(sArray[1].Substring(1)),
            float.Parse(sArray[2].Substring(1)));

        return result;
    }

}