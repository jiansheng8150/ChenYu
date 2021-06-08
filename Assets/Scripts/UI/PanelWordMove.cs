using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelWordMove : MonoBehaviour {

    private Game game;
	// Use this for initialization
	void Start () {
        GameObject gameObj = UIManager.Instance.panels["PanelGame"];
        game = gameObj.GetComponent<Game>();
    }
	
	// Update is called once per frame
	void Update () {
        
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name.Length == 10 && collision.name.IndexOf("PanelWord") == 0)
        {
            int index = int.Parse(collision.name.Substring(9,1));
            game.enterIndex(collision.transform, index-1);
            //Debug.Log("trigger enter"+ collision.name);
        }
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        //Debug.Log("trigger stay");
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name.Length == 10 && collision.name.IndexOf("PanelWord") == 0)
        {
            int index = int.Parse(collision.name.Substring(9, 1));
            game.exitIndex(collision.transform, index-1);
            //Debug.Log("trigger exit");
        }
    }
}
