using System.Security.AccessControl;
using System.Transactions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace TDD.DbTestHelpers.Core
{
    public class DbBaseTest<TFixture> where TFixture : IDbFixture, new()
    {
        private readonly TFixture _fixture;
        private TransactionScope _transaction;

        public DbContext fixContext()
        {
            return _fixture.GetContext;
        }

        protected DbBaseTest()
        {
            _fixture = new TFixture();
        }

        [OneTimeSetUp]
        public void BaseFixtureSetUp()
        {
            ApplyFixtures();
        }

        [SetUp]
        public void BaseSetUp()
        {
            if (_fixture.RefillBeforeEachTest)
            {
                ApplyFixtures();
            }
            if (_fixture.UseTransactionScope)
            {
                _transaction = new TransactionScope(TransactionScopeOption.RequiresNew);
            }
        }

        [TearDown]
        public void BaseTearDown()
        {
            if (_fixture.UseTransactionScope)
            {
                _transaction.Dispose();
            }

        }

        private void ApplyFixtures()
        {
            _fixture.PrepareDatabase();
            _fixture.FillFixtures();
        }
    }
}
