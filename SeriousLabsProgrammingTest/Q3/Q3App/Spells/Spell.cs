using System;

namespace Q3App.Spells
{
    //Thought here was that these spells the definition of an action that is owned by someone to determine who the source is
    //and if they can cast it or not as it would have to be in theri spellbook. We can track the
    //owner of the spell so we can do things like decide if a spell can be cast only on ourselves by requiring the target 
    //be only the owner. 
    public abstract class Spell 
    {
        protected readonly Character Owner;

        public Spell(Character owner)
        {
            Owner = owner;
        }

        public abstract void Cast(Character target);

        public abstract string GetSpellId();
    }
}
