using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using UnityEngine;

public class ProjectController : MonoBehaviour
{
    List<GameObject> productObjects = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        

    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 100, 50), "Open"))
        {
            var fullFileName = OpenDialogFile();
            if (fullFileName != null)
            {
                SomeValue.fileName = fullFileName.Replace(".ifc", "");
                GenerateWexBim();
                RenderIFCModel();
            }
        }
    }

    private void RenderIFCModel()
    {
        //generate Model
        BimReader.ReadWexbimFile(SomeValue.WexbimFileName);
        foreach(var p in MyBimGeomorty.products)
        {
            GenerateModel.GenerateProduct(p);
        }

        //generate spatial structure
        var projData = BimReader.GetBimSpatialStructure(SomeValue.BimFileName);
        GenerateModel.GenerateSpatialStructure(projData);
        foreach(var p in SomeValue.products)
        {
            GenerateModel.AppendCollider(p);
        }

        projData.ThisGameObject.transform.rotation = Quaternion.Euler(-90, 0, 0);
    }

    private string OpenDialogFile()
    {
        OpenFileName openFileName = new OpenFileName();
        openFileName.structSize = Marshal.SizeOf(openFileName);
        openFileName.filter = "IFC文件(*.ifc)\0*.ifc";
        openFileName.file = new string(new char[256]);
        openFileName.maxFile = openFileName.file.Length;
        openFileName.fileTitle = new string(new char[64]);
        openFileName.maxFileTitle = openFileName.fileTitle.Length;
        openFileName.initialDir = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);//默认路径
        openFileName.title = "选择所需的ifc文件";
        openFileName.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000008;

        if (OpenFileName.GetSaveFileName(openFileName))
        {
            //UnityEngine.Debug.Log(openFileName.file);
            //UnityEngine.Debug.Log(openFileName.fileTitle);
            return openFileName.file;
        }
        else
        {
            return null;
        }
    }

    private void GenerateWexBim()
    {
        using(Process myProcess = new Process())
        {
            ProcessStartInfo startInfo = new ProcessStartInfo(SomeValue.processName,SomeValue.BimFileName);
            myProcess.StartInfo = startInfo;
            myProcess.StartInfo.UseShellExecute = false;
            myProcess.Start();
            myProcess.WaitForExit();
        }
    }
}
 