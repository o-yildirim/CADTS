using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaisticsPanelManager : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnEnable()
    {
        //Sayfa her açıldığında databaseden istatistik çekecek
        Debug.Log("Istatistik paneli açıldı");
    }
}
