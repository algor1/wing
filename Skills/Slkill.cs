using UnityEngine;
using System.Collections.Generic;
 
[System.Serializable]
public class Skill
{
    public int id;
    public string skill;
    public string sprite_path;
    public int points;
    public int level;
    public int tech;
    public int difficulty;
    public int depend_id;
    public int depend_tech;
    public bool rootSkill;


    public Skill() { }
    public Skill(Skill _sk )
    {
        id = _sk.id;
        name = _sk.name;
        sprite = _sk.sprite;
        points = 0;
        level = 1;
        tech = 1;
        depend = _sk.depend;
        rootSkill = _sk.rootSkill;
    }
    public Skill(Skill _sk,int _tech)
    {
        id = _sk.id;
        name = _sk.name;
        sprite = _sk.sprite;
        points = 0;
        level = 1;
        tech = _tech;
        depend = _sk.depend;
        rootSkill = _sk.rootSkill;
    }

    
}