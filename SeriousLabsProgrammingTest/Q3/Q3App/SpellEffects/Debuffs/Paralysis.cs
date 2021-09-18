namespace Q3App.SpellEffects.Debuffs
{
    public class Paralysis : Debuff, INoAction
    {
        private const long SPELL_LENGTH = 30;

        private Character Caster;

        public Paralysis(Character target, Character caster)
        {
            //not sure if the constructor is the best place for this but should work for now.
            target.ClearConcentration();
            Caster = caster;
        }

        //I am duping this logic a lot, should see if I can make it less like this... DRY
        public override long GetExpirationTime()
        {
            return GetApplicationTime() + SPELL_LENGTH;
        }

        public override void ResolveDebuffRemoval()
        {
            //Quick and dirty way to clear the caster concentration if its dispelled. Should work for now.
            Caster.ClearConcentration(); 
        }
    }
}
