using Q3App.SpellEffects.Debuffs;

namespace Q3App.SpellEffects.Buffs
{
    public class Concentration : Buff, INoAction
    {
        private Character Target;
        private Debuff LinkedDebuff;

        public Concentration(Character target, Debuff linkedDebuff) : base()
        {
            Target = target;
            LinkedDebuff = linkedDebuff;
        }

        public override long GetExpirationTime()
        {
            //We expire when the debuff expires
            return LinkedDebuff.GetExpirationTime();
        }

        public override void ResolveBuffRemoval()
        {
            Target.RemoveDebuff(LinkedDebuff);
        }
    }
}
