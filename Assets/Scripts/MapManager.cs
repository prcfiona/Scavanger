using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapManager : MonoBehaviour
{
    public GameObject[] outWallArray;
    public GameObject[] floorArray;
    public GameObject[] wallArray;
    public GameObject[] foodArray;
    public GameObject[] enemyArray;
    public GameObject exitPerfab;

    private int minCountWall=2;
    private int maxCountWall = 8; 
    public float rows = 10;
    public float cols = 10;

    private List<Vector2> positionList = new List<Vector2>();
    private Transform mapHolder;
    private GameManager gameManager;


    //生成地图
    public void InitMap()
    {
        gameManager = this.GetComponent<GameManager>();
        mapHolder = new GameObject("Map").transform;
        //围墙和地板
        for (int x = 0; x < cols; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                if (x == 0 || y == 0 || x == cols - 1 || y == cols - 1)
                {
                    int index = Random.Range(1, outWallArray.Length);
                    GameObject go = GameObject.Instantiate(outWallArray[index], new Vector3(x, y, 0), Quaternion.identity) as GameObject;
                    go.transform.SetParent(mapHolder);
                }
                else
                {
                    int index = Random.Range(1, floorArray.Length);
                    GameObject go = GameObject.Instantiate(floorArray[index], new Vector3(x, y, 0), Quaternion.identity) as GameObject;
                    go.transform.SetParent(mapHolder);
                }
            }
        }
        //初始化障碍物出现的位置数组
        positionList.Clear();
        for(int x = 2; x < cols - 2; x++)
        {
            for(int y = 2; y < rows - 2; y++)
            {
                positionList.Add(new Vector2(x, y));
            }
        }
        //障碍物生成
        int wallCount = Random.Range(minCountWall, maxCountWall + 1);   //随机生成障碍物的数量
        InstantiateItem(wallCount, wallArray);
        //食物生成
        int foodCount = Random.Range(1, gameManager.level);
        InstantiateItem(foodCount, foodArray);
        //敌人生成
        int enemyCount = Random.Range(1, gameManager.level / 2);
        InstantiateItem(enemyCount, enemyArray);
        //出口生成
        GameObject go1 = GameObject.Instantiate(exitPerfab, new Vector2(8, 8), Quaternion.identity) as GameObject;
        go1.transform.SetParent(mapHolder);
    }
    //实例化方法
    private void InstantiateItem(int count,GameObject[] array)
    {
        for (int i = 0; i < count; i++)
        {
            Vector2 pos = RandomPosition();
            GameObject go = GameObject.Instantiate(RandomPerfab(array), pos, Quaternion.identity) as GameObject;
            go.transform.SetParent(mapHolder);
        }
    }
    //随机生成位置
    private Vector2 RandomPosition()
    {
        int positionIndex = Random.Range(0, positionList.Count);
        Vector2 pos = positionList[positionIndex];
        positionList.RemoveAt(positionIndex);
        return pos;
    }
    //随机生成物体（障碍物 敌人 食物）
    private GameObject RandomPerfab(GameObject[] perfabArray)
    {
        int perfabIndex = Random.Range(0, perfabArray.Length);
        return perfabArray[perfabIndex];
    }
}
