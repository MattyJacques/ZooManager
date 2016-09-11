using UnityEngine;
using System.Collections;

[System.Serializable]
public class Achievements {
	public bool noot;
	public bool clap;
	public bool life;
	public bool oneofmany;
	public bool howdidyou;
	public bool cash;
	public int nootnum;
	public int clapnum;
	public int lifenum;
	public int onenum;
	public int hownum;
	public int cashnum;


	public Achievements() 
	{
		this.noot = false;
		this.clap = false;
		this.life = false;
		this.oneofmany = false;
		this.howdidyou = false;
		this.cash = false;
		this.nootnum = 0;
		this.clapnum = 0;
		this.lifenum = 0;
		this.onenum = 0;
		this.hownum = 0;
		this.cashnum = 0;
	}

}
