﻿using Emgu.CV;
using Emgu.CV.CvEnum;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

//define Smartray APiX
using SmartRay;
using SmartRay.Api;

namespace APIX_Winform_Demo
{
    /******************************************************************************
     *                               SmartRay Save file          Sample           *
     *----------------------------------------------------------------------------*
     * Copyright (c) SmartRay GmbH 2020.  All Rights Reserved.		              *
     *----------------------------------------------------------------------------*
     ******************************************************************************/

    /**
     * @file SensorHelper.cs
     * @author SmartRay GmbH
     * @date 2023.06
     * @brief This is the sample for convert SmartRay data to Halcon,Opencv data format and etc
     * @see http://www.smartray.com/
     */

    public class SensorHelper
    {
        [DllImport("gdi32")]
        private static extern int DeleteObject(IntPtr o);
        /// <summary>
        /// save smartray Api.Point3d[,] data to ply format
        /// </summary>
        /// <param name="path">save file path</param>
        /// <param name="PointCloud">Api.Point3d[,] data Array</param>
        /// <param name="transportResolution">Transport resolution</param>
        /// <param name="binary">binary mode/Ascii Mode, default is binary mode</param>
        /// <param name="isNullPointsEnable">save null points,default is false</param>
        /// <returns>Task<bool>, you should use await..., if scuessed is true</returns>
        public Task<bool> SaveToPly(string path, Point3F[,] PointCloud, float transportResolution = 0.019f, bool binary = true, bool isNullPointsEnable = false)
        {
            Task<bool> mSaveTask = new Task<bool>(() =>
            {
                try
                {
                    FileStream fst = new FileStream(path, FileMode.Create);
                    StreamWriter swt_Ascii = new StreamWriter(fst, System.Text.Encoding.ASCII);
                    BinaryWriter swt_Binary = new BinaryWriter(fst);

                    //statics points
                    List<Point3F> mListPoints = new List<Point3F>();
                    mListPoints.Clear();
                    if (!isNullPointsEnable)
                    {
                        for (int i = 0; i < PointCloud.GetLength(0); i++)
                        {
                            for (int k = 0; k < PointCloud.GetLength(1); k++)
                            {
                                if (PointCloud[i, k].Z > -9999)
                                {
                                    var points3d = new Point3F();
                                    points3d.X = PointCloud[i, k].X;
                                    points3d.Y = PointCloud[i, k].Y;
                                    points3d.Z = PointCloud[i, k].Z;
                                    mListPoints.Add(points3d);
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < PointCloud.GetLength(0); i++)
                        {
                            for (int k = 0; k < PointCloud.GetLength(1); k++)
                            {

                                var points3d = new Point3F();
                                points3d.X = PointCloud[i, k].X;
                                points3d.Y = PointCloud[i, k].Y;
                                points3d.Z = PointCloud[i, k].Z;
                                mListPoints.Add(points3d);

                            }
                        }
                    }

                    //construct ply file header
                    string fileHead_text = string.Empty;
                    fileHead_text += "ply" + Environment.NewLine;
                    if (!binary)
                        fileHead_text += "format ascii 1.0" + Environment.NewLine;
                    else
                        fileHead_text += "format binary_little_endian 1.0" + Environment.NewLine;
                    fileHead_text += "comment Created by Smartray sensor" + Environment.NewLine;
                    fileHead_text += "comment Created " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + Environment.NewLine;
                    fileHead_text += "obj_info Generated by Smartray sensor" + Environment.NewLine;
                    fileHead_text += "element vertex " + mListPoints.Count.ToString() + Environment.NewLine;
                    fileHead_text += "property float x" + Environment.NewLine;
                    fileHead_text += "property float y" + Environment.NewLine;
                    fileHead_text += "property float z" + Environment.NewLine;
                    fileHead_text += "end_header";

                    //write header
                    swt_Ascii.WriteLine(fileHead_text);
                    swt_Ascii.Flush();
                    //write data array
                    if (!binary)
                    {
                        for (int i = 0; i < mListPoints.Count; i++)
                        {
                            swt_Ascii.Write(mListPoints[i].X.ToString() + " ");
                            swt_Ascii.Write(mListPoints[i].Y.ToString() + " ");
                            swt_Ascii.WriteLine(mListPoints[i].Z.ToString() + " ");
                        }
                        swt_Ascii.Flush();
                        swt_Ascii.Close();
                    }
                    else
                    {
                        for (int i = 0; i < mListPoints.Count; i++)
                        {

                            swt_Binary.Write((float)mListPoints[i].X);
                            swt_Binary.Write((float)mListPoints[i].Y);
                            swt_Binary.Write((float)mListPoints[i].Z);
                        }
                        swt_Binary.Flush();
                        swt_Binary.Close();
                    }

                    fst.Close();
                    return true;
                }
                catch (Exception ce)
                {
                    Console.WriteLine(ce.Message);
                    return false;
                }
            });
            mSaveTask.Start();
            return mSaveTask;
        }


        public Task<bool> SaveToPly(string path, Point3F[] PointCloud, float transportResolution = 0.019f, bool binary = true, bool isNullPointsEnable = false)
        {
            Task<bool> mSaveTask = new Task<bool>(() =>
            {
                try
                {
                    FileStream fst = new FileStream(path, FileMode.Create);
                    StreamWriter swt_Ascii = new StreamWriter(fst, System.Text.Encoding.ASCII);
                    BinaryWriter swt_Binary = new BinaryWriter(fst);

                    //statics points
                    List<Point3F> mListPoints = new List<Point3F>();
                    mListPoints.Clear();
                    if (!isNullPointsEnable)
                    {
                        for (int i = 0; i < PointCloud.GetLength(0); i++)
                        {

                            if (PointCloud[i].Z > -9999)
                            {
                                var points3d = new Point3F();
                                points3d.X = PointCloud[i].X;
                                points3d.Y = PointCloud[i].Y;
                                points3d.Z = PointCloud[i].Z;
                                mListPoints.Add(points3d);
                            }

                        }
                    }
                    else
                    {
                        for (int i = 0; i < PointCloud.GetLength(0); i++)
                        {


                            var points3d = new Point3F();
                            points3d.X = PointCloud[i].X;
                            points3d.Y = PointCloud[i].Y;
                            points3d.Z = PointCloud[i].Z;
                            mListPoints.Add(points3d);

                        }
                    }

                    //construct ply file header
                    string fileHead_text = string.Empty;
                    fileHead_text += "ply" + Environment.NewLine;
                    if (!binary)
                        fileHead_text += "format ascii 1.0" + Environment.NewLine;
                    else
                        fileHead_text += "format binary_little_endian 1.0" + Environment.NewLine;
                    fileHead_text += "comment Created by Smartray sensor" + Environment.NewLine;
                    fileHead_text += "comment Created " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + Environment.NewLine;
                    fileHead_text += "obj_info Generated by Smartray sensor" + Environment.NewLine;
                    fileHead_text += "element vertex " + mListPoints.Count.ToString() + Environment.NewLine;
                    fileHead_text += "property float x" + Environment.NewLine;
                    fileHead_text += "property float y" + Environment.NewLine;
                    fileHead_text += "property float z" + Environment.NewLine;
                    fileHead_text += "end_header";

                    //write header
                    swt_Ascii.WriteLine(fileHead_text);
                    swt_Ascii.Flush();
                    //write data array
                    if (!binary)
                    {
                        for (int i = 0; i < mListPoints.Count; i++)
                        {
                            swt_Ascii.Write(mListPoints[i].X.ToString() + " ");
                            swt_Ascii.Write(mListPoints[i].Y.ToString() + " ");
                            swt_Ascii.WriteLine(mListPoints[i].Z.ToString() + " ");
                        }
                        swt_Ascii.Flush();
                        swt_Ascii.Close();
                    }
                    else
                    {
                        for (int i = 0; i < mListPoints.Count; i++)
                        {

                            swt_Binary.Write((float)mListPoints[i].X);
                            swt_Binary.Write((float)mListPoints[i].Y);
                            swt_Binary.Write((float)mListPoints[i].Z);
                        }
                        swt_Binary.Flush();
                        swt_Binary.Close();
                    }

                    fst.Close();
                    return true;
                }
                catch (Exception ce)
                {
                    Console.WriteLine(ce.Message);
                    return false;
                }
            });
            mSaveTask.Start();
            return mSaveTask;

        }

        public Task<bool> SaveToPly(string path, Point3F[][] PointCloud, float transportResolution = 0.019f, bool binary = true, bool isNullPointsEnable = false)
        {
            Task<bool> mSaveTask = new Task<bool>(() =>
            {
                try
                {
                    FileStream fst = new FileStream(path, FileMode.Create);
                    StreamWriter swt_Ascii = new StreamWriter(fst, System.Text.Encoding.ASCII);
                    BinaryWriter swt_Binary = new BinaryWriter(fst);

                    //statics points
                    List<Point3F> mListPoints = new List<Point3F>();
                    mListPoints.Clear();
                    if (!isNullPointsEnable)
                    {
                        for (int i = 0; i < PointCloud.GetLength(0); i++)
                        {
                            for (int k = 0; k < PointCloud[i].GetLength(0); k++)
                            {
                                if (PointCloud[i][k].Z > -9999)
                                {
                                    var points3d = new Point3F();
                                    points3d.X = PointCloud[i][k].X;
                                    points3d.Y = PointCloud[i][k].Y;
                                    points3d.Z = PointCloud[i][k].Z;
                                    mListPoints.Add(points3d);
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < PointCloud.GetLength(0); i++)
                        {
                            for (int k = 0; k < PointCloud.GetLength(1); k++)
                            {

                                var points3d = new Point3F();
                                points3d.X = PointCloud[i][k].X;
                                points3d.Y = PointCloud[i][k].Y;
                                points3d.Z = PointCloud[i][k].Z;
                                mListPoints.Add(points3d);

                            }
                        }
                    }

                    //construct ply file header
                    string fileHead_text = string.Empty;
                    fileHead_text += "ply" + Environment.NewLine;
                    if (!binary)
                        fileHead_text += "format ascii 1.0" + Environment.NewLine;
                    else
                        fileHead_text += "format binary_little_endian 1.0" + Environment.NewLine;
                    fileHead_text += "comment Created by Smartray sensor" + Environment.NewLine;
                    fileHead_text += "comment Created " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + Environment.NewLine;
                    fileHead_text += "obj_info Generated by Smartray sensor" + Environment.NewLine;
                    fileHead_text += "element vertex " + mListPoints.Count.ToString() + Environment.NewLine;
                    fileHead_text += "property float x" + Environment.NewLine;
                    fileHead_text += "property float y" + Environment.NewLine;
                    fileHead_text += "property float z" + Environment.NewLine;
                    fileHead_text += "end_header";

                    //write header
                    swt_Ascii.WriteLine(fileHead_text);
                    swt_Ascii.Flush();
                    //write data array
                    if (!binary)
                    {
                        for (int i = 0; i < mListPoints.Count; i++)
                        {
                            swt_Ascii.Write(mListPoints[i].X.ToString() + " ");
                            swt_Ascii.Write(mListPoints[i].Y.ToString() + " ");
                            swt_Ascii.WriteLine(mListPoints[i].Z.ToString() + " ");
                        }
                        swt_Ascii.Flush();
                        swt_Ascii.Close();
                    }
                    else
                    {
                        for (int i = 0; i < mListPoints.Count; i++)
                        {

                            swt_Binary.Write((float)mListPoints[i].X);
                            swt_Binary.Write((float)mListPoints[i].Y);
                            swt_Binary.Write((float)mListPoints[i].Z);
                        }
                        swt_Binary.Flush();
                        swt_Binary.Close();
                    }

                    fst.Close();
                    return true;
                }
                catch (Exception ce)
                {
                    Console.WriteLine(ce.Message);
                    return false;
                }
            });
            mSaveTask.Start();
            return mSaveTask;
        }

        /// <summary>
        /// save smartray Api.Point3d[,] data to PCD format
        /// </summary>
        /// <param name="path">save file path</param>
        /// <param name="PointCloud">Api.Point3d[,] data Array</param>
        /// <param name="transportResolution">Transport resolution</param>
        /// <param name="binary">binary mode/Ascii Mode, default is binary mode</param>
        /// <param name="isNullPointsEnable">save null points,default is false</param>
        /// <returns>Task<bool>, you should use await..., if scuessed is true</returns>
        public Task<bool> SaveToPCD(string path, Point3F[,] PointCloud, float transportResolution = 0.019f, bool binary = true, bool isNullPointsEnable = false)
        {
            Task<bool> mSaveTask = new Task<bool>(() =>
            {
                try
                {
                    FileStream fst = new FileStream(path, FileMode.Create);
                    StreamWriter swt_Ascii = new StreamWriter(fst, System.Text.Encoding.ASCII);
                    BinaryWriter swt_Binary = new BinaryWriter(fst);
                    //statics points
                    List<Point3F> mListPoints = new List<Point3F>();
                    if (!isNullPointsEnable)
                    {
                        for (int i = 0; i < PointCloud.GetLength(0); i++)
                        {
                            for (int k = 0; k < PointCloud.GetLength(1); k++)
                            {
                                if (PointCloud[i, k].Z > -999999)
                                {
                                    var points3d = new Point3F();
                                    points3d.X = PointCloud[i, k].X;
                                    points3d.Y = PointCloud[i, k].Y;
                                    points3d.Z = PointCloud[i, k].Z;
                                    mListPoints.Add(points3d);
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < PointCloud.GetLength(0); i++)
                        {
                            for (int k = 0; k < PointCloud.GetLength(1); k++)
                            {

                                var points3d = new Point3F();
                                points3d.X = PointCloud[i, k].X;
                                points3d.Y = PointCloud[i, k].Y;
                                points3d.Z = PointCloud[i, k].Z;
                                mListPoints.Add(points3d);

                            }
                        }
                    }
                    //construct PCD file header
                    string fileHead_text = string.Empty;
                    fileHead_text = "# .PCD v0.7 - Point Cloud Data file format" + Environment.NewLine;
                    fileHead_text += "VERSION 0.7" + Environment.NewLine;
                    fileHead_text += "FIELDS x y z" + Environment.NewLine;
                    fileHead_text += "SIZE 4 4 4" + Environment.NewLine;
                    fileHead_text += "TYPE F F F" + Environment.NewLine;
                    fileHead_text += "COUNT 1 1 1" + Environment.NewLine;
                    fileHead_text += "WIDTH " + mListPoints.Count.ToString() + Environment.NewLine;
                    fileHead_text += "HEIGHT 1" + Environment.NewLine;
                    fileHead_text += "VIEWPOINT 0 0 0 1 0 0 0" + Environment.NewLine;
                    fileHead_text += "POINTS " + mListPoints.Count.ToString();

                    //write header
                    swt_Ascii.WriteLine(fileHead_text);
                    swt_Ascii.Flush();
                    //write data array
                    if (!binary)
                    {
                        swt_Ascii.WriteLine("DATA ascii");
                        swt_Ascii.Flush();
                        for (int i = 0; i < mListPoints.Count; i++)
                        {
                            swt_Ascii.Write(mListPoints[i].X.ToString() + " ");
                            swt_Ascii.Write(mListPoints[i].Y.ToString() + " ");
                            swt_Ascii.WriteLine(mListPoints[i].Z.ToString() + " ");
                        }
                        swt_Ascii.Flush();
                    }
                    else
                    {
                        swt_Ascii.WriteLine("DATA binary");
                        swt_Ascii.Flush();
                        for (int i = 0; i < mListPoints.Count; i++)
                        {
                            swt_Binary.Write((Single)mListPoints[i].X);
                            swt_Binary.Write((Single)mListPoints[i].Y);
                            swt_Binary.Write((Single)mListPoints[i].Z);
                        }
                        swt_Binary.Flush();
                    }
                    swt_Ascii.Close();
                    swt_Binary.Close();
                    fst.Close();
                    return true;
                }
                catch (Exception ce)
                {

                    Console.WriteLine(ce.Message);
                    return false;
                }
            }
            );
            mSaveTask.Start();
            return mSaveTask;
        }


        /// <summary>
        /// Convert 16bit gray image to RGB image
        /// </summary>
        /// <param name="inputImage">Mat image(16bit)</param>
        /// <returns>Color image</returns>
        public Task<Mat> Gray2ColorImage(Mat inputImage)
        {
            Task<Mat> convertTask = new Task<Mat>(() =>
            {

                try
                {
                    Mat thresholdimage = new Mat();

                    ushort[] data = inputImage.GetData(false) as ushort[];
                    byte[] bytedata = new byte[data.Length];
                    ushort maximumValue = 0;
                    ushort minmumvalue = 65535;
                    Parallel.ForEach(data, item =>
                    {
                        maximumValue = item >= maximumValue ? item : maximumValue;
                        minmumvalue = item != 0 && item <= minmumvalue ? item : minmumvalue;
                    });

                    var valueSteps = maximumValue - minmumvalue;
                    Parallel.For(0, data.Length - 1, i =>
                    {
                        bytedata[i] = data[i] > 0 ? (byte)((data[i] - minmumvalue) * 255 / valueSteps) : (byte)0;
                    });

                    IntPtr ImagePointer = new IntPtr();
                    unsafe
                    {
                        fixed (byte* m = bytedata)
                        {
                            ImagePointer = (IntPtr)m;

                        }
                    }
                    Mat newScaleMat = new Mat(inputImage.Height, inputImage.Width, Emgu.CV.CvEnum.DepthType.Cv8U, 3);

                    thresholdimage = new Mat(inputImage.Height, inputImage.Width, Emgu.CV.CvEnum.DepthType.Cv8U, 1, ImagePointer, inputImage.Width);
                    CvInvoke.ApplyColorMap(thresholdimage, newScaleMat, ColorMapType.Rainbow);

                    DeleteObject(ImagePointer);
                    GC.Collect();

                    return newScaleMat;
                }
                catch (Exception ce)
                {
                    Console.WriteLine(ce);
                    return null;
                }
            });

            convertTask.Start();
            return convertTask;
        }


    }
}
