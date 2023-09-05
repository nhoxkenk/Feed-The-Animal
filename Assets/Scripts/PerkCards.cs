using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PerkCards : MonoBehaviour
{
    public TMPro.TMP_Text title;
    public TMPro.TMP_Text description;
    public Image backing;
    public Button button;

    UnityEvent applyPerkEvent;

    private Perk myPerk;

    public void SetUpCard(Perk perk)
    {
        title.text = perk.title;
        description.text = perk.description;
        backing.color = perk.cardColor;
        applyPerkEvent = perk.connection;
        myPerk = perk;
    }

    public void ExecutePerk()
    {
        applyPerkEvent.Invoke();
        PerkManager.instance.OnCompletePerkSelection(myPerk);
    }
}
