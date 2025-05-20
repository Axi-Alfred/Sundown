using UnityEngine;

public class Player
{
    public int PlayerId { get; private set; }
    public int PlayerScore { get; private set; } = 0;
    public bool HasPlayed { get; set; }
    public int CurrentIconInt { get; private set; }

    private string playerName;
    public string PlayerName { get { return playerName; } set { playerName = value; } }

    private Sprite playerIcon;
    public Sprite PlayerIcon { get { return playerIcon; } set { playerIcon = value; } }

    // ✅ New: Store the raw texture from the camera
    public Texture2D PlayerPhotoTexture { get; private set; }

    public Player(string name, int iD)
    {
        playerName = name;
        PlayerId = iD;
        CurrentIconInt = iD;
    }

    public void AddScore(int score)
    {
        PlayerScore += score;
    }

    // ✅ Call this when setting the photo from the camera
    public void SetPhoto(Texture2D texture)
    {
        PlayerPhotoTexture = texture;

        // Convert texture to sprite
        playerIcon = Sprite.Create(
            texture,
            new Rect(0, 0, texture.width, texture.height),
            new Vector2(0.5f, 0.5f));
    }

    public override string ToString()
    {
        return "Player name: " + PlayerName + ", Player iD: " + PlayerId;
    }
}
