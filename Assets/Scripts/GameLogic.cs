using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameLogic : MonoBehaviour
{
    Block[] blocks;
    StackData grade6, grade7, grade8;
    public StackScript Stack1, Stack2, Stack3;
    public Material glass, wood, stone;
    public GameObject blockprefab;
    public InfoPanel infopanel;

    StackScript selected;

    // Start is called before the first frame update
    void Start()
    {
        grade6 = new StackData();
        grade7 = new StackData();
        grade8 = new StackData();
        StartCoroutine(GetData());
        infopanel.gameObject.SetActive(false);
        focusStack(Stack2);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            // Raycast from Screen to point under to find the clicked block.
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)){
                if (hit.collider != null && hit.collider.GetComponent<BlockPrefab>() != null)
                {
                    // If we hit the correct type of object, we can open the info panel.
                    infopanel.Open(hit.collider.GetComponent<BlockPrefab>().block);
                }
            }
        }
    }

    IEnumerator GetData() {

        // Callin data from the provided API.
        UnityWebRequest request = UnityWebRequest.Get("https://ga1vqcu3o1.execute-api.us-east-1.amazonaws.com/Assessment/stack");

        yield return request.Send();

        if (request.isNetworkError)
        {
            Debug.Log("Request error code: " + request.error);
        }
        else {           
            // On a successful response, we convert it to our block type for later processing.
            blocks = Newtonsoft.Json.JsonConvert.DeserializeObject<Block[]>(request.downloadHandler.text);           
        }

        if (blocks != null && blocks.Length != 0) {
            // Now we assign the blocks to the corresponding dataitem.
            foreach (Block block in blocks) {
                switch (block.grade) {
                    case "6th Grade": grade6.Blocks.Add(block); break;
                    case "7th Grade": grade7.Blocks.Add(block); break;
                    case "8th Grade": grade8.Blocks.Add(block); break;
                    default: Debug.Log(block.grade); break;
                }
            
            }
        }

        // Before spawning the different stacks, we must sort them.
        grade6.Blocks.Sort();
        grade7.Blocks.Sort();
        grade8.Blocks.Sort();

        SpawnStack(grade6, Stack1);        
        SpawnStack(grade7, Stack2);
        SpawnStack(grade8, Stack3);
    }

    public void SpawnStack(StackData data, StackScript stack) {
        float spacex = 0.35f;
        float spacey = 0.2f;
        int height = 0;
        int e = 0;
        for (int i = 0; i < data.Blocks.Count; i++) {           
            
            // First we instantiate the object.
            BlockPrefab blk = Instantiate(blockprefab, stack.transform).GetComponent<BlockPrefab>();                        
            blk.block = data.Blocks[i];

            // Once we assign the data for later use on InfoPanel, we assign the corresponding Material and text.
            switch (data.Blocks[i].mastery) {
                case 0:
                    blk.GetComponent<Renderer>().material = glass;
                    blk.text1.text = "Newbie";
                    blk.text2.text = "Newbie"; 
                    break;
                case 1:
                    blk.GetComponent<Renderer>().material = wood;
                    blk.text1.text = "Learned";
                    blk.text2.text = "Learned"; 
                    break;
                case 2:
                    blk.GetComponent<Renderer>().material = stone;
                    blk.text1.text = "Mastered";
                    blk.text2.text = "Mastered"; 
                    break;
                default:
                    blk.GetComponent<Renderer>().material = glass;
                    blk.text1.text = "Newbie";
                    blk.text2.text = "Newbie"; 
                    break;
            }

            // Depending on current height we spawn the blocks in the correct direction.
            if (height % 2 == 0)
            {
                blk.transform.localPosition = new Vector3(spacex * e, spacey * height, spacex);
                blk.transform.localRotation = Quaternion.Euler(0, 90, 0);
            }
            else {                
                blk.transform.localPosition = new Vector3(spacex, spacey * height, spacex * e);                
            }

            stack.blocks.Add(blk);            

            e++;
            // Each 3 iterations, we return the e auxiliar to 0 and increase the stack height.
            if (e > 2) { e = 0; height++; }            
        }
    }

    // Focusing on the required stack by changing from one camera to another.
    public void focusStack(StackScript stack) {
        Stack1.Campivot.SetActive(false);
        Stack2.Campivot.SetActive(false);
        Stack3.Campivot.SetActive(false);
        stack.Campivot.SetActive(true);
        selected = stack;
    }

    // For testing the stack survival, we loop through it deleting glass blocks.
    public void TestMyStack() {
        foreach (BlockPrefab block in selected.blocks.ToArray()) {
            block.GetComponent<Rigidbody>().useGravity = true;
            if (block.block.mastery == 0) {
                selected.blocks.Remove(block);
                Destroy(block.gameObject);
            }
        }
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Restart() {

        // Clearing the stacks for starting over.
        foreach (BlockPrefab block in Stack1.blocks)
        {
            Destroy(block.gameObject);
        }
        foreach (BlockPrefab block in Stack2.blocks)
        {
            Destroy(block.gameObject);
        }
        foreach (BlockPrefab block in Stack3.blocks)
        {
            Destroy(block.gameObject);
        }
        Stack1.blocks.Clear();
        Stack2.blocks.Clear();
        Stack3.blocks.Clear();
        Start();
    }
}
