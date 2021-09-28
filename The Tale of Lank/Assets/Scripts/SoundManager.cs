using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//All static cause only need one SoundManager.
public class SoundManager : MonoBehaviour
{
    private static AudioSource currentMusicSource;
    private static Music curMusic;
    private static Music prevMusic;
    private static float prevMusicSpot;

    public bool forcePlaySong;
    public Music forcedSong;


    private void Start()
    {
        currentMusicSource = gameObject.AddComponent<AudioSource>();
        curMusic = Music.None;
        prevMusic = Music.None;
        prevMusicSpot = 0;
    }

    private void Update()
    {
        //DEBUG
        if(forcePlaySong)
        {
            PlayMusic(forcedSong, true);
            forcePlaySong = false;
        }



        
        //Check if music has reached end and needs to be looped
        if(curMusic != Music.None)
        {
            if(!currentMusicSource.isPlaying)
            {
                Debug.Log("Not playing!");
                //Todo: Logic to play song from loop point
            }
        }
    }


    //Play a song. 
    public static void PlayMusic(Music music, bool forceRestart = false)
    {
        //Don't need to do anything if same song is already playing. Unless restart was specifically requested
        if(music == curMusic && !forceRestart)
        {
            return;
        }
        //Stop currently playing music and load previous song from where it was left off. Unless restart was specifically requested
        else if (music == prevMusic && !forceRestart)
        {           

            //tmp for swapping cur and prev
            Music tmpMusic = curMusic;
            float tmpMusicSpot = currentMusicSource.time;

            //Load prev info
            currentMusicSource.clip = Resources.Load<AudioClip>(music.GetResourcePath());
            currentMusicSource.time = prevMusicSpot;
            curMusic = prevMusic;            

            //Store current music to previous
            prevMusic = tmpMusic;
            prevMusicSpot = tmpMusicSpot;
            currentMusicSource.Play();
            Debug.Log(currentMusicSource.clip);
        }
        //Otherwise just load the song
        else 
        {
            prevMusic = curMusic;
            if (curMusic == Music.None)
            {
                prevMusicSpot = 0;   
            }
            else
            {                
                prevMusicSpot = currentMusicSource.time;
            }

            curMusic = music;
            currentMusicSource.clip = Resources.Load<AudioClip>(music.GetResourcePath());
            currentMusicSource.Play();
            Debug.Log(currentMusicSource.clip);
        }
    }


    //Play a fanfare. Like a sound effect, but it pauses the music until done.
    public static void PlayFanfare()
    {

    }
}


public enum Music
{
    None,
    Overworld,
    Dungeon
}

public static class MusicEnumExtensions
{
    public static void Play(this Music music)
    {
        SoundManager.PlayMusic(music);
    }

    public static SongInformation GetInfo(this Music music)
    {
        if(music == Music.None)
        {
            return new SongInformation("", "", 0);
        }
        else if(music == Music.Overworld)
        {
            return new SongInformation("Overworld", "TLoZ: LttP", 0);
        }
        else if(music == Music.Dungeon)
        {
            return new SongInformation("Dungeon", "TLoZ: BSZ", 0);
        }
        else
        {
            return new SongInformation("", "", 0);
        }
    }

    public static string GetResourcePath(this Music music)
    {
        if(music == Music.None)
        {
            return "";
        }
        else if(music == Music.Overworld)
        {
            return "Music/overworldLttP";
        }
        else if(music == Music.Dungeon)
        {
            return "Music/dungeonBSZ";
        }
        else
        {
            return "";
        }
    }


}

public struct SongInformation
{
    public string name;
    public string source;
    public float loopStart;

    public SongInformation(string name, string source, float loopStart)
    {
        this.name = name;
        this.source = source;
        this.loopStart = loopStart;
    }
}