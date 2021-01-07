public class SelectPartyCloser : UICloser
{
    private SelectParty selectParty;

    private void Awake()
    {
        selectParty = FindObjectOfType<SelectParty>();
    }

    protected override void CloseUI()
    {
        base.CloseUI();
        selectParty.scrollBar.SetActive(false);
        selectParty.UpdateAll();
    }
}
