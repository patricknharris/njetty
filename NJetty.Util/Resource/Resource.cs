#region Apache License
//
// Licensed to the Apache Software Foundation (ASF) under one or more 
// contributor license agreements. See the NOTICE file distributed with
// this work for additional information regarding copyright ownership. 
// The ASF licenses this file to you under the Apache License, Version 2.0
// (the "License"); you may not use this file except in compliance with 
// the License. You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
#endregion


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;




namespace NJetty.Util.Resource
{

    /// <summary>
    /// TODO: Class/Interface Information here
    /// </summary>
    /// <author>  
    ///     <a href="mailto:leopoldo.agdeppa@gmail.com">Leopoldo Lee Agdeppa III</a>
    /// </author>
    /// <date>
    /// December 2008
    /// </date>
    [Serializable]
    public class Resource //: ISerializable
    {

        //public static bool __defaultUseCaches = true;
        //object _associate;
        
        ///**
        // * Change the default setting for url connection caches.
        // * Subsequent URLConnections will use this default.
        // * @param useCaches
        // */
        //public static void setDefaultUseCaches (bool useCaches)
        //{
        //    __defaultUseCaches=useCaches;
        //}
        
        //public static bool getDefaultUseCaches ()
        //{
        //    return __defaultUseCaches;
        //}
        
        ///* ------------------------------------------------------------ */
        ///** Construct a resource from a url.
        // * @param url A URL.
        // * @return A Resource object.
        // */
        //public static Resource newResource(URL url)
        //{
        //    return newResource(url, __defaultUseCaches);
        //}
        
        ///* ------------------------------------------------------------ */   
        ///**
        // * Construct a resource from a url.
        // * @param url the url for which to make the resource
        // * @param useCaches true enables URLConnection caching if applicable to the type of resource
        // * @return
        // */
        //static Resource newResource(URL url, bool useCaches)
        //{
        //    if (url==null)
        //        return null;

        //    string url_string=url.toExternalForm();
        //    if( url_string.startsWith( "file:"))
        //    {
        //        try
        //        {
        //            FileResource fileResource= new FileResource(url);
        //            return fileResource;
        //        }
        //        catch(Exception e)
        //        {
        //            Log.debug(Log.EXCEPTION,e);
        //            return new BadResource(url,e.toString());
        //        }
        //    }
        //    else if( url_string.startsWith( "jar:file:"))
        //    {
        //        return new JarFileResource(url, useCaches);
        //    }
        //    else if( url_string.startsWith( "jar:"))
        //    {
        //        return new JarResource(url, useCaches);
        //    }

        //    return new URLResource(url,null,useCaches);
        //}

        
        
        ///* ------------------------------------------------------------ */
        ///** Construct a resource from a string.
        // * @param resource A URL or filename.
        // * @return A Resource object.
        // */
        //public static Resource newResource(string resource)
        //{
        //    return newResource(resource, __defaultUseCaches);
        //}
        
        ///* ------------------------------------------------------------ */
        ///** Construct a resource from a string.
        // * @param resource A URL or filename.
        // * @param useCaches controls URLConnection caching
        // * @return A Resource object.
        // */
        //public static Resource newResource (string resource, bool useCaches)
        //{
        //    URL url=null;
        //    try
        //    {
        //        // Try to format as a URL?
        //        url = new URL(resource);
        //    }
        //    catch(MalformedURLException e)
        //    {
        //        if(!resource.startsWith("ftp:") &&
        //           !resource.startsWith("file:") &&
        //           !resource.startsWith("jar:"))
        //        {
        //            try
        //            {
        //                // It's a file.
        //                if (resource.startsWith("./"))
        //                    resource=resource.substring(2);
                        
        //                File file=new File(resource).getCanonicalFile();
        //                url=new URL(URIUtil.encodePath(file.toURL().toString()));            
                        
        //                URLConnection connection=url.openConnection();
        //                connection.setUseCaches(useCaches);
        //                FileResource fileResource= new FileResource(url,connection,file);
        //                return fileResource;
        //            }
        //            catch(Exception e2)
        //            {
        //                Log.debug(Log.EXCEPTION,e2);
        //                throw e;
        //            }
        //        }
        //        else
        //        {
        //            Log.warn("Bad Resource: "+resource);
        //            throw e;
        //        }
        //    }

        //    // Make sure that any special characters stripped really are ignorable.
        //    string nurl=url.toString();
        //    if (nurl.length()>0 &&  nurl.charAt(nurl.length()-1)!=resource.charAt(resource.length()-1))
        //    {
        //        if ((nurl.charAt(nurl.length()-1)!='/' ||
        //             nurl.charAt(nurl.length()-2)!=resource.charAt(resource.length()-1))
        //            &&
        //            (resource.charAt(resource.length()-1)!='/' ||
        //             resource.charAt(resource.length()-2)!=nurl.charAt(nurl.length()-1)
        //             ))
        //        {
        //            return new BadResource(url,"Trailing special characters stripped by URL in "+resource);
        //        }
        //    }
        //    return newResource(url);
        //}

