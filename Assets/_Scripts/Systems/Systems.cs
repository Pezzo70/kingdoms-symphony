using Kingdom.Audio;

public class Systems : PersistentSingleton<Systems> 
{ 
    public void Start(){}

    public AudioSystem AudioSystem => this.GetComponentInChildren<AudioSystem>();

}
