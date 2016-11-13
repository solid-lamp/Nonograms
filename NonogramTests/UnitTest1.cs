using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nonograms;

namespace NonogramTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Pewniak_1()
        {
            var origin = new Nonogram(null, null) {row = new List<Line>[1]};
            origin.row[0] = new List<Line>();
            origin.board = new bool?[15, 15];
            origin.height = origin.width = 15;
            origin.row[0].Add(new Line(1));
            origin.row[0].Add(new Line(5));
            origin.row[0].Add(new Line(1));
            origin.row[0].Add(new Line(3));
            Assert.AreEqual(true, origin.UpdateRow(0));
        }
        [TestMethod]
        public void Pewniak_2()
        {
            var origin = new Nonogram(null, null) { row = new List<Line>[1] };
            origin.row[0] = new List<Line>();
            origin.board = new bool?[15, 15];
            origin.height = origin.width = 15;
            origin.row[0].Add(new Line(7));
            Assert.AreEqual(false, origin.UpdateRow(0));
        }
        [TestMethod]
        public void Pewniak_3()
        {
            var origin = new Nonogram(null, null) { row = new List<Line>[1] };
            origin.row[0] = new List<Line>();
            origin.height = origin.width = 16;
            origin.board = new bool?[origin.height, origin.width];
            origin.row[0].Add(new Line(8));
            Assert.AreEqual(false, origin.UpdateRow(0));
        }
        [TestMethod]
        public void Pewniak_4()
        {
            var origin = new Nonogram(null, null) { row = new List<Line>[1] };
            origin.row[0] = new List<Line> {new Line(7),new Line(3)};
            origin.height = origin.width = 15;
            origin.board = new bool?[origin.height, origin.width];
            origin.board[0, 10] = false;
            var result = new bool?[]
                {null, null, null, true, true, true, true, null, null, false, null, true, true, null};
            Assert.AreEqual(true,origin.UpdateRow(0));
            for (int i =0;i<origin.width;i++)
                Assert.AreEqual(result[i], origin.board[0,i]);
            
        }

        [TestMethod]
        public void NonogramGetEmptyLinesInRow_1()
        {
            var origin = new Nonogram(null, null) { row = new List<Line>[1] };
            origin.board = new bool?[8, 11];
            origin.width = 11;
            origin.board[0, 0] =
                origin.board[0, 1] =
                    origin.board[0, 2] = origin.board[0, 5] = origin.board[0, 6] = origin.board[0, 8] = false;
            var result = new List<bool?[]> {new bool?[] {null, null}, new bool?[] {null}, new bool?[] {null, null}};
            var resultF = origin.GetEmptyLinesInRow(0);
            Assert.AreEqual(resultF.Count,result.Count);
            for (int i =0;i<resultF.Count;i++)
                for (int j =0;j<resultF[i].Length;j++)
                    Assert.AreEqual(resultF[i][j],result[i][j]);
        }
        [TestMethod]
        public void NonogramGetEmptyLinesInRow_2()
        {
            var origin = new Nonogram(null, null) { row = new List<Line>[1] };
            origin.board = new bool?[8, 11];
            origin.width = 11;
            origin.board[0, 0] =
                origin.board[0, 1] =
                    origin.board[0, 2] = origin.board[0, 5] = origin.board[0, 6] = origin.board[0, 8] =  origin.board[0,10] = false;
            var result = new List<bool?[]> {new bool?[] {null, null}, new bool?[] {null}, new bool?[] {null}};
            var resultF = origin.GetEmptyLinesInRow(0);
            Assert.AreEqual(resultF.Count, result.Count);
            for (int i = 0 ; i < resultF.Count ; i++)
                for (int j = 0 ; j < resultF[i].Length ; j++)
                    Assert.AreEqual(resultF[i][j], result[i][j]);
        }
        [TestMethod]
        public void NonogramGetEmptyLinesInRow_3()
        {
            var origin = new Nonogram(null, null) { row = new List<Line>[1] };
            origin.board = new bool?[8, 11];
            origin.width = 11;
            var result = new List<bool?[]>
            {
                new bool?[] {null, null, null, null, null, null, null, null, null, null, null}
            };
            var resultF = origin.GetEmptyLinesInRow(0);
            Assert.AreEqual(resultF.Count, result.Count);
            for (int i = 0 ; i < resultF.Count ; i++)
                for (int j = 0 ; j < resultF[i].Length ; j++)
                    Assert.AreEqual(result[i][j], resultF[i][j]);
        }
        [TestMethod]
        public void NonogramGetEmptyLinesInRow_4()
        {
            var origin = new Nonogram(null, null) { row = new List<Line>[1] };
            origin.board = new bool?[8, 11];
            origin.width = 11;
            origin.board[0, 2] = false;
            var result = new List<bool?[]>
            {
                new bool?[] {null, null},
                new bool?[] {null, null, null, null, null, null, null, null}
            };
            var resultF = origin.GetEmptyLinesInRow(0);
            Assert.AreEqual(resultF.Count, result.Count);
            for (int i = 0 ; i < resultF.Count ; i++)
                for (int j = 0 ; j < resultF[i].Length ; j++)
                    Assert.AreEqual(resultF[i][j], result[i][j]);
        }
        [TestMethod]
        public void NonogramGetEmptyLinesInRow_5()
        { 
            var origin = new Nonogram(null, null) { row = new List<Line>[1] };
            origin.width = 16;
            origin.height = origin.width;
            origin.board = new bool?[origin.height, origin.width];
            
            origin.board[0, 0] = origin.board[0, 1] = origin.board[0, 3] = origin.board[0, 4] = origin.board[0, 7] = origin.board[0, 10] = origin.board[0, 15] = false;
            origin.board[0, 2] = origin.board[0, 12] = origin.board[0, 13] = true;
            var result = new List<bool?[]> { new bool?[] { null, null }, new bool?[] { null,null }, new bool?[] { null,true,true,null } };
            var resultF = origin.GetEmptyLinesInRow(0);
            Assert.AreEqual(resultF.Count, result.Count);
            for (int i = 0 ; i < resultF.Count ; i++)
                for (int j = 0 ; j < resultF[i].Length ; j++)
                    Assert.AreEqual(resultF[i][j], result[i][j]);
        }
        [TestMethod]
        public void NonogramGetEmptyLinesInRow_6()
        {
            var origin = new Nonogram(null, null) { row = new List<Line>[1] };
            origin.width = 6;
            origin.height = origin.width;
            origin.board = new bool?[origin.height, origin.width];

            origin.board[0, 2] = origin.board[0, 3] = origin.board[0, 4] = false;
            origin.board[0, 0] = origin.board[0, 5] = true;
            var result = new List<bool?[]> { new bool?[] { true, null } };
            var resultF = origin.GetEmptyLinesInRow(0);
            Assert.AreEqual(resultF.Count, result.Count);
            for (int i = 0 ; i < resultF.Count ; i++)
                for (int j = 0 ; j < resultF[i].Length ; j++)
                    Assert.AreEqual(resultF[i][j], result[i][j]);
        }

        [TestMethod]
        public void NonogramGetEmptyLinesInRow_7()
        {
            var origin = new Nonogram(null, null) { row = new List<Line>[1] };
            origin.width = 1;
            origin.height = origin.width;
            origin.board = new bool?[origin.height, origin.width];

            origin.board[0, 0 ] =true;
           var result = new List<bool?[]> { };
            var resultF = origin.GetEmptyLinesInRow(0);
            Assert.AreEqual(resultF.Count, result.Count);
            for (int i = 0 ; i < resultF.Count ; i++)
                for (int j = 0 ; j < resultF[i].Length ; j++)
                    Assert.AreEqual(resultF[i][j], result[i][j]);
        }
        [TestMethod]
        public void NonogramGetEmptyLinesInRow_8()
        {
            var origin = new Nonogram(null, null) { row = new List<Line>[1] };
            origin.width = 3;
            origin.height = origin.width;
            origin.board = new bool?[origin.height, origin.width];

            origin.board[0, 0] = true;
            var result = new List<bool?[]> { new bool?[] { true, null,null }};
            var resultF = origin.GetEmptyLinesInRow(0);
            Assert.AreEqual(resultF.Count, result.Count);
            for (int i = 0 ; i < resultF.Count ; i++)
                for (int j = 0 ; j < resultF[i].Length ; j++)
                    Assert.AreEqual(resultF[i][j], result[i][j]);
        }
    }
}