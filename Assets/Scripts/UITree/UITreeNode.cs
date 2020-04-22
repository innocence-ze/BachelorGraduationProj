using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class UITreeNode : UIBehaviour
{
    #region private && public  members
    private UITreeData TreeData = null;
    private UITree UITree = null;
    private Toggle toggle = null;
    private Image icon = null;
    private Text text = null;
    private Transform _toggleTransform = null;
    private Transform _myTransform = null;
    private Transform _container = null;

    private List<GameObject> _children = new List<GameObject>();

    #endregion

    #region get && reset ui component

    private void GetComponent()
    {
        _myTransform = transform;
        _container = _myTransform.Find("Container");
        toggle = _container.Find("Toggle").GetComponent<Toggle>();
        icon = _container.Find("IconContainer/Icon").GetComponent<Image>();
        text = _container.Find("Text").GetComponent<Text>();
        _toggleTransform = toggle.transform.Find("Image");
        UITree = _myTransform.parent.parent.parent.GetComponent<UITree>();

    }
    private void ResetComponent()
    {
        _container.localPosition = new Vector3(0, _container.localPosition.y, 0);
        _toggleTransform.localEulerAngles = new Vector3(0, 0, 90);
        _toggleTransform.gameObject.SetActive(true);
    }

    #endregion

    #region external call interface

    public void Inject(UITreeData data)
    {
        if (null == _myTransform)
            GetComponent();
        ResetComponent();
        TreeData = data;
        text.text = data.Name;
        toggle.isOn = false;
        toggle.onValueChanged.AddListener(OpenOrClose);
        icon.GetComponent<Button>().onClick.AddListener(GetProduct);
        _container.localPosition += new Vector3(_container.GetComponent<RectTransform>().sizeDelta.y * TreeData.Layer, 0, 0);
        if (data.ChildNodes.Count.Equals(0))
        {
            _toggleTransform.gameObject.SetActive(false);
            icon.sprite = UITree.m_lastLayerIcon;
        }
        else
            icon.sprite = toggle.isOn ? UITree.m_openIcon : UITree.m_closeIcon;
    }

    public void Clear()
    {
        RemoveListener();
        text.text = "Hierarchy";
        toggle.isOn = true;
        icon.sprite = null;
        TreeData = null;
        UITree = null;
        toggle = null;
        icon = null;
        text = null;
        _toggleTransform = null;
        _myTransform = null;
        _container = null;
        _children.Clear();
    }

    #endregion

    #region open && close

    private void OpenOrClose(bool isOn)
    {
        if (isOn) OpenChildren();
        else CloseChildren();
        _toggleTransform.localEulerAngles = isOn ? new Vector3(0, 0, 0) : new Vector3(0, 0, 90);
        icon.sprite = toggle.isOn ? UITree.m_openIcon : UITree.m_closeIcon;
    }
    private void OpenChildren()
    {
        _children = UITree.Pop(TreeData.ChildNodes, transform.GetSiblingIndex());
    }

    protected void CloseChildren()
    {
        for (int i = 0; i < _children.Count; i++)
        {
            UITreeNode node = _children[i].GetComponent<UITreeNode>();
            node.RemoveListener();
            node.CloseChildren();
        }
        UITree.Push(_children);
        _children = new List<GameObject>();
    }
    private void RemoveListener()
    {
        toggle.onValueChanged.RemoveListener(OpenOrClose);
        icon.GetComponent<Button>().onClick.RemoveListener(GetProduct);
    }

    private void GetProduct()
    {
        var go = SomeValue.project.EntityLabel == TreeData.Label ? SomeValue.project.ThisGameObject : null;
        if(go == null)
        {
            var spa = SomeValue.spatialStructures.Find(ss => ss.EntityLabel == TreeData.Label);
            if (spa != null)
                go = spa.ThisGameObject;
        }
        if (go == null)
        {
            var ele = SomeValue.Elements.Find(e => e.EntityLabel == TreeData.Label);
            if (ele != null)
                go = ele.ThisGameObject;
        }
        GameEvents.current.selectedProduct = go;
    }

    #endregion
}