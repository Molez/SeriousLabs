using Q3App.SpellEffects.Buffs;
using Q3App.SpellEffects.Debuffs;
using Q3App.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using Q3App.SpellEffects;

namespace Q3App
{
    public class Character
    {
        private const int BASE_EXP = 1000;
        private const int ADDATIVE_EXP = 500;
        private const int HEALTH_PER_LEVEL = 10;
        private const int STRENGTH_PER_LEVEL = 1;
        private const int EXP_MULTIPLIER = 10;

        //Could have also used { get; set; } to save some space
        private string Name;
        private Guid Id; // In-case we need to identify two characters and see if they are the same
        private int Level;
        private int Health;
        private int Armour;
        private int Strength;
        private int TotalExperience;

        private IEnumerable<Spell> SpellBook;

        private IList<Buff> Buffs;
        private IList<Debuff> Debuffs;

        public Character(string name)
        {
            //Assume any new character is created a sprout and no types defined that might vary your starting stats
            Level = 1;
            Health = 150;
            Armour = 5;
            Strength = 10;
            TotalExperience = 0;
            Name = name;
            Id = Guid.NewGuid();
            SpellBook = new List<Spell> //Might want to make this a set unless duped spells make sense.
            {
                new OgreStrength(this),
                new CurseOfWeakness(this),
                new Paralyze(this),
                new DispellMagic(this)
            };

            Buffs = new List<Buff>();
            Debuffs = new List<Debuff>();
        }

        #region Getters and Setters
        public string GetName()
        {
            return Name;
        }

        public string GetId()
        {
            return Id.ToString();
        }

        public int GetTotalExperience()
        {
            return TotalExperience;
        }

        public int GetLevel()
        {
            return Level;
        }
        public int GetHealth()
        {
            return Health;
        }
        public int GetArmour()
        {
            return Armour;
        }

        public int GetBaseStrength()
        {
            return Strength;

        }
        public int GetStrength()
        {
            CheckBuffsAndDebuff();
            return GetModifiedStrength();
        }
        #endregion

        //Armor values are applied by the character upon receiving of damgae. The called should submit its
        //raw damage value here.
        public void ApplyDamage(int rawDamage)
        {
            var mitigatedDamage = rawDamage - Armour;
           
            mitigatedDamage = mitigatedDamage >= 0 ? mitigatedDamage : 0; //Quick fix for no negative healing damage. Armor is no longer a healing stat :D

            if (Health > 0 && mitigatedDamage > 0)
            {
                ClearConcentration();
            }

            Health = mitigatedDamage > Health ? 0 : Health - mitigatedDamage;
        }

        public void GainExperience(int experience)
        {
            // TODO: Gain the experience and level up (potentially multiple times) if necessary.
            // * To level up, 1000 experience is required. Each subsequent level after 2
            // requires additional an 500 experience per level. For example, 1 -> 2 needs
            // 1000XP, 2 -> 3 needs 1500XP (total 2500XP), 3 -> 4 needs 2000XP (total 4500XP), etc.
            // * At a level up, maximum health increases by 10 and strength increases by 1
            TotalExperience += experience;

            while (GetTotalExperienceUntilNextLevel() <= TotalExperience)
            {
                //Note: Doing this means I kind of recalcualte the same thing a lot because of the addative XP calculation.
                //It might be better to keep track of what the next level exp was and then just add to it and build it incrementally
                //intead of re-calculating it over and over. Unless I can think of a better way to do addative exp rather than loop and sum
                //every time.
                LevelUp();
            }
        }

        public void CastSpell(string spellId, Character target)
        {
            //Only cast if we can perform actions and the spell id is in our spell book.
            if (CanPerformAction() && SpellBook.Any(spell => spell.GetSpellId() == spellId))
            {
                var spellToCast = SpellBook.Single(spell => spell.GetSpellId() == spellId);
                spellToCast.Cast(target);
            }
        }

        public bool CanPerformAction()
        {
            CheckBuffsAndDebuff();

            var hasNoActionBuffs = Buffs.Any(buff => buff is INoAction);
            var hasNoActionDebuffs = Debuffs.Any(debuff => debuff is INoAction);
            
            return !hasNoActionBuffs && !hasNoActionDebuffs;
        }

