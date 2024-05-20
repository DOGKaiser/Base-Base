using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class ObjectPoolMgrTests {
    private const string PATH = "AudioSource";
    private const string NAME = "AudioSource";

    private ObjectPoolMgr objectPoolMgr;

    [SetUp]
    public void SetUp() {

        // Get the instance of the ObjectPoolMgr
        objectPoolMgr = ObjectPoolMgr.Instance;
    }

    [Test, Order(1)]
    public void IsLoaded_NotLoaded_ReturnsFalse() {
        // Test that IsLoaded returns false when the object is not loaded
        Assert.IsFalse(objectPoolMgr.IsLoaded(PATH));
    }

    [Test]
    public void GetObject_ObjectNotLoaded_LoadsAndReturnsObject() {
        // Test that GetObject loads and returns the object when it's not already loaded
        GameObject obj = objectPoolMgr.GetObject(PATH);
        Assert.IsNotNull(obj);
        Assert.AreEqual(NAME, obj.name);
    }

    [Test]
    public void GetObject_ObjectLoaded_ReturnsObject() {
        // Get an object from the pool to load it
        GameObject obj1 = objectPoolMgr.GetObject(PATH);

        // Test that GetObject returns the loaded object
        GameObject obj2 = objectPoolMgr.GetObject(PATH);
        Assert.IsNotNull(obj2);
        Assert.AreEqual(NAME, obj2.name);
    }

    [Test]
    public void ReuseObject_ReusesObject() {
        // Get an object from the pool
        GameObject obj = objectPoolMgr.GetObject(PATH);

        // Reuse the object
        objectPoolMgr.ReuseObject(PATH, obj);

        // Test that the object is inactive
        Assert.IsFalse(obj.activeSelf);
    }

    [UnityTest, Order(2)]
    public IEnumerator GetObject_ReusesExistingObject() {
        // Get an object from the pool
        GameObject obj1 = objectPoolMgr.GetObject(PATH);

        // Reuse the object
        objectPoolMgr.ReuseObject(PATH, obj1);

        // Wait for the object to be reused
        yield return new WaitForSeconds(0.2f);

        // Get another object from the pool
        GameObject obj2 = objectPoolMgr.GetObject(PATH);

        // Test that the reused object is the same as the original object
        Assert.AreEqual(obj1, obj2);
    }
}