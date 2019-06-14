﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public class ResultsConcatter : MonoBehaviour
{
    public string FolderName = "il2cpp-results";
    public string OutputFile = "output.csv";

    // Start is called before the first frame update

    void Start()
    {
        File.Delete(Path.Combine(FolderName, OutputFile));
        
        var files = Directory.GetFiles(FolderName);
        var concattedFiles = new List<(string key, List<string> values)>();
        concattedFiles.Add(("Frame No.", new List<string>()));

        foreach (var file in files)
        {
            var fileSplit = file.Split('.');
            Debug.Log(fileSplit[0]);
            var colName = fileSplit[0].Replace(FolderName + @"\", "");
            Debug.Log(colName);
            
            concattedFiles[0].values.Add(colName);
            var readings = ParseCsv(file);

            for (var i = 0; i < readings.Count; i++)
            {
                if (i + 1 >= concattedFiles.Count)
                {
                    concattedFiles.Add((readings[i].measurement, new List<string>(){readings[i].frameTime}));
                }
                else
                {
                    concattedFiles[i+1].values.Add(readings[i].frameTime);
                }
            }
        }

        OutputCsv(concattedFiles);
        Debug.Log("Nice. The files were concatted.");
    }

    private List<(string measurement, string frameTime)> ParseCsv(string file)
    {
        var results = new List<(string, string)>();
        using (var parser = new StreamReader(file))
        {
            //Throw away the first line
            parser.ReadLine();
            //And read the rest of the lines into the dictionary
            while (!parser.EndOfStream)
            {
                var line = parser.ReadLine();
                var split = line.Split(',');
                var rowName = split[0];
                var time = int.Parse(split[1]);
                var measurement = (1f * Stopwatch.Frequency) / time;
                
                results.Add((rowName, measurement.ToString()));
            }
        }

        return results;
    }

    private void OutputCsv(List<(string key, List<string> values)> readings)
    {
        using (var file = new StreamWriter(Path.Combine(FolderName, OutputFile)))
        {
            foreach (var reading in readings)
            {
                var vals = string.Join(",", reading.values);
                var o = $"{reading.key},{vals}";
                file.WriteLine(o);
            }
        }
    }
}
