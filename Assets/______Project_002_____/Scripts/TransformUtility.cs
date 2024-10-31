using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class TransformUtility
{
    // Tag 가 달린 GameObject 반환
    public static GameObject FindGameObjectWithTag(GameObject root, string tag)
    {
        return FindFirstChildWithTag(root, tag);
    }
    
    
    private static GameObject FindFirstChildWithTag(GameObject parent, string tag)
    {    
        foreach (Transform child in parent.transform)
        {
            // 자식이 태그와 일치하는 경우 해당 객체 반환
            if (child.CompareTag(tag))
            {
                return child.gameObject;
            }

            // 자식의 자식도 검색 (재귀 호출)
            GameObject foundChild = FindFirstChildWithTag(child.gameObject, tag);
            if (foundChild != null)
            {
                return foundChild;
            }
        }
        
        return null;
    }
}
