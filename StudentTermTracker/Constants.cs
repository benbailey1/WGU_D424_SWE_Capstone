namespace StudentTermTracker
{
    public static class Constants
    {
#if ANDROID
        public const string API_BASE_URL = "http://10.0.2.2:5176"; /*"http://localhost:5176/";*/ /* "http://10.0.2.2:5176/";*/
#elif IOS
        public const string API_BASE_URL = "http://localhost:5176/";
#else
        public const string API_BASE_URL = "http://localhost:5176/";
#endif
    }
}
