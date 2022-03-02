using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generate_Rooms : MonoBehaviour {

    public int boardRows, boardColumns;
    public int minRoomSize, maxRoomSize;
    public GameObject floorSprite;

    private GameObject[,] boardPositionsFloor;
    private static HashSet<Rect> rooms = new HashSet<Rect>();

    private static float overlapMargin = 10;

    public class Room
    {
        public Room left, right;
        public Rect rect;
        public Rect room = new Rect(-1, -1, 0, 0); // i.e null
        public int debugId;

        private static int debugCounter = 0;

        public Room(Rect mrect)
        {
            rect = mrect;
            debugId = debugCounter;
            debugCounter++;
        }

        public bool IsLeaf()
        {
            return left == null && right == null;
        }

        public bool Split(int minRoomSize, int maxRoomSize)
        {
            if (!IsLeaf())
            {
                return false;
            }

            //if too wide split vertically, too long split horizontally
            bool splitH;
            if (rect.width / rect.height >= 1.25)
            {
                splitH = false;
            }
            if (rect.height / rect.width >= 1.25)
            {
                splitH = true;
            } else //square room
            {
                splitH = Random.Range(0.0f, 1.0f) > 0.5;
            }

            if (Mathf.Min(rect.width, rect.height) / 2 < minRoomSize)
            {
                //Debug.Log("Room: " + debugId + "will be a leaf room");
                return false;
            }

            if (splitH)
            {
                int splitPosition = Random.Range(minRoomSize, (int)(rect.width - minRoomSize));
                left = new Room(new Rect(rect.x, rect.y, rect.width, splitPosition));
                right = new Room(new Rect(rect.x, rect.y, rect.width, rect.height - splitPosition));
            }
            else
            {
                int splitPosition = Random.Range(minRoomSize, (int)(rect.height - minRoomSize));
                left = new Room(new Rect(rect.x, rect.y, splitPosition, rect.height));
                right = new Room(new Rect(rect.x, rect.y, rect.width - splitPosition, rect.height));
            }
            return true;
        }

        public void CreateRoom()
        {
            if(left != null)
            {
                left.CreateRoom();
            }
            if(right != null)
            {
                right.CreateRoom();
            }
            if(IsLeaf())
            {
                int roomWidth = (int)Random.Range(rect.width / 2, rect.width - 2);
                int roomHeight = (int)Random.Range(rect.height / 2, rect.height - 2);
                int roomX = (int)Random.Range(1, rect.width - roomWidth - 1);
                int roomY = (int)Random.Range(1, rect.height - roomHeight - 1);

                if (roomWidth > 0 && roomHeight > 0)
                {
                    room = new Rect(rect.x + roomX, rect.y + roomY, roomWidth, roomHeight);
                    //Debug.Log("Created room " + room + " in sub-dungeon " + debugId + " " + rect);
                    rooms.Add(room);
                }
            }
        }
    }

    //returns true if overlaps with another rect
    public static bool checkOverlap(Rect r, Rect rect2)     
    {
        Vector2 l1= new Vector2(r.x, r.y);
        Vector2 r1 = new Vector2(r.x + r.width, r.y + r.height);
        Vector2 l2 = new Vector2(rect2.x, rect2.y);
        Vector2 r2 = new Vector2(rect2.x + rect2.width, rect2.y + rect2.height);

        if (l1.x > r2.x || l2.x > r1.x)
        {
            return false;
        }

        if (l1.y < r2.y || l2.y < r1.y)
        {
            return false;
        }
        return true;
    }

    public void CreateBSP(Room subRoom)
    {
        //Debug.Log("Splitting sub-dungeon " + subRoom.debugId + ": " + subRoom.rect);
        if (subRoom.IsLeaf())
        {
            // if the sub-dungeon is too large
            if (subRoom.rect.width > maxRoomSize || subRoom.rect.height > maxRoomSize || Random.Range(0.0f, 1.0f) > 0.25)
            {

                if (subRoom.Split(minRoomSize, maxRoomSize))
                {
                    //Debug.Log("Splitted sub-dungeon " + subRoom.debugId + " in "
                        //+ subRoom.left.debugId + ": " + subRoom.left.rect + ", "
                        //+ subRoom.right.debugId + ": " + subRoom.right.rect);

                    CreateBSP(subRoom.left);
                    CreateBSP(subRoom.right);
                }
            }
        }
    }

    public static Vector3 separate(Rect a, Rect b)
    {
        float randX = Random.Range(a.width + b.width, a.width * b.width);
        float randY = Random.Range(a.height + b.height, a.height * b.height);
        Debug.Log("randX range: (" + a.width + " , " + a.width + b.width + ")");
        Debug.Log("randY range: (" + a.height + " , " + a.height + b.height + ")");
        Debug.Log("rand values gotten: (" + randX + " , " + randY + ")");
        return new Vector3(randX, 1, randY);
    }

    public void DrawRooms(Room subRoom)
    {
        if (subRoom == null)
        {
            return;
        }
        if (subRoom.IsLeaf())
        {
            Vector3 newLoc = new Vector3();
            int tries = 0;
            foreach (Rect r in rooms)
            {             
                int numberOfTries = 10;

                foreach (Rect other in rooms)
                {
                    while (!checkOverlap(r, other) && tries < numberOfTries)
                    {
                        Debug.Log("number of tries left: " + tries);
                        newLoc = separate(r, other);
                        tries++;
                    }
                    if(tries == numberOfTries)
                    {
                        //rooms.RemoveWhere;
                    }
                    tries = 0;
                }
                GameObject instance = Instantiate(floorSprite, new Vector3(r.x + r.width, 0f, r.y + r.height), Quaternion.identity) as GameObject;
                instance.transform.localScale = new Vector3(r.width, 1, r.height);
                boardPositionsFloor[(int)r.x, (int)r.y] = instance;

                Debug.Log("moving rect: " + r + "to new location: " + newLoc);
                instance.transform.position = newLoc;
                instance.transform.SetParent(transform);
            }

            //for (int i = (int)subRoom.room.x; i < subRoom.room.xMax; i++)
            //{
            //    for (int j = (int)subRoom.room.y; j < subRoom.room.yMax; j++)
            //    {
            //        GameObject instance = Instantiate(floorSprite, new Vector3(i, 0f, j), Quaternion.identity) as GameObject;
            //        instance.transform.SetParent(transform);
            //        boardPositionsFloor[i, j] = instance;
            //    }
            //}
        }
        else
        {
            DrawRooms(subRoom.left);
            DrawRooms(subRoom.right);
        }
    }

    private void Start()
    {
        Room rootRoom = new Room(new Rect(0, 0, boardRows, boardColumns));
        CreateBSP(rootRoom);
        rootRoom.CreateRoom();
        boardPositionsFloor = new GameObject[boardRows, boardColumns];
        DrawRooms(rootRoom);
    }
}


//private void Start()
//    {
//        float level1AreaX = Random.Range(600, 800);
//        float level1AreaY = Random.Range(800, 1000);
//        GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Plane);
//        floor.transform.localScale = new Vector3(level1AreaX, 0, level1AreaY);
//        floor.GetComponent<MeshRenderer>().material = Resources.Load("Material/floor_color.mat", typeof(Material)) as Material;
//    }

