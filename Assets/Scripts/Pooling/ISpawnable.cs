namespace Game
{
    ///<summary>
    ///Pooled Objects can use this interface to call something when spawning
    ///</summary>///
    public interface ISpawnable
    {
        void OnSpawn() { }
    }
}