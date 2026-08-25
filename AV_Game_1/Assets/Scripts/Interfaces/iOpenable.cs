public interface iOpenable
{
    // Whether this object can currently be opened - lets the object own its
    // own gating (already open, empty, out of range, etc.) instead of the
    // caller having to know its internal state.
    bool CanBeOpened
    {
        get;
    }

    void TryOpen(sPlayerCharacter _player);
}
