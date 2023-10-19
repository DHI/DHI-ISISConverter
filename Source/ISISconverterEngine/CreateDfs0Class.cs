using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DHI.Generic.MikeZero.DFS;
using DHI.Generic.MikeZero;
namespace ISISConverterEngine
{
    public class CreateDfs0FileISIS
    {
        public static void CreateDfs0File2(string filename, bool calendarAxis)
        {
            DfsFactory factory = new DfsFactory();
            DfsBuilder builder = DfsBuilder.Create("TemporalAxisTest", "dfs Timeseries Bridge", 10000);

            // Set up file header
            builder.SetDataType(1);
            builder.SetGeographicalProjection(factory.CreateProjectionUndefined());
            if (calendarAxis)
                builder.SetTemporalAxis(factory.CreateTemporalEqCalendarAxis(eumUnit.eumUsec, new DateTime(2010, 01, 04, 12, 34, 00), 4, 10));
            else
                builder.SetTemporalAxis(factory.CreateTemporalEqTimeAxis(eumUnit.eumUsec, 3, 10));
            builder.SetItemStatisticsType(StatType.RegularStat);

            // Set up first item
            DfsDynamicItemBuilder item1 = builder.CreateDynamicItemBuilder();
            item1.Set("WaterLevel item", eumQuantity.Create(eumItem.eumIWaterLevel, eumUnit.eumUmeter),
                      DfsSimpleType.Float);
            item1.SetValueType(DataValueType.Instantaneous);
            item1.SetAxis(factory.CreateAxisEqD0());
            item1.SetReferenceCoordinates(1f, 2f, 3f);
            builder.AddDynamicItem(item1.GetDynamicItemInfo());

            DfsDynamicItemBuilder item2 = builder.CreateDynamicItemBuilder();
            item2.Set("WaterDepth item", eumQuantity.Create(eumItem.eumIWaterDepth, eumUnit.eumUmeter),
                      DfsSimpleType.Float);
            item2.SetValueType(DataValueType.Instantaneous);
            item2.SetAxis(factory.CreateAxisEqD0());
            item2.SetReferenceCoordinates(1f, 2f, 3f);
            builder.AddDynamicItem(item2.GetDynamicItemInfo());

            // Create file
            builder.CreateFile(filename);
            IDfsFile file = builder.GetFile();

            double[] times = new double[10];
            double[,] values = new double[10, 2];

            // Write data to file
            values[0, 0] = 0f;  // water level
            values[0, 1] = 100f;  // water depth
            values[1, 0] = 1f;  // water level
            values[1, 1] = 101f;  // water depth
            values[2, 0] = 2f;  // water level
            values[2, 1] = 102f;  // water depth
            values[3, 0] = 3f;  // etc...
            values[3, 1] = 103f;
            values[4, 0] = 4f;
            values[4, 1] = 104f;
            values[5, 0] = 5f;
            values[5, 1] = 105f;
            values[6, 0] = 10f;
            values[6, 1] = 110f;
            values[7, 0] = 11f;
            values[7, 1] = 111f;
            values[8, 0] = 12f;
            values[8, 1] = 112f;
            values[9, 0] = 13f;
            values[9, 1] = 113f;

            DHI.Generic.MikeZero.DFS.dfs0.Dfs0Util.WriteDfs0DataDouble(file, times, values);

            file.Close();
        }

        public static void CreateDfs0FileNonEquidis(string filename, bool calendarAxis,List<ValueTimePairClass> TimeSeries)
        {
            DfsFactory factory = new DfsFactory();
            DfsBuilder builder = DfsBuilder.Create("TemporalAxisTest", "dfs Timeseries Bridge", 10000);

            // Set up file header
            builder.SetDataType(1);
            builder.SetGeographicalProjection(factory.CreateProjectionUndefined());
            if (calendarAxis)
                builder.SetTemporalAxis(factory.CreateTemporalEqCalendarAxis(eumUnit.eumUsec, new DateTime(2010, 01, 04, 12, 34, 00), 4, 10));
            else
                builder.SetTemporalAxis(factory.CreateTemporalEqTimeAxis(eumUnit.eumUsec, 3, 10));
            builder.SetItemStatisticsType(StatType.RegularStat);

            // Set up first item
            DfsDynamicItemBuilder item1 = builder.CreateDynamicItemBuilder();
            item1.Set("WaterLevel item", eumQuantity.Create(eumItem.eumIWaterLevel, eumUnit.eumUmeter),
                      DfsSimpleType.Float);
            item1.SetValueType(DataValueType.Instantaneous);
            item1.SetAxis(factory.CreateAxisEqD0());
            item1.SetReferenceCoordinates(1f, 2f, 3f);
            builder.AddDynamicItem(item1.GetDynamicItemInfo());

            // Create file
            builder.CreateFile(filename);
            IDfsFile file = builder.GetFile();

            // Write data to file
            file.WriteItemTimeStepNext(0, new float[] { 0f });  // water level
            file.WriteItemTimeStepNext(0, new float[] { 100f });  // water depth
            file.WriteItemTimeStepNext(0, new float[] { 1f });  // water level
            file.WriteItemTimeStepNext(0, new float[] { 101f });  // water depth
            file.WriteItemTimeStepNext(0, new float[] { 2f });  // water level
            file.WriteItemTimeStepNext(0, new float[] { 102f });  // water depth
            file.WriteItemTimeStepNext(0, new float[] { 3f });  // etc...
            file.WriteItemTimeStepNext(0, new float[] { 103f });
            file.WriteItemTimeStepNext(0, new float[] { 4f });
            file.WriteItemTimeStepNext(0, new float[] { 104f });
            file.WriteItemTimeStepNext(0, new float[] { 5f });
            file.WriteItemTimeStepNext(0, new float[] { 105f });
            file.WriteItemTimeStepNext(0, new float[] { 10f });
            file.WriteItemTimeStepNext(0, new float[] { 110f });
            file.WriteItemTimeStepNext(0, new float[] { 11f });
            file.WriteItemTimeStepNext(0, new float[] { 111f });
            file.WriteItemTimeStepNext(0, new float[] { 12f });
            file.WriteItemTimeStepNext(0, new float[] { 112f });
            file.WriteItemTimeStepNext(0, new float[] { 13f });
            file.WriteItemTimeStepNext(0, new float[] { 113f });

            file.Close();
        }


