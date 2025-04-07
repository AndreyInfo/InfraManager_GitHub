using System;

namespace InfraManager.DAL.ServiceDesk
{
    public class Priority : SequencedLookup, IMarkableForDelete, IDefault
    {
        public const int NoColor = -1;

        protected Priority() : base()
        {
        }

        public Priority(string name, int sequence) 
            : base(name, sequence)
        {
        }


        public int Color { get; set; }
        
        public bool Default { get; set; }

        public bool Removed { get; private set; }

        public void MarkForDelete() => Removed = true;
    }
}
