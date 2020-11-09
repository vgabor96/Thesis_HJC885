using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System;

public class HandleTextFile : MonoBehaviour
{
   const int decimals = 3;
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
                temp += "\t"+'('+(float)Math.Round(item.x,decimals)+','+' '+ (float)Math.Round(item.y, decimals) + ',' + ' ' + (float)Math.Round(item.z, decimals)+')';
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
    public static void WriteSolution(Vector3 bulletdest,Vector3 bulletdestpic, List<Vector3> movement)
    {
        string path = "Assets/Resources/test.txt";

        //Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(path, true);
        //writer.WriteLine("Test");
        string temp = string.Empty;
        foreach (Vector3 item in movement)
        {
            string x = item.x.ToString("0.000");
            x = x.Replace(",", ".");
            string y = item.y.ToString("0.000");
           y= y.Replace(",", ".");
            string z = item.z.ToString("0.000");
          z = z.Replace(",", ".");
            temp += "\t" + '(' + x + ',' + ' ' + y  + ',' + ' ' + z + ')';
        }
        writer.WriteLine(bulletdest+"\t"+bulletdestpic+temp);
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
                    for (int i = 2; i < splittedvectors.Length; i++)
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