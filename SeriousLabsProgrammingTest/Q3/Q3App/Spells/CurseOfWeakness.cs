using Q3App.SpellEffects.Debuffs;
using System;

namespace Q3App.Spells
{
    public class CurseOfWeakness : Spell
    {
        public static string SPELL_ID = "curse-of-weakness";

        public CurseOfWeakness(Character owner)
            : base(owner)
        {

        }

        public override void Cast(Character target)
        {
            target.AddDebuff(new Weakness());
        }

        public override string GetSpellId()
        {
            throw new NotImplementedException();
        }
    }
}
