namespace ClinicalTools.SimEncounters.MainMenu
{
    public interface IDeviceIdLogin : ILogin
    {
        string DeviceId { get; set; }
    }
}