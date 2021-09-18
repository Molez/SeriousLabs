using NUnit.Framework;
using Q3App;
using Q3App.SpellEffects.Buffs;
using Q3App.SpellEffects.Debuffs;
using Q3App.Spells;
using System.Linq;

namespace Q3Test
{
    [TestFixture]
    public class Q3SpellTests
    {
        private Character _testProtagonist;
        private Character _testEnemy;

        [SetUp]
        public void SetuUp()
        {
            _testProtagonist = new Character("Frodo");
            _testEnemy = new Character("Sauron");
        }

        [Test]
        public void CannotCastUnknownSpell()
        {
            Assert.IsFalse(_testProtagonist.CanCastSpell("something"));
            Assert.IsFalse(_testProtagonist.CastSpell("something", _testEnemy));
        }

        [Test]
        public void CanCastKnownSpell()
        {
            Assert.IsTrue(_testProtagonist.CanCastSpell(CurseOfWeakness.SPELL_ID));
            Assert.IsTrue(_testProtagonist.CastSpell(CurseOfWeakness.SPELL_ID, _testEnemy));
        }

        [Test]
        public void CannotCastWhileConcentrating()
        {
            Assert.IsTrue(_testProtagonist.CanPerformAction());
            Assert.IsTrue(_testProtagonist.CastSpell(Paralyze.SPELL_ID, _testEnemy));

            Assert.IsFalse(_testProtagonist.CanPerformAction());
            Assert.IsTrue(_testProtagonist.CanCastSpell(CurseOfWeakness.SPELL_ID));
            Assert.IsFalse(_testProtagonist.CastSpell(CurseOfWeakness.SPELL_ID, _testEnemy));
        }


        [Test]
        public void DamageBreaksConcentrating()
        {
            Assert.IsTrue(_testProtagonist.CanPerformAction());
            Assert.IsTrue(_testProtagonist.CastSpell(Paralyze.SPELL_ID, _testEnemy));

            Assert.IsFalse(_testProtagonist.CanPerformAction());
            Assert.IsTrue(_testProtagonist.CanCastSpell(CurseOfWeakness.SPELL_ID));
            Assert.IsFalse(_testProtagonist.CastSpell(CurseOfWeakness.SPELL_ID, _testEnemy));

            _testProtagonist.ApplyDamage(8);
            Assert.IsTrue(_testProtagonist.CanPerformAction());
            Assert.IsTrue(_testProtagonist.CanCastSpell(CurseOfWeakness.SPELL_ID));
            Assert.IsTrue(_testProtagonist.CastSpell(CurseOfWeakness.SPELL_ID, _testEnemy));
        }

        [Test]
        public void FullyMitigatedDamageWillNotBreakConcentration()
        {
            Assert.IsTrue(_testProtagonist.CanPerformAction());
            Assert.IsTrue(_testProtagonist.CastSpell(Paralyze.SPELL_ID, _testEnemy));

            Assert.IsFalse(_testProtagonist.CanPerformAction());
            Assert.IsTrue(_testProtagonist.CanCastSpell(CurseOfWeakness.SPELL_ID));
            Assert.IsFalse(_testProtagonist.CastSpell(CurseOfWeakness.SPELL_ID, _testEnemy));

            _testProtagonist.ApplyDamage(1);
            Assert.IsFalse(_testProtagonist.CanPerformAction());
            Assert.IsTrue(_testProtagonist.CanCastSpell(CurseOfWeakness.SPELL_ID));
            Assert.IsFalse(_testProtagonist.CastSpell(CurseOfWeakness.SPELL_ID, _testEnemy));
        }

