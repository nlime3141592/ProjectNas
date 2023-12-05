﻿using System;
using System.Runtime.Remoting.Messaging;
using System.Threading;

namespace NAS
{
    public static class LogExtensions
    {
        public static bool WriteLog(this object _instance, string _message)
        {
            try
            {
                Console.WriteLine("[{0}] {1}", _instance.GetType().Name, _message);
                System.Diagnostics.Debug.WriteLine("[{0}] {1}", _instance.GetType().Name, _message);
                return true;
            }
            catch(ThreadInterruptedException)
            {
                Console.WriteLine("[{0}] {1}", _instance.GetType().Name, _message);
                System.Diagnostics.Debug.WriteLine("[{0}] {1}", _instance.GetType().Name, _message);
                return true;
            }
            catch(Exception _exception)
            {
                Console.WriteLine("[{0}] Log Exception Occurred.", _instance.GetType().Name);
                System.Diagnostics.Debug.WriteLine("[{0}] Log Exception Occurred.", _instance.GetType().Name);
                return false;
            }
        }
        /*
        public static bool WriteLog(this object _instance, string _header, string _message)
        {
            try
            {
                Console.WriteLine("[{0}] {1}", _header, _message);
                return true;
            }
            catch (ThreadInterruptedException)
            {
                Console.WriteLine("[{0}] {1}", _header, _message);
                return true;
            }
            catch (Exception _exception)
            {
                Console.WriteLine("[{0}] Log Exception Occurred.", _header);
                Console.WriteLine("{0}", _exception.Message);
                return false;
            }
        }
        */
        public static bool WriteLog(this object _instance, string _format, params object[] _args)
        {
            try
            {
                string _message = string.Format(_format, _args);
                Console.WriteLine("[{0}] {1}", _instance.GetType().Name, _message);
                System.Diagnostics.Debug.WriteLine("[{0}] {1}", _instance.GetType().Name, _message);
                return true;
            }
            catch (ThreadInterruptedException)
            {
                string _message = string.Format(_format, _args);
                Console.WriteLine("[{0}] {1}", _instance.GetType().Name, _message);
                System.Diagnostics.Debug.WriteLine("[{0}] {1}", _instance.GetType().Name, _message);
                return true;
            }
            catch (Exception)
            {
                Console.WriteLine("[{0}] Log Exception Occurred.", _instance.GetType().Name);
                System.Diagnostics.Debug.WriteLine("[{0}] Log Exception Occurred.", _instance.GetType().Name);
                return false;
            }
        }
        /*
        public static bool WriteLog(this object _instance, string _header, string _format, params object[] _args)
        {
            try
            {
                string _message = string.Format(_format, _args);
                Console.WriteLine("[{0}] {1}", _header, _message);
                return true;
            }
            catch (ThreadInterruptedException)
            {
                string _message = string.Format(_format, _args);
                Console.WriteLine("[{0}] {1}", _header, _message);
                return true;
            }
            catch (Exception)
            {
                Console.WriteLine("[{0}] Log Exception Occurred.", _header);
                return false;
            }
        }
        */
    }
}
