namespace Q3App.SpellEffects.Debuffs
{
    public class Weakness : Debuff, IStrengthModifier
    {
        private const long SPELL_LENGTH = 60;
        private const int STRENGHT_MODIFIER= 5;

        public override long GetExpirationTime()
        {
            return GetApplicationTime() + SPELL_LENGTH;
        }

        public int GetModifiedStrenght(int baseStrength)
        {
            return baseStrength - STRENGHT_MODIFIER;
        }

        public override void ResolveDebuffRemoval()
        {
            //No-op
        }
    }
}
