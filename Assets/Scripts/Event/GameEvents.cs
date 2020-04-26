using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class GameEvents : MonoBehaviour
{
    public static GameEvents current;

    [HideInInspector]
    public GameObject selectedProduct;
    private GameObject lastSelect = null;

    private void Awake()
    {
        current = this;
    }

    public event Func<int,GameObject> OnProductsSelected;
    public void ProductSelected(int id)
    {
        if (OnProductsSelected != null)
        {
            var delArray = OnProductsSelected.GetInvocationList();
            selectedProduct = null;
            foreach(var del in delArray)
            {
                selectedProduct = del.DynamicInvoke(id) as GameObject;
                if (selectedProduct != null)
                    break;
            }
        }
    }


    public UITree uiTree;
    private string ifcFileName;
    private string wexbimFileName;

    public void OpenButtonClicked()
    {
        var fullFileName = OpenDialogFile();
        if (fullFileName != null)
        {
            SomeValue.fileName = fullFileName.Replace(".ifc", "");
            wexbimFileName = SomeValue.WexbimFileName;
            ifcFileName = SomeValue.BimFileName;
            GenerateWexBim(ifcFileName);
            RenderIFCModel();
            GenerateUiTree(SomeValue.project);
            Camera.main.GetComponent<CameraFollow>().target.position = new Vector3(0, 0.5f, 0);
            Camera.main.transform.position = default;
            Camera.main.transform.rotation = default;
        }
    }

    private void RenderIFCModel()
    {
        var projData = BimReader.GetBimSpatialStructure(ifcFileName);
        BimReader.ReadWexbimFile(wexbimFileName);

        var allPD = new List<IProductData>();
        allPD.AddRange(SomeValue.Elements);
        allPD.AddRange(SomeValue.spatialStructures);

        foreach (var p in MyBimGeomorty.products)
        {
            var pd = GenerateModel.GenerateProduct(p, allPD);
            if (pd == null)
                print(p.entityLabel);
            GenerateModel.AppendCollider(pd);
        }
        projData.ThisGameObject.transform.rotation = Quaternion.Euler(-90, 0, 0);
    }

    private string OpenDialogFile()
    {
        OpenFileName openFileName = new OpenFileName();
        openFileName.structSize = System.Runtime.InteropServices.Marshal.SizeOf(openFileName);
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
            return openFileName.file;
        }
        else
        {
            return null;
        }
    }

    private void GenerateWexBim(string ifcFileName)
    {
        using (Process myProcess = new Process())
        {
            ProcessStartInfo startInfo = new ProcessStartInfo(SomeValue.processName, ifcFileName);
            myProcess.StartInfo = startInfo;
            myProcess.StartInfo.UseShellExecute = false;
            myProcess.Start();
            myProcess.WaitForExit();
        }
    }

    private void GenerateUiTree(IProjectData projData)
    {
        var uiTreeData = new UITreeData(projData.Name, projData.EntityLabel);
        foreach (var prodData in projData.RelatedObjects)
        {
            Helper(prodData, uiTreeData);
        }
        uiTree.Inject(uiTreeData);
    }

    private void Helper(IObjectData objData, UITreeData parUiTree)
    {
        var curUiTree = new UITreeData(objData.Name, objData.EntityLabel);
        if (objData is IElementData)
        {
            IElementData eleData = objData as IElementData;
            var typeUiTreeData = parUiTree.FindChildren(eleData.TypeName);
            if (typeUiTreeData == default)
            {
                typeUiTreeData = new UITreeData(eleData.TypeName, eleData.ProductGeoData.typeId);
                parUiTree.AddChild(typeUiTreeData);
            }
            var curEleUiTreeData = new UITreeData(objData.Name, objData.EntityLabel);
            typeUiTreeData.AddChild(curEleUiTreeData);
            foreach (var decEle in objData.RelatedObjects)
            {
                curEleUiTreeData.AddChild(new UITreeData(decEle.Name, decEle.EntityLabel));
            }
            return;
        }
        parUiTree.AddChild(curUiTree);
        foreach (var p in objData.RelatedObjects)
        {
            Helper(p, curUiTree);
        }
    }

    public void ClearButtonClicked()
    {
        System.IO.File.Delete(wexbimFileName);
        SomeValue.fileName = default;
        Destroy(SomeValue.project.ThisGameObject);

        SomeValue.project = default;
        SomeValue.Elements.Clear();
        SomeValue.spatialStructures.Clear();
        MyBimGeomorty.regions.Clear();
        MyBimGeomorty.colors.Clear();
        MyBimGeomorty.products.Clear();

        uiTree.Clear();
        ClearGeneralPropertiesPannel();
        ClearPropertiesPannel();

        Transform target = Camera.main.GetComponent<CameraFollow>().target;
        target.position = Vector3.zero;
        target.rotation = new Quaternion();
        Camera.main.transform.position = new Vector3(0, 0, -1);
        Camera.main.transform.rotation = new Quaternion();
    }

    private void SetTextFormat(Text text, Text formatedText)
    {
        text.color = formatedText.color;
        text.alignment = formatedText.alignment;
        text.font = formatedText.font;
        text.fontSize = formatedText.fontSize;
    }

    public GameObject propertiesPannel;
    private List<GameObject> propertiesText = new List<GameObject>();

    private void ChangePropertiesPannel()
    {
        ClearPropertiesPannel();
        var properties = selectedProduct.GetComponent<ObjectData>().GetProperties();
        var content = propertiesPannel.GetComponentInChildren<GridLayoutGroup>();
        foreach(var prop in properties)
        {
            var go = new GameObject();
            go.transform.SetParent(content.transform);
            var text = go.AddComponent<Text>();
            text.text = " ▲" + prop.Key;
            SetTextFormat(text, content.transform.GetChild(0).GetComponent<Text>());
            text.fontStyle = FontStyle.Bold;
            propertiesText.Add(go);
            foreach(var propData in prop.Value)
            {
                var dataKeyGo = new GameObject();
                dataKeyGo.transform.SetParent(content.transform);
                var dataKeyText = dataKeyGo.AddComponent<Text>();
                dataKeyText.text = "     " + propData.Key + ":";
                SetTextFormat(dataKeyText, content.transform.GetChild(0).GetComponent<Text>());
                propertiesText.Add(dataKeyGo);

                var dataValueGo = new GameObject();
                dataValueGo.transform.SetParent(content.transform);
                var dataValueText = dataValueGo.AddComponent<Text>();
                dataValueText.text = "         --" + propData.Value;
                SetTextFormat(dataValueText, content.transform.GetChild(0).GetComponent<Text>());
                propertiesText.Add(dataValueGo);
            }
        }
    }

    private void ClearPropertiesPannel()
    {
        foreach(var go in propertiesText)
        {
            go.SetActive(false);
            Destroy(go, 0.1f);
        }
        propertiesText.Clear();
    }

    public GameObject generalProertiesPannel;
    private List<GameObject> generalPropTexts = new List<GameObject>();
    
    private void ChangeGeneralPropertiesPannel()
    {
        ClearGeneralPropertiesPannel();
        var generalProperties = selectedProduct.GetComponent<ObjectData>().GetGeneralProperties();
        var content = generalProertiesPannel.GetComponentInChildren<GridLayoutGroup>();
        foreach (var genProp in generalProperties)
        {
            var go = new GameObject();
            go.transform.SetParent(content.transform);
            var text = go.AddComponent<Text>();
            text.text = " " + genProp.Key.PadRight(30, '·') + genProp.Value;
            SetTextFormat(text, content.transform.GetChild(0).GetComponent<Text>());
            generalPropTexts.Add(go);
        }
    }

    private void ClearGeneralPropertiesPannel()
    {
        foreach (var go in generalPropTexts)
        {
            go.SetActive(false);
            Destroy(go, 0.1f);
        }
        generalPropTexts.Clear();
    }

    private void Update()
    {
        if (lastSelect != selectedProduct && selectedProduct != null)
        {
            lastSelect = selectedProduct;
            ChangePropertiesPannel();
            ChangeGeneralPropertiesPannel();
        }
    }
}
