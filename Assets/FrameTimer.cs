using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Serialization;
using Debug = UnityEngine.Debug;

public class FrameTimer : MonoBehaviour
{
    public string Folder = "results";
    public string outputFile = "results.csv";
    public int FrameCountCap = 10800;
    
    private StreamWriter _resultsFile;
    private Stopwatch _stopwatch;
    private int _frameCount;
    
    // Start is called before the first frame update
    private void Start()
    {
        if (!Directory.Exists(Folder))
            Directory.CreateDirectory(Folder);
        _resultsFile = new StreamWriter(Folder + outputFile, false);
        _resultsFile.WriteLine("Frame Count,Frame Time (ns)");
        _stopwatch = Stopwatch.StartNew();
        Debug.Log(Stopwatch.Frequency);
    }

    // Update is called once per frame
    private void Update()
    {
        _stopwatch.Stop();
        var time = _stopwatch.ElapsedTicks;
        
        if (_frameCount == FrameCountCap)
        {
            _resultsFile.Flush();
            _resultsFile.Close();
            
            Debug.Log("Benchmarking done, you may exit the game!");
            Application.Quit();
        }
        else if (_frameCount < FrameCountCap)
        {
            _resultsFile.WriteLine($"{_frameCount},{time}");
        }
        _frameCount++;
        
        _stopwatch.Restart();
    }
}
