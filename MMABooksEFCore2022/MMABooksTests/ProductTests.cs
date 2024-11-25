using System.Collections.Generic;
using System.Linq;
using System;

using NUnit.Framework;
using MMABooksEFClasses.MarisModels;
using Microsoft.EntityFrameworkCore;

namespace MMABooksTests
{
    [TestFixture]
    public class ProductTests
    {
        Products? p;
        List<Products>? products;
        MMABooksContext dbContext;

        [SetUp]
        public void Setup()
        {
            dbContext = new MMABooksContext();
            dbContext.Database.ExecuteSqlRaw("call usp_testingResetData()");
        }

        [Test]
        public void GetAllTest()
        {
            products = dbContext.Products.OrderBy(p => p.ProductCode).ToList();
            Assert.AreEqual(16, products.Count);
            Assert.AreEqual("A4CS", products[0].ProductCode);
            PrintAll(products);
        }

        [Test]
        public void GetByPrimaryKeyTest()
        {
            p = dbContext.Products.Find("A4CS");
            Assert.IsNotNull(p);
            Assert.AreEqual("Murach's ASP.NET 4 Web Programming with C# 2010", p.Description);
            Console.WriteLine(p);
        }

        [Test]
        public void GetUsingWhere()
        {
            // get a list of all of the products that have a unit price of 56.50
            products = dbContext.Products.Where(p => p.UnitPrice.Equals(56.50m)).OrderBy(p => p.ProductCode).ToList();
            Assert.AreEqual(7, products.Count);
            Assert.AreEqual("A4CS", products[0].ProductCode);
            PrintAll(products);
        }

        [Test]
        public void GetWithCalculatedFieldTest()
        {
            // get a list of objects that include the productcode, unitprice, quantity and inventoryvalue
            var products = dbContext.Products.Select(
            p => new { p.ProductCode, p.UnitPrice, p.OnHandQuantity, Value = p.UnitPrice * p.OnHandQuantity }).
            OrderBy(p => p.ProductCode).ToList();
            Assert.AreEqual(16, products.Count);
            foreach (var p in products)
            {
                Console.WriteLine(p);
            }
        }

        [Test]
        public void DeleteTest()
        {
            p = dbContext.Products.Find("ZJLR");
            dbContext.Products.Remove(p);
            dbContext.SaveChanges();
            Assert.IsNull(dbContext.Products.Find("ZJLR"));

        }

        [Test]
        public void CreateTest()
        {
            p = new Products();
            p.ProductCode = "??";
            p.Description = "??????";
            p.UnitPrice = 29.99m;
            p.OnHandQuantity = 3;
            dbContext.Products.Add(p);
            dbContext.SaveChanges();
            Assert.IsNotNull(dbContext.Products.Find("??"));

        }

        [Test]
        public void UpdateTest()
        {
            p = dbContext.Products.Find("A4CS");
            p.Description = "????";
            dbContext.Products.Update(p);
            dbContext.SaveChanges();
            p = dbContext.Products.Find("A4CS");
            Assert.AreEqual("????", p.Description);

        }

        public void PrintAll(List<Products> products)
        {
            foreach (Products p in products)
            {
                Console.WriteLine(p);
            }
        }

    }
}