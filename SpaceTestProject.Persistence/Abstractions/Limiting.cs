namespace SpaceTestProject.Persistence.Abstractions
{
    public class Limiting
    {
        public int CountOfRecords { get; }

        public Limiting(int countOfRecords)
        {
            CountOfRecords = countOfRecords;
        }
    }
}