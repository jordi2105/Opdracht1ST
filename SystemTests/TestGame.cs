using System;
using System.Collections.Generic;
using System.Diagnostics;
using SystemTests.Specifications;
using Rogue;
using Rogue.DomainObjects;
using Rogue.Services;

namespace SystemTests
{
    class TestGame : Game
    {
        private readonly List<ISpecification> specifications;

        public TestGame(DungeonGenerator dungeonGenerator, GameSerializer gameSerializer, MonsterSpawner monsterSpawner, ItemSpawner itemSpawner, Random random, List<ISpecification> specifications) : base(dungeonGenerator, gameSerializer, monsterSpawner, itemSpawner, random)
        {
            this.specifications = specifications;
        }

        protected override void initialize()
        {
            base.initialize();
            this.validateSpecifications();
        }

        public override void turn()
        {
            base.turn();
            this.validateSpecifications();
        }

        private void validateSpecifications()
        {
            foreach (ISpecification specification in this.specifications) {
                bool result = specification.validate(this);
                Debug.WriteLine(specification.GetType().Name + ' ' + (result ? "succeeded" : "failed"));
            }
        }
    }
}
