using System;

namespace Q3App.SpellEffects
{
    public abstract class SpellEffect
    {
        private long ApplicationTime;
        private Guid SpellInstanceId; //An id for a specific instance of a spell effect so we can target specific ones if we are stacking

        public SpellEffect()
        {
            //Looking for something that is timezone agnostic and wont cause me issues.
            ApplicationTime = DateTimeOffset.Now.ToUnixTimeSeconds();
            SpellInstanceId = Guid.NewGuid();
        }

        public long GetApplicationTime()
        {
            return ApplicationTime;
        }

        public string GetSpellInstanceId()
        {
            return SpellInstanceId.ToString();
        }

        public abstract long GetExpirationTime();
    }
}
