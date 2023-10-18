    
    namespace srcds_server_query_timeout;

    public class QueryTimeoutException : Exception
    {
        public QueryTimeoutException(string msg) : base(msg)
        {  }
    }