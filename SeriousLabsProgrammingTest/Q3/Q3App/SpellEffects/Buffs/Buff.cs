namespace Q3App.SpellEffects.Buffs
{
    public abstract class Buff : SpellEffect
    {
        public Buff() : base()
        {

        }

        public abstract void ResolveBuffRemoval();
    }
}
