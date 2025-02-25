using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneChange : NetworkBehaviour
{
    public void LoadScene()
    {

        NetworkManager.SceneManager.LoadScene("L1-Square", LoadSceneMode.Single);
    }
}
