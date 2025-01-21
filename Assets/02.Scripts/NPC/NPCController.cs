using UnityEngine;

namespace yjlee.npc
{
    public class NPCController : MonoBehaviour
    {
        public GameObject shopGameobject;

        void Start()
        {

        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0.0f);

                if(hit.collider != null && hit.collider.CompareTag("NPC"))
                {
                    shopGameobject.SetActive(true);
                    Debug.Log("::: NOC Click :::");
                }
            }
        }
    }
}