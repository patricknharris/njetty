# NJetty 0.1 Development Roadmap #

Port jetty-utils from java to C# as NJetty.Util including Unit Tests


---

## Port Jetty Utils to C# as NJetty.Util ##
  * **Components** (**done**)
    * AbstractLifeCycle (**done**)
    * Container (**done**)
    * ContainerRelationship (**done**)
    * IContainerListner (**done**)
    * ILifeCycle (**done**)
    * IListner (**done**)
  * **Logging (Log in jetty)** (**done**)
    * Log : Static Class for logging. can use NLog or StdErrLog (**done**)
    * NLogLog : Log Addapter for NLog. (**done**)
    * StdErrLog : log using Console.Error. (**done**)
  * **Resource**
    * BadResource
    * FileResource
    * JarFileResource (just port everything for now)
    * Resource
    * ResourceCollection
    * IResourceFactory (**done**)
    * URLResource
  * **Threading (Thread in jetty)** (**done**)
    * ~~BoundedThreadPool~~ (**deprecated@jetty, N/A**)
    * IThreadPool (**done**)
    * IThread (**done**)
    * QueuedThreadPool (**done**)
    * Timeout (**done**)
    * TimeoutTask (**done**)
  * **Util**
    * ArrayQueue (**done**)
    * AttributesMap (**done**)
    * ByteArrayISO8859Writer (**done**)
    * ByteArrayOutputStream2 (**done**)
    * ConcurrentDictionary (**done**)
    * DateCache (**done**)
    * FileInfoExtensions (**done**)
    * FilterOutputStream (**done**)
    * FilterWriter (**done**)
    * IAttributes (**done**)
    * IFilenameFilter (**done**)
    * IntrospectionUtil (**done**)
    * IO (**done**)
    * LazyList (**done**)
    * Loader
    * LongExtension (**done**)
    * QuotedStringTokenizer (**done**)
    * Scanner (**done**)
    * SingletonList (**done**)
    * StringExtensions (**done**)
    * StringMap (**done**)
    * StringTokenizer (**done**)
    * StringUtil (**done**)
    * TypeUtil (**done**)
    * URIUtil (**done**)
    * Utf8StringBuilder (**done**)
    * Utf8StringBuffer (**done**)
    * MultiException (**done**)
    * RolloverFileOutputStream (**done**)
    * MultiMap (**done**)
    * MultiPartOutputStream (**done**)
    * MultiPartWriter (**done**)
    * UrlEncoded (**done**)
  * **Util.Ajax**
    * AjaxFilter
    * ~~IContinuation~~ (**deprecated@jetty, might not need to implement**)
    * ContinuationSupport
    * JSON
    * JSONDateConvertor
    * JSONEnumConvertor
    * JSONObjectConvertor
    * WaitingContinuation
  * **TODO.Package**
    * Todo.Class

---

## Port Jetty Utils Test to C# as NJetty.Util.Test ##
  * **Component** (**done**)
    * LifeCycleTest (**done**)
    * LifeCycleListenerTest (**done**)
    * TestListener (**done**)
  * **Logging** (**done**)
    * LogTest (**done**)
  * **Resource**
    * (test data in this package TestData)
    * ResourceTest
    * ResourceCollectionTest
  * **Threading** (**done**)
    * ThreadPoolTest (**done**)
    * TimeoutTest (**done**)
  * **Util**
    * LazyListTest (**done**)
    * ArrayQueueTest (**done**)
    * URITest (**done**)
    * QuotedStringTokenizerTest (**done**)
    * StringMapTest (**done**)
    * Utf8StringBufferTest (**done**)
    * Utf8StringBuilderTest (**done**)
    * TestIntrospectionUtil (**done**)
    * URLEncodedTest (**done**)
    * DateCacheTest (**done**)
    * StringUtilTest (**done**)
  * **Util.Ajax**
    * JSONTest