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
            origin.board = new bool?[15, 15];
            origin.height = origin.width = 16;
            origin.row[0].Add(new Line(8));
            Assert.AreEqual(false, origin.UpdateRow(0));
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
            var result = new List<Line> {new Line(2), new Line(1), new Line(2)};
            var resultF = origin.GetEmptyLinesInRow(0);
            Assert.AreEqual(resultF.Count,result.Count);
            int l = resultF.Count;
            for (int i =0;i<l;i++)
                Assert.AreEqual(result[i].Vector.Count,resultF[i].Vector.Count);
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
            var result = new List<Line> { new Line(2), new Line(1), new Line(1) };
            var resultF = origin.GetEmptyLinesInRow(0);
            Assert.AreEqual(resultF.Count, result.Count);
            int l = resultF.Count;
            for (int i = 0 ; i < l ; i++)
                Assert.AreEqual(result[i].Vector.Count, resultF[i].Vector.Count);
        }
        [TestMethod]
        public void NonogramGetEmptyLinesInRow_3()
        {
            var origin = new Nonogram(null, null) { row = new List<Line>[1] };
            origin.board = new bool?[8, 11];
            origin.width = 11;
            var result = new List<Line> { new Line(11) };
            var resultF = origin.GetEmptyLinesInRow(0);
            Assert.AreEqual(resultF.Count, result.Count);
            int l = resultF.Count;
            for (int i = 0 ; i < l ; i++)
                Assert.AreEqual(result[i].Vector.Count, resultF[i].Vector.Count);
        }
    }
}