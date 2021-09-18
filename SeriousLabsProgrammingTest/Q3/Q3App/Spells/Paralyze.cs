using Q3App.SpellEffects.Buffs;
using Q3App.SpellEffects.Debuffs;

namespace Q3App.Spells
{
    public class Paralyze : Spell
    {
        public static string SPELL_ID = "paralyze";

        public Paralyze(Character owner)
            : base(owner)
        {

        }

        public override void Cast(Character target)
        {
            var paralysisSpellEffect = new Paralysis(target, Owner);
            target.AddDebuff(paralysisSpellEffect);
            Owner.AddBuff(new Concentration(target, paralysisSpellEffect));
        }

        public override string GetSpellId()
        {
            return SPELL_ID;
        }
    }
}