        [Test]
        public void ParalyseBreaksConcentrationButNowParalized()
        {
            var thirdEnemy = new Character("Zovaal");

            Assert.IsTrue(_testProtagonist.CanPerformAction());
            Assert.IsTrue(_testProtagonist.CastSpell(Paralyze.SPELL_ID, _testEnemy));

            Assert.IsFalse(_testProtagonist.CanPerformAction());
            Assert.IsTrue(_testProtagonist.CanCastSpell(CurseOfWeakness.SPELL_ID));
            Assert.IsFalse(_testProtagonist.CastSpell(CurseOfWeakness.SPELL_ID, _testEnemy));
            Assert.IsTrue(_testProtagonist.GetBuffs().Any(buff => buff is Concentration));
            Assert.IsFalse(_testProtagonist.GetDebuffs().Any(debuff => debuff is Paralysis));

            thirdEnemy.CastSpell(Paralyze.SPELL_ID, _testProtagonist);
            Assert.IsFalse(_testProtagonist.CanPerformAction());
            Assert.IsTrue(_testProtagonist.CanCastSpell(CurseOfWeakness.SPELL_ID));
            Assert.IsFalse(_testProtagonist.CastSpell(CurseOfWeakness.SPELL_ID, _testEnemy));
            Assert.IsFalse(_testProtagonist.GetBuffs().Any(buff => buff is Concentration));
            Assert.IsTrue(_testProtagonist.GetDebuffs().Any(debuff => debuff is Paralysis));
        }

        [Test]
        public void CanWeakenATarget()
        {
            Assert.AreEqual(150, _testProtagonist.GetHealth());
            _testEnemy.AttackOtherCharacter(_testProtagonist);
            Assert.AreEqual(145, _testProtagonist.GetHealth());

            _testProtagonist.CastSpell(CurseOfWeakness.SPELL_ID, _testEnemy);

            _testEnemy.AttackOtherCharacter(_testProtagonist); // Should do no damage now
            Assert.AreEqual(145, _testProtagonist.GetHealth());
        }

        [Test]
        public void CanBuffATarget()
        {
            Assert.AreEqual(150, _testProtagonist.GetHealth());
            _testEnemy.AttackOtherCharacter(_testProtagonist);
            Assert.AreEqual(145, _testProtagonist.GetHealth());

            _testEnemy.CastSpell(OgreStrength.SPELL_ID, _testEnemy);

            _testEnemy.AttackOtherCharacter(_testProtagonist); // Should do 10 * 2 - 5 = 15 damage
            Assert.AreEqual(130, _testProtagonist.GetHealth());
        }

        [Test]
        public void CanBuffAndWeakenATarget()
        {
            Assert.AreEqual(150, _testProtagonist.GetHealth());
            _testEnemy.AttackOtherCharacter(_testProtagonist);
            Assert.AreEqual(145, _testProtagonist.GetHealth());

            _testProtagonist.CastSpell(CurseOfWeakness.SPELL_ID, _testEnemy);
            _testEnemy.CastSpell(OgreStrength.SPELL_ID, _testEnemy);

            _testEnemy.AttackOtherCharacter(_testProtagonist); // Should do 10 * 2 - 5 - 5 = 10 damage
            Assert.AreEqual(135, _testProtagonist.GetHealth());
        }

        [Test]
        public void CanDispellABuffedTarget()
        {
            Assert.AreEqual(150, _testProtagonist.GetHealth());
            _testEnemy.AttackOtherCharacter(_testProtagonist);
            Assert.AreEqual(145, _testProtagonist.GetHealth());

            _testEnemy.CastSpell(OgreStrength.SPELL_ID, _testEnemy);

            _testEnemy.AttackOtherCharacter(_testProtagonist); // Should do 10 * 2 - 5 = 15 damage
            Assert.AreEqual(130, _testProtagonist.GetHealth());

            _testProtagonist.CastSpell(DispellMagic.SPELL_ID, _testEnemy);

            _testEnemy.AttackOtherCharacter(_testProtagonist); // Should do 10 - 5 = 5 damage
            Assert.AreEqual(125, _testProtagonist.GetHealth());
        }
    }
}
