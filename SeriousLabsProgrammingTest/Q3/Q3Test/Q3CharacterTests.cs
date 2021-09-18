using NUnit.Framework;
using Q3App;

namespace Q3Test
{
    [TestFixture]
    public class Q3CharacterTests
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
        public void ApplyNonFatalDamage()
        {
            _testProtagonist.ApplyDamage(10);

            Assert.AreEqual(145, _testProtagonist.GetHealth());
        }

        [Test]
        public void ApplyFatalOneShotDamage()
        {
            _testProtagonist.ApplyDamage(200);

            Assert.AreEqual(0, _testProtagonist.GetHealth());
        }

        [Test]
        public void ApplyMultiHitKillDamage()
        {
            _testProtagonist.ApplyDamage(50);
            Assert.AreEqual(105, _testProtagonist.GetHealth());

            _testProtagonist.ApplyDamage(105);
            Assert.AreEqual(5, _testProtagonist.GetHealth());

            _testProtagonist.ApplyDamage(50);
            Assert.AreEqual(0, _testProtagonist.GetHealth());
        }

        [Test]
        public void ApplyDamageToUnconcious()
        {
            _testProtagonist.ApplyDamage(200);
            Assert.AreEqual(0, _testProtagonist.GetHealth());

            //Again
            _testProtagonist.ApplyDamage(200);
            Assert.AreEqual(0, _testProtagonist.GetHealth());
        }

        [Test]
        public void GainExperienceNoLevel()
        {
            _testProtagonist.GainExperience(50);

            Assert.AreEqual(50, _testProtagonist.GetTotalExperience());
            Assert.AreEqual(1, _testProtagonist.GetLevel());
        }

        [Test]
        public void GainExperienceLevelThresholds()
        {
            // 1-> 2 needs 1000XP, 2 -> 3 needs 1500XP (total 2500XP), 3 -> 4 needs 2000XP (total 4500XP), 4 - 5 needs 2500 (total 7000XP), etc.
            Assert.AreEqual(1, _testProtagonist.GetLevel());
            Assert.AreEqual(1000, _testProtagonist.GetTotalExperienceUntilNextLevel());

            _testProtagonist.GainExperience(999);
            Assert.AreEqual(1, _testProtagonist.GetLevel());
            _testProtagonist.GainExperience(1);
            Assert.AreEqual(1000, _testProtagonist.GetTotalExperience());
            Assert.AreEqual(2, _testProtagonist.GetLevel());

            Assert.AreEqual(2500, _testProtagonist.GetTotalExperienceUntilNextLevel());
            _testProtagonist.GainExperience(1499);
            Assert.AreEqual(2, _testProtagonist.GetLevel());
            _testProtagonist.GainExperience(1);
            Assert.AreEqual(2500, _testProtagonist.GetTotalExperience());
            Assert.AreEqual(3, _testProtagonist.GetLevel());

            Assert.AreEqual(4500, _testProtagonist.GetTotalExperienceUntilNextLevel());
            _testProtagonist.GainExperience(1999);
            Assert.AreEqual(3, _testProtagonist.GetLevel());
            _testProtagonist.GainExperience(1);
            Assert.AreEqual(4500, _testProtagonist.GetTotalExperience());
            Assert.AreEqual(4, _testProtagonist.GetLevel());

            Assert.AreEqual(7000, _testProtagonist.GetTotalExperienceUntilNextLevel());
            _testProtagonist.GainExperience(2499);
            Assert.AreEqual(4, _testProtagonist.GetLevel());
            _testProtagonist.GainExperience(1);
            Assert.AreEqual(7000, _testProtagonist.GetTotalExperience());
            Assert.AreEqual(5, _testProtagonist.GetLevel());
        }

