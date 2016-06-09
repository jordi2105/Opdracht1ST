using System;
using System.Collections.Generic;
using System.Diagnostics;
using SystemTests.Specifications;
using Rogue;
using Rogue.Services;

namespace SystemTests
{
    class TestGame : Game
    {
        private readonly List<ISpecification> specifications;

        public TestGame(IInputReader inputReader, GameSerializer gameSerializer, IGameProvider gameBuilder, Random random, List<ISpecification> specifications) 
            : base(inputReader, gameSerializer, gameBuilder, random, null)
        {
            this.specifications = specifications;
        }

        public override void initialize()
        {
            base.initialize();
            this.validateSpecifications();
        }

        public override bool turn()
        {
            try {
                base.turn();
                this.validateSpecifications();
                return true;
            } catch (Exception) {
                return false;
            }
            
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
