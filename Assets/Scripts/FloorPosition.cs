using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System;

[Serializable] 
public class PosVec
{
    public double x = 0;
    public double y = 0;
    public double z = 0;
    public void SetPosVec(double xp, double yp, double zp)
    {
        this.x = xp;
        this.y = yp;
        this.z = zp;
    }
}
[Serializable]
public class pointerListItem
{
    public string data = "718_0.49_0.40_2";
    public int Id = 18;
    public PosVec Position;
    public int mode = 2;
}
[Serializable]
public class PosData
{
    public string SensorType = "IR";
    public List<pointerListItem> pointerList;
}
[Serializable]
public class Pos2D
{
    public double x = 0;
    public double y = 0;
    public void Set2PosVec(double xp, double yp)
    {
        this.x = xp;
        this.y = yp;
    }
}
public class FloorPosition : MonoBehaviour
{
    
    UdpClient udpClient;
    IPEndPoint ipEndPoint;
    [SerializeField]
    public Pos2D OutPosVec;
    void Start()
    {
        udpClient = new UdpClient(9028);
        ipEndPoint = new IPEndPoint(IPAddress.Any, 0);

        //debug & initial value
        string dd = "{\"SensorType\":\"IR\",\"pointerList\":[{\"data\":\"718_0.49_0.40_2\",\"Id\":18,\"Position\":{\"x\":0.1867834448814392,\"y\":0.3985801339149475,\"z\":0.0},\"mode\":2}]}#";// remove last "#"
        string debug_trimEnd = dd.TrimEnd('#');
        string lidar_dd = "{\"SensorType\":\"Rada\",\"pointerList\":[{\"data\":\"" + 
            "564543_0.21_0.97_2\",\"Id\":43,\"Position\":{\"x\":0.21222946047782899,\"y\":0.9683724641799927,\"z\":0.0},\"mode\":2}"+
            ",{\"data\":\"610609_0.81_0.94_1\",\"Id\":9,\"Position\":{\"x\":0.8078190088272095,\"y\":0.9443104267120361,\"z\":0.0},"+
            "\"mode\":1},{\"data\":\"610608_0.40_0.97_1\",\"Id\":8,\"Position\":{\"x\":0.3951264023780823,\"y\":0.9707027077674866,"+
            "\"z\":0.0},\"mode\":1}]}#";
        string lidar_trimEnd = lidar_dd.TrimEnd('#');

        PosData tpData = new PosData();
        PosVec tpos = new PosVec();
        tpos.SetPosVec(0.4867834448814392, 0.3985801339149475, 0);
        pointerListItem pItem = new pointerListItem();
        pItem.Position = tpos;
        List<pointerListItem> npc = new List<pointerListItem> ();
        npc.Add(pItem);
        tpData.pointerList = npc;
        Debug.Log(debug_trimEnd);
        PosData test = JsonUtility.FromJson<PosData>(debug_trimEnd);
        PosData Lidar_test = JsonUtility.FromJson<PosData>(lidar_trimEnd);
        Debug.Log(Lidar_test.pointerList[0].Position.x);
        if (test.SensorType.Equals("IR")) // exclude LiDAR
        {
            OutPosVec.Set2PosVec(test.pointerList[0].Position.x, test.pointerList[0].Position.y);
        }
        if (Lidar_test.SensorType.Equals("IR")) // exclude LiDAR
        {
            OutPosVec.Set2PosVec(Lidar_test.pointerList[0].Position.x, Lidar_test.pointerList[0].Position.y);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Byte[] receiveBytes;
        try
        {
            if (udpClient.Available > 0)
            {
                receiveBytes = udpClient.Receive(ref ipEndPoint);
                string returnData = Encoding.ASCII.GetString(receiveBytes);
                returnData = returnData.TrimEnd('#');
                Debug.Log("Received(Trim #): " + returnData.ToString());
                PosData test = JsonUtility.FromJson<PosData>(returnData);
                if (test.SensorType == "IR") // exclude LiDAR
                {
                    OutPosVec.Set2PosVec(test.pointerList[0].Position.x, test.pointerList[0].Position.y);
                }

            } else
            {
                //Debug.Log("No data received.");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
        //
    }

}
