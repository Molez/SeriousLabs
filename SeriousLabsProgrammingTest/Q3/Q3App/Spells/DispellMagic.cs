namespace Q3App.Spells
{
    public class DispellMagic : Spell
    {
        public static string SPELL_ID = "dispell-magic";

        public DispellMagic(Character owner)
            : base(owner)
        {

        }

        public override void Cast(Character target)
        {
            //Technically this should only dispell magic type buffs/debuffs but we dont have a typing system for
            //that so we are going to assume every buff/debuff is magic.
            if(target.GetId() == Owner.GetId())
            {
                //Its being cast on the owner of the spell so clear all their debuffs
                target.ClearAllDebuffs();
            }
            else
            {
                //Its being cast on someone else so clear their buffs aka offensive purge. 
                //No concept of friendlies here but that would just be implemented here.
                target.ClearAllBuffs();
            }
        }

        public override string GetSpellId()
        {
            return SPELL_ID;
        }
    }
}
