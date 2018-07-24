using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Profile {
    public string username;
    public int id;
    public string imageURL;

    public Profile(string username, int id, string imageURL)
    {
        this.username = username;
        this.id = id;
        this.imageURL = imageURL;
    }

    public Image getImage(Image image)
    {
        return image;
    }
}
