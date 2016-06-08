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

        public TestGame(PlayerInputReader playerInputReader, GameSerializer gameSerializer, GameBuilder gameBuilder, Random random, List<ISpecification> specifications) 
            : base(playerInputReader, gameSerializer, gameBuilder, random, null)
        {
            this.specifications = specifications;
        }

        public override void initialize()
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
               if(!result)
                   Debug.WriteLine(specification.GetType().Name + ' ' + "failed");
               // Debug.WriteLine(specification.GetType().Name + ' ' + (result ? "succeeded" : "failed"));
            }
        }
    }
}
