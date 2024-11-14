using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Project002
{
    public class SaveManager : MonoBehaviour
    {
        private string jsonPathProject;
        private string fileName = "GameSave";

       public static SaveManager Instance { get; set; }

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
            DontDestroyOnLoad(gameObject);
        }


        private void Start()
        {
            jsonPathProject = Application.dataPath + Path.AltDirectorySeparatorChar;
        }

        //--------------------------------------------------------------------------------Saving
        public void SaveGame()
        {
            AllGameData data = new AllGameData();

            data.playerData = GetPlayerData();

            SaveGameData(data);
        } 

        private PlayerData GetPlayerData()
        {
            int currentLevel = LevelManager.Instance.currentStageLevel;

            float[] playerPos = new float[3];

            playerPos[0] = LevelManager.Instance.playerbody.transform.position.x;
            playerPos[1] = LevelManager.Instance.playerbody.transform .position.y;
            playerPos[2] = LevelManager.Instance.playerbody.transform.position.z;


            return new PlayerData(currentLevel, playerPos);
        }

        //--------------------------------------------------------------------------------Loading


        public AllGameData Loading()
        {
            AllGameData gameData = LoadGameData();

            return gameData;
        }

        public void LoadGame()
        {
            SetPlayerData(Loading().playerData);
        }


        void SetPlayerData(PlayerData playerData)
        {
            LevelManager.Instance.currentStageLevel = playerData.currentLevel;

            Vector3 loadedPos;
            loadedPos.x = playerData.playerPos[0];
            loadedPos.y = playerData.playerPos[1];
            loadedPos.z = playerData.playerPos[2]; 

            LevelManager.Instance.playerbody.transform.position = loadedPos;
        }




        // ---------------------------------------------------------------------Json

        public void SaveGameData(AllGameData gamedata)
        {
            string json = JsonUtility.ToJson(gamedata);

            using (StreamWriter writer = new StreamWriter(jsonPathProject + fileName + ".json"))
            {
                writer.Write(json);
                //writer.Write(encrypted);
                print("Saved game to json file at :" + jsonPathProject + fileName + ".json");
            };
        }

        public AllGameData LoadGameData()
        {
            using (StreamReader reader = new StreamReader(jsonPathProject + fileName + ".json"))
            {
                // jsonPathProject에서 파일을 읽고 string형태로 가져옴
                string json = reader.ReadToEnd();
                
                // string으로 가져온 파일을 오브젝트(AllGameData)로 변환해줌
                AllGameData data = JsonUtility.FromJson<AllGameData>(json);
                print("Saved game loaded from json file at :" + jsonPathProject);

                return data;
            };
        }

    }
}
