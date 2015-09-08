# NJetty 0.2 Development Roadmap #

Port Geronimo Servlet 3.0 Specification Implementation from java to C# as NJetty.Servlet\_3\_0.Spec




---

## Port Geronimo-Servlet-3.0-Spec to C# as NJetty.Servlet\_3\_0.Spec ##
  * **Javax.NServlet** (**done**)
    * AsyncEvent (**done**)
    * DispatcherType (**done**)
    * FilterRegistration (**done**)
    * IFilter (**done**)
    * IFilterChain (**done**)
    * IFilterConfig (**done**)
    * GenericServlet (**done**)
    * IAsyncContext (**done**)
    * IAsyncListener (**done**)
    * IRequestDispatcher (**done**)
    * IServlet (**done**)
    * IServletConfig (**done**)
    * IServletContext (**done**)
    * ServletContextAttributeEvent (**done**)
    * IServletContextAttributeListener (**done**)
    * ServletContextEvent (**done**)
    * IServletContextListener (**done**)
    * ServletException (**done**)
    * ServletInputStream (**done**)
    * ServletOutputStream (**done**)
    * ServletRegistration (**done**)
    * IServletRequest (**done**)
    * ServletRequestAttributeEvent (**done**)
    * IServletRequestAttributeListener (**done**)
    * ServletRequestEvent (**done**)
    * IServletRequestListener (**done**)
    * ServletRequestWrapper (**done**)
    * IServletResponse (**done**)
    * ServletResponseWrapper (**done**)
    * ISessionCookieConfig (**done**)
    * SessionTrackingMode (**done**)
    * ISingleThreadModel (**done**)
    * UnavailableException (**done**)
  * **Javax.NServlet.Http** (**done**)
    * Cookie (**done**)
    * HttpServlet (**done**)
    * IHttpServletRequest (**done**)
    * HttpServletRequestWrapper (**done**)
    * HttpServletResponseStatusCode (**done**)
    * IHttpServletResponse (**done**)
    * HttpServletResponseWrapper (**done**)
    * IHttpSession (**done**)
    * IHttpSessionActivationListener (**done**)
    * IHttpSessionAttributeListener (**done**)
    * HttpSessionBindingEvent (**done**)
    * IHttpSessionBindingListener (**done**)
    * IHttpSessionContext (**done**)
    * HttpSessionEvent (**done**)
    * IHttpSessionListener (**done**)
    * HttpUtils (**done**)
    * NoBodyOutputStream(inside HttpServlet as inner-class) (**done**)
    * NoBodyResponse(inside HttpServlet as inner-class) (**done**)
  * **Javax.NServlet.Http.Annotation**
    * FilterMapping
    * InitParam
    * Servlet
    * ServletContextListener
    * ServletFilter
  * **Javax.NServlet.Http.Annotation.Jaxrs**
    * DELETE
    * GET
    * HEAD
    * HttpMethod
    * POST
    * PUT