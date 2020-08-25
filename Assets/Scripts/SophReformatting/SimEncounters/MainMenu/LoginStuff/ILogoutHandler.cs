using System;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public interface ILogoutHandler
    {
        event Action Logout;
    }
}