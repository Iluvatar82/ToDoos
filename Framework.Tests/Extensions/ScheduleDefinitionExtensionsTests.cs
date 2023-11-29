using Framework.DomainModels.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Framework.Extensions.Tests
{
    [TestClass()]
    public class ScheduleDefinitionExtensionsTests
    {
        [TestMethod()]
        public void NextOccurrenceAfterTest_Interval_No_Start_End()
        {
            var definition = new ScheduleDefinition() { Interval = new ScheduleInterval() { Interval = 1m, Unit = DomainModels.Common.Enums.ScheduleTimeUnit.Day } };
            var nextOccurrenceAfterNow = definition.NextOccurrenceAfter(DateTime.Now);

            Assert.AreEqual(DateTime.Today.AddDays(1), nextOccurrenceAfterNow);
        }

        [TestMethod()]
        public void NextOccurrenceAfterTest_Weekdays_No_Start_End()
        {
            var definition = new ScheduleDefinition() { WeekDays = new ScheduleWeekdays() { Days = new List<bool>{ false, true, true, true, true, true, false } } };
            var time = new DateTime(2023, 11, 29, 8, 45, 30);
            var nextOccurrenceAfterNow = definition.NextOccurrenceAfter(time);

            Assert.AreEqual(time.Date.AddDays(1), nextOccurrenceAfterNow);
        }

        [TestMethod()]
        public void LastOccurrenceBeforeTest_Interval_No_Start_End()
        {
            var definition = new ScheduleDefinition() { Interval = new ScheduleInterval() { Interval = 1m, Unit = DomainModels.Common.Enums.ScheduleTimeUnit.Day } };
            var lastOccurrenceBeforeNow = definition.LastOccurrenceBefore(DateTime.Now);

            Assert.AreEqual(DateTime.Today, lastOccurrenceBeforeNow);
        }

        [TestMethod()]
        public void LastOccurrenceBeforeTest_Weekdays_No_Start_End()
        {
            var definition = new ScheduleDefinition() { WeekDays = new ScheduleWeekdays() { Days = new List<bool> { false, true, true, true, true, true, false } } };
            var time = new DateTime(2023, 11, 26, 23, 59, 59);
            var lastOccurrenceBeforeNow = definition.LastOccurrenceBefore(time);

            Assert.AreEqual(time.Date.AddDays(-2), lastOccurrenceBeforeNow);
        }

        [TestMethod()]
        public void GetOccurrencesTest_Interval_No_Start_End()
        {
            var definition = new ScheduleDefinition() { Interval = new ScheduleInterval() { Interval = 1m, Unit = DomainModels.Common.Enums.ScheduleTimeUnit.Day } };
            var occurrences = definition.GetOccurrences(DateTime.Today, DateTime.Today.AddDays(7).AddSeconds(-1));

            Assert.AreEqual(7, occurrences.Count);
            Enumerable.Range(0, 7).Select(d => DateTime.Today.AddDays(d)).ForEach((d, i) =>
            {
                Assert.AreEqual(d, occurrences[i]);
            });
        }

        [TestMethod()]
        public void GetOccurrencesTest_Weekdays_No_Start_End()
        {
            var definition = new ScheduleDefinition() { WeekDays = new ScheduleWeekdays() { Days = new List<bool> { false, true, true, true, true, true, false } } };
            var occurrences = definition.GetOccurrences(DateTime.Now, DateTime.Today.AddDays(7).AddSeconds(-1));

            Assert.AreEqual(5, occurrences.Count);
            Enumerable.Range(0, 7).Select(d => DateTime.Today.AddDays(d)).Where(d => definition.WeekDays.Days[(int)d.Date.DayOfWeek]).ForEach((d, i) =>
            {
                Assert.AreEqual(d, occurrences[i]);
            });
        }
    }
}