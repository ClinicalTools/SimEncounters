namespace ClinicalTools.SimEncounters.Writer
{
    public delegate void ValueChangedEventHandler<T>(object sender, T obj);
    public interface IChangableValue<T>
    {
        T Value { get; set; }
        event ValueChangedEventHandler<T> ValueChanged;
    }
}