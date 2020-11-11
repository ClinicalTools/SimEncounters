namespace ClinicalTools.SimEncounters
{
    public abstract class BaseValueField : BaseField, IValuePanelField
    {
        public abstract void Initialize();
        public abstract void Initialize(string value);
    }
}