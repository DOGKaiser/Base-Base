using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

public class GameDataScriptableObjectTests
{
    private class TestScriptableObject : ScriptableObject
    {
        public string name { get; set; }
    }

    [Test]
    public void Init_DoesNotThrow()
    {
        // Arrange
        GameDataScriptableObject<TestScriptableObject> gameData = new GameDataScriptableObject<TestScriptableObject>();
        string debugLog = "Test";

        // Act and Assert
        Assert.DoesNotThrow(() => gameData.Init(debugLog));
    }

    [Test]
    public void GetConfig_ReturnsNullIfItemDoesNotExist()
    {
        // Arrange
        GameDataScriptableObject<TestScriptableObject> gameData = new GameDataScriptableObject<TestScriptableObject>();
        string debugLog = "Test";
        gameData.Init(debugLog);
        string itemName = "NonExistentItem";

        // Act
        TestScriptableObject result = gameData.GetConfig(itemName);

        // Assert
        Assert.IsNull(result);
    }

    [Test]
    public void GetList_ReturnsEmptyListIfNoItemsExist()
    {
        // Arrange
        GameDataScriptableObject<TestScriptableObject> gameData = new GameDataScriptableObject<TestScriptableObject>();
        string debugLog = "Test";
        gameData.Init(debugLog);

        // Act
        List<TestScriptableObject> result = gameData.GetList();

        // Assert
        Assert.IsEmpty(result);
    }

    [Test]
    public void GetRandomKey_ThrowsIfNoItemsExist()
    {
        // Arrange
        GameDataScriptableObject<TestScriptableObject> gameData = new GameDataScriptableObject<TestScriptableObject>();
        string debugLog = "Test";
        gameData.Init(debugLog);

        // Act and Assert
        Assert.Throws<System.ArgumentOutOfRangeException>(() => gameData.GetRandomKey());
    }

    [Test]
    public void GetRandomScriptableObject_ThrowsIfNoItemsExist()
    {
        // Arrange
        GameDataScriptableObject<TestScriptableObject> gameData = new GameDataScriptableObject<TestScriptableObject>();
        string debugLog = "Test";
        gameData.Init(debugLog);

        // Act and Assert
        Assert.Throws<System.ArgumentOutOfRangeException>(() => gameData.GetRandomScriptableObject());
    }
}