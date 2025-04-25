using UnityEngine;

public class OddTapGameManager : MonoBehaviour
{
    public GameObject shapePrefab;
    public Sprite[] normalShapeSprites;
    public Sprite[] oddShapeSprites;

    public int totalRounds = 3;
    private int currentRound = 0;

    public int shapesPerRound = 5;
    public float spacing = 2f;

    private GameObject currentOddShape;

    void Start()
    {
        StartNextRound();
    }

    void StartNextRound()
    {
        ClearShapes();

        currentRound++;
        if (currentRound > totalRounds)
        {
            Debug.Log("🎉 You Win All Rounds!");
            return;
        }

        int oddIndex = Random.Range(0, shapesPerRound);

        for (int i = 0; i < shapesPerRound; i++)
        {
            Vector3 pos = new Vector3((i - shapesPerRound / 2f) * spacing, 0f, 0f);
            GameObject shape = Instantiate(shapePrefab, pos, Quaternion.identity);
            shape.tag = "Shape";

            SpriteRenderer sr = shape.GetComponent<SpriteRenderer>();
            Shape shapeScript = shape.GetComponent<Shape>();

            if (i == oddIndex)
            {
                shape.name = $"Shape_{i}_ODD";
                sr.sprite = oddShapeSprites[Random.Range(0, oddShapeSprites.Length)];
                shape.transform.localScale *= 1.5f;
                shapeScript.SetOdd(true);
                currentOddShape = shape;

                Debug.Log($"🎯 ODD: {shape.name} | Sprite: {sr.sprite.name} | IsOdd: {shapeScript.isOdd}");
            }
            else
            {
                shape.name = $"Shape_{i}_NORMAL";
                sr.sprite = normalShapeSprites[Random.Range(0, normalShapeSprites.Length)];
                shapeScript.SetOdd(false);

                Debug.Log($"🔵 NORMAL: {shape.name} | Sprite: {sr.sprite.name} | IsOdd: {shapeScript.isOdd}");
            }
        }

        Debug.Log($"▶️ Round {currentRound} started. Find the odd one!");
    }

    public void HandleShapeTapped(bool wasOdd)
    {
        if (wasOdd)
        {
            Debug.Log("✅ Correct! You found the odd one!");
            StartNextRound();
        }
        else
        {
            Debug.Log("❌ Nope! That’s not the odd one. Game Over.");
            ClearShapes();
        }
    }

    void ClearShapes()
    {
        GameObject[] allShapes = GameObject.FindGameObjectsWithTag("Shape");

        foreach (GameObject shape in allShapes)
        {
            Destroy(shape);
        }
    }
}
