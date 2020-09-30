namespace ClinicalTools.SimEncounters
{
    public abstract class BaseValueField : BaseField, IValueField
    {
        public abstract void Initialize();
        public abstract void Initialize(string value);
    }
}