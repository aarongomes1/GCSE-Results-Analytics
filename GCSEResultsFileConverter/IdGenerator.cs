
namespace GCSEResultsFileConverter
{
    internal class IdGenerator
    {
        private int _id = 1;

        public int GetNextId ()
        {
            return _id++;
        }
    }
}
