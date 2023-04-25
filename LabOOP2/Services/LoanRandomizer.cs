using LabOOP2.Domain.Services;

namespace LabOOP2.Services
{
    public class LoanRandomizer : ILoanRandomizer
    {
        private readonly Random _randomizer;
        private const int MinMonthAmount = 1;
        private const int MaxMonthAmount = 36;
        private const int MinPercentageAmount = 0;
        private const int MaxPercentageAmount = 250;

        public LoanRandomizer()
        {
            _randomizer = new Random();
        }

        public int GenerateDuration()
        {
            return _randomizer.Next(MinMonthAmount, MaxMonthAmount);
        }

        public int GeneratePercentage()
        {
            return _randomizer.Next(MinPercentageAmount, MaxPercentageAmount);
        }
    }
}
