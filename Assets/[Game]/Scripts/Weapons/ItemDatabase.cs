using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    private static ItemDatabase instance;
    public static ItemDatabase Instance {
        get {
            if(instance == null) {
                instance = FindObjectOfType<ItemDatabase>();
            }
            return instance;
        }
    }

    public List<Weapon> Weapons = new List<Weapon>();

    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    
    void Start()
    {
        Weapons = new List<Weapon>(Resources.LoadAll<Weapon>("Weapons/"));


        ///////

        // Weapon bow =  ItemDatabase.Instance.GetWeaponOfType(WeaponType.Bow); // TODO USAGE
    }

    public Weapon TempGetWeaponByName(string name){
        return Weapons.Find(x=>x.weaponName == name);

    }

    public Weapon GetWeaponOfType(WeaponType weaponType){
        return Weapons.Find(x=>x.weaponType == weaponType);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
