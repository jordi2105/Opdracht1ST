using System;
using System.Collections.Generic;
using System.Diagnostics;
using SystemTests.Specifications;
using Rogue;
using Rogue.DomainObjects;
using Rogue.Services;

namespace SystemTests
{
    class TestGame : AutomaticGame
    {
        private readonly List<ISpecification> specifications;

        public TestGame(GameSerializer gameSerializer, GameBuilder gameBuilder,Random random, List<ISpecification> specifications) 
            : base(gameSerializer,gameBuilder,random)
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
               if(!result)
                   Debug.WriteLine(specification.GetType().Name + ' ' + "failed");
               // Debug.WriteLine(specification.GetType().Name + ' ' + (result ? "succeeded" : "failed"));
            }
        }
    }
}
