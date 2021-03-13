

public class LIST
{
    public OKASIs omoti; 

    public void Start()
    {
        omoti = new OKASIs();//初期化する
        omoti.count = 100;
        omoti.okasi_Data = new OKASI();
        omoti.okasi_Data.name = "おもち";
        omoti.okasi_Data.heal = 100; 
    }

}

public class OKASIs
{
    public OKASI okasi_Data;
    public int count;
}

public class OKASI
{
    public string name;
    public int heal;
}