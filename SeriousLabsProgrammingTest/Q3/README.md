# Question 3B
The majority of the code should explain my process and I placed comments in code for some additional insight on what I was thnking or areas I thought might need improvement. However here are some extra thoughts and details on how it is laid out.

## Spells
To add spells I pulled from some basic concepts that exist in WoW such as spell id's, buffs/debuffs etc. The basic structure is

- Character
    - This class holds the basic information for a character. Items such as their current level, stats, health, etc as well as their current experience. Adding spells meant adding tracking for buffs and defuffs as well. These have to tie in such that we check for expired buffs/debuffs at the very least when certain actions are performed. We also have to take buffs/debuffs into consideration when performing tasks as they might stop us from performing actions or add/remove stats that need to be taken into account when applying damage.
- Spell
    - A item which can be cast by a character. I made them classes to that they could store their owner and make for an easy reference to who cast the spell but not strictly necssary. It also worked well into a list of spells that the character could store in a spellbook. 
    - Spells all have some basic fields like spell ID that apply to all spells.
    - Extending the class allows you to make spells of different types. These spells contain the logic for what happens when a spell is cast. This might be removal of buffs/debuffs, addition of buffs/debuffs or even potentially raw damage.
- Spell Effects
    - While this is generally a refernce to visible spell animations and FX in this case I used i to describe the side effect of a spell. 
    - There are two types of spell effects, buffs and debuffs. These are tracked slightly differently but the classes did come out identical and one might consider them for merger.
        - Debuff
            - A spell effect that has a negative effect and is generally placed on an opposing character by another.  Each debuff keeps track of its expiration time and application time as well as any logic required to run when the debuff is removed. 
            - For certain types of spells a linking was implemented to facilitate spell effects that are linked and may need to clear together or otherwise interact together.
        - Buff
            - A spell effect that is positive and generally cast on oneself. 
    - On top of buff/debuffs there is some interfaces to facilitate large types of buffs/debuffs to make manging them easier.     - INoAction is placed on any spell effect for which they should not be able to perform any actions while that buff/debuff is on them. This worked well for concentration and paralysis.
        - IStengthModifier signifies somethign that changes the characters strength in any way. this allows the character class to easily find the buffs/debuffs and have them apply their modifier. For simplicity they will take in the current strength and return the modified value to accomodate differences in how its calculated.