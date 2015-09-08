# NJetty 0.3 Development Roadmap #

Port jetty (core) from java to C# as NJetty including Unit Tests



---

## Port Jetty Core to C# as NJetty ##
  * **IO**
    * AbstractBuffer
    * IAsyncEndPoint
    * IBuffer
    * BufferCache
    * BufferDateCache
    * IBuffers
    * BufferUtil
    * ByteArrayBuffer
    * ByteArrayEndPoint
    * IConnection
    * IEndPoint
    * SimpleBuffers
    * View
    * WriterOutputStream
  * **IO.BIO**
    * SocketEndPoint
    * StreamEndPoint
    * StringEndPoint
  * **IO.NIO**
    * ChannelEndPoint
    * DirectNIOBuffer
    * INIOBuffer
    * RandomAccessFileBuffer
    * SelectChannelEndPoint
    * SelectorManager
  * **Core (jetty in jetty)**
    * AbstractBuffers
    * AbstractConnector
    * AbstractGenerator
    * IAuthenticator
    * IConnector
    * CookieCutter
    * Dispatcher
    * EncodedHttpURI
    * EofException
    * IGenerator
    * IHandler
    * IHandlerContainer
    * HttpConnection
    * IHttpContent
    * HttpException
    * HttpFields
    * HttpGenerator
    * HttpHeaders
    * HttpHeadersValues
    * HttpMethods
    * ~~HttpOnlyCookie~~ (**deprecated@jetty, N/A**)
    * HttpParser
    * HttpSchemes
    * HttpStatus
    * IHttpTokens
    * HttpURI
    * HttpVersions
    * InclusiveByteRange
    * LocalConnector
    * MimeTypes
    * NCSARequestLog
    * IParser
    * Request
    * IRequestLog
    * ResourceCache
    * Response
    * RetryRequest
    * Server
    * Servlet3Continuation
    * ISessionIdManager
    * ISessionManager
    * Suspendable
    * IUserRealm
  * **Core.BIO (jetty.bio in jetty)**
    * SocketConnector
  * **Core.Handler (jetty.handler in jetty)**
    * AbstractHandler
    * AbstractHandlerContainer
    * ICompleteHander
    * ContextHandler
    * ContextHandlerCollection
    * DefaultHander
    * ErrorHandler
    * HandlerCollection
    * HandlerList
    * HandlerWrapper
    * MovedContextHander
    * RequestLogHandler
    * ISecurityHandler
    * StatisticsHandler
  * **Core.NIO (jetty.nio in jetty)**
    * AbstractNIOConnector
    * BlockingChannelConnector
    * INIOConnector
    * SelectChannelConnector
  * **Core.Servlet (jetty.servlet in jetty, just add this for now)**
    * AbstractSessionIdManager
    * AbstractSessionManager
    * Context
    * DefaultServlet
    * ErrorPageHandler
    * FilterHolder
    * FilterMapping
    * HashSessionIdManager
    * HashSessionManager
    * Holder
    * Invoker
    * JDBCSessionIdManager
    * JDBCSessionManager
    * NIOResourceCache
    * NoJSPServlet
    * PathMap
    * ServletHandler
    * ServletHolder
    * ServletMapping
    * SessionHandler
    * StatisticsServlet
  * **XBean**
    * No Classes
  * **Resources** Resx Files
    * Encoding (**done**)
    * favico.ico
    * mime (**done**)
    * useragents


---

## Port Jetty Core Tests to as NJetty.Test ##
  * **IO**
    * BufferCacheTest
    * BufferTest
    * BufferUtilTest
    * IOTest
  * **Core**
    * BlockingChannelServerTest
    * BusySelectChannelServerTest
    * CheckReverseProxyHeadersTest
    * DumpHandler
    * HttpConnectionTest
    * HttpGeneratorClientTest
    * HttpGeneratorTest
    * HttpHeaderTest
    * HttpParserTest
    * HttpServerTestBase
    * HttpURITest
    * RandomConnector
    * RequestTest
    * ResourceCacheTest
    * ResponseTest
    * RFC2616Test
    * SelectChannelServerTest
    * ServerTest
    * SocketServerTest
    * SuspendableTest
    * UnreadInputTest
  * **Core.Handler**
    * ContextHandlerCollectionTest
    * ContextHandlerTest
    * StatisticsHandlerTest
  * **Core.Servlet**
    * AbstractSessionTest
    * DispatcherTest
    * InvokerTest
    * JDBCSessionServerTest
    * PathMapTest
    * SessionManagerTest
    * SessionTestClient
    * SessionTestServer
  * **Core.XBean**
    * No Classes