namespace Utilities
{
	public struct Tuple<TypeOne, TypeTwo>
	{
		public TypeOne ValOne;
		public TypeTwo ValTwo;
		public Tuple(TypeOne valOne, TypeTwo valTwo)
		{
			ValOne = valOne;
			ValTwo = valTwo;
		}
	}
}
