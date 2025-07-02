using FoodOrderingDataAccessLayer;

namespace FoodOrderingTests
{
    [TestClass()]
    public class CustomerTests
    {
        [TestMethod()]
        public void TestConstructor()
        {
            // Act
            var repository = new FoodRepository();

            // Assert
            Assert.IsNotNull(repository.Context, "Context should not be null after constructor.");
            Assert.IsInstanceOfType(repository.Context, typeof(FoodOrderingDataAccessLayer.Models.FoodDbContext), "Context should be of type FoodDbContext.");
        }

    }
}