
/*
This RPG data streaming assignment was created by Fernando Restituto.
Pixel RPG characters created by Sean Browning.
*/

using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


#region Assignment Instructions

/*  Hello!  Welcome to your first lab :)

Wax on, wax off.

    The development of saving and loading systems shares much in common with that of networked gameplay development.  
    Both involve developing around data which is packaged and passed into (or gotten from) a stream.  
    Thus, prior to attacking the problems of development for networked games, you will strengthen your abilities to develop solutions using the easier to work with HD saving/loading frameworks.

    Try to understand not just the framework tools, but also, 
    seek to familiarize yourself with how we are able to break data down, pass it into a stream and then rebuild it from another stream.


Lab Part 1

    Begin by exploring the UI elements that you are presented with upon hitting play.
    You can roll a new party, view party stats and hit a save and load button, both of which do nothing.
    You are challenged to create the functions that will save and load the party data which is being displayed on screen for you.

    Below, a SavePartyButtonPressed and a LoadPartyButtonPressed function are provided for you.
    Both are being called by the internal systems when the respective button is hit.
    You must code the save/load functionality.
    Access to Party Character data is provided via demo usage in the save and load functions.

    The PartyCharacter class members are defined as follows.  */

public partial class PartyCharacter
{
    public int classID;

    public int health;
    public int mana;

    public int strength;
    public int agility;
    public int wisdom;

    public LinkedList<int> equipment;



}


/*
    Access to the on screen party data can be achieved via …..

    Once you have loaded party data from the HD, you can have it loaded on screen via …...

    These are the stream reader/writer that I want you to use.
    https://docs.microsoft.com/en-us/dotnet/api/system.io.streamwriter
    https://docs.microsoft.com/en-us/dotnet/api/system.io.streamreader

    Alright, that’s all you need to get started on the first part of this assignment, here are your functions, good luck and journey well!
*/


#endregion


#region Assignment Part 1

static public class AssignmentPart1
{

    static public void SavePartyButtonPressed()
    {

        using (StreamWriter sw = new StreamWriter("A1data.txt"))
        {
            foreach (PartyCharacter pc in GameContent.partyCharacters)
            {
                Debug.Log("Saving data");
                sw.WriteLine(pc.classID);
                sw.WriteLine(pc.health);
                sw.WriteLine(pc.mana);
                sw.WriteLine(pc.strength);
                sw.WriteLine(pc.agility);
                sw.WriteLine(pc.wisdom);
               

               foreach(int equipID in pc.equipment)
                {
                    sw.WriteLine(equipID);
                }
            }                                      
                
        }
    }
    public static int classID;
    public static int health;
    public static int mana;
    public static int strength;
    public static int agility;
    public static int wisdom;
    static public void LoadPartyButtonPressed()
    {

       GameContent.partyCharacters.Clear();

        string line = "";
        string lineHealth = "";
        string lineMana = "";
        string lineStrength = "";
        string lineAgility = "";
        string lineWisdom = "";
        string lineEquipment = "";


        using (StreamReader sr = new StreamReader("A1data.txt"))
        {
            while((line = sr.ReadLine())!= null && (lineHealth = sr.ReadLine())!=null && (lineMana = sr.ReadLine()) != null
                && (lineStrength = sr.ReadLine()) != null && (lineAgility = sr.ReadLine()) != null && (lineWisdom = sr.ReadLine()) != null)
            {
                Debug.Log("Loading data");
                

                PartyCharacter pc = new PartyCharacter();
                GameContent.partyCharacters.AddLast(pc);


                
                //Debug.Log("This is my class ID:" + line);
                pc.classID = int.Parse(line);

                pc.health = int.Parse(lineHealth);

                pc.mana = int.Parse(lineMana);

                pc.strength = int.Parse(lineStrength);

                pc.agility = int.Parse(lineAgility);

                pc.wisdom = int.Parse(lineWisdom);

                //  pc.equipment.ToString();

                // pc.equipment = int.Parse(lineEquipment);
          


                // pc.AddLast(pc.classID, pc.health, pc.mana, pc.strength, pc.agility, pc.wisdom);

            }
           

        }

        GameContent.RefreshUI();

    }

}


#endregion


#region Assignment Part 2

//  Before Proceeding!
//  To inform the internal systems that you are proceeding onto the second part of this assignment,
//  change the below value of AssignmentConfiguration.PartOfAssignmentInDevelopment from 1 to 2.
//  This will enable the needed UI/function calls for your to proceed with your assignment.
static public class AssignmentConfiguration
{
    public const int PartOfAssignmentThatIsInDevelopment = 2;
}

/*

In this part of the assignment you are challenged to expand on the functionality that you have already created.  
    You are being challenged to save, load and manage multiple parties.
    You are being challenged to identify each party via a string name (a member of the Party class).

To aid you in this challenge, the UI has been altered.  

    The load button has been replaced with a drop down list.  
    When this load party drop down list is changed, LoadPartyDropDownChanged(string selectedName) will be called.  
    When this drop down is created, it will be populated with the return value of GetListOfPartyNames().

    GameStart() is called when the program starts.

    For quality of life, a new SavePartyButtonPressed() has been provided to you below.

    An new/delete button has been added, you will also find below NewPartyButtonPressed() and DeletePartyButtonPressed()

Again, you are being challenged to develop the ability to save and load multiple parties.
    This challenge is different from the previous.
    In the above challenge, what you had to develop was much more directly named.
    With this challenge however, there is a much more predicate process required.
    Let me ask you,
        What do you need to program to produce the saving, loading and management of multiple parties?
        What are the variables that you will need to declare?
        What are the things that you will need to do?  
    So much of development is just breaking problems down into smaller parts.
    Take the time to name each part of what you will create and then, do it.

Good luck, journey well.

*/

