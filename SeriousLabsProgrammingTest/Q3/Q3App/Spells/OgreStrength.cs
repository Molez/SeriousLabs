using Q3App.SpellEffects.Buffs;

namespace Q3App.Spells
{
    public class OgreStrength : Spell
    {
        public static string SPELL_ID = "ogre-spell";

        public OgreStrength(Character owner)
            : base(owner)
        {

        }

        public override void Cast(Character target)
        {
            target.AddBuff(new Strength());
        }

        public override string GetSpellId()
        {
            return SPELL_ID;
        }
    }
}
