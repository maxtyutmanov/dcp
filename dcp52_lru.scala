/*
This problem was asked by Google.

Implement an LRU (Least Recently Used) cache. It should be able to be initialized with a cache size n, and contain the following methods:

set(key, value): sets key to value. If there are already n items in the cache and we are adding a new item, then it should also remove the least recently used item.
get(key): gets the value at key. If no such key exists, return null.
Each operation should run in O(1) time.
 */

import scala.collection.mutable.HashMap

object Tests {
  def runSample1(): Unit = {
    val lru = new LruCache[String, Int](3)
    lru.set("a", 'a'.toInt)
    lru.set("b", 'b'.toInt)
    lru.set("c", 'c'.toInt)
    lru.set("d", 'd'.toInt)
    println(s"Should be None: ${lru.get("a")}")
    println(s"Should be ${'b'.toInt}: ${lru.get("b")}")
  }
  
  def runSample2(): Unit = {
    val lru = new LruCache[String, Int](3)
    lru.set("a", 'a'.toInt)
    lru.set("b", 'b'.toInt)
    lru.set("c", 'c'.toInt)
    println(s"Should be ${'a'.toInt}: ${lru.get("a")}")
    lru.set("d", 'd'.toInt)
    println(s"Should be None: ${lru.get("b")}")
    println(s"Should be ${'c'.toInt}: ${lru.get("c")}")
    println(s"Should be ${'d'.toInt}: ${lru.get("d")}")
  }
  
  def runSample3(): Unit = {
    val lru = new LruCache[String, Int](3)
    lru.set("a", 'a'.toInt)
    lru.set("b", 'b'.toInt)
    lru.set("c", 'c'.toInt)
    println(s"Should be ${'a'.toInt}: ${lru.get("a")}")
    lru.set("d", 'd'.toInt)
    println(s"Should be None: ${lru.get("b")}")
    println(s"Should be ${'c'.toInt}: ${lru.get("c")}")
    println(s"Should be ${'d'.toInt}: ${lru.get("d")}")
  }
}

class LruCache[K,V](val capacity: Int) {
  if (capacity <= 0) throw new Exception("GTFO")
  
  private val hm = new HashMap[K,LLNode[K,V]]
  private val ll = new LL[K,V]
  
  def set(key: K, value: V): Unit = {
    val existingNode = hm.get(key)
    if (existingNode.isDefined) {
      existingNode.get.value = value
      ll.moveToHead(existingNode.get)
    }
    else {
      if (hm.size == capacity) {
        // evict
        val removedKey = ll.removeTail().get
        hm.remove(removedKey)
      }
      
      val newNode = new LLNode(key, value)
      ll.moveToHead(newNode)
      hm += ((key, newNode))
    }
  }
  
  def get(key: K): Option[V] = {
    val existingNode = hm.get(key)
    if (existingNode.isDefined) {
      ll.moveToHead(existingNode.get)
      Some(existingNode.get.value)
    }
    else None
  }
  
  def remove(key: K) = {
    val llNode = hm.remove(key)
    if (llNode.isDefined) {
      llNode.get.remove()
    }
  }
  
  private class LL[K,V] {
    private var head: Option[LLNode[K,V]] = None
    private var tail: Option[LLNode[K,V]] = None
    
    def moveToHead(n: LLNode[K,V]): Unit = {
      // TODO: there MUST be a way to simplify this (e.g. artificial nodes for head and tail?)
      
      if (head.isDefined && head.get == n) return
      if (tail.isDefined && tail.get == n) tail = tail.get.getPrev
      
      n.remove()
      n.addBefore(head)
      head = Some(n)
      
      if (!tail.isDefined) tail = head
    }
    
    def removeTail(): Option[K] = {
      if (tail.isDefined) {
        val oldTail = tail.get
        tail = oldTail.getPrev
        oldTail.remove()
        Some(oldTail.key)
      }
      else None
    }
  }
  
  private class LLNode[K,V](val key: K, var value: V) {
    private var next: Option[LLNode[K,V]] = None
    private var prev: Option[LLNode[K,V]] = None
    
    def getPrev(): Option[LLNode[K,V]] = prev
    
    def remove(): Unit = {
      if (next.isDefined) next.get.prev = prev
      if (prev.isDefined) prev.get.next = next
      next = None
      prev = None
    }
    
    def addBefore(other: Option[LLNode[K,V]]): Unit = {
      next = other
      if (other.isDefined) other.get.prev = Some(this)
    }
  }
}