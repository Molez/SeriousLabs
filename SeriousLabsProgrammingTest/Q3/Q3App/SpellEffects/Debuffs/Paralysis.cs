namespace Q3App.SpellEffects.Debuffs
{
    public class Paralysis : Debuff, INoAction
    {
        private const long SPELL_LENGTH = 30;

        public Paralysis(Character target)
        {
            //not sure if the constructor is the best place for this but should work for now.
            target.ClearConcentration();
        }

        //I am duping this logic a lot, should see if I can make it less like this... DRY
        public override long GetExpirationTime()
        {
            return GetApplicationTime() + SPELL_LENGTH;
        }
    }
}
