using Rogue;

namespace SystemTests.Specifications
{
    internal interface ISpecification
    {
        bool validate(Game game);
    }
}