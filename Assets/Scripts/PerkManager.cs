using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class PerkManager : MonoBehaviour
{
    public static PerkManager instance;

    List<Perk> perkPool = new List<Perk>();
    Perk[] unweightedPerks;
    List<Perk> perkCurrent = new List<Perk>();

    List<PerkCards> offeredPerkCards = new List<PerkCards>();

    [SerializeField]
    PerkCards cardTemplate;

    [SerializeField]
    GameObject perkMenu;

    Vector3 cardPosition = new Vector3(385f, 200f, 0f);

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        unweightedPerks = GetComponents<Perk>();
        cardTemplate.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            GenerateSelectablePerks();
        }
    }

    void GenerateSelectablePerks()
    {
        List<Perk> selectedPerks = new List<Perk>();

        for(int i = 0; i < 3; i++)
        {
            List<Perk> possiblePerks = new List<Perk>();
            //create weighted list of perks
            foreach(Perk p in unweightedPerks)
            {
                bool isValidPerk = true;

                foreach(Perk pp in p.prerequisitePerks)
                {
                    if (!perkCurrent.Contains(pp) )
                    {
                        isValidPerk = false;
                    }
                }

                if(p.maxRepeatsOfPerk <= 0 || selectedPerks.Contains(p))
                {
                    isValidPerk = false;
                }

                //check if the player health if full or not ?
                int playerHealth = FindObjectOfType<PlayerManager>().returnCurrentHealth();
                if (p.title.Equals("Heal") && playerHealth == 5)
                {
                    isValidPerk = false;
                }

                if (isValidPerk)
                {
                    for(int j = 0; j < p.weigh; j++)
                    {
                        possiblePerks.Add(p);
                        
                    }
                }
            }
            //Debug.Log(possiblePerks.Count);
            selectedPerks.Add(possiblePerks[Random.Range(0, possiblePerks.Count)]);
        }

        foreach(Perk p in selectedPerks)
        {
            //Debug.Log(p.title.ToString());
            PerkCards pc = PerkCards.Instantiate(cardTemplate, cardTemplate.transform.parent);
            pc.SetUpCard(p);
            pc.transform.position = cardPosition;
            pc.gameObject.SetActive(true);
            offeredPerkCards.Add(pc);
            cardPosition += new Vector3(250, 0, 0);
        }
        perkMenu.gameObject.SetActive(true);
    }

    public void OnCompletePerkSelection(Perk p)
    {
        p.maxRepeatsOfPerk -= 1;
        perkCurrent.Add(p);

        foreach(PerkCards pp in offeredPerkCards)
        {
            Destroy(pp.gameObject);
        }

        offeredPerkCards.Clear();

        perkMenu.gameObject.SetActive(false);

        cardPosition = new Vector3(385f, 200f, 0f);
    }
}
