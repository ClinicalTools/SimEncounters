namespace ClinicalTools.SimEncounters
{
    public delegate void SelectedHandler<T>(object sender, T e);
    public interface ISelector<T>
    {
        void Select(object sender, T eventArgs);
        void AddSelectedListener(SelectedHandler<T> handler);
        void RemoveSelectedListener(SelectedHandler<T> handler);
    }
}