static public class AssignmentPart2
{
    public static string partyFile = "party.txt";
    private static LinkedList<CharacterData> parties;
    private static uint lastIndex = 0;
    private static string lastName = "";

    //Trying to use signifiers, getting used to them still
    public const int CharacterSaveSignifier = 0;
    public const int EquopmentSaveSignifier = 1;

    static public void GameStart()
    {
        LoadPartyData();
        GameContent.RefreshUI();

    }

    static public List<string> GetListOfPartyNames()
    {
        if (parties == null)
        {
            return new List<string>();
        }

        List<string> partyNames = new List<string>();

        //Loop through each party and add a name
        foreach (CharacterData partyData in parties)
        {
            partyNames.Add(partyData.name);
        }

        return partyNames;
    }

    static public void LoadPartyDropDownChanged(string selectedName)
    {
        lastName = selectedName;

        //Loop through each party and check if the selected name is the party name, if so load the party
        foreach (CharacterData partyData in parties)
        {
            if (selectedName == partyData.name)
            {
                partyData.Loading();
                break;
            }
        }
        GameContent.RefreshUI();
    }

    static public void SavePartyButtonPressed()
    {
       //Saving parties

        lastIndex++;
        CharacterData party = new CharacterData(lastIndex, GameContent.GetPartyNameFromInput());
        parties.AddLast(party);

        SavePartyData();
        party.Saving();

        GameContent.RefreshUI();
    }

    static public void DeletePartyButtonPressed()
    {
        var node = parties.First;
        
        //loop through party nodes
        while (node != null)
        {      
            var next = node.Next;

            //If node is the last name, then delete file and remove party nodes.
            if (node.Value.name == lastName)
            {
                string path = Application.dataPath + Path.DirectorySeparatorChar + node.Value.index + ".txt";
                if (File.Exists(path))
                {
                    File.Delete(path);
                }

                parties.Remove(node);
                break;
            }
            node = next;
        }

        SavePartyData();
        GameContent.RefreshUI();
    }

    static public void SavePartyData()
    {
        StreamWriter sw = new StreamWriter(Application.dataPath + Path.DirectorySeparatorChar + partyFile);
        sw.WriteLine("1," + lastIndex);

        //Loop through parties and save index and names
        foreach (CharacterData partyData in parties)
        {
            sw.WriteLine("2," + partyData.index + "," + partyData.name);
        }
        sw.Close();
    }

    //static public void NewPartyButtonPressed()
    //{

    //}

    static public void LoadPartyData()
    {
        parties = new LinkedList<CharacterData>();
        string path = Application.dataPath + Path.DirectorySeparatorChar + partyFile;
        //Check if file exists. Using data signifiers, either parse the last index OR Add the last party 
        if (File.Exists(path))
        {
            StreamReader sr = new StreamReader(path);
            string line = "";
            while ((line = sr.ReadLine()) != null)
            {
                string[] csv = line.Split(',');

                int dataSignifier = int.Parse(csv[0]);
                if (dataSignifier == 1)
                {
                    lastIndex = uint.Parse(csv[1]);
                }
                else if (dataSignifier == 2)
                {
                    parties.AddLast(new CharacterData(uint.Parse(csv[1]), csv[2]));
                }
            }
            sr.Close();
        }
    }
}

class CharacterData
{

    public uint index;
    public string name;

    public CharacterData(uint index, string name)
    {
        this.index = index;
        this.name = name;
    }

    public void Saving()
    {
        StreamWriter sw = new StreamWriter("A2Data.txt");

        foreach (PartyCharacter pc in GameContent.partyCharacters)
        {
            sw.WriteLine(AssignmentPart2.CharacterSaveSignifier + pc.classID);
            sw.WriteLine(AssignmentPart2.CharacterSaveSignifier + pc.health);
            sw.WriteLine(AssignmentPart2.CharacterSaveSignifier + pc.mana);
            sw.WriteLine(AssignmentPart2.CharacterSaveSignifier + pc.strength);
            sw.WriteLine(AssignmentPart2.CharacterSaveSignifier + pc.agility);
            sw.WriteLine(AssignmentPart2.CharacterSaveSignifier + pc.wisdom);

            foreach (int equipID in pc.equipment)
            {
                sw.WriteLine(AssignmentPart2.EquopmentSaveSignifier + equipID);
            }

        }

        sw.Close();
    }

    public void Loading()
    {
        string path = Application.dataPath + Path.DirectorySeparatorChar + index + ".txt";

        if (File.Exists(path))
        {
            GameContent.partyCharacters.Clear();
            string line = "";
            StreamReader sr = new StreamReader(path);

            while ((line = sr.ReadLine()) != null)
            {
                string[] csv = line.Split(',');

                int saveDataSignifier = int.Parse(csv[0]);
                if (saveDataSignifier == AssignmentPart2.CharacterSaveSignifier)
                {
                    /* 
                        * 1 : classID 
                        * 2: Health
                        * 3: Mana
                        * 4: Strength
                        * 5: Agility
                        * 6: Wisdom
                       */
                    PartyCharacter newChar = new PartyCharacter(
                        int.Parse(csv[1]),
                        int.Parse(csv[2]),
                        int.Parse(csv[3]),
                        int.Parse(csv[4]),
                        int.Parse(csv[5]),
                        int.Parse(csv[6])
                        );
                    GameContent.partyCharacters.AddLast(newChar);
                }
                if (saveDataSignifier == AssignmentPart2.EquopmentSaveSignifier)
                {
                    GameContent.partyCharacters.Last.Value.equipment.AddLast(int.Parse(csv[1]));
                }
            }
            sr.Close();
        }
        GameContent.RefreshUI();
    }
}

#endregion


