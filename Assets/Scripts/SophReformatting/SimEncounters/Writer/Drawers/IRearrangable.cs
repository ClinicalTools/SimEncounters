using System;

namespace ClinicalTools.SimEncounters.Writer
{
    public class RearrangedEventArgs : EventArgs
    {
        public int OriginalIndex { get; }
        public int NewIndex { get; }
        public RearrangedEventArgs(int orignalIndex, int newIndex)
        {
            OriginalIndex = orignalIndex;
            NewIndex = newIndex;
        }
    }
    public delegate void RearrangedHandler(object sender, RearrangedEventArgs e);

    public interface IRearrangable
    {
        event RearrangedHandler Rearranged;
    }
}