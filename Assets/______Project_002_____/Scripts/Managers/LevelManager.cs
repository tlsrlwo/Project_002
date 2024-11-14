using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Project002
{
    public class LevelManager : MonoBehaviour
    {
        public int currentStageLevel = 0;

        public GameObject playerbody;
        

        public Transform levelWayPoints;
        public List<Transform> levelEntrance = new List<Transform>();

        public static LevelManager Instance { get; set; }



        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }

            foreach (Transform child in levelWayPoints)
            {
                levelEntrance.Add(child);
            }
            if(playerbody == null)
            {
                playerbody = GameObject.Find("PicoChan");
            }
        }

        private void Start()
        {
            //playerbody.transform.position = levelEntrance[currentStageLevel].transform.position;
            SaveManager.Instance.LoadGame();
           
            DontDestroyOnLoad(gameObject);
        }




    }
}
