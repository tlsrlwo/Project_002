using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class TransformUtility
{
    // Tag �� �޸� GameObject ��ȯ
    public static GameObject FindGameObjectWithTag(GameObject root, string tag)
    {
        return FindFirstChildWithTag(root, tag);
    }
    
    
    private static GameObject FindFirstChildWithTag(GameObject parent, string tag)
    {    
        foreach (Transform child in parent.transform)
        {
            // �ڽ��� �±׿� ��ġ�ϴ� ��� �ش� ��ü ��ȯ
            if (child.CompareTag(tag))
            {
                return child.gameObject;
            }

            // �ڽ��� �ڽĵ� �˻� (��� ȣ��)
            GameObject foundChild = FindFirstChildWithTag(child.gameObject, tag);
            if (foundChild != null)
            {
                return foundChild;
            }
        }
        
        return null;
    }
}
