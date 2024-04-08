namespace Game
{
    [System.Serializable]
    public struct ElementSlot<T>
    {
        public int index0;
        public int index1;
        public T element;
        public ElementSlot(int idx0, int idx1, T element)
        {
            index0 = idx0;
            index1 = idx1;
            this.element = element;
        }
    }
}