        ///* ------------------------------------------------------------ */
        ///** Construct a system resource from a string.
        // * The resource is tried as classloader resource before being
        // * treated as a normal resource.
        // */
        //public static Resource newSystemResource(string resource)
        //{
        //    URL url=null;
        //    // Try to format as a URL?
        //    ClassLoader
        //        loader=Thread.currentThread().getContextClassLoader();
        //    if (loader!=null)
        //    {
        //        url=loader.getResource(resource);
        //        if (url==null && resource.startsWith("/"))
        //            url=loader.getResource(resource.substring(1));
        //    }
        //    if (url==null)
        //    {
        //        // TODO: bong
        //        //loader=Resource.class.getClassLoader();
        //        if (loader!=null)
        //        {
        //            url=loader.getResource(resource);
        //            if (url==null && resource.startsWith("/"))
        //                url=loader.getResource(resource.substring(1));
        //        }
        //    }
            
        //    if (url==null)
        //    {
        //        url=ClassLoader.getSystemResource(resource);
        //        if (url==null && resource.startsWith("/"))
        //            url=loader.getResource(resource.substring(1));
        //    }
            
        //    if (url==null)
        //        return null;
            
        //    return newResource(url);
        //}

        ///* ------------------------------------------------------------ */
        ///** Find a classpath resource.
        // */
        //public static Resource newClassPathResource(string resource)
        //{
        //    return newClassPathResource(resource,true,false);
        //}

        ///* ------------------------------------------------------------ */
        ///** Find a classpath resource.
        // * The {@java.lang.Class#getResource} method is used to lookup the resource. If it is not
        // * found, then the {@link Loader#getResource(Class, string, bool)} method is used.
        // * If it is still not found, then {@link ClassLoader#getSystemResource(string)} is used.
        // * Unlike {@link #getSystemResource} this method does not check for normal resources.
        // * @param name The relative name of the resouce
        // * @param useCaches True if URL caches are to be used.
        // * @param checkParents True if forced searching of parent classloaders is performed to work around 
        // * loaders with inverted priorities
        // * @return Resource or null
        // */
        //public static Resource newClassPathResource(string name,bool useCaches,bool checkParents)
        //{
        //    URL url;
        //    // TODO: bong
        //    //URL url=Resource.class.getResource(name);
            
        //    if (url==null)
        //    {
        //        try
        //        {
        //            url=Loader.getResource(Resource.class,name,checkParents);
        //        }
        //        catch(ClassNotFoundException e)
        //        {
        //            url=ClassLoader.getSystemResource(name);
        //        }
        //    }
        //    if (url==null)
        //        return null;
        //    return newResource(url,useCaches);
        //}
        
        

        ///* ------------------------------------------------------------ */
        //protected void finalize()
        //{
        //    release();
        //}

        ///* ------------------------------------------------------------ */
        ///** Release any resources held by the resource.
        // */
        //public abstract void release();
        

        ///* ------------------------------------------------------------ */
        ///**
        // * Returns true if the respresened resource exists.
        // */
        //public abstract bool exists();
        

        ///* ------------------------------------------------------------ */
        ///**
        // * Returns true if the respresenetd resource is a container/directory.
        // * If the resource is not a file, resources ending with "/" are
        // * considered directories.
        // */
        //public abstract bool isDirectory();

        ///* ------------------------------------------------------------ */
        ///**
        // * Returns the last modified time
        // */
        //public abstract long lastModified();


        ///* ------------------------------------------------------------ */
        ///**
        // * Return the length of the resource
        // */
        //public abstract long length();
        

        ///* ------------------------------------------------------------ */
        ///**
        // * Returns an URL representing the given resource
        // */
        //public abstract URL getURL();
        

        ///* ------------------------------------------------------------ */
        ///**
        // * Returns an File representing the given resource or NULL if this
        // * is not possible.
        // */
        //public abstract File getFile()
        //    throws IOException;
        

        ///* ------------------------------------------------------------ */
        ///**
        // * Returns the name of the resource
        // */
        //public abstract string getName();
        

        ///* ------------------------------------------------------------ */
        ///**
        // * Returns an input stream to the resource
        // */
        //public abstract InputStream getInputStream()
        //    throws java.io.IOException;

        ///* ------------------------------------------------------------ */
        ///**
        // * Returns an output stream to the resource
        // */
        //public abstract OutputStream getOutputStream()
        //    throws java.io.IOException, SecurityException;
        
        ///* ------------------------------------------------------------ */
        ///**
        // * Deletes the given resource
        // */
        //public abstract bool delete()
        //    throws SecurityException;
        
        ///* ------------------------------------------------------------ */
        ///**
        // * Rename the given resource
        // */
        //public abstract bool renameTo( Resource dest)
        //    throws SecurityException;
        
