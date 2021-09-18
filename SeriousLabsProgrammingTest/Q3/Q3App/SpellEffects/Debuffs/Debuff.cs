namespace Q3App.SpellEffects.Debuffs
{
    public abstract class Debuff : SpellEffect
    {
        public Debuff() : base()
        {

        }

        public abstract void ResolveDebuffRemoval();
    }
}
