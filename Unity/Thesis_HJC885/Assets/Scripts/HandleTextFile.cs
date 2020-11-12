using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System;

public class HandleTextFile : MonoBehaviour
{
   const int decimals = 3;

    static List<Utils.solutions_texthandler> solutions = new List<Utils.solutions_texthandler>();


    //private static Dictionary<Vector3, List<Vector3>> solutions_texthandler = new Dictionary<Vector3, List<Vector3>>();
    [MenuItem("Tools/Write file")]
    public static void WriteString(float time)
    {
        string path = "Assets/Resources/times.txt";

        //Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine(time);
        writer.Close();

        //Re-import the file to update the reference in the editor
        AssetDatabase.ImportAsset(path);
        TextAsset asset = (TextAsset)Resources.Load("times");

        //Print the text from the file
        //Debug.Log(asset.text);
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
    public static void WriteSolution(Vector3 bulletdest,Vector3 bulletdestpic, List<Vector3> movement, string filename)
    {
        string path = "Assets/Resources/" + filename;
        int i = Utils.GetIndexOfStructKey(solutions, bulletdest);
      
        if (i != -1)
        {
            solutions[i].bulletdestpic = bulletdestpic;
            solutions[i].movement = movement;
            string x;
            string y;
            string z;
            string xb;
            string yb;
            string zb;
            string tempbullet;
           StreamWriter writer = new StreamWriter(path, false);

            foreach (Utils.solutions_texthandler item in solutions)
            {

                string temp = string.Empty;
                foreach (Vector3 mov in item.movement)
                {
                    x = mov.x.ToString("0.000");
                    x = x.Replace(",", ".");
                    y = mov.y.ToString("0.000");
                    y = y.Replace(",", ".");
                    z = mov.z.ToString("0.000");
                    z = z.Replace(",", ".");
                    temp += "\t" + '(' + x + ',' + ' ' + y + ',' + ' ' + z + ')';
                }

                xb = item.bulletdest.x.ToString("0.000");
                xb = xb.Replace(",", ".");
                yb = item.bulletdest.y.ToString("0.000");
                yb = yb.Replace(",", ".");
                zb = item.bulletdest.z.ToString("0.000");
                zb = zb.Replace(",", ".");
                tempbullet = '(' + xb + ',' + ' ' + yb + ',' + ' ' + zb + ')';

                writer.WriteLine(tempbullet + "\t" + item.bulletdestpic + temp);


            }
            writer.Close();

        }
        else
        {
            StreamWriter writer = new StreamWriter(path, true);
            string temp = string.Empty;
            foreach (Vector3 item in movement)
            {
                string x = item.x.ToString("0.000");
                x = x.Replace(",", ".");
                string y = item.y.ToString("0.000");
                y = y.Replace(",", ".");
                string z = item.z.ToString("0.000");
                z = z.Replace(",", ".");
                temp += "\t" + '(' + x + ',' + ' ' + y + ',' + ' ' + z + ')';
            }

            string xb = bulletdest.x.ToString("0.000");
            xb = xb.Replace(",", ".");
            string yb = bulletdest.y.ToString("0.000");
            yb = yb.Replace(",", ".");
            string zb = bulletdest.z.ToString("0.000");
            zb = zb.Replace(",", ".");
            string tempbullet = '(' + xb + ',' + ' ' + yb + ',' + ' ' + zb + ')';

            writer.WriteLine(tempbullet + "\t" + bulletdestpic + temp);

            writer.Close();
        }

        //Write some text to the test.txt file

        //writer.WriteLine("Test");


        //Re-import the file to update the reference in the editor
        AssetDatabase.ImportAsset(path);
        TextAsset asset = (TextAsset)Resources.Load("test");

        //Print the text from the file
       // Debug.Log(asset.text);
    }

    [MenuItem("Tools/Read file")]
    public static Dictionary<Vector3,List<Vector3>> ReadSolutions(string filename)
    {
        solutions = new List<Utils.solutions_texthandler>();
        string path = "Assets/Resources/"+filename;

        Dictionary<Vector3, List<Vector3>> retsol = new Dictionary<Vector3, List<Vector3>>();
        //Read the text from directly from the test.txt file
        StreamReader reader = new StreamReader(path);
      
        while (!reader.EndOfStream)
        {
           
            string[] splittedvectors = reader.ReadLine().Split('\t');
            if (splittedvectors.Length>2)
            {

        
                Vector3 key = StringToVector3(splittedvectors[0]);
                Vector3 bulletdest = StringToVector3(splittedvectors[1]);


                    List<Vector3> movement = new List<Vector3>();
                    for (int i = 2; i < splittedvectors.Length; i++)
                    {
                        movement.Add(StringToVector3(splittedvectors[i]));
                    }
                solutions.Add(new Utils.solutions_texthandler(key, bulletdest,movement));
                retsol.Add(key, movement);
         
            }
            else
            {
                break;
            }
        }

       // Debug.Log(reader.ReadToEnd());
        reader.Close();

        return retsol;
    }

    [MenuItem("Tools/Read file")]
    public static List<Vector3> ReadBullets()
    {
      List<Vector3> bullets = new List<Vector3>();
        string path = "Assets/Resources/bullets.txt";


        //Read the text from directly from the test.txt file
        StreamReader reader = new StreamReader(path);

        while (!reader.EndOfStream)
        {

            bullets.Add(StringToVector3(reader.ReadLine()));
        }

        //Debug.Log(reader.ReadToEnd());
        reader.Close();

        return bullets;
    }

    [MenuItem("Tools/Write file")]
    public static void WriteBullet(Vector3 bulletdest)
    {
      

        //Write some text to the test.txt file
      

        if (!ReadBullets().Contains(bulletdest))
        {
            string path = "Assets/Resources/bullets.txt";
            StreamWriter writer = new StreamWriter(path, true);

            string x = bulletdest.x.ToString("0.000");
            x = x.Replace(",", ".");
            string y = bulletdest.y.ToString("0.000");
            y = y.Replace(",", ".");
            string z = bulletdest.z.ToString("0.000");
            z = z.Replace(",", ".");
            string temp = '(' + x + ',' + ' ' + y + ',' + ' ' + z + ')';

            writer.WriteLine(temp);
            writer.Close();

            Debug.Log(bulletdest);

            AssetDatabase.ImportAsset(path);
            TextAsset asset = (TextAsset)Resources.Load("bullet");
        }

     
      

        //Re-import the file to update the reference in the editor
    

        //Print the text from the file
        // Debug.Log(asset.text);
    }

    [MenuItem("Tools/Write file")]
    public static void WriteBullets(List<Vector3> bulletdests)
    {


        //Write some text to the test.txt file
        string path = "Assets/Resources/bullets.txt";
        StreamWriter writer = new StreamWriter(path, true);
        foreach (Vector3 bulletdest in bulletdests)
        {

                string x = bulletdest.x.ToString("0.000");
                x = x.Replace(",", ".");
                string y = bulletdest.y.ToString("0.000");
                y = y.Replace(",", ".");
                string z = bulletdest.z.ToString("0.000");
                z = z.Replace(",", ".");
                string temp = '(' + x + ',' + ' ' + y + ',' + ' ' + z + ')';

                writer.WriteLine(temp);
                           
        }

        AssetDatabase.ImportAsset(path);
        TextAsset asset = (TextAsset)Resources.Load("bullets");
        writer.Close();

        //Re-import the file to update the reference in the editor

        //Print the text from the file
        // Debug.Log(asset.text);
    }

    [MenuItem("Tools/Write file")]
    public static void Write_RandBullets(List<Vector3> bulletdests)
    {


        //Write some text to the test.txt file
        string path = "Assets/Resources/rand_bullets.txt";
        StreamWriter writer = new StreamWriter(path, true);
        foreach (Vector3 bulletdest in bulletdests)
        {

            string x = bulletdest.x.ToString("0.000");
            x = x.Replace(",", ".");
            string y = bulletdest.y.ToString("0.000");
            y = y.Replace(",", ".");
            string z = bulletdest.z.ToString("0.000");
            z = z.Replace(",", ".");
            string temp = '(' + x + ',' + ' ' + y + ',' + ' ' + z + ')';

            writer.WriteLine(temp);

        }

        AssetDatabase.ImportAsset(path);
        TextAsset asset = (TextAsset)Resources.Load("Write_RandBullets");
        writer.Close();

        //Re-import the file to update the reference in the editor

        //Print the text from the file
        // Debug.Log(asset.text);
    }

    [MenuItem("Tools/Write file")]
    public static void WriteFitness(double fitness)
    {


        //Write some text to the test.txt file


            string path = "Assets/Resources/fitnesses.txt";
            StreamWriter writer = new StreamWriter(path, true);

            writer.WriteLine(fitness);
            writer.Close();

            AssetDatabase.ImportAsset(path);
            TextAsset asset = (TextAsset)Resources.Load("fitness");
 




        //Re-import the file to update the reference in the editor


        //Print the text from the file
        // Debug.Log(asset.text);
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