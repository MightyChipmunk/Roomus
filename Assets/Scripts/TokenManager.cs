using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokenManager : MonoBehaviour
{
    string token;
    public string Token { get { return token; } set { token = value; } }

    string id;
    public string ID { get { return id; } set { id = value; } }

    public static TokenManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
}