        public static void CreateDfs0NeqCalWaterlevel(string filename, bool calendarAxis, List<ValueTimePairClass> TimeSeries)
        {
            {
                DfsFactory factory = new DfsFactory();
                DfsBuilder builder = DfsBuilder.Create("TemporalAxisTest", "dfs Timeseries Bridge", 10000);

                // Set up file header
                builder.SetDataType(1);
                builder.SetGeographicalProjection(factory.CreateProjectionUndefined());
                if (calendarAxis)
                    builder.SetTemporalAxis(factory.CreateTemporalNonEqCalendarAxis(eumUnit.eumUsec, TimeSeries[0].dateTime));
                else
                    builder.SetTemporalAxis(factory.CreateTemporalNonEqTimeAxis(eumUnit.eumUsec));
                builder.SetItemStatisticsType(StatType.RegularStat);

                // Set up first item
                DfsDynamicItemBuilder item1 = builder.CreateDynamicItemBuilder();
                item1.Set("WaterLevel item", eumQuantity.Create(eumItem.eumIWaterLevel, eumUnit.eumUmeter),
                          DfsSimpleType.Float);
                item1.SetValueType(DataValueType.Instantaneous);
                item1.SetAxis(factory.CreateAxisEqD0());
                item1.SetReferenceCoordinates(1f, 2f, 3f);
                builder.AddDynamicItem(item1.GetDynamicItemInfo());

                // Create file
                builder.CreateFile(filename);
                IDfsFile file = builder.GetFile();

                // Write data to file
                file.WriteItemTimeStepNext(0, new float[] { (float)TimeSeries[0].Value });
                for (int i = 1; i < TimeSeries.Count; i++)
                {
                    TimeSpan span = (TimeSpan)(TimeSeries[i].dateTime - TimeSeries[0].dateTime);
                    file.WriteItemTimeStepNext(span.TotalSeconds, new float[] { (float)TimeSeries[i].Value });

                }
                file.Close();
            }
        }

        public static void CreateDfs0NeqCalDischarge(string filename, bool calendarAxis, List<ValueTimePairClass> TimeSeries)
        {
            {
                DfsFactory factory = new DfsFactory();
                DfsBuilder builder = DfsBuilder.Create("TemporalAxisTest", "dfs Timeseries Bridge", 10000);

                // Set up file header
                builder.SetDataType(1);
                builder.SetGeographicalProjection(factory.CreateProjectionUndefined());
                if (calendarAxis)
                    builder.SetTemporalAxis(factory.CreateTemporalNonEqCalendarAxis(eumUnit.eumUsec, TimeSeries[0].dateTime));
                else
                    builder.SetTemporalAxis(factory.CreateTemporalNonEqTimeAxis(eumUnit.eumUsec));
                builder.SetItemStatisticsType(StatType.RegularStat);

                // Set up first item
                DfsDynamicItemBuilder item1 = builder.CreateDynamicItemBuilder();
                item1.Set("Discharge item", eumQuantity.Create(eumItem.eumIDischarge, eumUnit.eumUm3PerSec),
                          DfsSimpleType.Float);
                item1.SetValueType(DataValueType.Instantaneous);
                item1.SetAxis(factory.CreateAxisEqD0());
                item1.SetReferenceCoordinates(1f, 2f, 3f);
                builder.AddDynamicItem(item1.GetDynamicItemInfo());

                // Create file
                builder.CreateFile(filename);
                IDfsFile file = builder.GetFile();

                // Write data to file
                file.WriteItemTimeStepNext(0, new float[] {(float) TimeSeries[0].Value });
                for (int i = 1; i < TimeSeries.Count; i++)
                {
                    TimeSpan span = (TimeSpan)(TimeSeries[i].dateTime - TimeSeries[0].dateTime);
                    file.WriteItemTimeStepNext(span.TotalSeconds, new float[] { (float)TimeSeries[i].Value });

                }
                file.Close();
            }
        }
    }
}