        [Test]
        public void GainExperienceMultiLevel()
        {
            Assert.AreEqual(1, _testProtagonist.GetLevel());
            Assert.AreEqual(150, _testProtagonist.GetHealth());
            Assert.AreEqual(10, _testProtagonist.GetStrength());

            //Gain enough exp to go straight to level 5 in one go.
            _testProtagonist.GainExperience(7250);
            Assert.AreEqual(5, _testProtagonist.GetLevel());
            Assert.AreEqual(190, _testProtagonist.GetHealth());
            Assert.AreEqual(14, _testProtagonist.GetStrength());
        }

        [Test]
        public void LevelingUpIncreasesStatsProperly()
        {
            Assert.AreEqual(1, _testProtagonist.GetLevel());
            Assert.AreEqual(150, _testProtagonist.GetHealth());
            Assert.AreEqual(10, _testProtagonist.GetStrength());
            
            _testProtagonist.GainExperience(1000);
            Assert.AreEqual(2, _testProtagonist.GetLevel());
            Assert.AreEqual(160, _testProtagonist.GetHealth());
            Assert.AreEqual(11, _testProtagonist.GetStrength());

            _testProtagonist.GainExperience(1500);
            Assert.AreEqual(3, _testProtagonist.GetLevel());
            Assert.AreEqual(170, _testProtagonist.GetHealth());
            Assert.AreEqual(12, _testProtagonist.GetStrength());

            _testProtagonist.GainExperience(2000);
            Assert.AreEqual(4, _testProtagonist.GetLevel());
            Assert.AreEqual(180, _testProtagonist.GetHealth());
            Assert.AreEqual(13, _testProtagonist.GetStrength());

            _testProtagonist.GainExperience(2500);
            Assert.AreEqual(5, _testProtagonist.GetLevel());
            Assert.AreEqual(190, _testProtagonist.GetHealth());
            Assert.AreEqual(14, _testProtagonist.GetStrength());
        }

        [Test]
        public void CanAttackAnotherCharacter()
        {
            _testProtagonist.AttackOtherCharacter(_testEnemy);

            Assert.AreEqual(145, _testEnemy.GetHealth());
        }

        [Test]
        public void CanAttackAnotherCharacterOneShot()
        {
            //Power up Frodo with 10 million exp because someone decided 150hp and 10 strength was a good idea.
            _testProtagonist.GainExperience(10000000);
            Assert.AreEqual(10000000, _testProtagonist.GetTotalExperience());

            _testProtagonist.AttackOtherCharacter(_testEnemy);

            Assert.AreEqual(0, _testEnemy.GetHealth());
            //Technically gained a bit of exp for one shotting a level 1 Sauron (aren't we proud of ourselves).
            Assert.AreEqual(10000000 + 10, _testProtagonist.GetTotalExperience());

            //Hit him again while he is down
            _testProtagonist.AttackOtherCharacter(_testEnemy);
            Assert.AreEqual(0, _testEnemy.GetHealth());
            //No Exp gain here as they were already knocked out
            Assert.AreEqual(10000000 + 10, _testProtagonist.GetTotalExperience());
        }

        [Test]
        public void GainRightExpOnKnockout()
        {
            //Power up Sauron with 2.55 million exp making him level 100
            _testEnemy.GainExperience(2550000);
            Assert.AreEqual(0, _testProtagonist.GetTotalExperience());

            while(_testEnemy.GetHealth() > 0)
            {
                _testProtagonist.AttackOtherCharacter(_testEnemy);
            }

            Assert.AreEqual(0, _testEnemy.GetHealth());
            //Technically gained a bit of exp for one shotting a level 1 Sauron (aren't we proud of ourselves).
            Assert.AreEqual(_testEnemy.GetLevel() * 10, _testProtagonist.GetTotalExperience());

            //Hit him again while he is down
            _testProtagonist.AttackOtherCharacter(_testEnemy);
            Assert.AreEqual(0, _testEnemy.GetHealth());
            //No Exp gain here as they were already knocked out
            Assert.AreEqual(_testEnemy.GetLevel() * 10, _testProtagonist.GetTotalExperience());
        }
    }
}
