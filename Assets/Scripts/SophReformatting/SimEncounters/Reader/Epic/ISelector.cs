namespace ClinicalTools.SimEncounters
{
    public delegate void SelectedHandler<T>(object sender, T e);
    public interface ISelector<T>
    {
        void Select(object sender, T eventArgs);
        void AddEarlySelectedListener(SelectedHandler<T> handler);
        void RemoveEarlySelectedListener(SelectedHandler<T> handler);
        void AddSelectedListener(SelectedHandler<T> handler);
        void RemoveSelectedListener(SelectedHandler<T> handler);
        void AddLateSelectedListener(SelectedHandler<T> handler);
        void RemoveLateSelectedListener(SelectedHandler<T> handler);
    }
}