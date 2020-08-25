namespace ClinicalTools.SimEncounters
{
    public abstract class BaseValueField : BaseField
    {
        public abstract void Initialize();
        public abstract void Initialize(string value);
    }
}