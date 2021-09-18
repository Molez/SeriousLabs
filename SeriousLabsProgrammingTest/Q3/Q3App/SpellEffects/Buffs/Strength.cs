namespace Q3App.SpellEffects.Buffs
{
    public class Strength : Buff, IStrengthModifier
    {
        private const long SPELL_LENGTH = 30;
        private const int STRENGHT_MULTIPLIER= 2;

        public override long GetExpirationTime()
        {
            return GetApplicationTime() + SPELL_LENGTH;
        }

        public int GetModifiedStrenght(int baseStrength)
        {
            //This is super super naiive given that the order buffs are applied would alter your ending strength.
            //Aka  10 * 2 - 5 = 15 vs 10 - 5 * 2 = 10. But its good enough for now no one will find this IRL 5
            //minutes after it goes onto the PTR right...
            return baseStrength * STRENGHT_MULTIPLIER;
        }

        public override void ResolveBuffRemoval()
        {
            //No-Op
        }
    }
}