        ///* ------------------------------------------------------------ */
        ///**
        // * Returns a list of resource names contained in the given resource
        // * The resource names are not URL encoded.
        // */
        //public abstract string[] list();

        ///* ------------------------------------------------------------ */
        ///**
        // * Returns the resource contained inside the current resource with the
        // * given name.
        // * @param path The path segment to add, which should be encoded by the
        // * encode method. 
        // */
        //public abstract Resource addPath(string path)
        //    throws IOException,MalformedURLException;
        

        ///* ------------------------------------------------------------ */
        ///** Encode according to this resource type.
        // * The default implementation calls URI.encodePath(uri)
        // * @param uri 
        // * @return string encoded for this resource type.
        // */
        //public string encode(string uri)
        //{
        //    return URIUtil.encodePath(uri);
        //}
            
        ///* ------------------------------------------------------------ */
        //public object getAssociate()
        //{
        //    return _associate;
        //}

        ///* ------------------------------------------------------------ */
        //public void setAssociate(object o)
        //{
        //    _associate=o;
        //}
        
        ///* ------------------------------------------------------------ */
        ///**
        // * @return The canonical Alias of this resource or null if none.
        // */
        //public URL getAlias()
        //{
        //    return null;
        //}
        
        ///* ------------------------------------------------------------ */
        ///** Get the resource list as a HTML directory listing.
        // * @param base The base URL
        // * @param parent True if the parent directory should be included
        // * @return string of HTML
        // */
        //public string getListHTML(string base,
        //                          bool parent)
        //    throws IOException
        //{
        //    if (!isDirectory())
        //        return null;
            
        //    string[] ls = list();
        //    if (ls==null)
        //        return null;
        //    Arrays.sort(ls);
            
        //    string decodedBase = URIUtil.decodePath(base);
        //    string title = "Directory: "+decodedBase;

        //    StringBuilder buf=new StringBuilder(4096);
        //    buf.append("<HTML><HEAD><TITLE>");
        //    buf.append(title);
        //    buf.append("</TITLE></HEAD><BODY>\n<H1>");
        //    buf.append(title);
        //    buf.append("</H1><TABLE BORDER=0>");
            
        //    if (parent)
        //    {
        //        buf.append("<TR><TD><A HREF=");
        //        buf.append(URIUtil.addPaths(base,"../"));
        //        buf.append(">Parent Directory</A></TD><TD></TD><TD></TD></TR>\n");
        //    }
            
        //    DateFormat dfmt=DateFormat.getDateTimeInstance(DateFormat.MEDIUM,
        //                                                   DateFormat.MEDIUM);
        //    for (int i=0 ; i< ls.length ; i++)
        //    {
        //        string encoded=URIUtil.encodePath(ls[i]);
        //        Resource item = addPath(ls[i]);
                
        //        buf.append("<TR><TD><A HREF=\"");
        //        string path=URIUtil.addPaths(base,encoded);
                
        //        if (item.isDirectory() && !path.endsWith("/"))
        //            path=URIUtil.addPaths(path,URIUtil.SLASH);
        //        buf.append(path);
        //        buf.append("\">");
        //        buf.append(StringUtil.replace(StringUtil.replace(ls[i],"<","&lt;"),">","&gt;"));
        //        buf.append("&nbsp;");
        //        buf.append("</TD><TD ALIGN=right>");
        //        buf.append(item.length());
        //        buf.append(" bytes&nbsp;</TD><TD>");
        //        buf.append(dfmt.format(new Date(item.lastModified())));
        //        buf.append("</TD></TR>\n");
        //    }
        //    buf.append("</TABLE>\n");
        //buf.append("</BODY></HTML>\n");
            
        //    return buf.toString();
        //}
        
        ///* ------------------------------------------------------------ */
        ///** 
        // * @param out 
        // * @param start First byte to write
        // * @param count Bytes to write or -1 for all of them.
        // */
        //public void writeTo(OutputStream out,long start,long count)
        //    throws IOException
        //{
        //    InputStream in = getInputStream();
        //    try
        //    {
        //        in.skip(start);
        //        if (count<0)
        //            IO.copy(in,out);
        //        else
        //            IO.copy(in,out,count);
        //    }
        //    finally
        //    {
        //        in.close();
        //    }
        //}





        //#region Serialization Method and Constructor

        //public Resource(SerializationInfo info, StreamingContext ctxt)
        //{
        //    //Get the values from info and assign them to the appropriate properties

        //    //EmpId = (int)info.GetValue("EmployeeId", typeof(int));
        //    //EmpName = (string)info.GetValue("EmployeeName", typeof(string));
        //}

        

        //public void GetObjectData(SerializationInfo info, StreamingContext context)
        //{
        //    //info.AddValue("EmployeeId", EmpId);
        //    //info.AddValue("EmployeeName", EmpName);
        //}

        //#endregion
    }
}
