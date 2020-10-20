namespace ClinicalTools.SimEncounters
{
    public delegate void SelectedHandler<T>(object sender, T e);
    public interface ISelector<T> : ISelectedListener<T>
    {
        void Select(object sender, T eventArgs);
    }
    public interface ISelectedListener<T>
    {
        void AddSelectedListener(SelectedHandler<T> handler);
        void RemoveSelectedListener(SelectedHandler<T> handler);
    }
}