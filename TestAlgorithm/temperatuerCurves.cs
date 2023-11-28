using Emgu.CV;
using SmartRay.Api;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace APIX_Winform_Demo.TestAlgorithm
{
    public class TemperatuerCurves
    {
        public unsafe T[,] RowToArrD<T>(T[] src, int row)
        {
            if (src.Length % row != 0) return null;
            int col = src.Length / row;
            T[,] dst = new T[row, col];
            //for (int i = 0; i < row; i++)
            //{
            //    //说明：“二维数组”【顺序储存】<=>“一维数组”
            //    Buffer.BlockCopy(src, i * col * Marshal.SizeOf(typeof(T)),
            //        dst, i * col * Marshal.SizeOf(typeof(T)), col * Marshal.SizeOf(typeof(T)));
            //}
            return dst;
        }

        public List<float> DataMeasurement(SRImageHandlerArgument SourceData)
        {
            List<float> result = new List<float>();
            var SourceDataRow = SourceData.pointcloud[0];
            Point3F[] SourceDataRowMiddle = SourceDataRow.Skip((int)(SourceData.imagewidth * (SourceData.imageheight / 2))).Take((int)SourceData.imagewidth).ToArray();
            //分割数据
            var segement1 = SourceDataRowMiddle.Where<Point3F>(a => a.Z > 0).ToArray();
            var segement2 = SourceDataRowMiddle.Where<Point3F>(a => a.Z < 0 && a.Z > -999).ToArray();
            //0上的拟合直线
            PointF[] InputPoints = Enumerable.Range(0, segement1.Length).Select(i => new PointF(segement1[i].Y, segement1[i].Z)).ToArray();
            PointF[] MeasurementPoints = Enumerable.Range(0, segement2.Length).Select(i => new PointF(segement2[i].Y, segement2[i].Z)).ToArray();

            #region measure the height

            double param = 0;//距离模型中的数值参数C
            double reps = 1e-6;//坐标原点到直线之间的距离精度
            double aeps = 1e-6;//角度精度
            CvInvoke.FitLine(InputPoints, out PointF direction, out PointF PointOnline, Emgu.CV.CvEnum.DistType.Huber, param, reps, aeps);
            float A = direction.Y, B = direction.X * -1, C = direction.X * PointOnline.Y - direction.Y * PointOnline.X;
            List<float> distances = new List<float>();
            foreach (PointF p in MeasurementPoints)
            {
                float distance = Math.Abs(A * p.X + B * p.Y + C) / (float)Math.Sqrt(A * A + B * B);
                distances.Add(distance);
            }

            distances.Sort();
            var results = Enumerable.Range((int)(distances.Count * 0.15), (int)(distances.Count * 0.85)).Select(i => distances[i]).ToArray();
            float averageHeight = results.Average();
            float maxHeight = results.Max();

            float minHeight = results.Min();
            result.Add(averageHeight);
            result.Add(maxHeight);
            result.Add(minHeight);

            #endregion measure the height

            #region 分区域直接获取Z的高度,分192份，192

            int step = 20;
            for (int i = 0; i < 1920;)
            {
                float aa = Enumerable.Range(i, step).Select(s => new PointF(SourceDataRowMiddle[s].Y, SourceDataRowMiddle[s].Z)).Where<PointF>(a => a.Y > -99).Select(k => k.Y).Average();
                i += step;
                result.Add((float)aa);
            }
            return result;

            #endregion 分区域直接获取Z的高度,分192份，192
        }
    }
}