        private void LevelUp()
        {
            Level += 1; //Is there a Max level?
            Health += HEALTH_PER_LEVEL;
            Strength += STRENGTH_PER_LEVEL;
        }

        public int GetTotalExperienceUntilNextLevel()
        {
            //If I am level 1 I want to calculate exp for level 2
            var targetLevel = Level + 1;
            return ((targetLevel - 1) * BASE_EXP) + CalculateAddativeExp(targetLevel);
        }

        private int CalculateAddativeExp(int targetLevel)
        {
            var addativeExp = 0;

            //I want to skip levels up to 2 as that one has no addative exp.
            if (targetLevel > 2)
            {
                //There is probably a much better way to do this...
                addativeExp =  Enumerable.Range(3, targetLevel - 2).Select(Level => (Level - 2) * ADDATIVE_EXP).Sum();
            }

            return addativeExp;
        }

        public void AttackOtherCharacter(Character otherCharacter)
        {
            // TODO: ApplyDamage to other character.
            // * If you reduce the other character’s health to zero, you knock them
            // unconscious and gain experience equal to 10x their level.
            // * Attacking an unconscious character does nothing.
            // * Attacking damage is equal to the character’s strength
            // * Attacking damage is reduced by the character’s armour value.
            // * Attacks do not affect a character’s armour.

            //Assuming that health of 0 is equivalent to unconcious as I have no
            //reason currently to track a unconcious state because there is no
            //way to be health 0 and not unconcious. 
            //
            //So only do things if the enemies health is over 0
            if (CanPerformAction() && otherCharacter.GetHealth() > 0)
            {
                otherCharacter.ApplyDamage(GetStrength()); //Other character will handle their armor mitigation
                
                if(otherCharacter.GetHealth() <= 0)
                {
                    //We knocked them out
                    GainExperience(otherCharacter.Level * EXP_MULTIPLIER);
                }
            }
        }

        private int GetModifiedStrength()
        {
            //Apply the buffs to our base strenght
            var buffedStrength = Buffs.Where(buff => buff is IStrengthModifier)
                                      .Aggregate(Strength, (aggregate, next) => ((IStrengthModifier)next).GetModifiedStrenght(aggregate));
            //Take our buffed strength and debuff it
            return Debuffs.Where(debuff => debuff is IStrengthModifier)
                          .Aggregate(buffedStrength, (aggregate, next) => ((IStrengthModifier)next).GetModifiedStrenght(aggregate));
        }

        private void CheckBuffsAndDebuff()
        {
            var now = DateTimeOffset.Now.ToUnixTimeSeconds();
            //We are not handling any spell linking here but we probably will have to. For now the linked spells should share the same
            //expiration so they should fall off together for the most part.

            //Clear any buffs that are expired
            var expiredBuffs = Buffs.Where(buff => buff.GetExpirationTime() > now).ToList();
            foreach(var buff in expiredBuffs)
            {
                buff.ResolveBuffRemoval();
                Buffs.Remove(buff);
            }
            
            //Clear any debuffs that are expired
            Debuffs = Debuffs.Where(debuff => debuff.GetExpirationTime() > now).ToList();
        }

        public void AddBuff(Buff buff)
        {
            Buffs.Add(buff);
        }

        public void ClearAllBuffs()
        {
            foreach(var buff in Buffs)
            {
                buff.ResolveBuffRemoval();
            }

            Buffs.Clear();
        }

        public void AddDebuff(Debuff debuff)
        {
            Debuffs.Add(debuff);
        }

        public void RemoveDebuff(Debuff debuff)
        {
            Debuffs.Remove(debuff);
        }

        public void ClearAllDebuffs()
        {
            Debuffs.Clear();
        }

        public void ClearConcentration()
        {
            var concentrationBuffs = Buffs.Where(buff => buff is Concentration).ToList();

            foreach (var concentrationBuff in concentrationBuffs)
            {
                concentrationBuff.ResolveBuffRemoval();
                Buffs.Remove(concentrationBuff);
            }
        }
    }
}
