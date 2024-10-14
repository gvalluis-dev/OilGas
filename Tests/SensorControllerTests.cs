using k8s.KubeConfigModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using OilGas.Controllers;
using OilGas.Data;
using OilGas.DTO;
using OilGas.Models;
using OilGas.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    public class SensorControllerTests
    {
        private readonly SensorContext _context;
        private readonly SensorController _controller;


        public SensorControllerTests()
        {
            // Configuring In-Memory Database instead of having a mock for EF
            var options = new DbContextOptionsBuilder<SensorContext>()
                              .UseInMemoryDatabase(databaseName: "TestDatabase")
                              .Options;
            _context = new SensorContext(options);

            // Getting Data
            _context.SensorData.Add(new SensorData { EquipmentId = "EQ-12345", Value = 78, TimeStamp = DateTime.Now });
            _context.SaveChanges();

            //Creating the controller instance with the in-memory context
            var sensorDataService = new SensorDataService(_context);
            _controller = new SensorController(_context, sensorDataService);
        }

        private void ClearDatabase()
        {
            _context.SensorData.RemoveRange(_context.SensorData); // Clears data from SensorData
            _context.SaveChanges();
        }

        //Test if getting the average from last 24h sensor works
        [Fact]
        public void GetSensorAverage_ReturnsCorrectAverage()
        {
            ClearDatabase();
            // Act
            var result = _controller.GetSensorAverage("24h");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var values = Assert.IsAssignableFrom<IEnumerable<object>>(okResult.Value);

            // Assert
            var okValue = Assert.IsType<SensorAverageDTO>(values);

            Assert.NotNull(values);
        }

        [Fact]
        public async Task GetSensorAverage_ReturnsCorrectAverageFor24h()
        {
            ClearDatabase();
            // Create data
            _context.SensorData.AddRange(
                new SensorData { EquipmentId = "EQ-1111", Value = 47, TimeStamp = DateTime.Now.AddHours(-24) },
                new SensorData { EquipmentId = "EQ-1111", Value = 59, TimeStamp = DateTime.Now.AddHours(-10) },
                new SensorData { EquipmentId = "EQ-2222", Value = 78, TimeStamp = DateTime.Now.AddHours(-22) }
            );
            await _context.SaveChangesAsync();

            // Call 1 day average
            var result = _controller.GetSensorAverage("24h");

            // Assert - Verify result
            var okResult = Assert.IsType<OkObjectResult>(result);
            var averageValues = Assert.IsAssignableFrom<IEnumerable<SensorAverageDTO>>(okResult.Value);
            Assert.NotNull(averageValues);

            // Check if average is correct
            var avgList = averageValues.ToList();
            Assert.Equal(2, avgList.Count); // Should have 2 groups of equipment

            // Check equip average
            var eq1111 = avgList.FirstOrDefault(a => a.EquipmentId == "EQ-1111");
            Assert.Equal(59, eq1111.AverageValue);

            var eq2222 = avgList.FirstOrDefault(a => a.EquipmentId == "EQ-2222");
            Assert.Equal(78, eq2222.AverageValue);
        }

        [Fact]
        public void GetSensorAverage_ReturnsNotFoundIfNoData()
        {
            ClearDatabase();
            // Act - Call method when there is no data of the equip
            var result = _controller.GetSensorAverage("24h");

            // Assert - Check result
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("No data found for the specified period.", notFoundResult.Value);
        }

    }
}
